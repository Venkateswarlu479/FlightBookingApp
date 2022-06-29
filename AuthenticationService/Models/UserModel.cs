using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Models
{
    /// <summary>
    /// UserModel class
    /// </summary>
    public class UserModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime TokenExpires { get; set; }
        public DateTime TokenCreated { get; set; }
    }
}
