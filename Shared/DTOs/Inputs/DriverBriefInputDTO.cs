using System;

namespace Shared
{
    public class DriverBriefInputDTO
    {
        public int DriverID { get; set; }
        public int PersonID { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}