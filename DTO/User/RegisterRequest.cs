using Microsoft.AspNetCore.Identity;

namespace canbecheaperAPI.DTO.User
{
    public record RegisterRequest(string Name, string Email, string  Password);

}
