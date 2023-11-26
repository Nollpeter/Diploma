using BlazorCraft.Web.Infrastructure.Attributes;
using BlazorCraft.Web.Shared._Exercises.Exam;

namespace BlazorCraft.Web.Tests.Exam;

[TestForPage(typeof(Pages._11_Exam.Exam))]
public class Test_Exam_PuttingItTogether : ExamTestBase<Exercise_Exam>
{
    // EmployeeEdit Used
    // EmployeeCreate Used
    // EmployeeEdit opened with Employee bound on Clicking Details
    // Employees are loaded on render
    // Table is rendered properly
    
    // On EmployeeEdit Close, List is refreshed
    // ON EmployeeCreate Close, List is refreshed
    
    //E2E tests
    //Create new employee
    //Edit employee
    //Delete employee
}