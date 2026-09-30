using System;

public class Assignment
{
    // Private member variables
    private string _studentName;
    private string _topic;

    // Constructor that sets the member variables
    public Assignment(string studentName, string topic)
    {
        _studentName = studentName;
        _topic = topic;
    }

    // Getter method for the student name (needed for derived classes later)
    public string GetStudentName()
    {
        return _studentName;
    }

    // Returns a summary containing student name and topic
    public string GetSummary()
    {
        return $"{_studentName} - {_topic}";
    }
}