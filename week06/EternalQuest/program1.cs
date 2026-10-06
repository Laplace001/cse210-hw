```csharp
using System;
using System.Collections.Generic;
using System.IO;

namespace EternalQuest
{
    // ============================================================
    // BASE CLASS - Demonstrates ABSTRACTION and INHERITANCE
    // ============================================================
    // This abstract class contains information and behavior
    // common to all types of goals.
    abstract class Goal
    {
        private string _name;
        private string _description;
        private int _points;

        public Goal(string name, string description, int points)
        {
            _name = name;
            _description = description;
            _points = points;
        }

        public string Name
        {
            get { return _name; }
        }

        public string Description
        {
            get { return _description; }
        }

        public int Points
        {
            get { return _points; }
        }

        // Each derived goal provides its own implementation.
        public abstract int RecordEvent();

        // Each derived goal displays its own status.
        public abstract string GetStatus();

        // Used when saving goals to a file.
        public abstract string GetSaveString();
    }


    // ============================================================
    // SIMPLE GOAL
    // ============================================================
    // A simple goal can only be completed once.
    class SimpleGoal : Goal
    {
        private bool _isComplete;

        public SimpleGoal(string name, string description, int points)
            : base(name, description, points)
        {
            _isComplete = false;
        }

        public SimpleGoal(
            string name,
            string description,
            int points,
            bool isComplete)
            : base(name, description, points)
        {
            _isComplete = isComplete;
        }

        public override int RecordEvent()
        {
            if (_isComplete)
            {
                Console.WriteLine("This goal has already been completed.");
                return 0;
            }

            _isComplete = true;
            Console.WriteLine($"Congratulations! You completed: {Name}");
            return Points;
        }

        public override string GetStatus()
        {
            return _isComplete ? "[X]" : "[ ]";
        }

        public override string GetSaveString()
        {
            return $"Simple|{Name}|{Description}|{Points}|{_isComplete}";
        }
    }


    // ============================================================
    // ETERNAL GOAL
    // ============================================================
    // An eternal goal can be recorded many times.
    class EternalGoal : Goal
    {
        public EternalGoal(string name, string description, int points)
            : base(name, description, points)
        {
        }

        public override int RecordEvent()
        {
            Console.WriteLine($"Great work! You made progress on: {Name}");
            return Points;
        }

        public override string GetStatus()
        {
            return "[∞]";
        }

        public override string GetSaveString()
        {
            return $"Eternal|{Name}|{Description}|{Points}";
        }
    }


    // ============================================================
    // CHECKLIST GOAL
    // ============================================================
    // A checklist goal must be completed a certain number of times.
    class ChecklistGoal : Goal
    {
        private int _target;
        private int _amountCompleted;
        private int _bonus;

        public ChecklistGoal(
            string name,
            string description,
            int points,
            int target,
            int bonus)
            : base(name, description, points)
        {
            _target = target;
            _amountCompleted = 0;
            _bonus = bonus;
        }

        public ChecklistGoal(
            string name,
            string description,
            int points,
            int target,
            int amountCompleted,
            int bonus)
            : base(name, description, points)
        {
            _target = target;
            _amountCompleted = amountCompleted;
            _bonus = bonus;
        }

        public override int RecordEvent()
        {
            if (_amountCompleted >= _target)
            {
                Console.WriteLine("This checklist goal is already complete.");
                return 0;
            }

            _amountCompleted++;

            int earnedPoints = Points;

            Console.WriteLine(
                $"Progress: {_amountCompleted}/{_target} times.");

            if (_amountCompleted == _target)
            {
                earnedPoints += _bonus;

                Console.WriteLine("************************************");
                Console.WriteLine("CHECKLIST GOAL COMPLETED!");
                Console.WriteLine($"Bonus earned: {_bonus} points!");
                Console.WriteLine("************************************");
            }

            return earnedPoints;
        }

        public override string GetStatus()
        {
            if (_amountCompleted >= _target)
            {
                return $"[X] Completed {_amountCompleted}/{_target} times";
            }

            return $"[ ] Completed {_amountCompleted}/{_target} times";
        }

        public override string GetSaveString()
        {
            return $"Checklist|{Name}|{Description}|{Points}|{_target}|{_amountCompleted}|{_bonus}";
        }
    }


    // ============================================================
    // PROGRESS GOAL - EXTRA CREATIVE FEATURE
    // ============================================================
    // This is an additional goal type added to exceed the basic
    // assignment requirements.
    //
    // The user makes progress toward a larger target.
    class ProgressGoal : Goal
    {
        private int _target;
        private int _currentProgress;
        private int _pointsPerUnit;

        public ProgressGoal(
            string name,
            string description,
            int target,
            int pointsPerUnit)
            : base(name, description, pointsPerUnit)
        {
            _target = target;
            _currentProgress = 0;
            _pointsPerUnit = pointsPerUnit;
        }

        public ProgressGoal(
            string name,
            string description,
            int target,
            int currentProgress,
            int pointsPerUnit)
            : base(name, description, pointsPerUnit)
        {
            _target = target;
            _currentProgress = currentProgress;
            _pointsPerUnit = pointsPerUnit;
        }

        public override int RecordEvent()
        {
            if (_currentProgress >= _target)
            {
                Console.WriteLine("This progress goal is already complete.");
                return 0;
            }

            Console.Write("How many units of progress did you make? ");
            int amount;

            if (!int.TryParse(Console.ReadLine(), out amount) || amount <= 0)
            {
                Console.WriteLine("Invalid amount.");
                return 0;
            }

            _currentProgress += amount;

            if (_currentProgress > _target)
            {
                _currentProgress = _target;
            }

            int earnedPoints = amount * _pointsPerUnit;

            if (_currentProgress == _target)
            {
                earnedPoints += 500;
                Console.WriteLine("Progress goal completed!");
                Console.WriteLine("Completion bonus: 500 points!");
            }

            return earnedPoints;
        }

        public override string GetStatus()
        {
            if (_currentProgress >= _target)
            {
                return $"[X] Progress {_currentProgress}/{_target}";
            }

            return $"[ ] Progress {_currentProgress}/{_target}";
        }

        public override string GetSaveString()
        {
            return $"Progress|{Name}|{Description}|{_target}|{_currentProgress}|{_pointsPerUnit}";
        }
    }


    // ============================================================
    // QUEST MANAGER
    // ============================================================
    // This class manages the user's goals, score, levels, and
    // saving/loading functionality.
    class QuestManager
    {
        private List<Goal> _goals;
        private int _score;

        public QuestManager()
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
            Console.WriteLine("\n========== YOUR ETERNAL QUEST ==========");

            if (_goals.Count == 0)
            {
                Console.WriteLine("You do not have any goals yet.");
                return;
            }

            for (int i = 0; i < _goals.Count; i++)
            {
                Goal goal = _goals[i];

                Console.WriteLine(
                    $"{i + 1}. {goal.GetStatus()} {goal.Name}");
                Console.WriteLine(
                    $"   {goal.Description}");
                Console.WriteLine(
                    $"   Points: {goal.Points}");
            }
        }

        public void RecordEvent()
        {
            DisplayGoals();

            if (_goals.Count == 0)
            {
                return;
            }

            Console.Write("\nWhich goal did you accomplish? ");

            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Invalid choice.");
                return;
            }

            if (choice < 1 || choice > _goals.Count)
            {
                Console.WriteLine("Invalid goal number.");
                return;
            }

            Goal selectedGoal = _goals[choice - 1];

            int pointsEarned = selectedGoal.RecordEvent();

            _score += pointsEarned;

            Console.WriteLine($"\nYou earned {pointsEarned} points!");
            Console.WriteLine($"Total score: {_score}");

            CheckLevel();
        }

        public void DisplayScore()
        {
            Console.WriteLine("\n==================================");
            Console.WriteLine($"Current Score: {_score}");
            Console.WriteLine($"Current Level: {GetLevel()}");
            Console.WriteLine($"Next Level: {GetNextLevel()}");
            Console.WriteLine("==================================");
        }

        // ========================================================
        // EXTRA GAMIFICATION FEATURE: LEVEL SYSTEM
        // ========================================================
        public int GetLevel()
        {
            return (_score / 1000) + 1;
        }

        public int GetNextLevel()
        {
            int currentLevel = GetLevel();
            return currentLevel * 1000;
        }

        private void CheckLevel()
        {
            int level = GetLevel();

            Console.WriteLine($"\nYou are now Level {level}!");

            if (_score >= 5000)
            {
                Console.WriteLine("Badge: Eternal Champion!");
            }
            else if (_score >= 3000)
            {
                Console.WriteLine("Badge: Quest Master!");
            }
            else if (_score >= 1000)
            {
                Console.WriteLine("Badge: Rising Hero!");
            }
            else
            {
                Console.WriteLine("Badge: Quest Beginner!");
            }
        }

        // ========================================================
        // SAVE FUNCTION
        // ========================================================
        public void Save()
        {
            Console.Write("Enter a filename to save: ");
            string filename = Console.ReadLine();

            try
            {
                using (StreamWriter writer = new StreamWriter(filename))
                {
                    writer.WriteLine(_score);

                    foreach (Goal goal in _goals)
                    {
                        writer.WriteLine(goal.GetSaveString());
                    }
                }

                Console.WriteLine("Goals saved successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving file: {ex.Message}");
            }
        }


        // ========================================================
        // LOAD FUNCTION
        // ========================================================
        public void Load()
        {
            Console.Write("Enter the filename to load: ");
            string filename = Console.ReadLine();

            try
            {
                if (!File.Exists(filename))
                {
                    Console.WriteLine("File not found.");
                    return;
                }

                string[] lines = File.ReadAllLines(filename);

                _goals.Clear();

                if (lines.Length > 0)
                {
                    _score = int.Parse(lines[0]);
                }

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
                        int completed = int.Parse(parts[5]);
                        int bonus = int.Parse(parts[6]);

                        _goals.Add(
                            new ChecklistGoal(
                                name,
                                description,
                                points,
                                target,
                                completed,
                                bonus));
                    }

                    else if (parts[0] == "Progress")
                    {
                        string name = parts[1];
                        string description = parts[2];
                        int target = int.Parse(parts[3]);
                        int progress = int.Parse(parts[4]);
                        int pointsPerUnit = int.Parse(parts[5]);

                        _goals.Add(
                            new ProgressGoal(
                                name,
                                description,
                                target,
                                progress,
                                pointsPerUnit));
                    }
                }

                Console.WriteLine("Goals loaded successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading file: {ex.Message}");
            }
        }
    }


    // ============================================================
    // PROGRAM CLASS
    // ============================================================
    class Program
    {
        static void Main(string[] args)
        {
            QuestManager quest = new QuestManager();

            Console.WriteLine("==========================================");
            Console.WriteLine("       WELCOME TO THE ETERNAL QUEST       ");
            Console.WriteLine("==========================================");

            bool running = true;

            while (running)
            {
                Console.WriteLine("\nPlease choose one of the following:");

                Console.WriteLine("1. Create New Goal");
                Console.WriteLine("2. Record Goal Event");
                Console.WriteLine("3. Display Goals");
                Console.WriteLine("4. Display Score");
                Console.WriteLine("5. Save Goals");
                Console.WriteLine("6. Load Goals");
                Console.WriteLine("7. Quit");

                Console.Write("Select an option: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CreateGoal(quest);
                        break;

                    case "2":
                        quest.RecordEvent();
                        break;

                    case "3":
                        quest.DisplayGoals();
                        break;

                    case "4":
                        quest.DisplayScore();
                        break;

                    case "5":
                        quest.Save();
                        break;

                    case "6":
                        quest.Load();
                        break;

                    case "7":
                        running = false;
                        Console.WriteLine(
                            "\nThank you for using Eternal Quest!");
                        Console.WriteLine(
                            "Keep working toward your goals!");
                        break;

                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }


        // ========================================================
        // CREATE GOAL
        // ========================================================
        static void CreateGoal(QuestManager quest)
        {
            Console.WriteLine("\n========== CREATE GOAL ==========");

            Console.WriteLine("1. Simple Goal");
            Console.WriteLine("2. Eternal Goal");
            Console.WriteLine("3. Checklist Goal");
            Console.WriteLine("4. Progress Goal");

            Console.Write("Choose goal type: ");
            string type = Console.ReadLine();

            Console.Write("Enter goal name: ");
            string name = Console.ReadLine();

            Console.Write("Enter goal description: ");
            string description = Console.ReadLine();

            switch (type)
            {
                case "1":
                    Console.Write("Enter points: ");
                    int simplePoints = GetInteger();

                    quest.AddGoal(
                        new SimpleGoal(
                            name,
                            description,
                            simplePoints));

                    Console.WriteLine("Simple goal created!");
                    break;


                case "2":
                    Console.Write("Enter points for each completion: ");
                    int eternalPoints = GetInteger();

                    quest.AddGoal(
                        new EternalGoal(
                            name,
                            description,
                            eternalPoints));

                    Console.WriteLine("Eternal goal created!");
                    break;


                case "3":
                    Console.Write("Enter points for each completion: ");
                    int checklistPoints = GetInteger();

                    Console.Write("How many times must it be completed? ");
                    int target = GetInteger();

                    Console.Write("Enter completion bonus: ");
                    int bonus = GetInteger();

                    quest.AddGoal(
                        new ChecklistGoal(
                            name,
                            description,
                            checklistPoints,
                            target,
                            bonus));

                    Console.WriteLine("Checklist goal created!");
                    break;


                case "4":
                    Console.Write("Enter the target amount: ");
                    int progressTarget = GetInteger();

                    Console.Write("Enter points per unit of progress: ");
                    int pointsPerUnit = GetInteger();

                    quest.AddGoal(
                        new ProgressGoal(
                            name,
                            description,
                            progressTarget,
                            pointsPerUnit));

                    Console.WriteLine("Progress goal created!");
                    break;


                default:
                    Console.WriteLine("Invalid goal type.");
                    break;
            }
        }


        // ========================================================
        // INTEGER INPUT VALIDATION
        // ========================================================
        static int GetInteger()
        {
            int value;

            while (!int.TryParse(Console.ReadLine(), out value) || value < 0)
            {
                Console.Write("Please enter a valid positive number: ");
            }

            return value;
        }
    }
}

