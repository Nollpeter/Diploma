using Bunit;
using Microsoft.AspNetCore.Components;

namespace BlazorCraft.Web.Tests.Routing;

public class ComponentTestBase<TTestComponent> : TestContext where TTestComponent : ComponentBase{}