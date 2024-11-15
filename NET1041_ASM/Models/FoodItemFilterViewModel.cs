namespace NET1041_ASM.Models
{
    public class FoodItemFilterViewModel
    {
        public string Keyword { get; set; }
        public decimal? PriceFrom { get; set; }
        public decimal? PriceTo { get; set; }
        public int? CategoryID { get; set; }
        public string SortBy { get; set; } = "Name";
        public string SortOrder { get; set; } = "asc";
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 5;

        public List<FoodItem> FoodItems { get; set; }
        public List<Category> Categories { get; set; }
    }
}
