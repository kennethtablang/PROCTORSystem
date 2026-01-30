using Microsoft.AspNetCore.SignalR.Client;
using System.Drawing;
using System.Drawing.Imaging;
using System.Management;
using System.Net;
using System.Net.Sockets;

namespace PROCTORSystem.Agent;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private HubConnection? _hubConnection;
    private readonly IConfiguration _configuration;
    private readonly string _hubUrl;
    private string _machineName = Environment.MachineName;

    public Worker(ILogger<Worker> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
        _hubUrl = _configuration.GetValue<string>("HubUrl") ?? "http://localhost:5237/monitoringHub";
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        _hubConnection = new HubConnectionBuilder()
            .WithUrl(_hubUrl)
            .WithAutomaticReconnect()
            .Build();

        _hubConnection.On<string, string>("ReceiveCommand", (command, payload) =>
        {
            _logger.LogInformation($"Received command: {command} - {payload}");
            // TODO: Execute shell commands (shutdown, lock, etc.)
        });

        await _hubConnection.StartAsync(cancellationToken);
        _logger.LogInformation("Connected to SignalR Hub");

        await RegisterAgent();

        await base.StartAsync(cancellationToken);
    }

    private async Task RegisterAgent()
    {
        if (_hubConnection == null) return;

        var ip = GetLocalIP();
        var studentInfo = new
        {
            MachineName = _machineName,
            IPAddress = ip,
            OperatingSystem = Environment.OSVersion.ToString(),
            IsOnline = true
        };

        try
        {
            await _hubConnection.InvokeAsync("RegisterStudent", studentInfo);
            _logger.LogInformation("Registered successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to register");
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_hubConnection != null && _hubConnection.State == HubConnectionState.Connected)
            {
                try
                {
                    string screenBase64 = CaptureScreen();
                    await _hubConnection.InvokeAsync("StreamScreen", _machineName, screenBase64, stoppingToken);
                }
                catch (Exception ex)
                {
                   // _logger.LogError(ex, "Error streaming screen");
                   // Suppress frequent errors
                }
            }
            // 200ms = 5 FPS roughly
            await Task.Delay(200, stoppingToken);
        }
    }

    private string CaptureScreen()
    {
        try
        {
            Rectangle bounds = GetScreenBounds();
            using Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
            }

            using MemoryStream ms = new MemoryStream();
            // Compress as JPEG
            bitmap.Save(ms, ImageFormat.Jpeg);
            byte[] byteImage = ms.ToArray();
            return Convert.ToBase64String(byteImage); 
        }
        catch
        {
            return string.Empty;
        }
    }

    private Rectangle GetScreenBounds()
    {
        // Simple primary screen bounds
        // For multi-monitor, this needs expansion
        // System.Windows.Forms.Screen.PrimaryScreen.Bounds is typical but requires WinForms ref
        // Fallback to a fixed resolution or use native calls currently omitted for brevity
        // Using System.Drawing.Common, we might assume 1920x1080 or better logic
        return new Rectangle(0, 0, 1920, 1080); 
    }

    private string GetLocalIP()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        return host.AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork)?.ToString() ?? "127.0.0.1";
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_hubConnection != null)
        {
            await _hubConnection.DisposeAsync();
        }
        await base.StopAsync(cancellationToken);
    }
}
