using System;
using System.Collections.Generic;

namespace JournalProgram
{
    public class PromptGenerator
    {
        private List<string> _prompts = new List<string>
        {
            "Who was the most interesting person I interacted with today?",
            "What was the best part of my day?",
            "How did I see the hand of the Lord in my life today?",
            "What was the strongest emotion I felt today?",
            "If I had one thing I could do over today, what would it be?",
            "What is one thing I learned today?",
            "What am I grateful for today?",
            "What challenge did I face today and how did I handle it?",
            "What is one goal I want to accomplish tomorrow?",
            "What made me smile today?"
            "What's of first task tomorrow"
            "What activity make you smile today"
        };

        private Random _random = new Random();

        public string GetRandomPrompt()
        {
            int index = _random.Next(_prompts.Count);
            return _prompts[index];
        }
    }
}