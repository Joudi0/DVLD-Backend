using System;

namespace Shared
{
    public class UserFullInputDTO
    {
        public int UserID { get; set; }
        public int PersonID { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public bool IsActive { get; set; }
        public int? UserRole { get; set; }
        public string PasswordSalt { get; set; }
        public string Password { get; set; }
    }
}