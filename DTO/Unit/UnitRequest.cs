namespace canbecheaperAPI.DTO.Unit
{
    public record UnitRequest(
    int userId,
    int weightUnit,
    int lengthUnit,
    int volumeUnit,
    int pieceUnit
    );
}
