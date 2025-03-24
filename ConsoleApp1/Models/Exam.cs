using System;
using System.Collections.Generic;

namespace Model.Models;

public partial class Exam
{
    public int ExamId { get; set; }

    public int CourseId { get; set; }

    public DateTime Date { get; set; }

    public string Room { get; set; } = null!;

    public string? Type { get; set; }

    public bool? IsDelete { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual ICollection<Result> Results { get; set; } = new List<Result>();
}
