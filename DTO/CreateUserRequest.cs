using Microsoft.AspNetCore.Identity;

namespace canbecheaperAPI.DTO
{
    public record CreateUserRequest(string email, string password, string name, int age);
  
}
