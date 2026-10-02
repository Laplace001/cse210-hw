using System;

namespace EternalQuest
{
    public class ChecklistGoal : Goal
    {
        private int _target;
        private int _amountCompleted;
        private int _bonus;

        public ChecklistGoal(
            string name,
            string description,
            int points,
            int target,
            int bonus,
            int amountCompleted = 0)
            : base(name, description, points)
        {
            _target = target;
            _bonus = bonus;
            _amountCompleted = amountCompleted;
        }

        public override bool IsComplete()
        {
            return _amountCompleted >= _target;
        }

        public override int RecordEvent()
        {
            if (IsComplete())
            {
                Console.WriteLine("This checklist goal has already been completed.");
                return 0;
            }

            _amountCompleted++;

            int earnedPoints = GetPoints();

            if (_amountCompleted == _target)
            {
                earnedPoints += _bonus;

                Console.WriteLine(
                    $"Congratulations! You completed the goal and earned a {_bonus} point bonus!");
            }

            return earnedPoints;
        }

        public override string GetStatus()
        {
            return $"[ {(IsComplete() ? "X" : " ")} ] Completed {_amountCompleted}/{_target} times";
        }

        public override string GetSaveString()
        {
            return $"Checklist|{GetName()}|{GetDescription()}|{GetPoints()}|{_target}|{_bonus}|{_amountCompleted}";
        }
    }
}