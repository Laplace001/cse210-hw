```csharp
using System;

namespace ExerciseTracking
{
    // Base class for all activities
    public abstract class Activity
    {
        // Private member variables for encapsulation
        private DateTime _date;
        private int _minutes;

        // Constructor
        public Activity(DateTime date, int minutes)
        {
            _date = date;
            _minutes = minutes;
        }

        // Protected properties allow derived classes to access the values
        protected DateTime Date
        {
            get { return _date; }
        }

        protected int Minutes
        {
            get { return _minutes; }
        }

        // These methods must be implemented by derived classes
        public abstract double GetDistance();

        public abstract double GetSpeed();

        public abstract double GetPace();

        // Common summary method
        public virtual string GetSummary()
        {
            return $"{Date:dd MMM yyyy} Activity ({Minutes} min): " +
                   $"Distance {GetDistance():0.00} km, " +
                   $"Speed {GetSpeed():0.00} kph, " +
                   $"Pace: {GetPace():0.00} min per km";
        }
    }
}
```
