using System;

namespace Shared
{
    public class DetainedLicenseFullOutputDTO
    {
        public int DetainID { get; set; }
        public int LicenseID { get; set; }
        public LicenseFullOutputDTO LicenseDetails { get; set; }
        public DateTime DetainDate { get; set; }
        public decimal FineFees { get; set; }
        public int CreatedByUserID { get; set; }
        public UserFullOutputDTO CreatedByUserDetails { get; set; }
        public bool IsReleased { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public int? ReleasedByUserID { get; set; }
        public UserFullOutputDTO ReleasedByUserDetails { get; set; }
        public int? ReleaseApplicationID { get; set; }
        public ApplicationFullOutputDTO ReleaseApplicationDetails { get; set; }
    }
}