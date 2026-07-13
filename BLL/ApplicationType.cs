using DAL;
using Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL
{
    public class ApplicationType
    {

        public static async Task<ApplicationTypeFullOutputDTO> getApplicationTypeByID(int ApplicationTypeID)
        {
            ApplicationTypeFullOutputDTO fullDto = await ApplicationTypeDAL.getApplicationTypeByID(ApplicationTypeID);
            if (fullDto == null) return null;
            return fullDto;
        }

public static async Task<ApplicationTypeBriefOutputDTO> getApplicationTypeBriefOutputByID(int ApplicationTypeID)
{
    // Fetch the full flat record from DAL
    var fullDto = await ApplicationTypeDAL.getApplicationTypeByID(ApplicationTypeID);
    if (fullDto == null) return null;

    // Map it instantly in memory to BriefOutputDTO and populate its specific brief nested objects
    return new ApplicationTypeBriefOutputDTO
    {
                    ApplicationTypeID = fullDto.ApplicationTypeID,
                    ApplicationTypeTitle = fullDto.ApplicationTypeTitle,
                    ApplicationFees = fullDto.ApplicationFees
    };
}        public static async Task<List<ApplicationTypeBriefOutputDTO>> getAllBrief()
        {
            List<ApplicationTypeFullOutputDTO> fullList = await ApplicationTypeDAL.getAll();
            List<ApplicationTypeBriefOutputDTO> briefList = new List<ApplicationTypeBriefOutputDTO>();
            
            if (fullList == null) return briefList;

            foreach (var item in fullList)
            {
                briefList.Add(new ApplicationTypeBriefOutputDTO
                {
                    ApplicationTypeID = item.ApplicationTypeID,
                    ApplicationTypeTitle = item.ApplicationTypeTitle,
                    ApplicationFees = item.ApplicationFees                });
            }
            return briefList;
        }
        public static async Task<List<ApplicationTypeFullOutputDTO>> getAllFull()
        {
            List<ApplicationTypeFullOutputDTO> fullList = await ApplicationTypeDAL.getAll();
            return fullList;
        }

    }
}
