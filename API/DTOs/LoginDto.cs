using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.DTOs
{
    public class LoginDto
    {
        public string UserName { get; set; }

        public string Password { get; set; }
    }
}
