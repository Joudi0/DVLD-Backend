using System;

namespace Shared
{
    public class ApplicationLogFullInputDTO
    {
        public int LogID { get; set; }
        public int? ApplicationID { get; set; }
        public int? UserID { get; set; }
        public DateTime? InsertedDate { get; set; }
    }
}