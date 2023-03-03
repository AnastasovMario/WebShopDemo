using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using WebShopDemo.Core.Constants;
using WebShopDemo.Core.Contracts;
using WebShopDemo.Core.Data;
using WebShopDemo.Core.Data.Common;
using WebShopDemo.Core.Data.Models.Account;
using WebShopDemo.Core.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("MSSQLConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();


//това е за логин
//Service configure ни помага да конфигурираме всичко по проекта.
//AddDefaultIdentity конфигурираме identity-то.
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
	options.SignIn.RequireConfirmedAccount = true;
	//най-важно е конфигурирането на паролата.
	options.Password.RequireNonAlphanumeric = true;
	options.Password.RequiredLength = 6;

	//Всеки user да има имейл
	options.User.RequireUniqueEmail = true;
	//Това, което го няма в тези символи, не може да го въведеш;
	//options.User.AllowedUserNameCharacters;

	//Колко локаута може да има
	options.Lockout.MaxFailedAccessAttempts = 5;
	//addRoles го екстендваме, когато искаме да добавим роли.
}).AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();

//ще ни върне към login страницата и ще ни подаде login url-то.
builder.Services.ConfigureApplicationCookie(options =>
{
	//Има hidden поленце, където се поства returnUrl-то
    options.LoginPath = "/Account/Login";
});
builder.Services.AddAuthorization(options =>
{
	//Искаме да добавим политика, която се казва "CanDeleteProduct", която очаква потребителят да е manager и supervisor
	options.AddPolicy("CanDeleteProduct", policy =>
		policy.RequireAssertion(context => 
		context.User.IsInRole(RoleConstants.Manager) && context.User.IsInRole(RoleConstants.Supervisor)));
});

builder.Services.AddControllersWithViews();

//Transient - всеки път дава различна инстанция
//Singleton - само 1 инстанция
//В рамките на request-a винаги е само една инстанция.
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IRepository, Repository>();

//добавяме cache
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
	//настройване на времето за сесията
	options.IdleTimeout = TimeSpan.FromMinutes(5);
	//хубаво е да го сетваме винаги на true;
	options.Cookie.HttpOnly = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
// Важен е реда, по който нареждаме всички методи.
if (app.Environment.IsDevelopment())
{
	app.UseMigrationsEndPoint();
}
else
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// преди аутентикации.
app.UseRouting();

//трябва да са след рутирането в този ред.
app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

//Default-ния се казва Index и всеки controller трябва да има такъв

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
