using MyPyramidWeb.Extensions;
using MyPyramidWeb.Models.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// URL с маленькой буквы
builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

builder.Services.AddServices();
builder.Services.AddHttpClient();

// Добавил свой конфиг-файл
builder.Configuration.AddJsonFile("appconfig.json", optional: false, reloadOnChange: true);

builder.Services.Configure<Dictionary<string, Dictionary<string, PyramidCredentialData>>>(
    builder.Configuration.GetSection("Pyramid:Credentials"));

builder.Services.Configure<Dictionary<string, SoapActionData>>(
    builder.Configuration.GetSection("Pyramid:SoapActions"));

builder.Services.Configure<AppInfoData>(builder.Configuration.GetSection("App:Info"));

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Pyramid/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseHostFiltering();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Pyramid}/{action=Index}/{id?}");

app.Run();