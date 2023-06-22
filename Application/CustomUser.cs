using Microsoft.AspNetCore.Identity;

namespace Application
{
    public class CustomUser:IdentityUser
    {
        public string? Picture { get; set; }
    }
}