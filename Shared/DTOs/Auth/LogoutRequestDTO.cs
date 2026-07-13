using System;

namespace Shared
{
    public class LogoutRequestDTO
    {
        public int UserID { get; set; }
        public string RefreshToken { get; set; }
    }
}