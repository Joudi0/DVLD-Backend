using System;

namespace Shared
{
    public class ApplicationLogFullOutputDTO
    {
        public int LogID { get; set; }
        public int? ApplicationID { get; set; }
        public ApplicationFullOutputDTO ApplicationDetails { get; set; }
        public int? UserID { get; set; }
        public UserFullOutputDTO UserDetails { get; set; }
        public DateTime? InsertedDate { get; set; }
    }
}