using System.Net.WebSockets;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

var httpUrl = @"http://127.0.0.1:8008/";
var webSocketUrl = @"ws://localhost:8008/";

var hostBuilder = new WebHostBuilder()
	.UseKestrel()
	.UseUrls(httpUrl);

hostBuilder.Configure(app =>
{
	app.UseWebSockets();
	app.Run(async context =>
	{
		if (!context.WebSockets.IsWebSocketRequest)
		{
			await context.Response.WriteAsync("Not WebSocketRequest");
		}
		else
		{
			try
			{
				await context.WebSockets.AcceptWebSocketAsync();
				Console.WriteLine("WebSocket connection accepted");
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}
	});
});

var host = hostBuilder.Build();
host.Start();

var httpClient = new HttpClient();
var httpResponseMessage = await httpClient.GetAsync(httpUrl);
var httpResponseMessageString = await httpResponseMessage.Content.ReadAsStringAsync();
Console.WriteLine(httpResponseMessageString);

var webSocketClient = new ClientWebSocket();
var token = new CancellationToken();
try
{
	await webSocketClient.ConnectAsync(new Uri(webSocketUrl), token);
}
catch (Exception e)
{
	Console.WriteLine(e);
	throw;
}