namespace NorthwindAPI.Models.DTO
{
    public class SupplierDTO
    {
        public SupplierDTO()
        {
            Products = new List<ProductDTO>();
        }

        public int SupplierId { get; set; }
        public string CompanyName { get; set; }
        public string? ContactName { get; set; }
        public string? ContactTitle { get; set; }
        public string? Country { get; set; }
        public int TotalProducts { get; init; }
        //New readonly property which returns the total number of products supplier has
        public virtual ICollection<ProductDTO> Products { get; set; }
    }
}
