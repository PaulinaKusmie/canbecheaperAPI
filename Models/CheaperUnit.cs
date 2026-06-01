using System;
using System.Collections.Generic;

namespace canbecheaperAPI.Models;

public partial class CheaperUnit
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int WeightUnit { get; set; }

    public int LengthUnit { get; set; }

    public int VolumeUnit { get; set; }

    public int PieceUnit { get; set; }

    public virtual CheaperUser User { get; set; } = null!;
}
