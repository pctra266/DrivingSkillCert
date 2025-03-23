using System;
using System.Collections.Generic;

namespace ConsoleApp1.Models;

public partial class Result
{
    public int ResultId { get; set; }

    public int ExamId { get; set; }

    public int UserId { get; set; }

    public decimal Score { get; set; }

    public bool PassStatus { get; set; }

    public bool? IsDelete { get; set; }

    public virtual Exam Exam { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
