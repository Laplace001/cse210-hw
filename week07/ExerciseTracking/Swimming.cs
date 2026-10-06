```csharp
using System;

namespace ExerciseTracking
{
    public class Swimming : Activity
    {
        private int _laps;

        // Constructor
        public Swimming(DateTime date, int minutes, int laps)
            : base(date, minutes)
        {
            _laps = laps;
        }

        // Distance in kilometers
        // laps * 50 meters / 1000
        public override double GetDistance()
        {
            return (_laps * 50.0) / 1000;
        }

        // Speed = distance / minutes * 60
        public override double GetSpeed()
        {
            return (GetDistance() / Minutes) * 60;
        }

        // Pace = minutes / distance
        public override double GetPace()
        {
            return Minutes / GetDistance();
        }
    }
}
```
