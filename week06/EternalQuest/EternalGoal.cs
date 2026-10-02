using System;

namespace EternalQuest
{
    public class EternalGoal : Goal
    {
        public EternalGoal(
            string name,
            string description,
            int points)
            : base(name, description, points)
        {
        }

        public override bool IsComplete()
        {
            return false;
        }

        public override int RecordEvent()
        {
            return GetPoints();
        }

        public override string GetStatus()
        {
            return "[ ]";
        }

        public override string GetSaveString()
        {
            return $"Eternal|{GetName()}|{GetDescription()}|{GetPoints()}";
        }
    }
}