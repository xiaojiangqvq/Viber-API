using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Drawing;
using System.Reflection;

namespace ViberChannelBulkSender;

public partial class Form1 : Form
{
	private readonly HttpClient _httpClient = new();
	private readonly JsonSerializerOptions _jsonOptions = new()
	{
		PropertyNameCaseInsensitive = true
	};

	private TcpListener? _localHttpServer;
	private bool _httpServerRunning;
	private const int LocalHttpPort = 5000;
	private Process? _ngrokProcess;
	private string? _ngrokPublicUrl;
	private const string NgrokExeRelativePath = @"..\\..\\..\\ngrok\\ngrok.exe";

	public Form1()
	{
		InitializeComponent();
		this.FormClosed += Form1_FormClosed;
		InitializeUiBranding();
	}

	private void Form1_FormClosed(object? sender, FormClosedEventArgs e)
	{
		try
		{
			if (_ngrokProcess != null && !_ngrokProcess.HasExited)
			{
				_ngrokProcess.Kill(true);
			}
		}
		catch
		{
			// ignore
		}
	}

	private const string AppVersion = "0.0.1";

	private void InitializeUiBranding()
	{
		// 标题：ViberBot v0.0.1（以后只改 AppVersion 即可升级）
		this.Text = $"ViberBot v{AppVersion}";

		// 设置窗口图标
		try
		{
			var exeDir = AppDomain.CurrentDomain.BaseDirectory;
			var iconFileName = "FC9F0D28-710B-4D38-890B-B207ED8001E3.png.JPG";
			var iconPath = Path.Combine(exeDir, iconFileName);
			if (File.Exists(iconPath))
			{
				using var bmp = new Bitmap(iconPath);
				var hIcon = bmp.GetHicon();
				using var icon = Icon.FromHandle(hIcon);
				this.Icon = (Icon)icon.Clone();
			}
		}
		catch (Exception ex)
		{
			AppendLog($"设置窗口图标失败：{ex.Message}");
		}
	}

	private async void btnToggleHttp_Click(object? sender, EventArgs e)
	{
		if (_httpServerRunning)
		{
			StopLocalHttpServer();
		}
		else
		{
			await StartLocalHttpServerAsync();
		}
	}

	private async void btnInitWebhook_Click(object? sender, EventArgs e)
	{
		var tokens = txtTokens.Text
			.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
			.Select(t => t.Trim())
			.Where(t => t.Length > 0)
			.ToList();

		if (tokens.Count == 0)
		{
			MessageBox.Show("请至少填写一个频道 Token（每行一个）。");
			return;
		}

		if (!_httpServerRunning)
		{
			var result = MessageBox.Show("本地 HTTP 服务尚未启动，是否现在启动？", "提示", MessageBoxButtons.YesNo);
			if (result == DialogResult.Yes)
			{
				await StartLocalHttpServerAsync();
			}
			else
			{
				return;
			}
		}

		btnInitWebhook.Enabled = false;
		try
		{
			lblStatus.Text = "正在启动 ngrok 并为所有频道设置 webhook...";
			AppendLog("开始启动 ngrok 并为所有频道设置 webhook。");

			var ok = await EnsureNgrokStartedAndUrlAsync();
			if (!ok || string.IsNullOrEmpty(_ngrokPublicUrl))
			{
				return;
			}

			await InitWebhooksForTokensAsync(tokens, _ngrokPublicUrl);
		}
		finally
		{
			btnInitWebhook.Enabled = true;
		}
	}

	private async void btnSend_Click(object? sender, EventArgs e)
	{
		var messageText = txtMessage.Text.Trim();
		if (string.IsNullOrEmpty(messageText))
		{
			MessageBox.Show("请先填写要群发的文本内容。");
			return;
		}

		var tokens = txtTokens.Text
			.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
			.Select(t => t.Trim())
			.Where(t => t.Length > 0)
			.ToList();

		if (tokens.Count == 0)
		{
			MessageBox.Show("请至少填写一个频道 Token（每行一个）。");
			return;
		}

		var batchSize = (int)numBatchSize.Value;
		var delayMinutes = (int)numBatchDelay.Value;
		if (batchSize <= 0)
		{
			batchSize = tokens.Count;
		}

		btnSend.Enabled = false;
		lblStatus.Text = $"正在向 {tokens.Count} 个频道发送（每批 {batchSize} 个，间隔 {delayMinutes} 分钟）...";
		AppendLog($"开始向 {tokens.Count} 个频道发送消息（每批 {batchSize} 个，间隔 {delayMinutes} 分钟）。");

		int success = 0, failed = 0;
		int totalBatches = (int)Math.Ceiling(tokens.Count / (double)batchSize);

		for (int batchIndex = 0; batchIndex < totalBatches; batchIndex++)
		{
			var batchTokens = tokens.Skip(batchIndex * batchSize).Take(batchSize).ToList();
			AppendLog($"开始发送第 {batchIndex + 1} 批，共 {batchTokens.Count} 个频道。");

			foreach (var token in batchTokens)
			{
				try
				{
					lblStatus.Text = $"第 {batchIndex + 1}/{totalBatches} 批：正在处理频道 Token：{Shorten(token, 10)}";

					var fromId = await GetSuperAdminIdAsync(token);
					if (fromId == null)
					{
						failed++;
						continue;
					}

					var sendOk = await SendTextMessageAsync(token, fromId, messageText);
					if (sendOk)
					{
						success++;
					}
					else
					{
						failed++;
					}
				}
				catch (Exception ex)
				{
					failed++;
					AppendLog($"Token {Shorten(token, 10)}: 未预期的错误：{ex.Message}");
				}
			}

			AppendLog($"第 {batchIndex + 1} 批发送完成。当前累计：成功 {success}，失败 {failed}。");

			if (batchIndex < totalBatches - 1 && delayMinutes > 0)
			{
				lblStatus.Text = $"第 {batchIndex + 1} 批发送完成，暂停 {delayMinutes} 分钟后继续第 {batchIndex + 2} 批...";
				AppendLog($"将暂停 {delayMinutes} 分钟后继续发送下一批。");
				await Task.Delay(TimeSpan.FromMinutes(delayMinutes));
			}
		}

		lblStatus.Text = $"全部完成。成功：{success}，失败：{failed}";
		AppendLog($"发送完成。成功：{success}，失败：{failed}。");
		btnSend.Enabled = true;
	}

	private async Task<string?> GetSuperAdminIdAsync(string authToken)
	{
		var payload = new { auth_token = authToken };
		var json = JsonSerializer.Serialize(payload);
		using var content = new StringContent(json, Encoding.UTF8, "application/json");

		using var response = await _httpClient.PostAsync("https://chatapi.viber.com/pa/get_account_info", content);
		var responseBody = await response.Content.ReadAsStringAsync();

		if (!response.IsSuccessStatusCode)
		{
			AppendLog($"get_account_info HTTP error ({response.StatusCode}) for token {Shorten(authToken, 10)}: {responseBody}");
			return null;
		}

		GetAccountInfoResponse? result;
		try
		{
			result = JsonSerializer.Deserialize<GetAccountInfoResponse>(responseBody, _jsonOptions);
		}
		catch (Exception ex)
		{
			AppendLog($"get_account_info parse error for token {Shorten(authToken, 10)}: {ex.Message}");
			return null;
		}

		if (result == null)
		{
			AppendLog($"get_account_info returned empty result for token {Shorten(authToken, 10)}.");
			return null;
		}

		if (result.status != 0)
		{
			AppendLog($"get_account_info failed for token {Shorten(authToken, 10)}: {result.status} - {result.status_message}");
			return null;
		}

		var superAdmin = result.members?
			.FirstOrDefault(m => string.Equals(m.role, "superadmin", StringComparison.OrdinalIgnoreCase));

		if (superAdmin == null)
		{
			AppendLog($"No superadmin found for token {Shorten(authToken, 10)}. Using first member as fallback.");
			superAdmin = result.members?.FirstOrDefault();
		}

		if (superAdmin == null || string.IsNullOrEmpty(superAdmin.id))
		{
			AppendLog($"Could not determine sender id for token {Shorten(authToken, 10)}.");
			return null;
		}

		return superAdmin.id;
	}

	private async Task<bool> SendTextMessageAsync(string authToken, string fromId, string text)
	{
		var payload = new
		{
			auth_token = authToken,
			from = fromId,
			type = "text",
			text = text
		};

		var json = JsonSerializer.Serialize(payload);
		using var content = new StringContent(json, Encoding.UTF8, "application/json");

		using var response = await _httpClient.PostAsync("https://chatapi.viber.com/pa/post", content);
		var responseBody = await response.Content.ReadAsStringAsync();

		if (!response.IsSuccessStatusCode)
		{
			AppendLog($"post HTTP error ({response.StatusCode}) for from {Shorten(fromId, 8)}: {responseBody}");
			return false;
		}

		SendMessageResponse? result;
		try
		{
			result = JsonSerializer.Deserialize<SendMessageResponse>(responseBody, _jsonOptions);
		}
		catch (Exception ex)
		{
			AppendLog($"post parse error for from {Shorten(fromId, 8)}: {ex.Message}");
			return false;
		}

		if (result == null)
		{
			AppendLog($"post returned empty result for from {Shorten(fromId, 8)}.");
			return false;
		}

		if (result.status == 0)
		{
			AppendLog($"Message sent successfully from {Shorten(fromId, 8)}.");
			return true;
		}

		AppendLog($"Message failed from {Shorten(fromId, 8)}: {result.status} - {result.status_message}");
		return false;
	}

	private async Task<bool> SetWebhookAsync(string authToken, string publicUrl)
	{
		var url = publicUrl.TrimEnd('/') + "/";

		var payload = new
		{
			url,
			event_types = new[] { "delivered", "seen", "failed", "subscribed", "unsubscribed", "conversation_started" },
			send_name = true,
			send_photo = true
		};

		var json = JsonSerializer.Serialize(payload);
		using var request = new HttpRequestMessage(HttpMethod.Post, "https://chatapi.viber.com/pa/set_webhook");
		request.Content = new StringContent(json, Encoding.UTF8, "application/json");
		request.Headers.Add("X-Viber-Auth-Token", authToken);

		using var response = await _httpClient.SendAsync(request);
		var responseBody = await response.Content.ReadAsStringAsync();

		if (!response.IsSuccessStatusCode)
		{
			AppendLog($"set_webhook HTTP error ({response.StatusCode}) for token {Shorten(authToken, 10)}: {responseBody}");
			return false;
		}

		SetWebhookResponse? result;
		try
		{
			result = JsonSerializer.Deserialize<SetWebhookResponse>(responseBody, _jsonOptions);
		}
		catch (Exception ex)
		{
			AppendLog($"set_webhook parse error for token {Shorten(authToken, 10)}: {ex.Message}");
			return false;
		}

		if (result == null)
		{
			AppendLog($"set_webhook returned empty result for token {Shorten(authToken, 10)}.");
			return false;
		}

		if (result.status != 0)
		{
			AppendLog($"set_webhook failed for token {Shorten(authToken, 10)}: {result.status} - {result.status_message}");
			return false;
		}

		AppendLog($"set_webhook 成功，token {Shorten(authToken, 10)}。");
		return true;
	}

	private async Task InitWebhooksForTokensAsync(List<string> tokens, string publicUrl)
	{
		int success = 0, failed = 0;

		foreach (var token in tokens)
		{
			lblStatus.Text = $"正在为频道 Token：{Shorten(token, 10)} 设置 webhook...";
			if (await SetWebhookAsync(token, publicUrl))
			{
				success++;
			}
			else
			{
				failed++;
			}
		}

		lblStatus.Text = $"Webhook 初始化完成。成功：{success}，失败：{failed}";
		AppendLog($"Webhook 初始化完成。成功：{success}，失败：{failed}。");
	}

	private async Task<bool> EnsureNgrokStartedAndUrlAsync()
	{
		try
		{
			if (_ngrokProcess == null || _ngrokProcess.HasExited)
			{
				var exePath = Path.GetFullPath(NgrokExeRelativePath);
				if (!File.Exists(exePath))
				{
					AppendLog($"未找到 ngrok 可执行文件：{exePath}");
					MessageBox.Show($"未找到 ngrok 可执行文件：{exePath}");
					return false;
				}

				var psi = new ProcessStartInfo(exePath, $"http {LocalHttpPort}")
				{
					UseShellExecute = false,
					RedirectStandardOutput = true,
					RedirectStandardError = true,
					CreateNoWindow = true,
					WorkingDirectory = Path.GetDirectoryName(exePath) ?? Environment.CurrentDirectory
				};

				_ngrokProcess = Process.Start(psi);
				AppendLog("已启动 ngrok http 隧道，等待其就绪...");
				await Task.Delay(3000);
			}

			// 尝试多次从 ngrok 本地 API 读取公网地址
			for (int i = 0; i < 5; i++)
			{
				var url = await GetNgrokPublicUrlAsync();
				if (!string.IsNullOrEmpty(url))
				{
					_ngrokPublicUrl = url;
					lblHttpUrl.Text = $"本地 HTTP 服务：http://localhost:{LocalHttpPort}/   公网：{_ngrokPublicUrl}";
					AppendLog($"ngrok 公网地址：{_ngrokPublicUrl}");
					return true;
				}

				await Task.Delay(1000);
			}

			AppendLog("未能从 ngrok 读取公网地址，请检查 ngrok 是否正常运行。");
			MessageBox.Show("未能从 ngrok 读取公网地址，请检查 ngrok 是否正常运行。");
			return false;
		}
		catch (Exception ex)
		{
			AppendLog($"启动或查询 ngrok 失败：{ex.Message}");
			MessageBox.Show($"启动或查询 ngrok 失败：{ex.Message}");
			return false;
		}
	}

	private async Task<string?> GetNgrokPublicUrlAsync()
	{
		try
		{
			using var response = await _httpClient.GetAsync("http://127.0.0.1:4040/api/tunnels");
			var body = await response.Content.ReadAsStringAsync();

			if (!response.IsSuccessStatusCode)
			{
				AppendLog($"查询 ngrok 公网地址失败：HTTP {(int)response.StatusCode} - {response.ReasonPhrase}");
				return null;
			}

			NgrokTunnelsResponse? result;
			try
			{
				result = JsonSerializer.Deserialize<NgrokTunnelsResponse>(body, _jsonOptions);
			}
			catch (Exception ex)
			{
				AppendLog($"解析 ngrok 公网地址返回数据失败：{ex.Message}");
				return null;
			}

			var tunnel = result?.tunnels?
				.FirstOrDefault(t => t.public_url != null && t.public_url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
				?? result?.tunnels?.FirstOrDefault(t => !string.IsNullOrEmpty(t.public_url));

			return tunnel?.public_url;
		}
		catch (Exception ex)
		{
			AppendLog($"查询 ngrok 公网地址异常：{ex.Message}");
			return null;
		}
	}

	private void AppendLog(string message)
	{
		var line = $"[{DateTime.Now:HH:mm:ss}] {message}";
		if (txtLog.InvokeRequired)
		{
			txtLog.BeginInvoke(new Action<string>(AppendLogLine), line);
		}
		else
		{
			AppendLogLine(line);
		}
	}

	private void AppendLogLine(string line)
	{
		if (txtLog.TextLength == 0)
		{
			txtLog.Text = line;
		}
		else
		{
			txtLog.AppendText(Environment.NewLine + line);
		}
	}

	private static string Shorten(string value, int max)
	{
		if (string.IsNullOrEmpty(value) || value.Length <= max) return value;
		return value.Substring(0, max) + "...";
	}

	private async Task StartLocalHttpServerAsync()
	{
		if (_httpServerRunning)
		{
			return;
		}

		try
		{
			_localHttpServer = new TcpListener(IPAddress.Loopback, LocalHttpPort);
			_localHttpServer.Start();
			_httpServerRunning = true;
			btnToggleHttp.Text = "停止 HTTP 服务";
			lblHttpUrl.Text = $"本地 HTTP 服务：http://localhost:{LocalHttpPort}/（配合 ngrok http {LocalHttpPort} 使用）";
			AppendLog($"本地 HTTP 服务已在端口 {LocalHttpPort} 启动。");

			_ = Task.Run(AcceptLoopAsync);
		}
		catch (Exception ex)
		{
			AppendLog($"启动本地 HTTP 服务失败：{ex.Message}");
			_httpServerRunning = false;
			try { _localHttpServer?.Stop(); } catch { }
			_localHttpServer = null;
			btnToggleHttp.Text = "启动 HTTP 服务";
			lblHttpUrl.Text = "本地 HTTP 服务：已停止（配合 ngrok http 5000 使用）";
		}
	}

	private void StopLocalHttpServer()
	{
		if (!_httpServerRunning)
		{
			return;
		}

		_httpServerRunning = false;
		try { _localHttpServer?.Stop(); } catch { }
		_localHttpServer = null;
		btnToggleHttp.Text = "启动 HTTP 服务";
		lblHttpUrl.Text = "本地 HTTP 服务：已停止（配合 ngrok http 5000 使用）";
		AppendLog("本地 HTTP 服务已停止。");
	}

	private async Task AcceptLoopAsync()
	{
		while (_httpServerRunning && _localHttpServer != null)
		{
			TcpClient client;
			try
			{
				client = await _localHttpServer.AcceptTcpClientAsync();
			}
			catch (ObjectDisposedException)
			{
				break;
			}
			catch (Exception ex)
			{
				if (!_httpServerRunning)
				{
					break;
				}
				AppendLog($"HTTP 服务接受连接时出错：{ex.Message}");
				continue;
			}

			_ = Task.Run(() => HandleClientAsync(client));
		}
	}

	private async Task HandleClientAsync(TcpClient client)
	{
		using (client)
		{
			try
			{
				using var stream = client.GetStream();
				var buffer = new byte[4096];
				var sb = new StringBuilder();
				int bytesRead;
				do
				{
					bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
					if (bytesRead <= 0) break;
					sb.Append(Encoding.ASCII.GetString(buffer, 0, bytesRead));
					if (sb.ToString().Contains("\r\n\r\n")) break;
				} while (stream.DataAvailable);

				AppendLog("收到 webhook 测试 HTTP 请求。");

				var responseText = "OK";
				var responseBytes = Encoding.ASCII.GetBytes(
					"HTTP/1.1 200 OK\r\n" +
					"Content-Type: text/plain; charset=utf-8\r\n" +
					$"Content-Length: {responseText.Length}\r\n" +
					"Connection: close\r\n" +
					"\r\n" +
					responseText
				);
				await stream.WriteAsync(responseBytes, 0, responseBytes.Length);
			}
			catch (Exception ex)
			{
				AppendLog($"HTTP 服务处理请求时出错：{ex.Message}");
			}
		}
	}

	private sealed class GetAccountInfoResponse
	{
		public int status { get; set; }
		public string? status_message { get; set; }
		public List<Member>? members { get; set; }
	}

	private sealed class Member
	{
		public string? id { get; set; }
		public string? name { get; set; }
		public string? role { get; set; }
	}

	private sealed class SendMessageResponse
	{
		public int status { get; set; }
		public string? status_message { get; set; }
	}

	private sealed class SetWebhookResponse
	{
		public int status { get; set; }
		public string? status_message { get; set; }
		public List<string>? event_types { get; set; }
	}

	private sealed class NgrokTunnelsResponse
	{
		public List<NgrokTunnel>? tunnels { get; set; }
	}

	private sealed class NgrokTunnel
	{
		public string? public_url { get; set; }
		public string? proto { get; set; }
	}
}
