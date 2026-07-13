using System;

namespace Shared
{
    public class DriverBriefOutputDTO
    {
        public int DriverID { get; set; }
        public int PersonID { get; set; }
        public PersonBriefOutputDTO PersonDetails { get; set; }
        public int CreatedByUserID { get; set; }
        public UserBriefOutputDTO CreatedByUserDetails { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}