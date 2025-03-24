using System;
using System.Collections.Generic;

namespace Model.Models;

public partial class BankQuestion
{
    public int BankId { get; set; }

    public int? CourseId { get; set; }

    public string? BankName { get; set; }

    public virtual Course? Course { get; set; }

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
}
