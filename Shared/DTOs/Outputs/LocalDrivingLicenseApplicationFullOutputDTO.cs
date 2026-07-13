using System;

namespace Shared
{
    public class LocalDrivingLicenseApplicationFullOutputDTO
    {
        public int LocalDrivingLicenseApplicationID { get; set; }
        public int ApplicationID { get; set; }
        public ApplicationFullOutputDTO ApplicationDetails { get; set; }
        public int LicenseClassID { get; set; }
        public LicenseFullOutputDTO LicenseClassDetails { get; set; }
    }
}