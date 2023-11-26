using BlazorCraft.Web.Infrastructure.Attributes;
using BlazorCraft.Web.Shared._Exercises._1_Components._5_RenderFragments;
using BlazorCraft.Web.Tests.Introduction;
using Bunit;
using Employee = BlazorCraft.Web.Shared._Exercises._1_Components._5_RenderFragments.RenderFragments_LessonFinal.Employee;

namespace BlazorCraft.Web.Tests.RenderFragments;

[TestForPage(typeof(Pages._4_RenderFragments.RenderFragments))]
public class Test_RenderFragments_ExFinal : RenderFragmentsTestBase<RenderFragments_LessonFinal>
{
    public const string EmployeesParamName = "Employees";
    public const string ContainerName = ".ex-final-container";
    
    [Title(EmployeesParamName + " parameted is defined")]
    [Description("This test verifies that the "+EmployeesParamName+" parameter is defined in the component")]
    public async Task Test1()
    {
        var component = new RenderFragments_LessonFinal();
        ValidateComponentProperty(component, EmployeesParamName, typeof(List<Employee>));
        
    }
    
    [Title("List view renders properly")]
    [Description("This test validates if the list view for the component renders properly")]
    public async Task Test2()
    {
        var component = new RenderFragments_LessonFinal();
        ValidateComponentProperty(component, EmployeesParamName, typeof(List<Employee>));

        TestContext testContext = new TestContext();

        var r = new Random();
        List<Employee> employees = new List<Employee>()
        {
            new Employee(r.Next(1000), $"test_{r.Next(1000)}",$"test_{r.Next(1000)}",$"test_{r.Next(1000)}"),
            new Employee(r.Next(1000), $"test_{r.Next(1000)}",$"test_{r.Next(1000)}",$"test_{r.Next(1000)}")
        };

        var renderedComponent = testContext.RenderComponent<RenderFragments_LessonFinal>(ComponentParameter.CreateParameter(EmployeesParamName, employees));

        var element = renderedComponent.Find(ContainerName).InnerHtml;
        element.MarkupMatches("<h2>Employees</h2>" +
                              "<ul>" +
                              $"<li><span>Id: {employees[0].Id}</span><span>FirstName: {employees[0].FirstName}</span><span>LastName: {employees[0].LastName}</span><span>Position: {employees[0].Position}</span></li>"+
                              $"<li><span>Id: {employees[1].Id}</span><span>FirstName: {employees[1].FirstName}</span><span>LastName: {employees[1].LastName}</span><span>Position: {employees[1].Position}</span></li>"+
                              "</ul>"
            );


        
    }
    
    [Title("Table view renders properly")]
    [Description("This test validates if the table view for the component renders properly")]
    public async Task Test3()
    {
        var component = new RenderFragments_LessonFinal();
        ValidateComponentProperty(component, EmployeesParamName, typeof(List<Employee>));

        TestContext testContext = new TestContext();

        var r = new Random();
        List<Employee> employees = new List<Employee>()
        {
            new Employee(r.Next(1000), $"test_{r.Next(1000)}",$"test_{r.Next(1000)}",$"test_{r.Next(1000)}"),
            new Employee(r.Next(1000), $"test_{r.Next(1000)}",$"test_{r.Next(1000)}",$"test_{r.Next(1000)}")
        };

        var renderedComponent = testContext.RenderComponent<RenderFragments_LessonFinal>(ComponentParameter.CreateParameter(EmployeesParamName, employees));
        renderedComponent.Find(".table-view").Click();
        
        var element = renderedComponent.Find(ContainerName).InnerHtml;
        element.MarkupMatches("<h2>Employees</h2>" +
                              """<table class="table">""" +
                              $"<thead><tr><th>{nameof(Employee.Id)}</th><th>{nameof(Employee.FirstName)}</th><th>{nameof(Employee.LastName)}</th><th>{nameof(Employee.Position)}</th></tr></thead>"+
                              "<tbody>"+
                              $"<tr><td>{employees[0].Id}</td><td>{employees[0].FirstName}</td><td>{employees[0].LastName}</td><td>{employees[0].Position}</td></tr>"+
                              $"<tr><td>{employees[1].Id}</td><td>{employees[1].FirstName}</td><td>{employees[1].LastName}</td><td>{employees[1].Position}</td></li>"+
                              "</tbody>"+
                              "</table>"
        );
    }
    
}