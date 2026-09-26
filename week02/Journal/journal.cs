using System;
using System.Collections.Generic;
using System.IO;

public class Journal
{
    
    private List<Entry> _entries = new List<Entry>();
    private List<string> _prompts = new List<string>()
    {
        "Who was the most interesting person I interacted with today?",
        "What was the best part of my day?",
        "How did I see the hand of the Lord in my life today?",
        "What was the strongest emotion I felt today?",
        "If I had one thing I could do over today, what would it be?",
        "What is something new I learned today?",
        "What am I most grateful for right now?",
        "What challenge did I overcome today, and how did I do it?"
    };

    private Random _random = new Random();

    
    public void WriteNewEntry()
    {
        string prompt = GetRandomPrompt();
        Console.WriteLine($"\n{prompt}");
        Console.Write("> ");
        string response = Console.ReadLine();
        string date = DateTime.Now.ToShortDateString();

        Entry newEntry = new Entry(prompt, response, date);
        _entries.Add(newEntry);
    }


    private string GetRandomPrompt()
    {
        int index = _random.Next(_prompts.Count);
        return _prompts[index];
    }

    
    public void DisplayJournal()
    {
        if (_entries.Count == 0)
        {
            Console.WriteLine("\nThe journal is empty.");
            return;
        }

        Console.WriteLine();
        foreach (Entry entry in _entries)
        {
            entry.Display();
        }
    }

    
    public void SaveJournal()
    {
        Console.Write("What is the filename? ");
        string filename = Console.ReadLine();

        try
        {
            using (StreamWriter writer = new StreamWriter(filename))
            {
                foreach (Entry entry in _entries)
                {
                    writer.WriteLine(entry.ToFileString());
                }
            }
            Console.WriteLine("Journal saved successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving journal: {ex.Message}");
        }
    }

    
    public void LoadJournal()
    {
        Console.Write("What is the filename? ");
        string filename = Console.ReadLine();

        try
        {
            if (!File.Exists(filename))
            {
                Console.WriteLine("File not found.");
                return;
            }

    
            _entries.Clear();

            string[] lines = File.ReadAllLines(filename);
            foreach (string line in lines)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    Entry entry = Entry.FromFileString(line);
                    if (entry != null)
                    {
                        _entries.Add(entry);
                    }
                }
            }
            Console.WriteLine("Journal loaded successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading journal: {ex.Message}");
        }
    }
}
