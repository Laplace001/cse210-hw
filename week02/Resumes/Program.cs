using System;

class Program
{
    static void Main(string[] args)
    {
        // This section creates Job 1
        Job job1 = new Job();
        job1._jobTitle = "Software Engineer";
        job1._company = "Microsoft";
        job1._startYear = 2019;
        job1._endYear = 2022;

        // This section creates Job 2
        Job job2 = new Job();
        job2._jobTitle = "Manager";
        job2._company = "Apple";
        job2._startYear = 2022;
        job2._endYear = 2023;

        // This section create instance
        Resume myResume = new Resume();
        myResume._name = "Allison Rose";

        // inclusion of  jobs to the resume's list
        myResume._jobs.Add(job1);
        myResume._jobs.Add(job2);

        // output complete resume
        myResume.Display();
    }
}
