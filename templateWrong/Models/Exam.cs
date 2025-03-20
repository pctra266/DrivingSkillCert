using System;
using System.Collections.Generic;

namespace templateWrong.Models;

public partial class Exam
{
    public int ExamId { get; set; }

    public int CourseId { get; set; }

    public DateOnly Date { get; set; }

    public string Room { get; set; } = null!;

    public bool? IsDelete { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual ICollection<Result> Results { get; set; } = new List<Result>();
}
