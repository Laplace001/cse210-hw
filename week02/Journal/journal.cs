using System;
using System.Collections.Generic;
using System.IO;

namespace JournalProgram
{
    public class Journal
    {
        public List<Entry> _entries = new List<Entry>();

        
        public void AddEntry(Entry newEntry)
        {
            _entries.Add(newEntry);
        }

        
        public void DisplayAll()
        {
            if (_entries.Count == 0)
            {
                Console.WriteLine("There are no journal entries to display.");
                return;
            }

            Console.WriteLine("\n========== YOUR JOURNAL ==========\n");

            foreach (Entry entry in _entries)
            {
                entry.Display();
            }
        }

        
        public void SaveToFile(string filename)
        {
            try
            {
                using (StreamWriter outputFile = new StreamWriter(filename))
                {
                    foreach (Entry entry in _entries)
                    {
                        outputFile.WriteLine(entry.ToFileString());
                    }
                }

                Console.WriteLine("Journal saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving journal: {ex.Message}");
            }
        }

        
        public void LoadFromFile(string filename)
        {
            try
            {
                if (!File.Exists(filename))
                {
                    Console.WriteLine("The specified file does not exist.");
                    return;
                }

                
                _entries.Clear();

                string[] lines = File.ReadAllLines(filename);

                foreach (string line in lines)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        Entry entry = Entry.FromFileString(line);
                        _entries.Add(entry);
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
}