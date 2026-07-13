using DAL;
using Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL
{
    public class Country
    {

        public static async Task<CountryFullOutputDTO> getCountryByID(int CountryID)
        {
            CountryFullOutputDTO fullDto = await CountryDAL.getCountryByID(CountryID);
            if (fullDto == null) return null;
            return fullDto;
        }

public static async Task<CountryBriefOutputDTO> getCountryBriefOutputByID(int CountryID)
{
    // Fetch the full flat record from DAL
    var fullDto = await CountryDAL.getCountryByID(CountryID);
    if (fullDto == null) return null;

    // Map it instantly in memory to BriefOutputDTO and populate its specific brief nested objects
    return new CountryBriefOutputDTO
    {
                    CountryID = fullDto.CountryID,
                    CountryName = fullDto.CountryName
    };
}        public static async Task<List<CountryBriefOutputDTO>> getAllBrief()
        {
            List<CountryFullOutputDTO> fullList = await CountryDAL.getAll();
            List<CountryBriefOutputDTO> briefList = new List<CountryBriefOutputDTO>();
            
            if (fullList == null) return briefList;

            foreach (var item in fullList)
            {
                briefList.Add(new CountryBriefOutputDTO
                {
                    CountryID = item.CountryID,
                    CountryName = item.CountryName                });
            }
            return briefList;
        }
        public static async Task<List<CountryFullOutputDTO>> getAllFull()
        {
            List<CountryFullOutputDTO> fullList = await CountryDAL.getAll();
            return fullList;
        }

    }
}
