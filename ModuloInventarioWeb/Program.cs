using Microsoft.AspNetCore.Authentication.Cookies;
using ModuloInventarioWeb.Data;
using ModuloInventarioWeb.DbAccess;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<ISqlDataAccess, SqlDataAccess>();
builder.Services.AddSingleton<ICategoryData, CategoryData>();
builder.Services.AddSingleton<IDetalleMovimientoData, DetalleMovimientoData>();
builder.Services.AddSingleton<IMovimientoData, MovimientoData>();
builder.Services.AddSingleton<ICategoriaData, CategoriaData>();
builder.Services.AddSingleton<IRolData, RolData>();
builder.Services.AddSingleton<IUsuarioData, UsuarioData>();
builder.Services.AddSingleton<IProductoData, ProductoData>();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option =>
    {
        option.LoginPath = "/Autenticacion/Index";
        option.ExpireTimeSpan = TimeSpan.FromMinutes(15);
        option.AccessDeniedPath = "/Autentication/Index";
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Autenticacion}/{action=Index}/{id?}");

app.Run();

