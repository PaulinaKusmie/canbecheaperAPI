using System;
using System.Collections.Generic;

namespace canbecheaperAPI.Models;

public partial class CheaperUser
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public bool EmailConfirmed { get; set; }

    public int? EmailCode { get; set; }

    public DateTime? EmailCodeExpiresAt { get; set; }

    public sbyte EmailCodeAttempts { get; set; }
}
