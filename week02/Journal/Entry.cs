using System;

public class Entry
{
    // Member variables (abstraction - internal data is hidden)
    private string _prompt;
    private string _response;
    private string _date;

    // Constructor
    public Entry(string prompt, string response, string date)
    {
        _prompt = prompt;
        _response = response;
        _date = date;
    }

    // Method to get the prompt
    public string GetPrompt()
    {
        return _prompt;
    }

    // Method to get the response
    public string GetResponse()
    {
        return _response;
    }

    // Method to get the date
    public string GetDate()
    {
        return _date;
    }

    // Method to display the entry
    public void Display()
    {
        Console.WriteLine($"Date: {_date}");
        Console.WriteLine($"Prompt: {_prompt}");
        Console.WriteLine($"Response: {_response}");
        Console.WriteLine();
    }

    // Method to format entry for saving to file
    public string ToFileString()
    {
        // Use a delimiter that is unlikely to appear in the text
        return $"{_date}~|~{_prompt}~|~{_response}";
    }

    // Static method to parse entry from file line
    public static Entry FromFileString(string line)
    {
        string[] parts = line.Split(new string[] { "~|~" }, StringSplitOptions.None);
        if (parts.Length == 3)
        {
            return new Entry(parts[1], parts[2], parts[0]);
        }
        return null;
    }
}