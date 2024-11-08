using Authentication.Web.Options;

using Microsoft.AspNetCore.Authentication.Cookies;

using Zitadel.Authentication;
using Zitadel.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ZitadelOptions>(builder.Configuration.GetSection("Zitadel"));

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = ZitadelDefaults.AuthenticationScheme;
})
.AddCookie()
.AddZitadel(options => 
{
    var zitadelSettings = builder.Configuration
    .GetRequiredSection(ZitadelOptions.Section)
    .Get<ZitadelOptions>();

    options.ClientId = zitadelSettings!.ClientId;
    options.ClientSecret = zitadelSettings.ClientSecret;
    options.Authority = zitadelSettings.BaseUri;
    options.RequireHttpsMetadata = false;
});
//.AddZitadel(builder.Configuration);

// .AddOpenIdConnect(options => ConfigureOidc(options, builder.Configuration));

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");

app.Run();