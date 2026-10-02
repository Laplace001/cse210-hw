using System;
using System.Collections.Generic;
using System.IO;

namespace EternalQuest
{
    public class GoalManager
    {
        private List<Goal> _goals;
        private int _score;

        public GoalManager()
        {
            _goals = new List<Goal>();
            _score = 0;
        }

        public void AddGoal(Goal goal)
        {
            _goals.Add(goal);
        }

        public void DisplayGoals()
        {
            if (_goals.Count == 0)
            {
                Console.WriteLine("You do not have any goals yet.");
                return;
            }

            Console.WriteLine("\nYour Goals:");

            for (int i = 0; i < _goals.Count; i++)
            {
                Console.Write($"{i + 1}. ");
                _goals[i].Display();
            }
        }

        public void DisplayScore()
        {
            Console.WriteLine($"\nYour current score is: {_score}");
        }

        public void RecordEvent()
        {
            if (_goals.Count == 0)
            {
                Console.WriteLine("You have no goals to record.");
                return;
            }

            DisplayGoals();

            Console.Write("\nWhich goal did you accomplish? ");
            string input = Console.ReadLine();

            if (!int.TryParse(input, out int choice))
            {
                Console.WriteLine("Invalid choice.");
                return;
            }

            if (choice < 1 || choice > _goals.Count)
            {
                Console.WriteLine("That goal does not exist.");
                return;
            }

            Goal selectedGoal = _goals[choice - 1];

            int pointsEarned = selectedGoal.RecordEvent();

            _score += pointsEarned;

            Console.WriteLine(
                $"You earned {pointsEarned} points!");

            Console.WriteLine(
                $"Your new score is: {_score}");
        }

        public void CreateSimpleGoal()
        {
            Console.WriteLine("\n--- Create Simple Goal ---");

            Console.Write("Goal name: ");
            string name = Console.ReadLine();

            Console.Write("Description: ");
            string description = Console.ReadLine();

            int points = ReadInteger("Points awarded: ");

            Goal goal = new SimpleGoal(
                name,
                description,
                points);

            AddGoal(goal);

            Console.WriteLine("Simple goal created successfully.");
        }

        public void CreateEternalGoal()
        {
            Console.WriteLine("\n--- Create Eternal Goal ---");

            Console.Write("Goal name: ");
            string name = Console.ReadLine();

            Console.Write("Description: ");
            string description = Console.ReadLine();

            int points = ReadInteger("Points awarded each time: ");

            Goal goal = new EternalGoal(
                name,
                description,
                points);

            AddGoal(goal);

            Console.WriteLine("Eternal goal created successfully.");
        }

        public void CreateChecklistGoal()
        {
            Console.WriteLine("\n--- Create Checklist Goal ---");

            Console.Write("Goal name: ");
            string name = Console.ReadLine();

            Console.Write("Description: ");
            string description = Console.ReadLine();

            int points = ReadInteger("Points awarded each time: ");

            int target = ReadInteger(
                "How many times must this goal be completed? ");

            int bonus = ReadInteger(
                "Bonus points for completing the goal: ");

            Goal goal = new ChecklistGoal(
                name,
                description,
                points,
                target,
                bonus);

            AddGoal(goal);

            Console.WriteLine("Checklist goal created successfully.");
        }

        private int ReadInteger(string message)
        {
            while (true)
            {
                Console.Write(message);

                string input = Console.ReadLine();

                if (int.TryParse(input, out int value) && value >= 0)
                {
                    return value;
                }

                Console.WriteLine("Please enter a valid positive number.");
            }
        }

        public void SaveGoals()
        {
            Console.Write("\nEnter file name to save: ");
            string fileName = Console.ReadLine();

            try
            {
                using (StreamWriter writer = new StreamWriter(fileName))
                {
                    writer.WriteLine(_score);

                    foreach (Goal goal in _goals)
                    {
                        writer.WriteLine(goal.GetSaveString());
                    }
                }

                Console.WriteLine("Goals saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving goals: " + ex.Message);
            }
        }

        public void LoadGoals()
        {
            Console.Write("\nEnter file name to load: ");
            string fileName = Console.ReadLine();

            if (!File.Exists(fileName))
            {
                Console.WriteLine("File not found.");
                return;
            }

            try
            {
                string[] lines = File.ReadAllLines(fileName);

                if (lines.Length == 0)
                {
                    Console.WriteLine("The file is empty.");
                    return;
                }

                _goals.Clear();

                _score = int.Parse(lines[0]);

                for (int i = 1; i < lines.Length; i++)
                {
                    string[] parts = lines[i].Split('|');

                    if (parts[0] == "Simple")
                    {
                        string name = parts[1];
                        string description = parts[2];
                        int points = int.Parse(parts[3]);
                        bool complete = bool.Parse(parts[4]);

                        _goals.Add(
                            new SimpleGoal(
                                name,
                                description,
                                points,
                                complete));
                    }
                    else if (parts[0] == "Eternal")
                    {
                        string name = parts[1];
                        string description = parts[2];
                        int points = int.Parse(parts[3]);

                        _goals.Add(
                            new EternalGoal(
                                name,
                                description,
                                points));
                    }
                    else if (parts[0] == "Checklist")
                    {
                        string name = parts[1];
                        string description = parts[2];
                        int points = int.Parse(parts[3]);
                        int target = int.Parse(parts[4]);
                        int bonus = int.Parse(parts[5]);
                        int completed = int.Parse(parts[6]);

                        _goals.Add(
                            new ChecklistGoal(
                                name,
                                description,
                                points,
                                target,
                                bonus,
                                completed));
                    }
                }

                Console.WriteLine("Goals loaded successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading goals: " + ex.Message);
            }
        }
    }
}