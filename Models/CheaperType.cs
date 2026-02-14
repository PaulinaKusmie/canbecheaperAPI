using System;
using System.Collections.Generic;

namespace canbecheaperAPI.Models;

public partial class CheaperType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public int UserId { get; set; }

    public virtual ICollection<CheaperProductPrice> CheaperProductPrices { get; set; } = new List<CheaperProductPrice>();
}
