using System;
using System.Collections.Generic;

namespace canbecheaperAPI.Models;

public partial class CheaperPrice
{
    public int Id { get; set; }

    public string Price { get; set; } = null!;

    public virtual ICollection<CheaperProductPrice> CheaperProductPrices { get; set; } = new List<CheaperProductPrice>();
}
