using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Shared;

namespace DAL
{
    public class PersonDAL
    {  

        public static async Task<int> addPerson(PersonFullOutputDTO dto)
        {
            int PersonID = -1;
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_People_Insert", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(@"NationalNo", SqlDbType.NVarChar).Value = dto.NationalNo;
		    command.Parameters.Add(@"FirstName", SqlDbType.NVarChar).Value = dto.FirstName;
		    command.Parameters.Add(@"SecondName", SqlDbType.NVarChar).Value = dto.SecondName;
		    command.Parameters.Add(@"ThirdName", SqlDbType.NVarChar).Value = string.IsNullOrEmpty(dto.ThirdName) ? DBNull.Value : (object)dto.ThirdName;
		    command.Parameters.Add(@"LastName", SqlDbType.NVarChar).Value = dto.LastName;
		    command.Parameters.Add(@"DateOfBirth", SqlDbType.DateTime).Value = dto.DateOfBirth;
		    command.Parameters.Add(@"Gendor", SqlDbType.TinyInt).Value = dto.Gendor;
		    command.Parameters.Add(@"Address", SqlDbType.NVarChar).Value = dto.Address;
		    command.Parameters.Add(@"Phone", SqlDbType.NVarChar).Value = dto.Phone;
		    command.Parameters.Add(@"Email", SqlDbType.NVarChar).Value = string.IsNullOrEmpty(dto.Email) ? DBNull.Value : (object)dto.Email;
		    command.Parameters.Add(@"NationalityCountryID", SqlDbType.Int).Value = dto.NationalityCountryID;
		    command.Parameters.Add(@"ImagePath", SqlDbType.NVarChar).Value = string.IsNullOrEmpty(dto.ImagePath) ? DBNull.Value : (object)dto.ImagePath;
            try
            {
                await connection.OpenAsync();
                object result = await command.ExecuteScalarAsync();
                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    PersonID = insertedID;
                }
            }
            catch (Exception) { throw; }
            return PersonID;
        }
        public static async Task<PersonFullOutputDTO> getPersonByID(int PersonID)
        {
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_People_SelectByPersonID", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@PersonID", PersonID);
            try
            {
                await connection.OpenAsync();
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return new PersonFullOutputDTO
                    {
        PersonID = (int)reader["PersonID"],
        NationalNo = (string)reader["NationalNo"],
        FirstName = (string)reader["FirstName"],
        SecondName = (string)reader["SecondName"],
        ThirdName = (reader["ThirdName"] == DBNull.Value) ? "" : (string)reader["ThirdName"],
        LastName = (string)reader["LastName"],
        DateOfBirth = (DateTime)reader["DateOfBirth"],
        Gendor = (byte)reader["Gendor"],
        Address = (string)reader["Address"],
        Phone = (string)reader["Phone"],
        Email = (reader["Email"] == DBNull.Value) ? "" : (string)reader["Email"],
        NationalityCountryID = (int)reader["NationalityCountryID"],
        ImagePath = (reader["ImagePath"] == DBNull.Value) ? "" : (string)reader["ImagePath"]
                    };
                }
            }
            catch (Exception) { throw; }
            return null;
        }
        public static async Task<bool> updatePerson(PersonFullOutputDTO dto)
        {
            int rowsAffected = 0;
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_People_Update", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(@"PersonID", SqlDbType.Int).Value = dto.PersonID;
		    command.Parameters.Add(@"NationalNo", SqlDbType.NVarChar).Value = dto.NationalNo;
		    command.Parameters.Add(@"FirstName", SqlDbType.NVarChar).Value = dto.FirstName;
		    command.Parameters.Add(@"SecondName", SqlDbType.NVarChar).Value = dto.SecondName;
		    command.Parameters.Add(@"ThirdName", SqlDbType.NVarChar).Value = string.IsNullOrEmpty(dto.ThirdName) ? DBNull.Value : (object)dto.ThirdName;
		    command.Parameters.Add(@"LastName", SqlDbType.NVarChar).Value = dto.LastName;
		    command.Parameters.Add(@"DateOfBirth", SqlDbType.DateTime).Value = dto.DateOfBirth;
		    command.Parameters.Add(@"Gendor", SqlDbType.TinyInt).Value = dto.Gendor;
		    command.Parameters.Add(@"Address", SqlDbType.NVarChar).Value = dto.Address;
		    command.Parameters.Add(@"Phone", SqlDbType.NVarChar).Value = dto.Phone;
		    command.Parameters.Add(@"Email", SqlDbType.NVarChar).Value = string.IsNullOrEmpty(dto.Email) ? DBNull.Value : (object)dto.Email;
		    command.Parameters.Add(@"NationalityCountryID", SqlDbType.Int).Value = dto.NationalityCountryID;
		    command.Parameters.Add(@"ImagePath", SqlDbType.NVarChar).Value = string.IsNullOrEmpty(dto.ImagePath) ? DBNull.Value : (object)dto.ImagePath;
            try
            {
                await connection.OpenAsync();
                rowsAffected = await command.ExecuteNonQueryAsync();
            }
            catch (Exception) { throw; }
            return (rowsAffected > 0);
        }
        public static async Task<bool> deletePerson(int PersonID)
        {
            int rowsAffected = 0;
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_People_Delete", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@PersonID", PersonID);
            try
            {
                await connection.OpenAsync();
                rowsAffected = await command.ExecuteNonQueryAsync();
            }
            catch (Exception) { throw; }
            return (rowsAffected > 0);
        }
        public static async Task<bool> isPersonExistByID(int PersonID)
        {
            bool isFound = false;
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_People_IsExistByPersonID", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@PersonID", PersonID);
            try
            {
                await connection.OpenAsync();
                object result = await command.ExecuteScalarAsync();
                if (result != null)
                {
                    isFound = Convert.ToInt32(result) == 1;
                }
            }
            catch (Exception) { throw; }
            return isFound;
        }
        public static async Task<List<PersonFullOutputDTO>> PagingDAL(int RowsPerPage, int PageNumber, string SortColumn, string Direction)
        {
            var list = new List<PersonFullOutputDTO>();
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_People_Paging", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@RowsPerPage", RowsPerPage);
            command.Parameters.AddWithValue("@PageNumber", PageNumber);             
            command.Parameters.AddWithValue("@SortColumn", string.IsNullOrEmpty(SortColumn) ? DBNull.Value : SortColumn);
            command.Parameters.AddWithValue("@Direction", string.IsNullOrEmpty(Direction) ? DBNull.Value : Direction);    
            try
            {
                await connection.OpenAsync();
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    list.Add(new PersonFullOutputDTO
                    {
        PersonID = (int)reader["PersonID"],
        NationalNo = (string)reader["NationalNo"],
        FirstName = (string)reader["FirstName"],
        SecondName = (string)reader["SecondName"],
        ThirdName = (reader["ThirdName"] == DBNull.Value) ? "" : (string)reader["ThirdName"],
        LastName = (string)reader["LastName"],
        DateOfBirth = (DateTime)reader["DateOfBirth"],
        Gendor = (byte)reader["Gendor"],
        Address = (string)reader["Address"],
        Phone = (string)reader["Phone"],
        Email = (reader["Email"] == DBNull.Value) ? "" : (string)reader["Email"],
        NationalityCountryID = (int)reader["NationalityCountryID"],
        ImagePath = (reader["ImagePath"] == DBNull.Value) ? "" : (string)reader["ImagePath"]
                    });
                }
            }
            catch (Exception) { throw; }
            return list;
        }
        public static async Task<List<PersonFullOutputDTO>> getAll()
        {
            var list = new List<PersonFullOutputDTO>();
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_People_SelectAll", connection);
            command.CommandType = CommandType.StoredProcedure;
            try
            {
                await connection.OpenAsync();
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    list.Add(new PersonFullOutputDTO
                    {
        PersonID = (int)reader["PersonID"],
        NationalNo = (string)reader["NationalNo"],
        FirstName = (string)reader["FirstName"],
        SecondName = (string)reader["SecondName"],
        ThirdName = (reader["ThirdName"] == DBNull.Value) ? "" : (string)reader["ThirdName"],
        LastName = (string)reader["LastName"],
        DateOfBirth = (DateTime)reader["DateOfBirth"],
        Gendor = (byte)reader["Gendor"],
        Address = (string)reader["Address"],
        Phone = (string)reader["Phone"],
        Email = (reader["Email"] == DBNull.Value) ? "" : (string)reader["Email"],
        NationalityCountryID = (int)reader["NationalityCountryID"],
        ImagePath = (reader["ImagePath"] == DBNull.Value) ? "" : (string)reader["ImagePath"]
                    });
                }
            }
            catch (Exception) { throw; }
            return list;
        }
        public static async Task<PersonFullOutputDTO> getPersonByNationalNo(string NationalNo)
        {
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_People_SelectByNationalNo", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@NationalNo", NationalNo);
            try
            {
                await connection.OpenAsync();
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return new PersonFullOutputDTO
                    {
        PersonID = (int)reader["PersonID"],
        NationalNo = (string)reader["NationalNo"],
        FirstName = (string)reader["FirstName"],
        SecondName = (string)reader["SecondName"],
        ThirdName = (reader["ThirdName"] == DBNull.Value) ? "" : (string)reader["ThirdName"],
        LastName = (string)reader["LastName"],
        DateOfBirth = (DateTime)reader["DateOfBirth"],
        Gendor = (byte)reader["Gendor"],
        Address = (string)reader["Address"],
        Phone = (string)reader["Phone"],
        Email = (reader["Email"] == DBNull.Value) ? "" : (string)reader["Email"],
        NationalityCountryID = (int)reader["NationalityCountryID"],
        ImagePath = (reader["ImagePath"] == DBNull.Value) ? "" : (string)reader["ImagePath"]
                    };
                }
            }
            catch (Exception) { throw; }
            return null;
        }
        public static async Task<bool> isPersonExistByNationalNo(string NationalNo)
        {
            bool isFound = false;
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_People_IsExistByNationalNo", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@NationalNo", NationalNo);
            try
            {
                await connection.OpenAsync();
                object result = await command.ExecuteScalarAsync();
                if (result != null)
                {
                    isFound = Convert.ToInt32(result) == 1;
                }
            }
            catch (Exception) { throw; }
            return isFound;
        }
        public static async Task<List<PersonFullOutputDTO>> getAllByFirstName(string FirstName)
        {
            var list = new List<PersonFullOutputDTO>();
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_People_SelectAllByFirstName", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@FirstName", FirstName);
            try
            {
                await connection.OpenAsync();
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    list.Add(new PersonFullOutputDTO
                    {
        PersonID = (int)reader["PersonID"],
        NationalNo = (string)reader["NationalNo"],
        FirstName = (string)reader["FirstName"],
        SecondName = (string)reader["SecondName"],
        ThirdName = (reader["ThirdName"] == DBNull.Value) ? "" : (string)reader["ThirdName"],
        LastName = (string)reader["LastName"],
        DateOfBirth = (DateTime)reader["DateOfBirth"],
        Gendor = (byte)reader["Gendor"],
        Address = (string)reader["Address"],
        Phone = (string)reader["Phone"],
        Email = (reader["Email"] == DBNull.Value) ? "" : (string)reader["Email"],
        NationalityCountryID = (int)reader["NationalityCountryID"],
        ImagePath = (reader["ImagePath"] == DBNull.Value) ? "" : (string)reader["ImagePath"]
                    });
                }
            }
            catch (Exception) { throw; }
            return list;
        }  
    }
}