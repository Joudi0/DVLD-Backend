using System;

namespace Shared
{
    public class TestFullOutputDTO
    {
        public int TestID { get; set; }
        public int TestAppointmentID { get; set; }
        public TestAppointmentFullOutputDTO TestAppointmentDetails { get; set; }
        public bool TestResult { get; set; }
        public string Notes { get; set; }
        public int CreatedByUserID { get; set; }
        public UserFullOutputDTO CreatedByUserDetails { get; set; }
    }
}