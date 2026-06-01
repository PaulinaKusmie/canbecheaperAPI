namespace canbecheaperAPI.DTO.ProductPrice
{
    public record ProductPriceResponse(int id, int productId, int priceId, int typeId, int userId, DateTime? CreatedAt, string name , double price);

}
