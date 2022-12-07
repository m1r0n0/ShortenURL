using Microsoft.Extensions.Configuration;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Data;
using AutoMapper;
using System;
using BusinessLayer.DTOs;
using ShortenURL.Models;
using BusinessLayer.Services;
using ShortenURL.Controllers;
using BusinessLayer.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationContext>();
builder.Services.AddAutoMapper(typeof(AppMappingProfileForCreateLinkVM), typeof(AppMappingProfileForUseLinkVM), typeof(AppMappingProfileForMyLinksVM));
builder.Services.AddScoped<IShortenService,ShortenService>();
builder.Services.AddScoped<IRedirectService, RedirectService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseDeveloperExceptionPage();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "shorturl.com",
    pattern: "{controller=Redirect}/{action=DoRedirect}/{id?}");

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();

public class AppMappingProfileForCreateLinkVM : Profile
{
    public AppMappingProfileForCreateLinkVM()
    {
        CreateMap<LinkViewModelDTO, CreateLinkViewModel>().ReverseMap();
    }
}
public class AppMappingProfileForUseLinkVM : Profile
{
    public AppMappingProfileForUseLinkVM()
    {
        CreateMap<LinkViewModelDTO, UseLinkViewModel>().ReverseMap();
    }
}
public class AppMappingProfileForMyLinksVM : Profile
{
    public AppMappingProfileForMyLinksVM()
    {
        CreateMap<LinkViewModelDTO, MyLinksViewModel>().ReverseMap();
    }
}
