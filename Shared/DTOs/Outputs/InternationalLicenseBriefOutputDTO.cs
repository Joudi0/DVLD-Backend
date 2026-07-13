using System;

namespace Shared
{
    public class InternationalLicenseBriefOutputDTO
    {
        public int InternationalLicenseID { get; set; }
        public int ApplicationID { get; set; }
        public ApplicationBriefOutputDTO ApplicationDetails { get; set; }
        public int DriverID { get; set; }
        public DriverBriefOutputDTO DriverDetails { get; set; }
        public int IssuedUsingLocalLicenseID { get; set; }
        public LicenseBriefOutputDTO IssuedUsingLocalLicenseDetails { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsActive { get; set; }
        public int CreatedByUserID { get; set; }
        public UserBriefOutputDTO CreatedByUserDetails { get; set; }
    }
}