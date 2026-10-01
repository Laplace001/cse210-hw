using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;


abstract class Goal
{
    private string _name;
    private string _description;
    private int _points;
    private bool _isComplete;

    public string Name => _name;
    public string Description => _description;
    public int Points => _points;
    public bool IsComplete => _isComplete;

    protected Goal(string name, string description, int points)
    {
        _name = name;
        _description = description;
        _points = points;
        _isComplete = false;
    }

    public abstract int RecordEvent();

    // Returns a string representation of the goal.
    public abstract string GetDetailsString();

    // Used by derived classes when a goal becomes complete.
    protected void MarkComplete()
    {
        _isComplete = true;
    }
}

class EternalGoal : Goal
{
    public EternalGoal(string name, string description, int points)
        : base(name, description, points)
    {
    }

    // Eternal goals never become complete.
    public override int RecordEvent()
    {
        return Points;
    }

    public override string GetDetailsString()
    {
        return $"[ ] {Name} ({Description})";
    }
}

class ChecklistGoal : Goal
{
    private int _targetCount;
    private int _currentCount;
    private int _bonus;

    public int TargetCount => _targetCount;
    public int CurrentCount => _currentCount;
    public int Bonus => _bonus;

    public ChecklistGoal(
        string name,
        string description,
        int points,
        int targetCount,
        int bonus)
        : base(name, description, points)
    {
        _targetCount = targetCount;
        _currentCount = 0;
        _bonus = bonus;
    }

    public override int RecordEvent()
    {
       
        if (IsComplete)
        {
            return 0;
        }

        _currentCount++;

        int earnedPoints = Points;

        // Check whether the goal has now been completed.
        if (_currentCount >= _targetCount)
        {
            MarkComplete();
            earnedPoints += _bonus;
        }

        return earnedPoints;
    }

    public override string GetDetailsString()
    {
        string status = IsComplete ? "[X]" : "[ ]";

        return $"{status} {Name} ({Description}) " +
               $"-- Completed {_currentCount}/{_targetCount} times";
    }
}

class MarathonGoal : Goal
{
    public MarathonGoal(string name, string description, int points)
        : base(name, description, points)
    {
    }

    public override int RecordEvent()
    {
        // A marathon goal is completed once it is recorded.
        MarkComplete();
        return Points;
    }

    public override string GetDetailsString()
    {
        string status = IsComplete ? "[X]" : "[ ]";

        return $"{status} {Name} ({Description})";
    }
}


class GoalData
{
    public string Type { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Points { get; set; }

    // Checklist-specific information
    public int TargetCount { get; set; }
    public int CurrentCount { get; set; }
    public int Bonus { get; set; }

    public bool IsComplete { get; set; }
}

class SaveData
{
    public int Score { get; set; }
    public List<GoalData> Goals { get; set; } = new List<GoalData>();
}


class GoalManager
{
    private List<Goal> _goals;
    private int _score;

    public GoalManager()
    {
        _goals = new List<Goal>();
        _score = 0;
    }

    public void CreateGoal()
    {
        Console.WriteLine("\nChoose the type of goal:");
        Console.WriteLine("1. Eternal Goal");
        Console.WriteLine("2. Checklist Goal");
        Console.WriteLine("3. Marathon Goal");

        Console.Write("Select a goal type: ");
        string choice = Console.ReadLine();

        Console.Write("Enter the goal name: ");
        string name = Console.ReadLine();

        Console.Write("Enter a short description: ");
        string description = Console.ReadLine();

        Console.Write("Enter the points for completing the goal: ");
        int points = int.Parse(Console.ReadLine());

        switch (choice)
        {
            case "1":
                _goals.Add(
                    new EternalGoal(name, description, points)
                );
                break;

            case "2":
                Console.Write("How many times must it be completed? ");
                int target = int.Parse(Console.ReadLine());

                Console.Write("Enter the bonus points: ");
                int bonus = int.Parse(Console.ReadLine());

                _goals.Add(
                    new ChecklistGoal(
                        name,
                        description,
                        points,
                        target,
                        bonus
                    )
                );
                break;

            case "3":
                _goals.Add(
                    new MarathonGoal(name, description, points)
                );
                break;

            default:
                Console.WriteLine("Invalid goal type.");
                return;
        }

        Console.WriteLine("Goal created successfully!");
    }

    public void RecordEvent()
    {
        if (_goals.Count == 0)
        {
            Console.WriteLine("You do not have any goals.");
            return;
        }

        DisplayGoals();

        Console.Write("Enter the number of the goal you completed: ");
        int index = int.Parse(Console.ReadLine()) - 1;

        if (index < 0 || index >= _goals.Count)
        {
            Console.WriteLine("Invalid goal number.");
            return;
        }

        Goal selectedGoal = _goals[index];

        int pointsEarned = selectedGoal.RecordEvent();

        _score += pointsEarned;

        Console.WriteLine(
            $"Congratulations! You earned {pointsEarned} points."
        );

        Console.WriteLine($"Your current score is {_score}.");
    }

    public void DisplayGoals()
    {
        Console.WriteLine("\nGoals:");

        if (_goals.Count == 0)
        {
            Console.WriteLine("No goals have been created.");
            return;
        }

        for (int i = 0; i < _goals.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {_goals[i].GetDetailsString()}");
        }
    }

    public void DisplayScore()
    {
        Console.WriteLine($"\nYour current score is: {_score}");
    }

    public void Save()
    {
        Console.Write("Enter the filename to save to: ");
        string filename = Console.ReadLine();

        SaveData saveData = new SaveData
        {
            Score = _score
        };

        foreach (Goal goal in _goals)
        {
            GoalData data = new GoalData
            {
                Name = goal.Name,
                Description = goal.Description,
                Points = goal.Points,
                IsComplete = goal.IsComplete
            };

            if (goal is EternalGoal)
            {
                data.Type = "Eternal";
            }
            else if (goal is ChecklistGoal checklist)
            {
                data.Type = "Checklist";
                data.TargetCount = checklist.TargetCount;
                data.CurrentCount = checklist.CurrentCount;
                data.Bonus = checklist.Bonus;
            }
            else if (goal is MarathonGoal)
            {
                data.Type = "Marathon";
            }

            saveData.Goals.Add(data);
        }

        string json = JsonSerializer.Serialize(
            saveData,
            new JsonSerializerOptions
            {
                WriteIndented = true
            }
        );

        File.WriteAllText(filename, json);

        Console.WriteLine("Goals and score saved successfully.");
    }

    public void Load()
    {
        Console.Write("Enter the filename to load: ");
        string filename = Console.ReadLine();

        if (!File.Exists(filename))
        {
            Console.WriteLine("File not found.");
            return;
        }

        string json = File.ReadAllText(filename);

        SaveData saveData =
            JsonSerializer.Deserialize<SaveData>(json);

        _goals.Clear();
        _score = saveData.Score;

        foreach (GoalData data in saveData.Goals)
        {
            Goal goal;

            switch (data.Type)
            {
                case "Eternal":
                    goal = new EternalGoal(
                        data.Name,
                        data.Description,
                        data.Points
                    );
                    break;

                case "Checklist":
                    ChecklistGoal checklist =
                        new ChecklistGoal(
                            data.Name,
                            data.Description,
                            data.Points,
                            data.TargetCount,
                            data.Bonus
                        );

                    // Recreate the recorded events.
                    for (int i = 0; i < data.CurrentCount; i++)
                    {
                        checklist.RecordEvent();
                    }

                    goal = checklist;
                    break;

                case "Marathon":
                    goal = new MarathonGoal(
                        data.Name,
                        data.Description,
                        data.Points
                    );

                    // Restore completion state.
                    if (data.IsComplete)
                    {
                        goal.RecordEvent();
                    }

                    break;

                default:
                    continue;
            }

            _goals.Add(goal);
        }

        Console.WriteLine("Goals and score loaded successfully.");
    }
}

class Program
{
    static void Main(string[] args)
    {
        GoalManager manager = new GoalManager();

        bool running = true;

        while (running)
        {
            Console.WriteLine("\n==============================");
            Console.WriteLine("       GOAL TRACKER");
            Console.WriteLine("==============================");

            Console.WriteLine("1. Create New Goal");
            Console.WriteLine("2. Record Goal Event");
            Console.WriteLine("3. Show Goals");
            Console.WriteLine("4. Show Score");
            Console.WriteLine("5. Save Goals");
            Console.WriteLine("6. Load Goals");
            Console.WriteLine("7. Quit");

            Console.Write("\nSelect an option: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    manager.CreateGoal();
                    break;

                case "2":
                    manager.RecordEvent();
                    break;

                case "3":
                    manager.DisplayGoals();
                    break;

                case "4":
                    manager.DisplayScore();
                    break;

                case "5":
                    manager.Save();
                    break;

                case "6":
                    manager.Load();
                    break;

                case "7":
                    running = false;
                    Console.WriteLine("Goodbye!");
                    break;

                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }
}