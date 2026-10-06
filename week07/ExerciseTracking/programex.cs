using System;
using System.Collections.Generic;

namespace ActivityTracker
{
    // Base Activity class
    public abstract class Activity
    {
        // Private member variables (encapsulation)
        private DateTime _date;
        private int _minutes;

        public Activity(DateTime date, int minutes)
        {
            _date = date;
            _minutes = minutes;
        }

        public DateTime Date => _date;
        public int Minutes => _minutes;

        // Abstract methods to be overridden in derived classes
        public abstract double GetDistance();   // miles
        public abstract double GetSpeed();      // mph
        public abstract double GetPace();       // min per mile

        // Summary defined in base class using overridden calculation methods
        public virtual string GetSummary()
        {
            return $"{Date:dd MMM yyyy} {GetType().Name} ({Minutes} min) - " +
                   $"Distance: {GetDistance():F1} miles, " +
                   $"Speed: {GetSpeed():F1} mph, " +
                   $"Pace: {GetPace():F1} min per mile";
        }
    }

    // Derived class for Running
    public class Running : Activity
    {
        private double _distance; // miles

        public Running(DateTime date, int minutes, double distance)
            : base(date, minutes)
        {
            _distance = distance;
        }

        public override double GetDistance() => _distance;

        public override double GetSpeed()
            => Minutes == 0 ? 0 : (_distance / Minutes) * 60;

        public override double GetPace()
            => _distance == 0 ? 0 : Minutes / _distance;
    }

    // Derived class for Cycling
    public class Cycling : Activity
    {
        private double _speed; // mph

        public Cycling(DateTime date, int minutes, double speed)
            : base(date, minutes)
        {
            _speed = speed;
        }

        public override double GetDistance()
            => (_speed * Minutes) / 60;

        public override double GetSpeed() => _speed;

        public override double GetPace()
            => _speed == 0 ? 0 : 60 / _speed;
    }

    // Derived class for Swimming
    public class Swimming : Activity
    {
        private int _laps;

        public Swimming(DateTime date, int minutes, int laps)
            : base(date, minutes)
        {
            _laps = laps;
        }

        // Distance (miles) = laps * 50 / 1000 * 0.62
        public override double GetDistance()
            => _laps * 50 / 1000.0 * 0.62;

        public override double GetSpeed()
            => Minutes == 0 ? 0 : (GetDistance() / Minutes) * 60;

        public override double GetPace()
        {
            double d = GetDistance();
            return d == 0 ? 0 : Minutes / d;
        }
    }

    // Main Program Entry Point
    class Program
    {
        static void Main(string[] args)
        {
            DateTime today = DateTime.Now;

            // Create at least one activity of each type
            Running running   = new Running(today, 30, 3.1);   // 3.1 miles
            Cycling cycling   = new Cycling(today, 45, 15.0);  // 15 mph
            Swimming swimming = new Swimming(today, 40, 30);   // 30 laps

            // Put them in the same list
            List<Activity> activities = new List<Activity>
            {
                running,
                cycling,
                swimming
            };

            Console.WriteLine("Exercise Activity Summary");
            Console.WriteLine("==========================");

            // Iterate and call GetSummary() polymorphically
            foreach (Activity activity in activities)
            {
                Console.WriteLine(activity.GetSummary());
            }
        }
    }
}