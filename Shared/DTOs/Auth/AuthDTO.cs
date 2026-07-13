using System;

namespace Shared
{
    public class AuthDTO
    {
        public int UserID { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public enRoles UserRoleID { get; set; }
    }
}