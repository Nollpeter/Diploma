using System.Reflection;
using BlazorCraft.Web.Infrastructure.Attributes;
using BlazorCraft.Web.Shared._Exercises.RehderFragments;
using BlazorCraft.Web.Tests.Routing;
using Microsoft.AspNetCore.Components;

namespace BlazorCraft.Web.Tests.RenderFragments;

public class RenderFragmentsTestBase<TTestComponent> : ComponentTestBase<TTestComponent>
    where TTestComponent : ComponentBase
{
    protected void ValidateRenderFragmentExists(object renderFragmentsEx1, string fragmentName,
        Type? renderFragmentType = null)
    {
        var renderFragment = renderFragmentsEx1.GetType().GetProperties()
            .FirstOrDefault(p =>
                p.Name == fragmentName && (p.PropertyType == (renderFragmentType ?? typeof(RenderFragment))));
        if (renderFragment == null)
        {
            throw new TestRunException($"The RenderFragment {fragmentName} is not defined in the component");
        }
    }

    public void AssertCorrectMarkup(string expected, string actual)
    {
        if (actual != expected)
        {
            throw new TestRunException(
                $"The component does not render correctly. Expected markup: {expected}, Actual markup: {actual}");
        }
    }

    public RenderFragment CreateFragment(string titleContent, string tag)
    {
        var titleFragment = new RenderFragment(builder =>
        {
            builder.OpenElement(0, tag);
            builder.AddContent(1, titleContent);
            builder.CloseElement();
        });
        return titleFragment;
    }
}