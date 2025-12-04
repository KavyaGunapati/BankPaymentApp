using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities
{
    public class RefreshToken
    {
        [Key] 
        public int Id { get; set; }

        [Required, StringLength(300)]
        public string Token { get; set; } = null!; // Store hashed in prod

        [Required] public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;

        public bool IsRevoked { get; set; } = false;
        public DateTime Expires { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}