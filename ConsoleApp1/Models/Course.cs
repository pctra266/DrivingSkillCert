using System;
using System.Collections.Generic;

namespace ConsoleApp1.Models;

public partial class Course
{
    public int CourseId { get; set; }

    public string CourseName { get; set; } = null!;

    public int TeacherId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public bool? IsDelete { get; set; }

    public virtual ICollection<BankQuestion> BankQuestions { get; set; } = new List<BankQuestion>();

    public virtual ICollection<Exam> Exams { get; set; } = new List<Exam>();

    public virtual ICollection<Registration> Registrations { get; set; } = new List<Registration>();

    public virtual User Teacher { get; set; } = null!;

    public override string ToString()
    {
        return $"Course ID: {CourseId}, " +
               $"Name: {CourseName}, " +
               $"Teacher: {Teacher?.ToString() ?? TeacherId.ToString()}, " +
               $"Start Date: {StartDate:yyyy-MM-dd}, " +
               $"End Date: {EndDate:yyyy-MM-dd}, " +
               $"Is Deleted: {IsDelete ?? false}, " +
               $"Bank Questions Count: {BankQuestions.Count}, " +
               $"Exams Count: {Exams.Count}, " +
               $"Registrations Count: {Registrations.Count}";
    }
}
