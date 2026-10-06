using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


public abstract class Goal
{
    private string _name;
    private string _description;
    private int _points;
    private bool _isComplete;

    protected Goal(string name, string description, int points)
    {
        _name = name;
        _description = description;
        _points = points;
        _isComplete = false;
    }

    public string Name => _name;
    public string Description => _description;
    public int Points => _points;
    public bool IsComplete
    {
        get => _isComplete;
        protected set => _isComplete = value;
    }

    
    public abstract int RecordEvent();
    public abstract string GetDisplayString();
    public abstract string GetSaveString();

    
    public virtual string GetDetailsString()
    {
        string checkbox = _isComplete ? "[X]" : "[ ]";
        return $"{checkbox} {_name} ({_description})";
    }
}


public class SimpleGoal : Goal
{
    public SimpleGoal(string name, string description, int points)
        : base(name, description, points)
    {
    }

    public override int RecordEvent()
    {
        if (!IsComplete)
        {
            IsComplete = true;
            return Points;
        }
        return 0;
    }

    public override string GetDisplayString()
    {
        string checkbox = IsComplete ? "[X]" : "[ ]";
        return $"{checkbox} {Name} ({Description})";
    }

    public override string GetSaveString()
    {
        return $"SimpleGoal:{Name},{Description},{Points},{IsComplete}";
    }
}


public class EternalGoal : Goal
{
    public EternalGoal(string name, string description, int points)
        : base(name, description, points)
    {
    }

    public override int RecordEvent()
    {
    
        return Points;
    }

    public override string GetDisplayString()
    {
        return $"[∞] {Name} ({Description}) - {Points} points each time";
    }

    public override string GetSaveString()
    {
        return $"EternalGoal:{Name},{Description},{Points}";
    }
}


public class ChecklistGoal : Goal
{
    private int _targetCount;
    private int _currentCount;
    private int _bonusPoints;

    public ChecklistGoal(string name, string description, int points,
        int targetCount, int bonusPoints, int currentCount = 0)
        : base(name, description, points)
    {
        _targetCount = targetCount;
        _bonusPoints = bonusPoints;
        _currentCount = currentCount;
        if (_currentCount >= _targetCount)
        {
            IsComplete = true;
        }
    }

    public int TargetCount => _targetCount;
    public int CurrentCount => _currentCount;
    public int BonusPoints => _bonusPoints;

    public override int RecordEvent()
    {
        if (IsComplete)
        {
            return 0; // Already complete
        }

        _currentCount++;
        int earnedPoints = Points;

        if (_currentCount >= _targetCount)
        {
            IsComplete = true;
            earnedPoints += _bonusPoints;
        }

        return earnedPoints;
    }

    public override string GetDisplayString()
    {
        string checkbox = IsComplete ? "[X]" : "[ ]";
        return $"{checkbox} {Name} ({Description}) - Completed {_currentCount}/{_targetCount} times";
    }

    public override string GetSaveString()
    {
        return $"ChecklistGoal:{Name},{Description},{Points},{_targetCount},{_bonusPoints},{_currentCount}";
    }
}

// Gamification: Level system
public class Player
{
    private int _score;
    private string _title;

    public Player()
    {
        _score = 0;
        _title = "Novice";
    }

    public int Score => _score;

    public string Title
    {
        get
        {
            UpdateTitle();
            return _title;
        }
    }

    public int Level => (_score / 1000) + 1;

    public void AddPoints(int points)
    {
        _score += points;
    }

    private void UpdateTitle()
    {
        int level = Level;
        _title = level switch
        {
            >= 10 => "Legendary Hero",
            >= 7 => "Champion",
            >= 5 => "Hero",
            >= 3 => "Adventurer",
            >= 2 => "Apprentice",
            _ => "Novice"
        };
    }

    public string GetStatusString()
    {
        return $"Score: {_score} | Level: {Level} ({Title})";
    }
}
public class EternalQuestProgram
{
    private List<Goal> _goals;
    private Player _player;

    public EternalQuestProgram()
    {
        _goals = new List<Goal>();
        _player = new Player();
    }

    public void Run()
    {
        bool running = true;
        while (running)
        {
            DisplayMenu();
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    CreateGoal();
                    break;
                case "2":
                    ListGoals();
                    break;
                case "3":
                    RecordEvent();
                    break;
                case "4":
                    SaveGoals();
                    break;
                case "5":
                    LoadGoals();
                    break;
                case "6":
                    running = false;
                    Console.WriteLine("Goodbye, and happy questing!");
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }

            if (running)
            {
                Console.WriteLine("\nPress Enter to continue...");
                Console.ReadLine();
            }
        }
    }

    private void DisplayMenu()
    {
        Console.Clear();
        Console.WriteLine("========================================");
        Console.WriteLine("         ETERNAL QUEST PROGRAM          ");
        Console.WriteLine("========================================");
        Console.WriteLine(_player.GetStatusString());
        Console.WriteLine("----------------------------------------");
        Console.WriteLine("1. Create a new goal");
        Console.WriteLine("2. List goals");
        Console.WriteLine("3. Record an event");
        Console.WriteLine("4. Save goals");
        Console.WriteLine("5. Load goals");
        Console.WriteLine("6. Quit");
        Console.Write("Select an option: ");
    }

    private void CreateGoal()
    {
        Console.Clear();
        Console.WriteLine("What type of goal would you like to create?");
        Console.WriteLine("1. Simple Goal (completed once)");
        Console.WriteLine("2. Eternal Goal (never complete)");
        Console.WriteLine("3. Checklist Goal (done N times for bonus)");
        Console.Write("Select a type: ");

        string type = Console.ReadLine();

        Console.Write("Enter goal name: ");
        string name = Console.ReadLine();

        Console.Write("Enter goal description: ");
        string description = Console.ReadLine();

        Console.Write("Enter points for each completion: ");
        if (!int.TryParse(Console.ReadLine(), out int points) || points < 0)
        {
            Console.WriteLine("Invalid points. Returning to menu.");
            return;
        }

        switch (type)
        {
            case "1":
                _goals.Add(new SimpleGoal(name, description, points));
                Console.WriteLine("Simple goal created!");
                break;
            case "2":
                _goals.Add(new EternalGoal(name, description, points));
                Console.WriteLine("Eternal goal created!");
                break;
            case "3":
                Console.Write("Enter target count (how many times to complete): ");
                if (!int.TryParse(Console.ReadLine(), out int target) || target <= 0)
                {
                    Console.WriteLine("Invalid target count. Returning to menu.");
                    return;
                }

                Console.Write("Enter bonus points for completing the checklist: ");
                if (!int.TryParse(Console.ReadLine(), out int bonus) || bonus < 0)
                {
                    Console.WriteLine("Invalid bonus points. Returning to menu.");
                    return;
                }

                _goals.Add(new ChecklistGoal(name, description, points, target, bonus));
                Console.WriteLine("Checklist goal created!");
                break;
            default:
                Console.WriteLine("Invalid goal type.");
                break;
        }
    }

    private void ListGoals()
    {
        Console.Clear();
        Console.WriteLine("Your Goals:");
        Console.WriteLine("----------------------------------------");

        if (_goals.Count == 0)
        {
            Console.WriteLine("You have no goals yet. Create some!");
            return;
        }

        for (int i = 0; i < _goals.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {_goals[i].GetDisplayString()}");
        }
    }

    private void RecordEvent()
    {
        Console.Clear();
        Console.WriteLine("Your Goals:");

        if (_goals.Count == 0)
        {
            Console.WriteLine("You have no goals to record.");
            return;
        }
        var availableGoals = new List<(int index, Goal goal)>();
        for (int i = 0; i < _goals.Count; i++)
        {
            availableGoals.Add((i, _goals[i]));
            Console.WriteLine($"{i + 1}. {_goals[i].GetDisplayString()}");
        }

        Console.Write("Which goal did you accomplish? ");
        if (!int.TryParse(Console.ReadLine(), out int choice) ||
            choice < 1 || choice > _goals.Count)
        {
            Console.WriteLine("Invalid choice.");
            return;
        }

        Goal selectedGoal = _goals[choice - 1];
        int earnedPoints = selectedGoal.RecordEvent();

        if (earnedPoints > 0)
        {
            _player.AddPoints(earnedPoints);
            Console.WriteLine($"\nCongratulations! You earned {earnedPoints} points!");
            Console.WriteLine(_player.GetStatusString());

            if (selectedGoal is ChecklistGoal cg && cg.IsComplete)
            {
                Console.WriteLine($"\n🎉 You completed the checklist goal '{cg.Name}'!");
                Console.WriteLine($"Bonus of {cg.BonusPoints} points awarded!");
            }

            if (selectedGoal is SimpleGoal sg && sg.IsComplete)
            {
                Console.WriteLine($"\n✅ You completed '{sg.Name}'!");
            }
        }
        else
        {
            Console.WriteLine("\nThis goal has already been completed. No points awarded.");
        }
    }

    private void SaveGoals()
    {
        Console.Write("Enter filename to save: ");
        string filename = Console.ReadLine();

        try
        {
            using (StreamWriter writer = new StreamWriter(filename))
            {
                writer.WriteLine(_player.Score);
                writer.WriteLine(_goals.Count);

                foreach (Goal goal in _goals)
                {
                    writer.WriteLine(goal.GetSaveString());
                }
            }

            Console.WriteLine($"Goals saved to {filename}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving file: {ex.Message}");
        }
    }

    private void LoadGoals()
    {
        Console.Write("Enter filename to load: ");
        string filename = Console.ReadLine();

        if (!File.Exists(filename))
        {
            Console.WriteLine("File not found.");
            return;
        }

        try
        {
            string[] lines = File.ReadAllLines(filename);

            if (lines.Length < 2)
            {
                Console.WriteLine("File format is invalid.");
                return;
            }


            _goals.Clear();

            
            if (int.TryParse(lines[0], out int score))
            {
                
                _player = new Player();
                _player.AddPoints(score);
            }

            if (int.TryParse(lines[1], out int goalCount))
            {
                for (int i = 0; i < goalCount && i + 2 < lines.Length; i++)
                {
                    Goal goal = ParseGoal(lines[i + 2]);
                    if (goal != null)
                    {
                        _goals.Add(goal);
                    }
                }
            }

            Console.WriteLine($"Goals loaded from {filename}");
            Console.WriteLine(_player.GetStatusString());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading file: {ex.Message}");
        }
    }

    private Goal ParseGoal(string line)
    {
        try
        {
            string[] parts = line.Split(':');
            if (parts.Length != 2) return null;

            string type = parts[0];
            string[] data = parts[1].Split(',');

            switch (type)
            {
                case "SimpleGoal":
                    bool isComplete = bool.Parse(data[3]);
                    var sg = new SimpleGoal(data[0], data[1], int.Parse(data[2]));
                    if (isComplete) sg.RecordEvent();
                    return sg;

                case "EternalGoal":
                    return new EternalGoal(data[0], data[1], int.Parse(data[2]));

                case "ChecklistGoal":
                    return new ChecklistGoal(
                        data[0],
                        data[1],
                        int.Parse(data[2]),
                        int.Parse(data[3]),
                        int.Parse(data[4]),
                        int.Parse(data[5]));

                default:
                    return null;
            }
        }
        catch
        {
            return null;
        }
    }

    public static void Main(string[] args)
    {
        EternalQuestProgram program = new EternalQuestProgram();
        program.Run();
    }
}