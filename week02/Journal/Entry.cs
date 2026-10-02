using System;

namespace JournalProgram
{
    public class Entry
    {
        public string _date;
        public string _promptText;
        public string _entryText;

        
        public void Display()
        {
            Console.WriteLine($"Date: {_date}");
            Console.WriteLine($"Prompt: {_promptText}");
            Console.WriteLine($"Response: {_entryText}");
            Console.WriteLine("----------------------------------------");
        }

        
        public string ToFileString()
        {
            return $"{_date}|{_promptText}|{_entryText}";
        }

        
        public static Entry FromFileString(string line)
        {
            string[] parts = line.Split('|');

            Entry entry = new Entry();

            if (parts.Length >= 3)
            {
                entry._date = parts[0];
                entry._promptText = parts[1];
                entry._entryText = parts[2];
            }

            return entry;
        }
    }
}