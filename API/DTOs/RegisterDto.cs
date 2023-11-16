using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class RegisterDto
    {
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name should be at least (2) and maximum (50) characters")]
        public string FirstName { get; set; } = string.Empty;

        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name should be at least (2) and maximum (50) characters")]
        public string LastName { get; set; } = string.Empty;

        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [StringLength(50, MinimumLength = 5, ErrorMessage = "Password should be at least (5) and maximum (50) characters")]
        public string Password { get; set; }
    }
}
