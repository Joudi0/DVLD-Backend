using System;

namespace Shared
{
    public class DetainedLicenseBriefOutputDTO
    {
        public int DetainID { get; set; }
        public int LicenseID { get; set; }
        public LicenseBriefOutputDTO LicenseDetails { get; set; }
        public DateTime DetainDate { get; set; }
        public decimal FineFees { get; set; }
        public int CreatedByUserID { get; set; }
        public UserBriefOutputDTO CreatedByUserDetails { get; set; }
        public bool IsReleased { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public int? ReleasedByUserID { get; set; }
        public UserBriefOutputDTO ReleasedByUserDetails { get; set; }
        public int? ReleaseApplicationID { get; set; }
        public ApplicationBriefOutputDTO ReleaseApplicationDetails { get; set; }
    }
}