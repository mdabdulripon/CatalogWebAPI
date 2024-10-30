namespace Catalog.Core.RequestHelpers
{
    public class ProductParams : PaginationParams
	{
        public string MerchantName { get; set; }
        public string OrderBy { get; set; }
        public string SearchTerm { get; set; }
        public string Categories { get; set; }
        public string Types { get; set; }
        public string Brands { get; set; }
        public string Gender { get; set; }
        public string Size { get; set; }

    }
}

