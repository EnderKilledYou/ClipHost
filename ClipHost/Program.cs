using BlazorQueue;
using Microsoft.AspNetCore.Http.Connections;
using ServiceStack.Configuration;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR().AddJsonProtocol(options => {
    options.PayloadSerializerOptions.PropertyNamingPolicy = null;
}); ;
builder.Services .AddSingleton<StreamerProcessWrangler>();
 
 
var app = builder.Build();

// Configure the HTTP request pipeline.
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
var monitorLoop = app.Services.GetRequiredService<StreamerProcessWrangler>();
await monitorLoop.StartAsync(default);
app.Run();