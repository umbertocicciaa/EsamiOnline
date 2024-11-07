namespace ExamService.Models;

public class BookedStudent(string studentId, string govId) : IComparable<BookedStudent>
{
    public string StudentId { get; } = studentId;
    public string GovId { get; } = govId;

    public int CompareTo(BookedStudent? other)
    {
        return other == null ? 1 : string.Compare(StudentId, other.StudentId, StringComparison.Ordinal);
    }

    public override bool Equals(object? obj)
    {
        if (obj == null) return false;
        if (obj.GetType() != GetType()) return false;
        var other = (BookedStudent)obj;
        return StudentId == other.StudentId && GovId == other.GovId;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(StudentId, GovId);
    }
}