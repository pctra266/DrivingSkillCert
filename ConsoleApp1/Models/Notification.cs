using System;
using System.Collections.Generic;

namespace Model.Models;

public partial class Notification
{
    public int NotificationId { get; set; }

    public int UserId { get; set; }

    public string Message { get; set; } = null!;

    public DateTime? SentDate { get; set; }

    public bool? IsRead { get; set; }

    public bool? IsDelete { get; set; }

    public virtual User User { get; set; } = null!;
}
