
using System;
using System.Collections.Generic;

namespace ExerciseTracking
{
    // =========================================================
    // BASE ACTIVITY CLASS
    // =========================================================
    public abstract class Activity
    {
        // Private variables - encapsulation
        private DateTime _date;
        private int _minutes;

        // Constructor
        public Activity(DateTime date, int minutes)
        {
            _date = date;
            _minutes = minutes;
        }

        // Protected properties for use by derived classes
        protected DateTime Date
        {
            get { return _date; }
        }

        protected int Minutes
        {
            get { return _minutes; }
        }

        // Abstract methods
        // Each derived class must implement these.
        public abstract double GetDistance();
        public abstract double GetSpeed();
        public abstract double GetPace();

        // Common summary method
        public virtual string GetSummary()
        {
            return Date.ToString("dd MMM yyyy") +
                   " Activity (" + Minutes + " min): " +
                   "Distance " + GetDistance().ToString("0.00") + " km, " +
                   "Speed " + GetSpeed().ToString("0.00") + " kph, " +
                   "Pace " + GetPace().ToString("0.00") + " min per km";
        }
    }


    // =========================================================
    // RUNNING CLASS
    // =========================================================
    public class Running : Activity
    {
        private double _distance;

        public Running(DateTime date, int minutes, double distance)
            : base(date, minutes)
        {
            _distance = distance;
        }

        // Distance for running
        public override double GetDistance()
        {
            return _distance;
        }

        // Speed = distance / minutes * 60
        public override double GetSpeed()
        {
            if (Minutes == 0)
            {
                return 0;
            }

            return (GetDistance() / Minutes) * 60;
        }

        // Pace = minutes / distance
        public override double GetPace()
        {
            if (GetDistance() == 0)
            {
                return 0;
            }

            return Minutes / GetDistance();
        }
    }


    // =========================================================
    // CYCLING CLASS
    // =========================================================
    public class Cycling : Activity
    {
        private double _speed;

        public Cycling(DateTime date, int minutes, double speed)
            : base(date, minutes)
        {
            _speed = speed;
        }

        // Speed is provided for cycling
        public override double GetSpeed()
        {
            return _speed;
        }

        // Distance = speed * time / 60
        public override double GetDistance()
        {
            return (GetSpeed() * Minutes) / 60;
        }

        // Pace = 60 / speed
        public override double GetPace()
        {
            if (GetSpeed() == 0)
            {
                return 0;
            }

            return 60 / GetSpeed();
        }
    }


    // =========================================================
    // SWIMMING CLASS
    // =========================================================
    public class Swimming : Activity
    {
        private int _laps;

        public Swimming(DateTime date, int minutes, int laps)
            : base(date, minutes)
        {
            _laps = laps;
        }

        // Distance in kilometers
        // Each lap = 50 meters
        // kilometers = laps * 50 / 1000
        public override double GetDistance()
        {
            return (_laps * 50.0) / 1000;
        }

        // Speed = distance / minutes * 60
        public override double GetSpeed()
        {
            if (Minutes == 0)
            {
                return 0;
            }

            return (GetDistance() / Minutes) * 60;
        }

        // Pace = minutes / distance
        public override double GetPace()
        {
            if (GetDistance() == 0)
            {
                return 0;
            }

            return Minutes / GetDistance();
        }
    }


    // =========================================================
    // PROGRAM CLASS
    // =========================================================
    class Program
    {
        static void Main(string[] args)
        {
            // Create one list containing all three activity types.
            List<Activity> activities = new List<Activity>();

            // Running: 30 minutes, 5 km
            Activity running = new Running(
                new DateTime(2026, 10, 1),
                30,
                5.0
            );

            // Cycling: 45 minutes, 20 kph
            Activity cycling = new Cycling(
                new DateTime(2026, 10, 2),
                45,
                20.0
            );

            // Swimming: 40 minutes, 40 laps
            Activity swimming = new Swimming(
                new DateTime(2026, 10, 3),
                40,
                40
            );

            // Add all activities to the same list.
            activities.Add(running);
            activities.Add(cycling);
            activities.Add(swimming);

            // Display summaries.
            Console.WriteLine("Exercise Activity Summary");
            Console.WriteLine("==========================");

            foreach (Activity activity in activities)
            {
                Console.WriteLine(activity.GetSummary());
            }

            // Keep console window open.
            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
```
