using System;

namespace Shared
{
    public class ApplicationLogBriefOutputDTO
    {
        public int LogID { get; set; }
        public int? ApplicationID { get; set; }
        public ApplicationBriefOutputDTO ApplicationDetails { get; set; }
        public int? UserID { get; set; }
        public UserBriefOutputDTO UserDetails { get; set; }
        public DateTime? InsertedDate { get; set; }
    }
}