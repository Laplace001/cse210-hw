```csharp
using System;

namespace ExerciseTracking
{
    public class Cycling : Activity
    {
        private double _speed;

        // Constructor
        public Cycling(DateTime date, int minutes, double speed)
            : base(date, minutes)
        {
            _speed = speed;
        }

        // Speed is already provided for cycling
        public override double GetSpeed()
        {
            return _speed;
        }

        // Distance = speed * time / 60
        public override double GetDistance()
        {
            return GetSpeed() * Minutes / 60;
        }

        // Pace = 60 / speed
        public override double GetPace()
        {
            return 60 / GetSpeed();
        }
    }
}
```
