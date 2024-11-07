using ExamService.Models;
using JetBrains.Annotations;
using Xunit;

namespace ExamService.Tests.Unit.Models;

[TestSubject(typeof(BookedStudent))]
public class BookedStudentTest
{
    [Fact]
    public void Should_be_equals()
    {
        var student1 = new BookedStudent("1", "1");
        var student2 = new BookedStudent("1", "1");
        var equals = student1.Equals(student2);
        Assert.True(equals);
    }
    
    [Fact]
    public void Should_not_be_equals()
    {
        var student1 = new BookedStudent("1", "1");
        var student2 = new BookedStudent("2", "2");
        var equals = student1.Equals(student2);
        Assert.False(equals);
    }
    
    [Fact]
    public void Should_have_same_hash_code()
    {
        var student1 = new BookedStudent("1", "1");
        var student2 = new BookedStudent("1", "1");
        Assert.Equal(student1.GetHashCode(), student2.GetHashCode());
    }
    
    [Fact]
    public void Should_not_have_same_hash_code()
    {
        var student1 = new BookedStudent("1", "1");
        var student2 = new BookedStudent("2", "2");
        Assert.NotEqual(student1.GetHashCode(), student2.GetHashCode());
    }
    
    [Fact]
    public void Should_compare_to()
    {
        var student1 = new BookedStudent("1", "1");
        var student2 = new BookedStudent("2", "2");
        Assert.Equal(-1, student1.CompareTo(student2));
    }
    
    [Fact]
    public void Should_not_compare_to()
    {
        var student1 = new BookedStudent("1", "1");
        var student2 = new BookedStudent("1", "1");
        Assert.Equal(0, student1.CompareTo(student2));
    }
    
    [Fact]
    public void Should_not_compare_to_null()
    {
        var student1 = new BookedStudent("1", "1");
        Assert.Equal(1, student1.CompareTo(null));
    }
    
}