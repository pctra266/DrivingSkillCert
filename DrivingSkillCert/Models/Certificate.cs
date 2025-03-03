using System;
using System.Collections.Generic;

namespace DrivingSkillCert.Models;

public partial class Certificate
{
    public int CertificateId { get; set; }

    public int UserId { get; set; }

    public DateOnly IssuedDate { get; set; }

    public DateOnly ExpirationDate { get; set; }

    public string CertificateCode { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
