using System;

namespace Shared
{
    public class TestAppointmentFullOutputDTO
    {
        public int TestAppointmentID { get; set; }
        public int TestTypeID { get; set; }
        public TestTypeFullOutputDTO TestTypeDetails { get; set; }
        public int LocalDrivingLicenseApplicationID { get; set; }
        public LicenseFullOutputDTO LocalDrivingLicenseApplicationDetails { get; set; }
        public DateTime AppointmentDate { get; set; }
        public decimal PaidFees { get; set; }
        public int CreatedByUserID { get; set; }
        public UserFullOutputDTO CreatedByUserDetails { get; set; }
        public bool IsLocked { get; set; }
        public int? RetakeTestApplicationID { get; set; }
        public TestFullOutputDTO RetakeTestApplicationDetails { get; set; }
    }
}