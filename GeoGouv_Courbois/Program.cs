using GeoGouv_Courbois.Configurations;
using GeoGouv_Courbois.Helpers;
using GeoGouv_Courbois.Services.Interfaces;
using GeoGouv_Courbois.Services;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Ajout des services au service d'injection de dépendance
builder.Services.AddControllersWithViews();

//Récupération des datas de configuration pour la connexion à l'api GéoGouv et la connexion Sql
builder.Services.Configure<GeoGouvApiConf>(builder.Configuration.GetSection("GeoGouvApiConf"));
builder.Services.Configure<SqlConfiguration>(builder.Configuration.GetSection("SqlConfiguration"));

//Ajout du service dans l'injection de dépendance
builder.Services.AddHttpClient<ICommuneService, CommuneService>((config, client) =>
{
    var geoGouvApiConf = config.GetRequiredService<IOptions<GeoGouvApiConf>>().Value;

    client.BaseAddress = new Uri(geoGouvApiConf.DefaultUrl);
    client.Timeout = TimeSpan.FromSeconds(geoGouvApiConf.Timeout);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

builder.Services.AddScoped<ISqlService, SqlService>();
builder.Services.AddScoped<StringNormalizer>();


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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
