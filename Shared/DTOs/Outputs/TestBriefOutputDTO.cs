using System;

namespace Shared
{
    public class TestBriefOutputDTO
    {
        public int TestID { get; set; }
        public int TestAppointmentID { get; set; }
        public TestAppointmentBriefOutputDTO TestAppointmentDetails { get; set; }
        public bool TestResult { get; set; }
        public string Notes { get; set; }
        public int CreatedByUserID { get; set; }
        public UserBriefOutputDTO CreatedByUserDetails { get; set; }
    }
}