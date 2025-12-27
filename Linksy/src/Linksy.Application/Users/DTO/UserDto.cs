using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Users.DTO
{
    public record class UserDto
    {
        public int Id { get; init; }
        public string Username { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public string Gender { get; init; } = string.Empty;
        public IEnumerable<string> Roles { get; init; } 
        public DateTime CreatedAt { get; init; }
        public DateTime? UpdatedAt { get; init; }
        public UserDto(int id, string username, string email, string firstName, string lastName, string gender, IEnumerable<string> roles, DateTime createdAt, DateTime? updatedAt)
        {
            Id = id;
            Username = username;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            Gender = gender;
            Roles = roles;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }
    }
}
