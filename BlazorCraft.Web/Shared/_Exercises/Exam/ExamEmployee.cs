using System.ComponentModel.DataAnnotations;

namespace BlazorCraft.Web.Shared._Exercises.Exam;

public class ExamEmployee
{
    public int Id { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]public string LastName { get; set; }
    [Required]public EmployeePosition? Position { get; set; }
    [Required]public EmployeeGender? Gender { get; set; }
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

    protected bool Equals(ExamEmployee other)
    {
        return Id == other.Id &&
               string.Equals(FirstName, other.FirstName, StringComparison.Ordinal) &&
               string.Equals(LastName, other.LastName, StringComparison.Ordinal) &&
               Equals(Position, other.Position) &&
               Equals(Gender, other.Gender) &&
               Equals(Salary, other.Salary) &&
               Nullable.Equals(BirthDate, other.BirthDate) &&
               Nullable.Equals(HireDate, other.HireDate) &&
               string.Equals(Address, other.Address, StringComparison.Ordinal);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((ExamEmployee)obj);
    }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(Id);
        hashCode.Add(FirstName);
        hashCode.Add(LastName);
        hashCode.Add(Position);
        hashCode.Add(Gender);
        hashCode.Add(ProfilePicture);
        hashCode.Add(Salary);
        hashCode.Add(BirthDate);
        hashCode.Add(HireDate);
        hashCode.Add(Address);
        return hashCode.ToHashCode();
    }

    public static bool operator ==(ExamEmployee? left, ExamEmployee? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(ExamEmployee? left, ExamEmployee? right)
    {
        return !Equals(left, right);
    }
}
// For simplicity's sake, let's agree on having only two genders.
public enum EmployeeGender
{
    Female,
    Male,
}

public enum EmployeePosition
{
    CEO,
    CFO,
    CTO,
    Developer
}