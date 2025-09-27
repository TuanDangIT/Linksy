using Linksy.Domain.Abstractions;
using Linksy.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Domain.Entities
{
    public class User : IdentityUser<int>, IAuditable
    {
        public string FirstName { get; private set; } = string.Empty;
        public string LastName { get; private set; } = string.Empty;
        public Gender Gender { get; private set; }
        public string? RefreshToken { get; private set; }
        public DateTime? RefreshTokenExpiryDate { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public User(string email, string username, string firstName, string lastName, Gender gender) : base(username)
        {
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            Gender = gender;
        }
        private User()
        {

        }
        public void SetRefreshToken(string refreshToken, DateTime expiryDate)
        {
            RefreshToken = refreshToken;
            RefreshTokenExpiryDate = expiryDate;
        }
    }
}
