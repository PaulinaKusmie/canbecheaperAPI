using System;
using System.Collections.Generic;

namespace canbecheaperAPI.Models;

public partial class CheaperProduct
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<CheaperProductPrice> CheaperProductPrices { get; set; } = new List<CheaperProductPrice>();
}
