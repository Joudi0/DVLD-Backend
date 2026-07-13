using System;

namespace Shared
{
    public class DriverFullOutputDTO
    {
        public int DriverID { get; set; }
        public int PersonID { get; set; }
        public PersonFullOutputDTO PersonDetails { get; set; }
        public int CreatedByUserID { get; set; }
        public UserFullOutputDTO CreatedByUserDetails { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}