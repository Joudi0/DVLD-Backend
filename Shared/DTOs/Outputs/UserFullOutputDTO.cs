using System;

namespace Shared
{
    public class UserFullOutputDTO
    {
        public int UserID { get; set; }
        public int PersonID { get; set; }
        public PersonFullOutputDTO PersonDetails { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public bool IsActive { get; set; }
        public int? UserRole { get; set; }
        public string PasswordSalt { get; set; }
    }
}