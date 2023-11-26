﻿using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorCraft.Web;
using BlazorCraft.Web.DI;
using BlazorCraft.Web.Shared.Examples._7_DependencyInjection;
using BlazorCraft.Web.Tests.Introduction;
using Blazored.LocalStorage;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddMudServices();
builder.Services.AddBlazorCraftServices();
builder.Services.AddBlazoredLocalStorage();


await builder.Build().RunAsync();
