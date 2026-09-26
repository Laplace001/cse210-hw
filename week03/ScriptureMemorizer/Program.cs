using System;
using System.Collections.Generic;


public class Word
{
    private string _text;
    private bool _isHidden;

    public Word(string text)
    {
        _text = text;
        _isHidden = false;
    }

    public void Hide()
    {
        _isHidden = true;
    }

    public void Show()
    {
        _isHidden = false;
    }

    public bool IsHidden()
    {
        return _isHidden;
    }

    public string GetDisplayText()
    {
        if (_isHidden)
        {
            return new string('_', _text.Length);
        }
        return _text;
    }
}


public class Reference
{
    private string _book;
    private int _chapter;
    private int _startVerse;
    private int _endVerse;

    
    public Reference(string book, int chapter, int verse)
    {
        _book = book;
        _chapter = chapter;
        _startVerse = verse;
        _endVerse = verse;
    }

   
    public Reference(string book, int chapter, int startVerse, int endVerse)
    {
        _book = book;
        _chapter = chapter;
        _startVerse = startVerse;
        _endVerse = endVerse;
    }

    public string GetDisplayText()
    {
        if (_startVerse == _endVerse)
        {
            return $"{_book} {_chapter}:{_startVerse}";
        }
        return $"{_book} {_chapter}:{_startVerse}-{_endVerse}";
    }
}


public class Scripture
{
    private Reference _reference;
    private List<Word> _words;
    private Random _random;

    public Scripture(Reference reference, string text)
    {
        _reference = reference;
        _words = new List<Word>();
        _random = new Random();

        string[] wordArray = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        foreach (string word in wordArray)
        {
            _words.Add(new Word(word));
        }
    }

    public string GetDisplayText()
    {
        string result = _reference.GetDisplayText() + " ";
        foreach (Word word in _words)
        {
            result += word.GetDisplayText() + " ";
        }
        return result.Trim();
    }

    
    public void HideRandomWords(int count)
    {
        List<Word> unhiddenWords = new List<Word>();
        foreach (Word word in _words)
        {
            if (!word.IsHidden())
            {
                unhiddenWords.Add(word);
            }
        }

       
        if (unhiddenWords.Count == 0)
        {
            return;
        }

           int wordsToHide = Math.Min(count, unhiddenWords.Count);
        for (int i = 0; i < wordsToHide; i++)
        {
            int index = _random.Next(unhiddenWords.Count);
            unhiddenWords[index].Hide();
            unhiddenWords.RemoveAt(index);
        }
    }

    public bool IsCompletelyHidden()
    {
        foreach (Word word in _words)
        {
            if (!word.IsHidden())
            {
                return false;
            }
        }
        return true;
    }
}

// Main program
public class Program
{
    public static void Main(string[] args)
    {
                Reference reference = new Reference("Proverbs", 3, 5, 6);
        string text = "Trust in the Lord with all thine heart and lean not unto thine own understanding";
        Scripture scripture = new Scripture(reference, text);

       
        while (true)
        {
            Console.Clear();
            Console.WriteLine(scripture.GetDisplayText());
            Console.WriteLine();
            Console.WriteLine("Press Enter to continue or type 'quit' to exit:");
            string input = Console.ReadLine();

            if (input != null && input.Trim().ToLower() == "quit")
            {
                break;
            }

            if (scripture.IsCompletelyHidden())
            {
                
                            Console.Clear();
                Console.WriteLine(scripture.GetDisplayText());
                break;
            }

            
            scripture.HideRandomWords(3);

            if (scripture.IsCompletelyHidden())
            {
                Console.Clear();
                Console.WriteLine(scripture.GetDisplayText());
                break;
            }
        }
    }
}