using System;
using System.Collections.Generic;

namespace ConsoleApp1.Models;

public partial class Question
{
    public int QuestionId { get; set; }

    public int? BankId { get; set; }

    public string? Question1 { get; set; }

    public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();

    public virtual BankQuestion? Bank { get; set; }
}
