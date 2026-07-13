using System;

namespace Shared
{
    public class ApplicationFullOutputDTO
    {
        public int ApplicationID { get; set; }
        public int ApplicantPersonID { get; set; }
        public PersonFullOutputDTO ApplicantPersonDetails { get; set; }
        public DateTime ApplicationDate { get; set; }
        public int ApplicationTypeID { get; set; }
        public ApplicationTypeFullOutputDTO ApplicationTypeDetails { get; set; }
        public byte ApplicationStatus { get; set; }
        public DateTime LastStatusDate { get; set; }
        public decimal PaidFees { get; set; }
        public int CreatedByUserID { get; set; }
        public UserFullOutputDTO CreatedByUserDetails { get; set; }
        public bool? IsDeleted { get; set; }
    }
}