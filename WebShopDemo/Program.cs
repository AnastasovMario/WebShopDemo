﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using WebShopDemo.Core.Contracts;
using WebShopDemo.Core.Data;
using WebShopDemo.Core.Data.Common;
using WebShopDemo.Core.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("MSSQLConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
	.AddEntityFrameworkStores<ApplicationDbContext>();
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
	options.Cookie.HttpOnly= true;
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
