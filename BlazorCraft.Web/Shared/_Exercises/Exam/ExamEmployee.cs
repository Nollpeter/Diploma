using System.ComponentModel.DataAnnotations;

namespace BlazorCraft.Web.Shared._Exercises.Exam;

public class ExamEmployee
{
    public int Id { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]public string LastName { get; set; }
    [Required]public Position? Position { get; set; }
    [Required]public Gender? Gender { get; set; }
    public string ProfilePicture { get; set; }
    [Required]
    [Range(10000,50000)]
    public int? Salary { get; set; }
    [Required]public DateTime? BirthDate { get; set; }
    [Required]public DateTime? HireDate { get; set; }
    public string  Address { get; set; }

    public ExamEmployee()
    {
    }

    public ExamEmployee(ExamEmployee employee)
    {
        Id = employee.Id;
        FirstName = employee.FirstName;
        LastName = employee.LastName;
        Position = employee.Position;
        Gender = employee.Gender;
        ProfilePicture = employee.ProfilePicture;
        Salary = employee.Salary;
        BirthDate = employee.BirthDate;
        HireDate = employee.HireDate;
        Address = employee.Address;
    }
}
// For simplicity's sake, let's agree on having only two genders.
public enum Gender
{
    Male,
    Female,
}

public enum Position
{
    CEO,
    CFO,
    CTO,
    Developer
}