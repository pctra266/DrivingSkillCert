using System;
using System.Collections.Generic;

namespace templateWrong.Models;

public partial class Course
{
    public int CourseId { get; set; }

    public string CourseName { get; set; } = null!;

    public int TeacherId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public bool? IsDelete { get; set; }

    public virtual ICollection<Exam> Exams { get; set; } = new List<Exam>();

    public virtual ICollection<Registration> Registrations { get; set; } = new List<Registration>();

    public virtual User Teacher { get; set; } = null!;
}
