using System;
using System.Collections.Generic;

namespace ConsoleApp1.Models;

public partial class Certificate
{
    public int CertificateId { get; set; }

    public int UserId { get; set; }

    public DateOnly IssuedDate { get; set; }

    public DateOnly ExpirationDate { get; set; }

    public string CertificateCode { get; set; } = null!;

    public bool? IsDelete { get; set; }

    public virtual User User { get; set; } = null!;
}
