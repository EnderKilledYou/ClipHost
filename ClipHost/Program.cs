using BlazorQueue;
using Microsoft.AspNetCore.Http.Connections;
 

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR().AddJsonProtocol(options => {
    options.PayloadSerializerOptions.PropertyNamingPolicy = null;
}); ;
builder.Services .AddHostedService<ClipProcessWrangler>();
builder.Services .AddSingleton<ClipIdManager>();
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

app.Run();