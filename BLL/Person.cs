using DAL;
using Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL
{
    public class Person
    {

        /// <summary>
        /// Adds a new record into the system, automatically embedding internal business rules and states.
        /// </summary>
        public static async Task<int> addPerson(PersonBriefInputDTO dto)
        {
            var fullDto = new PersonFullOutputDTO
            {
                PersonID = dto.PersonID,
                NationalNo = dto.NationalNo,
                FirstName = dto.FirstName,
                SecondName = dto.SecondName,
                ThirdName = dto.ThirdName,
                LastName = dto.LastName,
                DateOfBirth = dto.DateOfBirth,
                Gendor = dto.Gendor,
                Address = dto.Address,
                Phone = dto.Phone,
                Email = dto.Email,
                NationalityCountryID = dto.NationalityCountryID,
                ImagePath = dto.ImagePath
            };

            return await PersonDAL.addPerson(fullDto);
        }

        public static async Task<PersonFullOutputDTO> getPersonByID(int PersonID)
        {
            PersonFullOutputDTO fullDto = await PersonDAL.getPersonByID(PersonID);
            if (fullDto == null) return null;
            // Directly populate nested object using the specialized Full method
            if (fullDto.NationalityCountryID != default)
            {
                fullDto.NationalityCountryDetails = await Country.getCountryByID((int)fullDto.NationalityCountryID);
            }

            return fullDto;
        }

public static async Task<PersonBriefOutputDTO> getPersonBriefOutputByID(int PersonID)
{
    // Fetch the full flat record from DAL
    var fullDto = await PersonDAL.getPersonByID(PersonID);
    if (fullDto == null) return null;

    // Map it instantly in memory to BriefOutputDTO and populate its specific brief nested objects
    return new PersonBriefOutputDTO
    {
                NationalityCountryDetails = fullDto.NationalityCountryID != default ? await Country.getCountryBriefOutputByID((int)fullDto.NationalityCountryID) : null,
                    PersonID = fullDto.PersonID,
                    NationalNo = fullDto.NationalNo,
                    FirstName = fullDto.FirstName,
                    SecondName = fullDto.SecondName,
                    ThirdName = fullDto.ThirdName,
                    LastName = fullDto.LastName,
                    DateOfBirth = fullDto.DateOfBirth,
                    Gendor = fullDto.Gendor,
                    Address = fullDto.Address,
                    Phone = fullDto.Phone,
                    Email = fullDto.Email,
                    NationalityCountryID = fullDto.NationalityCountryID,
                    ImagePath = fullDto.ImagePath
    };
}
        /// <summary>
        /// Safe update for regular users using BriefInputDTO. Preserves system-controlled fields.
        /// </summary>
        public static async Task<bool> updatePerson(PersonBriefInputDTO dto)
        {
            // 1. Fetch the existing full record to preserve internal data (Roles, Active status, Balance, etc.)
            var existingRecord = await getPersonByID(dto.PersonID);
            if (existingRecord == null) return false;

            // 2. Safely overwrite only the client-editable properties                existingRecord.PersonID = dto.PersonID;
                existingRecord.NationalNo = dto.NationalNo;
                existingRecord.FirstName = dto.FirstName;
                existingRecord.SecondName = dto.SecondName;
                existingRecord.ThirdName = dto.ThirdName;
                existingRecord.LastName = dto.LastName;
                existingRecord.DateOfBirth = dto.DateOfBirth;
                existingRecord.Gendor = dto.Gendor;
                existingRecord.Address = dto.Address;
                existingRecord.Phone = dto.Phone;
                existingRecord.Email = dto.Email;
                existingRecord.NationalityCountryID = dto.NationalityCountryID;
                existingRecord.ImagePath = dto.ImagePath;
            
            // 3. Forward the fully preserved record to the DAL layer
            return await PersonDAL.updatePerson(existingRecord);
        }
        public static async Task<bool> deletePerson(int PersonID)
        {
            if (await isPersonExistByID(PersonID))
            {
                return await PersonDAL.deletePerson(PersonID);
            }
            return false;
        }
        public static Task<bool> isPersonExistByID(int PersonID)
        {
            return PersonDAL.isPersonExistByID(PersonID);
        }

        public static async Task<List<PersonBriefOutputDTO>> Paging(int rowsPerPage, int pageNumber, string sortColumn, string direction)
        {
            List<PersonFullOutputDTO> fullList = await PersonDAL.PagingDAL(rowsPerPage, pageNumber, sortColumn, direction);
            List<PersonBriefOutputDTO> briefList = new List<PersonBriefOutputDTO>();
            
            foreach (PersonFullOutputDTO item in fullList)
            {
                var briefItem = new PersonBriefOutputDTO
                {                    PersonID = item.PersonID,
                    NationalNo = item.NationalNo,
                    FirstName = item.FirstName,
                    SecondName = item.SecondName,
                    ThirdName = item.ThirdName,
                    LastName = item.LastName,
                    DateOfBirth = item.DateOfBirth,
                    Gendor = item.Gendor,
                    Address = item.Address,
                    Phone = item.Phone,
                    Email = item.Email,
                    NationalityCountryID = item.NationalityCountryID,
                    ImagePath = item.ImagePath
                };

                // Populate nested object for brief list item
                if (item.NationalityCountryID != default)
                {
                    briefItem.NationalityCountryDetails = await Country.getCountryBriefOutputByID((int)item.NationalityCountryID);
                }

                briefList.Add(briefItem);
            }
            
            return briefList;
        }
        public static async Task<List<PersonBriefOutputDTO>> getAllBrief()
        {
            List<PersonFullOutputDTO> fullList = await PersonDAL.getAll();
            List<PersonBriefOutputDTO> briefList = new List<PersonBriefOutputDTO>();
            
            if (fullList == null) return briefList;

            foreach (var item in fullList)
            {
                briefList.Add(new PersonBriefOutputDTO
                {
                NationalityCountryDetails = item.NationalityCountryID != default ? await Country.getCountryBriefOutputByID((int)item.NationalityCountryID) : null,
                    PersonID = item.PersonID,
                    NationalNo = item.NationalNo,
                    FirstName = item.FirstName,
                    SecondName = item.SecondName,
                    ThirdName = item.ThirdName,
                    LastName = item.LastName,
                    DateOfBirth = item.DateOfBirth,
                    Gendor = item.Gendor,
                    Address = item.Address,
                    Phone = item.Phone,
                    Email = item.Email,
                    NationalityCountryID = item.NationalityCountryID,
                    ImagePath = item.ImagePath                });
            }
            return briefList;
        }
        public static async Task<List<PersonFullOutputDTO>> getAllFull()
        {
            List<PersonFullOutputDTO> fullList = await PersonDAL.getAll();
            foreach (var item in fullList)
            {
                if (item.NationalityCountryID != default)
                {
                    item.NationalityCountryDetails = await Country.getCountryByID((int)item.NationalityCountryID);
                }
            }
            return fullList;
        }

        public static async Task<PersonFullOutputDTO> getPersonByNationalNo(string NationalNo)
        {
            PersonFullOutputDTO fullDto = await PersonDAL.getPersonByNationalNo(NationalNo);
            if (fullDto == null) return null;
            // Directly populate nested object using the specialized Full method
            if (fullDto.NationalityCountryID != default)
            {
                fullDto.NationalityCountryDetails = await Country.getCountryByID((int)fullDto.NationalityCountryID);
            }

            return fullDto;
        }

public static async Task<PersonBriefOutputDTO> getPersonBriefOutputByNationalNo(string NationalNo)
{
    // Fetch the full flat record from DAL
    var fullDto = await PersonDAL.getPersonByNationalNo(NationalNo);
    if (fullDto == null) return null;

    // Map it instantly in memory to BriefOutputDTO and populate its specific brief nested objects
    return new PersonBriefOutputDTO
    {
                NationalityCountryDetails = fullDto.NationalityCountryID != default ? await Country.getCountryBriefOutputByID((int)fullDto.NationalityCountryID) : null,
                    PersonID = fullDto.PersonID,
                    NationalNo = fullDto.NationalNo,
                    FirstName = fullDto.FirstName,
                    SecondName = fullDto.SecondName,
                    ThirdName = fullDto.ThirdName,
                    LastName = fullDto.LastName,
                    DateOfBirth = fullDto.DateOfBirth,
                    Gendor = fullDto.Gendor,
                    Address = fullDto.Address,
                    Phone = fullDto.Phone,
                    Email = fullDto.Email,
                    NationalityCountryID = fullDto.NationalityCountryID,
                    ImagePath = fullDto.ImagePath
    };
}        public static Task<bool> isPersonExistByNationalNo(string NationalNo)
        {
            return PersonDAL.isPersonExistByNationalNo(NationalNo);
        }
        public static async Task<List<PersonBriefOutputDTO>> getAllBriefByFirstName(string FirstName)
        {
            List<PersonFullOutputDTO> fullList = await PersonDAL.getAllByFirstName(FirstName);
            List<PersonBriefOutputDTO> briefList = new List<PersonBriefOutputDTO>();
            
            if (fullList == null) return briefList;

            foreach (var item in fullList)
            {
                briefList.Add(new PersonBriefOutputDTO
                {
                NationalityCountryDetails = item.NationalityCountryID != default ? await Country.getCountryBriefOutputByID((int)item.NationalityCountryID) : null,
                    PersonID = item.PersonID,
                    NationalNo = item.NationalNo,
                    FirstName = item.FirstName,
                    SecondName = item.SecondName,
                    ThirdName = item.ThirdName,
                    LastName = item.LastName,
                    DateOfBirth = item.DateOfBirth,
                    Gendor = item.Gendor,
                    Address = item.Address,
                    Phone = item.Phone,
                    Email = item.Email,
                    NationalityCountryID = item.NationalityCountryID,
                    ImagePath = item.ImagePath                });
            }
            return briefList;
        }
        public static async Task<List<PersonFullOutputDTO>> getAllFullByFirstName(string FirstName)
        {
            List<PersonFullOutputDTO> fullList = await PersonDAL.getAllByFirstName(FirstName);
            foreach (var item in fullList)
            {
                if (item.NationalityCountryID != default)
                {
                    item.NationalityCountryDetails = await Country.getCountryByID((int)item.NationalityCountryID);
                }
            }
            return fullList;
        }

    }
}
