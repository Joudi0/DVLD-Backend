using System;

namespace Shared
{
    public class LocalDrivingLicenseApplicationBriefOutputDTO
    {
        public int LocalDrivingLicenseApplicationID { get; set; }
        public int ApplicationID { get; set; }
        public ApplicationBriefOutputDTO ApplicationDetails { get; set; }
        public int LicenseClassID { get; set; }
        public LicenseBriefOutputDTO LicenseClassDetails { get; set; }
    }
}