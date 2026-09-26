using System;
using System.Collections.Generic;

// Comment class - tracks the name of the person and the text of the comment
public class Comment
{
    public string CommenterName { get; set; }
    public string Text { get; set; }

    public Comment(string commenterName, string text)
    {
        CommenterName = commenterName;
        Text = text;
    }
}

// Video class - tracks title, author, length (seconds), and a list of comments
public class Video
{
    public string Title { get; set; }
    public string Author { get; set; }
    public int LengthInSeconds { get; set; }
    private List<Comment> _comments = new List<Comment>();

    public Video(string title, string author, int lengthInSeconds)
    {
        Title = title;
        Author = author;
        LengthInSeconds = lengthInSeconds;
    }

    public void AddComment(Comment comment)
    {
        _comments.Add(comment);
    }

    public int GetNumberOfComments()
    {
        return _comments.Count;
    }

    public List<Comment> GetComments()
    {
        return _comments;
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        // Create the list that will hold all the videos
        List<Video> videos = new List<Video>();

        // ----- Video 1 -----
        Video video1 = new Video("Unboxing the New X200 Smartphone", "TechWithTina", 642);
        video1.AddComment(new Comment("JakeTheSnake", "That camera quality is insane!"));
        video1.AddComment(new Comment("MariaLovesTech", "Been waiting for this review all week."));
        video1.AddComment(new Comment("PixelPusher99", "Does it support wireless charging?"));
        video1.AddComment(new Comment("GadgetGuru", "Great video, very thorough as always."));
        videos.Add(video1);

        // ----- Video 2 -----
        Video video2 = new Video("15 Minute Beginner Yoga Flow", "CalmWithKaren", 905);
        video2.AddComment(new Comment("YogiBear22", "This was exactly what I needed today."));
        video2.AddComment(new Comment("SunriseSally", "Loved the breathing section at the start."));
        video2.AddComment(new Comment("ZenMasterFlex", "Can you do a longer version next time?"));
        videos.Add(video2);

        // ----- Video 3 -----
        Video video3 = new Video("How to Make Perfect Sourdough Bread", "BakingWithBen", 1187);
        video3.AddComment(new Comment("FlourPower", "My starter finally worked thanks to this!"));
        video3.AddComment(new Comment("CrustyCarla", "The crumb on that loaf is beautiful."));
        video3.AddComment(new Comment("GlutenFreeGary", "Any tips for a gluten-free version?"));
        video3.AddComment(new Comment("RiseAndShine", "Tried it twice and it came out perfect."));
        videos.Add(video3);

        // ----- Video 4 -----
        Video video4 = new Video("Top 10 Space Discoveries of the Year", "CosmicCarl", 754);
        video4.AddComment(new Comment("StarGazerSam", "The exoplanet segment blew my mind."));
        video4.AddComment(new Comment("NebulaNerd", "Please make a full video on black holes."));
        video4.AddComment(new Comment("AstroAmy", "Number 3 was my favorite discovery too!"));
        videos.Add(video4);

        // Iterate through the list and display each video's details and comments
        foreach (Video video in videos)
        {
            Console.WriteLine("==================================================");
            Console.WriteLine($"Title:  {video.Title}");
            Console.WriteLine($"Author: {video.Author}");
            Console.WriteLine($"Length: {video.LengthInSeconds} seconds");
            Console.WriteLine($"Number of Comments: {video.GetNumberOfComments()}");
            Console.WriteLine("Comments:");
            foreach (Comment comment in video.GetComments())
            {
                Console.WriteLine($"  - {comment.CommenterName}: {comment.Text}");
            }
            Console.WriteLine();
        }
    }
}