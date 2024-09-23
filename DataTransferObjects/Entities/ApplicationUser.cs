using Microsoft.AspNetCore.Identity;
using System.Diagnostics.CodeAnalysis;

namespace DataTransferObjets.Entities
{
    [ExcludeFromCodeCoverage] // Hasta que se implemente
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
