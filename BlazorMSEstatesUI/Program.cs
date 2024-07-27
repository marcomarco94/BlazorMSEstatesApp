using BlazorMSEstatesUI;
using Microsoft.AspNetCore.Rewrite;


var builder = WebApplication.CreateBuilder(args);
builder.ConfigureServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

var options = new RewriteOptions();
options.AddRedirectToHttps();
options.Rules.Add(new RedirectToWwwRule());
app.UseRewriter(options);

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();