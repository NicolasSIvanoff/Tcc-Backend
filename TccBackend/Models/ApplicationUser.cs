using Microsoft.AspNetCore.Identity;

namespace TccBackend.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public List<int> ConteudosVisitados { get; set; } = new List<int>();
    }
}
