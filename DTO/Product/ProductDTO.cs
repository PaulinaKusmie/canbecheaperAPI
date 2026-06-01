namespace canbecheaperAPI.DTO.Product
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ProductDTO(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
