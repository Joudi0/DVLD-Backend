using System;

namespace Shared
{
    public class RefreshRequestDTO
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string RefreshToken { get; set; }
    }
}