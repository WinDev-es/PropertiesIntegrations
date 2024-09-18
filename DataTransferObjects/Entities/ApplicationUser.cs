using Microsoft.AspNetCore.Identity;

namespace DataTransferObjets.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string Names { get; set; }
        public string Surnames { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Role { get; set; }
    }
}
