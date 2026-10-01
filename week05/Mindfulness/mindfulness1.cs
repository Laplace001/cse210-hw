using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace MindfulnessProgram
{
    // ============================================================
    // BASE ACTIVITY CLASS
    // ============================================================
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

        public string Name
        {
            get { return _name; }
        }

        public string Description
        {
            get { return _description; }
        }

        public int Duration
        {
            get { return _duration; }
            protected set { _duration = value; }
        }

        public void DisplayStartingMessage()
        {
            Console.Clear();

            Console.WriteLine("Welcome to the " + _name + ".\n");
            Console.WriteLine(_description);
            Console.WriteLine();

            Console.Write("How long, in seconds, would you like for your session? ");

            int seconds;

            while (!int.TryParse(Console.ReadLine(), out seconds) || seconds <= 0)
            {
                Console.Write("Please enter a valid positive number of seconds: ");
            }

            _duration = seconds;

            Console.WriteLine("\nGet ready to begin...");
            ShowSpinner(3);
        }

        public void DisplayEndingMessage()
        {
            Console.WriteLine("\nWell done!!");
            ShowSpinner(3);

            Console.WriteLine(
                "\nYou have completed another " +
                _duration +
                " seconds of the " +
                _name +
                ".");

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
                Console.Write(i);
                Thread.Sleep(1000);

                Console.Write("\b \b");
            }
        }

        public abstract void Run();
    }


    // ============================================================
    // BREATHING ACTIVITY
    // ============================================================
    public class BreathingActivity : Activity
    {
        public BreathingActivity()
            : base(
                "Breathing Activity",
                "This activity will help you relax by walking you through " +
                "breathing in and out slowly. Clear your mind and focus " +
                "on your breathing.")
        {
        }

        public override void Run()
        {
            DisplayStartingMessage();

            DateTime endTime = DateTime.Now.AddSeconds(Duration);

            while (DateTime.Now < endTime)
            {
                int remaining =
                    (int)Math.Ceiling(
                        (endTime - DateTime.Now).TotalSeconds);

                if (remaining <= 0)
                    break;

                int inSeconds = Math.Min(4, remaining);

                Console.Write("\nBreathe in... ");
                ShowCountdown(inSeconds);

                remaining =
                    (int)Math.Ceiling(
                        (endTime - DateTime.Now).TotalSeconds);

                if (remaining <= 0)
                    break;

                int outSeconds = Math.Min(4, remaining);

                Console.Write("\nBreathe out... ");
                ShowCountdown(outSeconds);

                Console.WriteLine();
            }

            DisplayEndingMessage();
        }
    }


    // ============================================================
    // REFLECTION ACTIVITY
    // ============================================================
    public class ReflectionActivity : Activity
    {
        private List<string> _prompts;
        private List<string> _questions;
        private Random _random;

        public ReflectionActivity()
            : base(
                "Reflection Activity",
                "This activity will help you reflect on times in your life " +
                "when you have shown strength and resilience. This will " +
                "help you recognize the power you have and how you can use " +
                "it in other aspects of your life.")
        {
            _random = new Random();

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

        private string GetRandomPrompt()
        {
            return _prompts[_random.Next(_prompts.Count)];
        }

        private string GetRandomQuestion()
        {
            return _questions[_random.Next(_questions.Count)];
        }

        public override void Run()
        {
            DisplayStartingMessage();

            Console.WriteLine("\nConsider the following prompt:\n");
            Console.WriteLine(" --- " + GetRandomPrompt() + " --- \n");

            Console.WriteLine(
                "When you have something in mind, press Enter to continue.");

            Console.ReadLine();

            Console.WriteLine(
                "Now ponder on each of the following questions " +
                "as they relate to this experience.");

            Console.Write("You may begin in: ");
            ShowCountdown(5);
            Console.WriteLine();

            DateTime endTime = DateTime.Now.AddSeconds(Duration);

            while (DateTime.Now < endTime)
            {
                Console.Write(
                    "\n> " + GetRandomQuestion() + " ");

                ShowSpinner(5);
            }

            DisplayEndingMessage();
        }
    }


    // ============================================================
    // LISTING ACTIVITY
    // ============================================================
    public class ListingActivity : Activity
    {
        private List<string> _prompts;
        private Random _random;

        public ListingActivity()
            : base(
                "Listing Activity",
                "This activity will help you reflect on the good things " +
                "in your life by having you list as many things as you " +
                "can in a certain area.")
        {
            _random = new Random();

            _prompts = new List<string>
            {
                "Who are people that you appreciate?",
                "What are personal strengths of yours?",
                "Who are people that you have helped this week?",
                "When have you felt the Holy Ghost this month?",
                "Who are some of your personal heroes?"
            };
        }

        private string GetRandomPrompt()
        {
            return _prompts[_random.Next(_prompts.Count)];
        }

        public override void Run()
        {
            DisplayStartingMessage();

            Console.WriteLine(
                "\nList as many responses as you can to the following prompt:");

            Console.WriteLine(
                " --- " + GetRandomPrompt() + " --- \n");

            Console.Write("You may begin in: ");
            ShowCountdown(5);
            Console.WriteLine();

            DateTime endTime = DateTime.Now.AddSeconds(Duration);

            int count = 0;

            while (DateTime.Now < endTime)
            {
                Console.Write("> ");

                string response = ReadLineWithDeadline(
                    endTime,
                    out bool timedOut);

                if (timedOut)
                    break;

                if (!string.IsNullOrWhiteSpace(response))
                {
                    count++;
                }
            }

            Console.WriteLine(
                "\nYou listed " + count + " items!");

            DisplayEndingMessage();
        }

        private string ReadLineWithDeadline(
            DateTime deadline,
            out bool timedOut)
        {
            timedOut = false;

            if (DateTime.Now >= deadline)
            {
                timedOut = true;
                return "";
            }

            string response = "";

            while (true)
            {
                if (DateTime.Now >= deadline)
                {
                    timedOut = true;
                    Console.WriteLine();
                    return response;
                }

                try
                {
                    if (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo key =
                            Console.ReadKey(true);

                        if (key.Key == ConsoleKey.Enter)
                        {
                            Console.WriteLine();
                            return response;
                        }

                        if (key.Key == ConsoleKey.Backspace)
                        {
                            if (response.Length > 0)
                            {
                                response =
                                    response.Substring(
                                        0,
                                        response.Length - 1);

                                Console.Write("\b \b");
                            }
                        }
                        else if (!char.IsControl(key.KeyChar))
                        {
                            response += key.KeyChar;
                            Console.Write(key.KeyChar);
                        }
                    }
                    else
                    {
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

                    return line ?? "";
                }
            }
        }
    }


    // ============================================================
    // ACTIVITY LOG
    // ============================================================
    public static class ActivityLog
    {
        private static readonly string LogFileName =
            Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "activity_log.txt");

        private static Dictionary<string, int> _counts =
            new Dictionary<string, int>();

        public static void Load()
        {
            _counts.Clear();

            if (!File.Exists(LogFileName))
                return;

            try
            {
                string[] lines =
                    File.ReadAllLines(LogFileName);

                foreach (string line in lines)
                {
                    string[] parts = line.Split('|');

                    if (parts.Length == 2)
                    {
                        int count;

                        if (int.TryParse(parts[1], out count))
                        {
                            _counts[parts[0]] = count;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    "Warning: could not read log file: " +
                    ex.Message);
            }
        }

        public static void Save()
        {
            try
            {
                List<string> lines =
                    new List<string>();

                foreach (KeyValuePair<string, int> item in _counts)
                {
                    lines.Add(
                        item.Key + "|" + item.Value);
                }

                File.WriteAllLines(
                    LogFileName,
                    lines);
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    "Warning: could not save log file: " +
                    ex.Message);
            }
        }

        public static void RecordActivity(string activityName)
        {
            if (_counts.ContainsKey(activityName))
            {
                _counts[activityName]++;
            }
            else
            {
                _counts[activityName] = 1;
            }

            Save();
        }

        public static int GetCount(string activityName)
        {
            if (_counts.ContainsKey(activityName))
                return _counts[activityName];

            return 0;
        }

        public static Dictionary<string, int> GetAllCounts()
        {
            return new Dictionary<string, int>(_counts);
        }

        public static void Reset()
        {
            _counts.Clear();
            Save();
        }

        public static string GetLogFilePath()
        {
            return LogFileName;
        }
    }


    // ============================================================
    // MAIN PROGRAM
    // ============================================================
    class Program
    {
        static void Main(string[] args)
        {
            ActivityLog.Load();

            bool running = true;

            while (running)
            {
                Console.Clear();

                Console.WriteLine("========================================");
                Console.WriteLine("        MINDFULNESS PROGRAM");
                Console.WriteLine("========================================");

                Console.WriteLine("\nMenu Options:");
                Console.WriteLine("1. Start breathing activity");
                Console.WriteLine("2. Start reflecting activity");
                Console.WriteLine("3. Start listing activity");
                Console.WriteLine("4. View activity log");
                Console.WriteLine("5. Reset activity log");
                Console.WriteLine("6. Quit");

                Console.Write("\nSelect a choice from the menu: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        RunActivity(
                            new BreathingActivity());
                        break;

                    case "2":
                        RunActivity(
                            new ReflectionActivity());
                        break;

                    case "3":
                        RunActivity(
                            new ListingActivity());
                        break;

                    case "4":
                        ShowLog();
                        break;

                    case "5":
                        ResetLog();
                        break;

                    case "6":
                        running = false;

                        Console.WriteLine(
                            "\nGoodbye! Stay mindful.");

                        break;

                    default:
                        Console.WriteLine(
                            "\nInvalid choice.");

                        Console.WriteLine(
                            "Press Enter to try again.");

                        Console.ReadLine();
                        break;
                }

                if (running &&
                    choice != "4" &&
                    choice != "5")
                {
                    Console.WriteLine(
                        "\nPress Enter to return to the menu...");

                    Console.ReadLine();
                }
            }
        }


        // ========================================================
        // RUN ACTIVITY
        // ========================================================
        static void RunActivity(Activity activity)
        {
            activity.Run();

            ActivityLog.RecordActivity(
                activity.Name);

            Console.WriteLine(
                "\n(Activity recorded: " +
                activity.Name +
                " - total sessions: " +
                ActivityLog.GetCount(activity.Name) +
                ")");
        }


        // ========================================================
        // SHOW LOG
        // ========================================================
        static void ShowLog()
        {
            Console.Clear();

            Console.WriteLine("========================================");
            Console.WriteLine("             ACTIVITY LOG");
            Console.WriteLine("========================================\n");

            Dictionary<string, int> counts =
                ActivityLog.GetAllCounts();

            if (counts.Count == 0)
            {
                Console.WriteLine(
                    "No activities performed yet.");
            }
            else
            {
                int total = 0;

                foreach (
                    KeyValuePair<string, int> item
                    in counts)
                {
                    Console.WriteLine(
                        "  " +
                        item.Key +
                        " : " +
                        item.Value +
                        " session(s)");

                    total += item.Value;
                }

                Console.WriteLine(
                    "\nTOTAL : " +
                    total +
                    " session(s)");

                Console.WriteLine(
                    "\nLog file: " +
                    ActivityLog.GetLogFilePath());
            }

            Console.WriteLine(
                "\nPress Enter to return to the menu...");

            Console.ReadLine();
        }


        // ========================================================
        // RESET LOG
        // ========================================================
        static void ResetLog()
        {
            Console.Write(
                "\nAre you sure you want to reset " +
                "the activity log? (y/n): ");

            string answer = Console.ReadLine();

            if (!string.IsNullOrEmpty(answer) &&
                answer.Trim().ToLower() == "y")
            {
                ActivityLog.Reset();

                Console.WriteLine(
                    "Activity log has been reset.");
            }
            else
            {
                Console.WriteLine(
                    "Reset cancelled.");
            }

            Console.WriteLine(
                "\nPress Enter to return to the menu...");

            Console.ReadLine();
        }
    }
}