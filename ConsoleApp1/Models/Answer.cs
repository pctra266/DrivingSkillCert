using System;
using System.Collections.Generic;

namespace Model.Models;

public partial class Answer
{
    public int AnswerId { get; set; }

    public int? QuestionId { get; set; }

    public string? Answer1 { get; set; }

    public bool? IsTrue { get; set; }

    public virtual Question? Question { get; set; }
}
