using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace MindfulnessProgram
{
     public abstract class Activity
    {
        private string _name;
        private string _description;
        private int _duration;

        public Activity(string name, string description)
        {
            _name = name;
            _description = description;
        }

        public string Name => _name;
        public string Description => _description;

        public int Duration
        {
            get => _duration;
            protected set => _duration = value;
        }

        public void DisplayStartingMessage()
        {
            Console.Clear();
            Console.WriteLine($"Welcome to the {_name}.\n");
            Console.WriteLine(_description);
            Console.WriteLine();

            Console.Write("How long, in seconds, would you like for your session? ");
            string input = Console.ReadLine();
            int seconds;
            while (!int.TryParse(input, out seconds) || seconds <= 0)
            {
                Console.Write("Please enter a valid positive number of seconds: ");
                input = Console.ReadLine();
            }
            _duration = seconds;

            Console.WriteLine("\nGet ready to begin...");
            ShowSpinner(3);
        }

        public void DisplayEndingMessage()
        {
            Console.WriteLine("\nWell done!!");
            ShowSpinner(3);
            Console.WriteLine($"\nYou have completed another {_duration} seconds of the {_name}.");
            ShowSpinner(3);
        }

        public void ShowSpinner(int seconds)
        {
            string[] spinner = { "|", "/", "-", "\\" };
            DateTime endTime = DateTime.Now.AddSeconds(seconds);
            int i = 0;
            while (DateTime.Now < endTime)
            {
                Console.Write(spinner[i % spinner.Length]);
                Thread.Sleep(250);
                Console.Write("\b \b");
                i++;
            }
        }

        public void ShowCountdown(int seconds)
        {
            for (int i = seconds; i > 0; i--)
            {
                string num = i.ToString();
                Console.Write(num);
                Thread.Sleep(1000);

                for (int c = 0; c < num.Length; c++)
                {
                    Console.Write("\b \b");
                }
            }
        }

        public abstract void Run();
    }

   
    public class BreathingActivity : Activity
    {
        public BreathingActivity()
            : base(
                "Breathing Activity",
                "This activity will help you relax by walking you through breathing in and out slowly. Clear your mind and focus on your breathing."
              )
        { }

        public override void Run()
        {
            DisplayStartingMessage();

            DateTime endTime = DateTime.Now.AddSeconds(Duration);

            while (DateTime.Now < endTime)
            {
                int remaining = (int)Math.Ceiling((endTime - DateTime.Now).TotalSeconds);
                if (remaining <= 0) break;

                int inSeconds = Math.Min(4, remaining);
                Console.Write("\nBreathe in... ");
                ShowCountdown(inSeconds);

                remaining = (int)Math.Ceiling((endTime - DateTime.Now).TotalSeconds);
                if (remaining <= 0) break;

                int outSeconds = Math.Min(4, remaining);
                Console.Write("\nBreathe out... ");
                ShowCountdown(outSeconds);
                Console.WriteLine();
            }

            DisplayEndingMessage();
        }
    }

    
    public class ReflectionActivity : Activity
    {
        private List<string> _prompts;
        private List<string> _questions;
        private Random _random = new Random();

        public ReflectionActivity()
            : base(
                "Reflection Activity",
                "This activity will help you reflect on times in your life when you have shown strength and resilience. This will help you recognize the power you have and how you can use it in other aspects of your life."
              )
        {
            _prompts = new List<string>
            {
                "Think of a time when you stood up for someone else.",
                "Think of a time when you did something really difficult.",
                "Think of a time when you helped someone in need.",
                "Think of a time when you did something truly selfless."
            };

            _questions = new List<string>
            {
                "Why was this experience meaningful to you?",
                "Have you ever done anything like this before?",
                "How did you get started?",
                "How did you feel when it was complete?",
                "What made this time different than other times when you were not as successful?",
                "What is your favorite thing about this experience?",
                "What could you learn from this experience that applies to other situations?",
                "What did you learn about yourself through this experience?",
                "How can you keep this experience in mind in the future?"
            };
        }

        private string GetRandomPrompt() => _prompts[_random.Next(_prompts.Count)];
        private string GetRandomQuestion() => _questions[_random.Next(_questions.Count)];

        public override void Run()
        {
            DisplayStartingMessage();

            Console.WriteLine("\nConsider the following prompt:\n");
            Console.WriteLine($" --- {GetRandomPrompt()} --- \n");
            Console.WriteLine("When you have something in mind, press Enter to continue.");
            Console.ReadLine();

            Console.WriteLine("Now ponder on each of the following questions as they relate to this experience.");
            Console.Write("You may begin in: ");
            ShowCountdown(5);
            Console.WriteLine();

            DateTime endTime = DateTime.Now.AddSeconds(Duration);

            while (DateTime.Now < endTime)
            {
                Console.Write($"\n> {GetRandomQuestion()} ");
                ShowSpinner(5);
            }

            DisplayEndingMessage();
        }
    }

    public class ListingActivity : Activity
    {
        private List<string> _prompts;
        private Random _random = new Random();

        public ListingActivity()
            : base(
                "Listing Activity",
                "This activity will help you reflect on the good things in your life by having you list as many things as you can in a certain area."
              )
        {
            _prompts = new List<string>
            {
                "Who are people that you appreciate?",
                "What are personal strengths of yours?",
                "Who are people that you have helped this week?",
                "When have you felt the Holy Ghost this month?",
                "Who are some of your personal heroes?"
            };
        }

        private string GetRandomPrompt() => _prompts[_random.Next(_prompts.Count)];

        public override void Run()
        {
            DisplayStartingMessage();

            Console.WriteLine("\nList as many responses as you can to the following prompt:");
            Console.WriteLine($" --- {GetRandomPrompt()} --- \n");
            Console.Write("You may begin in: ");
            ShowCountdown(5);
            Console.WriteLine();

            
            DateTime endTime = DateTime.Now.AddSeconds(Duration);

            int count = 0;

            while (DateTime.Now < endTime)
            {
                Console.Write("> ");

                string response = ReadLineWithDeadline(endTime, out bool timedOut);

                if (timedOut) break;

                if (!string.IsNullOrWhiteSpace(response))
                    count++;
            }

            Console.WriteLine($"\nYou listed {count} items!");
            DisplayEndingMessage();
        }

        private string ReadLineWithDeadline(DateTime deadline, out bool timedOut)
        {
            timedOut = false;

            if (DateTime.Now >= deadline)
            {
                timedOut = true;
                return string.Empty;
            }

            var buffer = new System.Text.StringBuilder();

            while (true)
            {
                if (DateTime.Now >= deadline)
                {
                    timedOut = true;
                    Console.WriteLine(); // move to a fresh line
                    return buffer.ToString();
                }

               
                try
                {
                    if (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo key = Console.ReadKey(intercept: true);

                        if (key.Key == ConsoleKey.Enter)
                        {
                            Console.WriteLine();
                            return buffer.ToString();
                        }
                        else if (key.Key == ConsoleKey.Backspace)
                        {
                            if (buffer.Length > 0)
                            {
                                buffer.Length--;
                                Console.Write("\b \b");
                            }
                        }
                        else if (!char.IsControl(key.KeyChar))
                        {
                            buffer.Append(key.KeyChar);
                            Console.Write(key.KeyChar);
                        }
                    }
                    else
                    {
                        // Avoid a busy-wait CPU spin
                        Thread.Sleep(50);
                    }
                }
                catch (InvalidOperationException)
                {
                    
                    string line = Console.ReadLine();
                    if (DateTime.Now >= deadline)
                    {
                        timedOut = true;
                    }
                    return line ?? string.Empty;
                }
            }
        }
    }

   //Keeping a log of how many times activities were performed
    public static class ActivityLog
    {
        private static readonly string LogFileName =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "activity_log.txt");

        private static Dictionary<string, int> _counts = new Dictionary<string, int>();

        public static void Load()
        {
            _counts.Clear();
            if (!File.Exists(LogFileName)) return;

            try
            {
                foreach (var line in File.ReadAllLines(LogFileName))
                {
                    var parts = line.Split('|');
                    if (parts.Length == 2 && int.TryParse(parts[1], out int count))
                    {
                        _counts[parts[0]] = count;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Warning: could not read log file: {ex.Message}");
            }
        }

        public static void Save()
        {
            try
            {
                var lines = new List<string>();
                foreach (var kv in _counts)
                    lines.Add($"{kv.Key}|{kv.Value}");

                File.WriteAllLines(LogFileName, lines);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Warning: could not save log file: {ex.Message}");
            }
        }

        public static void RecordActivity(string activityName)
        {
            if (_counts.ContainsKey(activityName))
                _counts[activityName]++;
            else
                _counts[activityName] = 1;

            Save();
        }

        public static int GetCount(string activityName) =>
            _counts.ContainsKey(activityName) ? _counts[activityName] : 0;

        public static Dictionary<string, int> GetAllCounts() =>
            new Dictionary<string, int>(_counts);

        public static void Reset()
        {
            _counts.Clear();
            Save();
        }

        public static string GetLogFilePath() => LogFileName;
    }

    
    class Program
    {
        static void Main(string[] args)
        {
            ActivityLog.Load();
            bool running = true;

            while (running)
            {
                Console.Clear();
                Console.WriteLine("Menu Options:");
                Console.WriteLine("  1. Start breathing activity");
                Console.WriteLine("  2. Start reflecting activity");
                Console.WriteLine("  3. Start listing activity");
                Console.WriteLine("  4. View activity log");
                Console.WriteLine("  5. Reset activity log");
                Console.WriteLine("  6. Quit");
                Console.Write("Select a choice from the menu: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": RunActivity(new BreathingActivity()); break;
                    case "2": RunActivity(new ReflectionActivity()); break;
                    case "3": RunActivity(new ListingActivity()); break;
                    case "4": ShowLog(); break;
                    case "5": ResetLog(); break;
                    case "6":
                        running = false;
                        Console.WriteLine("\nGoodbye! Stay mindful.");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Press Enter to try again.");
                        Console.ReadLine();
                        break;
                }

                if (running && choice != "4" && choice != "5")
                {
                    Console.WriteLine("\nPress Enter to return to the menu...");
                    Console.ReadLine();
                }
            }
        }

        static void RunActivity(Activity activity)
        {
            activity.Run();
            ActivityLog.RecordActivity(activity.Name);
            Console.WriteLine($"\n(Activity recorded: {activity.Name} - total sessions: {ActivityLog.GetCount(activity.Name)})");
        }

        static void ShowLog()
        {
            Console.Clear();
            Console.WriteLine("========================================");
            Console.WriteLine("            ACTIVITY LOG");
            Console.WriteLine("========================================\n");

            var counts = ActivityLog.GetAllCounts();
            if (counts.Count == 0)
            {
                Console.WriteLine("No activities performed yet.");
            }
            else
            {
                int total = 0;
                foreach (var kv in counts)
                {
                    Console.WriteLine($"  {kv.Key,-25} : {kv.Value} session(s)");
                    total += kv.Value;
                }
                Console.WriteLine($"\n  {"TOTAL",-25} : {total} session(s)");
                Console.WriteLine($"\n  Log file: {ActivityLog.GetLogFilePath()}");
            }

            Console.WriteLine("\nPress Enter to return to the menu...");
            Console.ReadLine();
        }

        static void ResetLog()
        {
            Console.Write("\nAre you sure you want to reset the activity log? (y/n): ");
            string answer = Console.ReadLine();
            if (!string.IsNullOrEmpty(answer) && answer.Trim().ToLower() == "y")
            {
                ActivityLog.Reset();
                Console.WriteLine("Activity log has been reset.");
            }
            else
            {
                Console.WriteLine("Reset cancelled.");
            }
            Console.WriteLine("\nPress Enter to return to the menu...");
            Console.ReadLine();
        }
    }
}