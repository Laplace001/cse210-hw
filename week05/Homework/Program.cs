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