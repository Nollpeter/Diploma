using BlazorCraft.Web.Infrastructure.Attributes;
using BlazorCraft.Web.Shared._Exercises.Exam;
using Bunit;

namespace BlazorCraft.Web.Tests.Exam;

[TestForPage(typeof(Pages._11_Exam.Exam))]
public class Test_Exam_EmployeeDetails : ExamTestBase<ExamEmployeeDetails>
{
    protected override TestContext SetupTestContext()
    {
        var setupTestContext = base.SetupTestContext();
        setupTestContext.Services.AddExamEmployeeService();
        return setupTestContext;
    }
    
     //[Precondition] EmployeeForm component is used in markup
//Preconditon: EmployeeForm is declared
     [ComponentUsedInMarkupTitle(typeof(EmployeeForm))]
     [ComponentUsedInMarkupDescription(typeof(EmployeeForm))]
     [Precondition]
     public async Task GivenEmployeeDetails_WhenRendered_ThenHasEmployeeFormDeclared()
     {
         var ctx = SetupTestContext();
         ExamEmployee employee = new ExamEmployee()
         {
             Id = 10,
         };
         var renderedComponent = ctx.RenderComponent<ExamEmployeeDetails>(
             builder => builder.Add(form => form.EmployeeId, employee.Id));
         var hasComponent = renderedComponent.HasComponent<EmployeeForm>();
         if (!hasComponent)
         {
             throw new TestRunException($"The component has no {nameof(EmployeeForm)} component declared");
         }
     }
     
     // Employee is bound to EmployeeForm
     //PRe: Employee of the Form is the Employee set as parameter
     [Title(nameof(EmployeeForm) +" Employee is bound to " + nameof(IExamEmployeeService) + " Employee")]
     //TODO Description
     [Description("")]
     public async Task GivenEmployeeDetails_WhenRendered_ThenEmployeeFormEmployeeBoundToEmployeeDetailsEmployee()
     {
         var ctx = SetupTestContext();

         ExamEmployee employee = new ExamEmployee()
         {
             Id = 10,
         };
         var renderedComponent = ctx.RenderComponent<ExamEmployeeDetails>(
             builder => builder.Add(form => form.EmployeeId, employee.Id));
         var findComponent = renderedComponent.FindComponent<EmployeeForm>();
         var instanceEmployee = findComponent.Instance.Employee;
         if (!ReferenceEquals(employee, instanceEmployee))
         {
             throw new TestRunException($"Employee of the {nameof(EmployeeForm)} is not bound to the Employee of the {nameof(ExamEmployeeDetails)}");
         }
     }

       


         // EmployeeForm is NOT Editable on render
// PRe: Employee form iseditmode = true
         [Title(nameof(EmployeeForm) + " is NOT Editable on render")]
         //TODO Description
         [Description("")]
         public async Task GivenEmployeeDetails_WhenRendered_ThenEmployeeFormIsNotEditable()
         {
             var ctx = SetupTestContext();

             ExamEmployee employee = new ExamEmployee()
             {
                 Id = 10,
             };
             var renderedComponent = ctx.RenderComponent<ExamEmployeeDetails>(
                 builder => builder.Add(form => form.EmployeeId, employee.Id));
             var findComponent = renderedComponent.FindComponent<EmployeeForm>();
             var isEditMode = findComponent.Instance.IsEditMode;
             if (!isEditMode)
             {
                 throw new TestRunException($"{nameof(EmployeeForm)} is not editable after render!");
             }
         }
         // Edit switches editmode
         [Title("Clicking Edit button makes employee form editable")]
        //TODO Description
         [Description("")]
         public async Task GivenEmployeeDetails_WhenEditButtonClick_ThenEmployeeFormIsEditable()
         {
             var ctx = SetupTestContext();

             ExamEmployee employee = new ExamEmployee()
             {
                 Id = 10,
             };
             var renderedComponent = ctx.RenderComponent<ExamEmployeeDetails>(
                 builder => builder.Add(form => form.EmployeeId, employee.Id));
             var findComponent = renderedComponent.FindComponent<EmployeeForm>();
             var isEditMode = findComponent.Instance.IsEditMode;
             if (!isEditMode)
             {
                 throw new TestRunException($"{nameof(EmployeeForm)} is not editable after render!");
             }
         }
         
         
        // Employee added if employee is valid on EmployeeForm then new employee is added to EmployeeService
        // Edit switches editmode
        [Title("Saving employee form updates EmployeeService employee")]
        //TODO Description
        [Description("")]
        public async Task GivenEmployeeDetails_WhenEditButtonClick_ThenEmployeeFormIsEditable()
        {
            var ctx = SetupTestContext();
            var examEmployeeService = ctx.Services.GetRequiredService<IExamEmployeeService>();
            var employee = await examEmployeeService.GetEmployee(1);
            var renderedComponent = ctx.RenderComponent<ExamEmployeeDetails>(
                builder => builder.Add(form => form.EmployeeId, employee.Id));
            var findComponent = renderedComponent.FindComponent<EmployeeForm>();
            
            
        }

         //Closed event invoked if closed is pressed
    
    // Edit not visible if editmode = true
    
    // Picutre load invoked
    // Picture load callback invoked
    // Delete deletes employee
    
}