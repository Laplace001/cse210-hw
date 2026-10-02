using System;

namespace EternalQuest
{
    public class SimpleGoal : Goal
    {
        private bool _isComplete;

        public SimpleGoal(
            string name,
            string description,
            int points,
            bool isComplete = false)
            : base(name, description, points)
        {
            _isComplete = isComplete;
        }

        public override bool IsComplete()
        {
            return _isComplete;
        }

        public override int RecordEvent()
        {
            if (!_isComplete)
            {
                _isComplete = true;
                return GetPoints();
            }

            Console.WriteLine("This goal has already been completed.");
            return 0;
        }

        public override string GetStatus()
        {
            return _isComplete ? "[X]" : "[ ]";
        }

        public override string GetSaveString()
        {
            return $"Simple|{GetName()}|{GetDescription()}|{GetPoints()}|{_isComplete}";
        }
    }
}