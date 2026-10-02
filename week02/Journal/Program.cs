using System;

namespace JournalProgram
{
    class Program
    {
        static void Main(string[] args)
        {
            Journal journal = new Journal();
            PromptGenerator promptGenerator = new PromptGenerator();

            string choice = "";
            // exceeding Requirements:
            // considered more than 5 prompts,automatic dates,
            // file error handling and a journal entry counter

        
            Console.WriteLine("========================================");
            Console.WriteLine("        WELCOME TO MY JOURNAL APP");
            Console.WriteLine("========================================");

            while (choice != "5")
            {
                Console.WriteLine("\nPlease choose one of the following choices:");
                Console.WriteLine("1. Write");
                Console.WriteLine("2. Display");
                Console.WriteLine("3. Load");
                Console.WriteLine("4. Save");
                Console.WriteLine("5. Quit");
                Console.Write("What would you like to do? ");

                choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        
                        string prompt = promptGenerator.GetRandomPrompt();

                        Console.WriteLine("\nPrompt:");
                        Console.WriteLine(prompt);

                        
                        Console.Write("\nYour response: ");
                        string response = Console.ReadLine();

                        
                        string date = DateTime.Now.ToShortDateString();

                        
                        Entry newEntry = new Entry
                        {
                            _date = date,
                            _promptText = prompt,
                            _entryText = response
                        };

                    
                        journal.AddEntry(newEntry);

                        Console.WriteLine("\nYour journal entry has been saved!");
                        break;

                    case "2":
                        
                        journal.DisplayAll();
                        break;

                    case "3":
                        
                        Console.Write("Enter the filename to load: ");
                        string loadFile = Console.ReadLine();

                        journal.LoadFromFile(loadFile);
                        break;

                    case "4":
                    
                        Console.Write("Enter the filename to save: ");
                        string saveFile = Console.ReadLine();

                        journal.SaveToFile(saveFile);
                        break;

                    case "5":
                        Console.WriteLine("\nThank you for using the Journal App!");
                        Console.WriteLine("Have a wonderful day!");
                        break;

                    default:
                        Console.WriteLine("\nInvalid choice. Please enter a number from 1 to 5.");
                        break;
                }
            }
        }
    }
}