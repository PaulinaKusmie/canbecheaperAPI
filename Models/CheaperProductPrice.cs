using System;
using System.Collections.Generic;

namespace canbecheaperAPI.Models;

public partial class CheaperProductPrice
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int PriceId { get; set; }

    public int TypeId { get; set; }

    public int UserId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual CheaperPrice Price { get; set; } = null!;

    public virtual CheaperProduct Product { get; set; } = null!;

    public virtual CheaperType Type { get; set; } = null!;
}
