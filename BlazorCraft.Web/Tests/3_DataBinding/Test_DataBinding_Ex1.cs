using BlazorCraft.Web.Infrastructure.Attributes;
using BlazorCraft.Web.Pages._3_DataBinding;
using BlazorCraft.Web.Shared._Exercises._3_DataBinding;
using Bunit;
using Microsoft.AspNetCore.Components;

namespace BlazorCraft.Web.Tests._3_DataBinding;



[TestForPage(typeof(ComponentDataBinding))]
public class Test_DataBinding_Ex1 : ComponentTestBase<DataBinding_Ex1>
{
    public const string EmployeeFirstNameParamName = "FirstName";
    public const string EmployeeFirstNameChangedName = "FirstNameChanged";

    public const string EmployeeLastNameParamName = "LastName";
    public const string EmployeeLastNameChangedName = "LastNameChanged";
    
    [ParameterDefinedTitle(EmployeeFirstNameParamName)]
    [ParameterDefinedDescription(EmployeeFirstNameParamName, typeof(string))]
    [Precondition]
    public async Task GivenComponentDataBinding_Ex1_WhenDeclared_ThenEmployeeFirstNameParameterDefined()
    {
        var component = new DataBinding_Ex1();
        ValidateComponentProperty(component, EmployeeFirstNameParamName, typeof(string));
        
    }
    
    [ParameterDefinedTitle(EmployeeLastNameParamName)]
    [ParameterDefinedDescription(EmployeeLastNameParamName, typeof(string))]
    [Precondition]
    public async Task GivenComponentDataBinding_Ex1_WhenDeclared_ThenEmployeeLastNameParameterDefined()
    {
        var component = new DataBinding_Ex1();
        ValidateComponentProperty(component, EmployeeLastNameParamName, typeof(string));
        
    }
    
    [ParameterDefinedTitle(EmployeeFirstNameChangedName)]
    [ParameterDefinedDescription(EmployeeFirstNameChangedName, typeof(EventCallback<string>))]
    [Precondition]
    public async Task GivenComponentDataBinding_Ex1_WhenDeclared_ThenEmployeeFirstNameChangedParameterDefined()
    {
        var component = new DataBinding_Ex1();
        ValidateComponentProperty(component, EmployeeFirstNameChangedName, typeof(EventCallback<string>));
        
    }
    
    [ParameterDefinedTitle(EmployeeLastNameChangedName)]
    [ParameterDefinedDescription(EmployeeLastNameChangedName, typeof(EventCallback<string>))]
    [Precondition]
    public async Task GivenComponentDataBinding_Ex1_WhenDeclared_ThenEmployeeLastNameChangedParameterDefined()
    {
        var component = new DataBinding_Ex1();
        ValidateComponentProperty(component, EmployeeLastNameChangedName, typeof(EventCallback<string>));
        
    }

    [Title(EmployeeFirstNameParamName + " binding Consumer -> Component")]
    [Description("This test verifies that once the Consumer component changes the " + EmployeeFirstNameParamName +
                 " it is reflected in the component ")]
    public async Task GivenEmployeeFirstNameParamNameBinding_WhenConsumerChanges_ThenItIsReflectedInComponent()
    {
        TestContext testContext = new TestContext();

        string firstName = "Theodore";

        var renderedComponent = testContext.RenderComponent<DataBinding_Ex1>(
            ComponentParameter.CreateParameter(EmployeeFirstNameParamName, firstName));

        var inputs = renderedComponent.FindAll("input").ToList();

        var idValue = inputs[0].GetAttribute("value");
        if (idValue != firstName.ToString())
        {
            throw new TestRunException(
                $"{EmployeeFirstNameParamName} is not bound properly, its value should be {firstName}, but it is instead {idValue}");
        }
    }

    [Title(EmployeeFirstNameParamName + " binding Component -> Consumer")]
    [Description("This test verifies that once the Component changes the " + EmployeeFirstNameParamName +
                 " it is reflected in the Consumer component ")]
    public async Task GivenEmployeeFirstNameParamNameBinding_WhenComponentChanges_ThenItIsReflectedInConsumerComponent()
    {
        TestContext testContext = new TestContext();

        string firstName = "Theodore";

        EventCallback<string> idChanged = EventCallback.Factory.Create<string>(this, value =>
        {
            firstName = value;
        });

        var renderedComponent = testContext.RenderComponent<DataBinding_Ex1>(
            ComponentParameter.CreateParameter(EmployeeFirstNameParamName, firstName),
            ComponentParameter.CreateParameter(EmployeeFirstNameChangedName, idChanged)
        );
        
        var inputs = renderedComponent.FindAll("input").ToList();
        
        var idValue = inputs[0].GetAttribute("value");
        if (idValue != firstName.ToString())
        {
            throw new TestRunException(
                $"{EmployeeFirstNameParamName} is not bound properly, its value should be {firstName}, but it is instead {idValue}");
        }
        
        var inputText = renderedComponent.FindAll("input");
        var changedValue = "Changed";
        
        await inputText[0].InputAsync(new ChangeEventArgs(){Value = changedValue});
        if (firstName != changedValue)
        {
            throw new TestRunException(
                $"{EmployeeLastNameParamName} is not bound two way. Upon changing the value of the input, the change is not reflected. Are you calling NameChanged?");
        }
        
    }
    
    [Title(EmployeeLastNameParamName + " binding Consumer -> Component")]
    [Description("This test verifies that once the Consumer component changes the " + EmployeeLastNameParamName +
                 " it is reflected in the component ")]
    public async Task GivenEmployeeLastNameParamNameBinding_WhenConsumerChanges_ThenItIsReflectedInComponent()
    {
        TestContext testContext = new TestContext();

        string lastName = "Test";

        var renderedComponent = testContext.RenderComponent<DataBinding_Ex1>(
            ComponentParameter.CreateParameter(EmployeeLastNameParamName, lastName));

        var input = renderedComponent.Find(".employee-last-name");

        var nameValue = input.GetAttribute("value");
        if (nameValue != lastName)
        {
            throw new TestRunException(
                $"{EmployeeLastNameParamName} is not bound properly, its value should be {lastName}, but it is instead {nameValue}");
        }
    }

    [Title(EmployeeLastNameParamName + " binding Component -> Consumer")]
    [Description("This test verifies that once the Component changes the " + EmployeeLastNameParamName +
                 " it is reflected in the Consumer component ")]
    public async Task GivenEmployeeLastNameParamNameBinding_WhenComponentChanges_ThenItIsReflectedInConsumerComponent()
    {
        TestContext testContext = new TestContext();

        string lastName = "Test";

        EventCallback<string> nameChanged = EventCallback.Factory.Create<string>(this, n =>
        {
            lastName = n;
        });

        var renderedComponent = testContext.RenderComponent<DataBinding_Ex1>(
            ComponentParameter.CreateParameter(EmployeeLastNameParamName, lastName),
            ComponentParameter.CreateParameter(EmployeeLastNameChangedName, nameChanged)
        );

        var input = renderedComponent.Find(".employee-last-name");
        
        var changedValue = "Changed value";
        await input.InputAsync(new ChangeEventArgs(){Value = changedValue});

        if (lastName != changedValue)
        {
            throw new TestRunException(
                $"{EmployeeLastNameParamName} is not bound two way. Upon changing the value of the input, the change is not reflected. Are you calling NameChanged?");
        }
    }

}