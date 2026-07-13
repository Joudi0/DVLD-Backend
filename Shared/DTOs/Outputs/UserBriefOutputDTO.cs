using System;

namespace Shared
{
    public class UserBriefOutputDTO
    {
        public int UserID { get; set; }
        public int PersonID { get; set; }
        public PersonBriefOutputDTO PersonDetails { get; set; }
        public string UserName { get; set; }
        public bool IsActive { get; set; }
        public int? UserRole { get; set; }
    }
}