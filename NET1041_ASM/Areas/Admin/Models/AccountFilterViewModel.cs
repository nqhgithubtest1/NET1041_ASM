using NET1041_ASM.Models;

namespace NET1041_ASM.Areas.Admin.Models
{
    public class AccountFilterViewModel
    {
        public string Keyword { get; set; }
        public bool? IsActive { get; set; }
        public string Role { get; set; }
        public string SortBy { get; set; }
        public string SortOrder { get; set; } = "asc";
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public List<User> Accounts { get; set; }
    }
}
