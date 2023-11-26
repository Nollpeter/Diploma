using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorCraft.Web;
using BlazorCraft.Web.Infrastructure;
using BlazorCraft.Web.Tests.Introduction;
using Blazored.LocalStorage;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddMudServices();
builder.Services.AddScoped<ITestRunnerService, TestRunnerService>();
builder.Services.AddTransient<Test_Components_Ex1_HelloWorld>();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<IPanelStateService, PanelStateService>();

await builder.Build().RunAsync();
