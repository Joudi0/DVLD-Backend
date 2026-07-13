using System;

namespace Shared
{
    public class RegisterRequestDTO
    {
        public int PersonID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}