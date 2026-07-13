using System;

namespace Shared
{
    public class LicenseFullOutputDTO
    {
        public int LicenseID { get; set; }
        public int ApplicationID { get; set; }
        public ApplicationFullOutputDTO ApplicationDetails { get; set; }
        public int DriverID { get; set; }
        public DriverFullOutputDTO DriverDetails { get; set; }
        public int LicenseClass { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Notes { get; set; }
        public decimal PaidFees { get; set; }
        public bool IsActive { get; set; }
        public byte IssueReason { get; set; }
        public int CreatedByUserID { get; set; }
        public UserFullOutputDTO CreatedByUserDetails { get; set; }
    }
}