using System;

namespace Shared
{
    public class TestAppointmentBriefOutputDTO
    {
        public int TestAppointmentID { get; set; }
        public int TestTypeID { get; set; }
        public TestTypeBriefOutputDTO TestTypeDetails { get; set; }
        public int LocalDrivingLicenseApplicationID { get; set; }
        public LicenseBriefOutputDTO LocalDrivingLicenseApplicationDetails { get; set; }
        public DateTime AppointmentDate { get; set; }
        public decimal PaidFees { get; set; }
        public int CreatedByUserID { get; set; }
        public UserBriefOutputDTO CreatedByUserDetails { get; set; }
        public bool IsLocked { get; set; }
        public int? RetakeTestApplicationID { get; set; }
        public TestBriefOutputDTO RetakeTestApplicationDetails { get; set; }
    }
}