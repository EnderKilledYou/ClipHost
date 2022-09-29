using BlazorQueue;
using Microsoft.AspNetCore.Http.Connections;
using ServiceStack.Configuration;
Licensing.RegisterLicense("TRIAL30WEB-e1JlZjpUUklBTDMwV0VCLE5hbWU6OS8yNy8yMDIyIDIyOTVkNjYwOTY3MzQxZjQ5MmY1Njc2OThiMTY5NWNjLFR5cGU6VHJpYWwsTWV0YTowLEhhc2g6cHF2WWJlWVdtb3JQSU56MEF0eHJ3MjFWTkZQQWtrSHVXT2hHRFAyQU1vNHlZN0kxeUJqZGN1RnJlUXVCTDlrbmxYQzRKdWg3T0lRS0k3L3E5elJiK014TlJnQ2g0ZjdEeFF2NVBtRTM4REU3VDNNbTBxaUxKZlVFdDYxTEQzNmxOR1dNK0w0TFNIYjZtU284UVhIL3ROYjZGaEEvVThQRnhEcWhRaWpzdEdZPSxFeHBpcnk6MjAyMi0xMC0yN30=");
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR().AddJsonProtocol(options => {
    options.PayloadSerializerOptions.PropertyNamingPolicy = null;
}); ;
builder.Services .AddSingleton<StreamerProcessWrangler>();
builder.Services.AddHostedService(a=>a.GetService<StreamerProcessWrangler>());

var app = builder.Build();
 
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseHttpsRedirection();
}
app.UseServiceStack(new AppHost());
app.MapHub<BlazorHub>("/BlazorHub", options =>
{
    options.Transports =
        HttpTransportType.WebSockets |
        HttpTransportType.LongPolling;
});
    
app.MapHub<ClipHub>("/ClipHub", options =>
{
options.Transports =
    HttpTransportType.WebSockets |
    HttpTransportType.LongPolling;
});

app.Run();