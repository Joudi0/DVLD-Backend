using DAL;
using Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL
{
    public class LicenseClasse
    {

        public static async Task<LicenseClasseFullOutputDTO> getLicenseClasseByID(int LicenseClassID)
        {
            LicenseClasseFullOutputDTO fullDto = await LicenseClasseDAL.getLicenseClasseByID(LicenseClassID);
            if (fullDto == null) return null;
            return fullDto;
        }

public static async Task<LicenseClasseBriefOutputDTO> getLicenseClasseBriefOutputByID(int LicenseClassID)
{
    // Fetch the full flat record from DAL
    var fullDto = await LicenseClasseDAL.getLicenseClasseByID(LicenseClassID);
    if (fullDto == null) return null;

    // Map it instantly in memory to BriefOutputDTO and populate its specific brief nested objects
    return new LicenseClasseBriefOutputDTO
    {
                    LicenseClassID = fullDto.LicenseClassID,
                    ClassName = fullDto.ClassName,
                    ClassDescription = fullDto.ClassDescription,
                    MinimumAllowedAge = fullDto.MinimumAllowedAge,
                    DefaultValidityLength = fullDto.DefaultValidityLength,
                    ClassFees = fullDto.ClassFees
    };
}        public static async Task<List<LicenseClasseBriefOutputDTO>> getAllBrief()
        {
            List<LicenseClasseFullOutputDTO> fullList = await LicenseClasseDAL.getAll();
            List<LicenseClasseBriefOutputDTO> briefList = new List<LicenseClasseBriefOutputDTO>();
            
            if (fullList == null) return briefList;

            foreach (var item in fullList)
            {
                briefList.Add(new LicenseClasseBriefOutputDTO
                {
                    LicenseClassID = item.LicenseClassID,
                    ClassName = item.ClassName,
                    ClassDescription = item.ClassDescription,
                    MinimumAllowedAge = item.MinimumAllowedAge,
                    DefaultValidityLength = item.DefaultValidityLength,
                    ClassFees = item.ClassFees                });
            }
            return briefList;
        }
        public static async Task<List<LicenseClasseFullOutputDTO>> getAllFull()
        {
            List<LicenseClasseFullOutputDTO> fullList = await LicenseClasseDAL.getAll();
            return fullList;
        }

    }
}
