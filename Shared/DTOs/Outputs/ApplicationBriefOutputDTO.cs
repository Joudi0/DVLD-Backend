using System;

namespace Shared
{
    public class ApplicationBriefOutputDTO
    {
        public int ApplicationID { get; set; }
        public int ApplicantPersonID { get; set; }
        public PersonBriefOutputDTO ApplicantPersonDetails { get; set; }
        public DateTime ApplicationDate { get; set; }
        public int ApplicationTypeID { get; set; }
        public ApplicationTypeBriefOutputDTO ApplicationTypeDetails { get; set; }
        public byte ApplicationStatus { get; set; }
        public DateTime LastStatusDate { get; set; }
        public decimal PaidFees { get; set; }
        public int CreatedByUserID { get; set; }
        public UserBriefOutputDTO CreatedByUserDetails { get; set; }
        public bool? IsDeleted { get; set; }
    }
}