namespace Catalog.Core.Dtos
{
    public class FilterDTO
    {
        public List<string> Categories { get; set; }
        public List<string> Brands { get; set; }
        public List<string> Gender { get; set; }
        public List<string> Types { get; set; }
        // public List<string> Size { get; set; }
    }
}