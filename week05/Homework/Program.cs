using System;

class Program
{
    static void Main(string[] args)
    {
        // Test the base Assignment class
        Console.WriteLine("=== Base Assignment Test ===");
        Assignment assignment = new Assignment("Samuel Bennett", "Multiplication");
        Console.WriteLine(assignment.GetSummary());
        Console.WriteLine();

        // Test the MathAssignment class
        Console.WriteLine("=== Math Assignment Test ===");
        MathAssignment mathAssignment = new MathAssignment(
            "Roberto Rodriguez",
            "Fractions",
            "7.3",
            "8-19"
        );
        Console.WriteLine(mathAssignment.GetSummary());
        Console.WriteLine(mathAssignment.GetHomeworkList());
        Console.WriteLine();

        // Test the WritingAssignment class
        Console.WriteLine("=== Writing Assignment Test ===");
        WritingAssignment writingAssignment = new WritingAssignment(
            "Mary Waters",
            "European History",
            "The Causes of World War II"
        );
        Console.WriteLine(writingAssignment.GetSummary());
        Console.WriteLine(writingAssignment.GetWritingInformation());
    }
}

// Base Assignment Class
public class Assignment
{
    private string _studentName;
    private string _topic;

    public Assignment(string studentName, string topic)
    {
        _studentName = studentName;
        _topic = topic;
    }

    public string GetStudentName()
    {
        return _studentName;
    }

    public string GetSummary()
    {
        return $"{_studentName} - {_topic}";
    }
}

// Derived MathAssignment Class
public class MathAssignment : Assignment
{
    private string _textbookSection;
    private string _problems;

    public MathAssignment(string studentName, string topic, string textbookSection, string problems)
        : base(studentName, topic)
    {
        _textbookSection = textbookSection;
        _problems = problems;
    }

    public string GetHomeworkList()
    {
        return $"Section {_textbookSection} Problems {_problems}";
    }
}

// Derived WritingAssignment Class
public class WritingAssignment : Assignment
{
    private string _title;

    public WritingAssignment(string studentName, string topic, string title)
        : base(studentName, topic)
    {
        _title = title;
    }

    public string GetWritingInformation()
    {
        return $"{_title} by {GetStudentName()}";
    }
}