using System;

namespace EternalQuest
{
    class Program
    {
        static void Main(string[] args)
        {
            GoalManager manager = new GoalManager();

            string choice = "";

            Console.WriteLine("=================================");
            Console.WriteLine("       ETERNAL QUEST");
            Console.WriteLine("=================================");
            Console.WriteLine("Welcome to your personal goal tracker!");

            while (choice != "8")
            {
                Console.WriteLine("\n-------------------------------");
                Console.WriteLine("Menu:");
                Console.WriteLine("1. Create Simple Goal");
                Console.WriteLine("2. Create Eternal Goal");
                Console.WriteLine("3. Create Checklist Goal");
                Console.WriteLine("4. Record Goal Event");
                Console.WriteLine("5. Display Goals");
                Console.WriteLine("6. Display Score");
                Console.WriteLine("7. Save / Load Goals");
                Console.WriteLine("8. Quit");
                Console.WriteLine("-------------------------------");

                Console.Write("Select a choice: ");
                choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        manager.CreateSimpleGoal();
                        break;

                    case "2":
                        manager.CreateEternalGoal();
                        break;

                    case "3":
                        manager.CreateChecklistGoal();
                        break;

                    case "4":
                        manager.RecordEvent();
                        break;

                    case "5":
                        manager.DisplayGoals();
                        break;

                    case "6":
                        manager.DisplayScore();
                        break;

                    case "7":
                        SaveLoadMenu(manager);
                        break;

                    case "8":
                        Console.WriteLine("\nThank you for using Eternal Quest!");
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        static void SaveLoadMenu(GoalManager manager)
        {
            string choice = "";

            while (choice != "3")
            {
                Console.WriteLine("\n--- Save / Load Menu ---");
                Console.WriteLine("1. Save Goals");
                Console.WriteLine("2. Load Goals");
                Console.WriteLine("3. Return to Main Menu");

                Console.Write("Select a choice: ");
                choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        manager.SaveGoals();
                        break;

                    case "2":
                        manager.LoadGoals();
                        break;

                    case "3":
                        break;

                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }
    }
}