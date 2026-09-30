using System;

// Inherits from the base Assignment class
public class WritingAssignment : Assignment
{
    // Private member variable specific to WritingAssignment
    private string _title;

    // Constructor accepting three parameters
    // Calls the base class constructor to set studentName and topic
    public WritingAssignment(string studentName, string topic, string title)
        : base(studentName, topic)
    {
        _title = title;
    }

    // Accesses the private _studentName using the public GetStudentName() getter from Assignment
    public string GetWritingInformation()
    {
        string studentName = GetStudentName();
        return $"{_title} by {studentName}";
    }
}