using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Shared;

namespace DAL
{
    public class LocalDrivingLicenseApplicationDAL
    {  

        public static async Task<int> addLocalDrivingLicenseApplication(LocalDrivingLicenseApplicationFullOutputDTO dto)
        {
            int LocalDrivingLicenseApplicationID = -1;
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_LocalDrivingLicenseApplications_Insert", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(@"ApplicationID", SqlDbType.Int).Value = dto.ApplicationID;
		    command.Parameters.Add(@"LicenseClassID", SqlDbType.Int).Value = dto.LicenseClassID;
            try
            {
                await connection.OpenAsync();
                object result = await command.ExecuteScalarAsync();
                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    LocalDrivingLicenseApplicationID = insertedID;
                }
            }
            catch (Exception) { throw; }
            return LocalDrivingLicenseApplicationID;
        }
        public static async Task<LocalDrivingLicenseApplicationFullOutputDTO> getLocalDrivingLicenseApplicationByID(int LocalDrivingLicenseApplicationID)
        {
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_LocalDrivingLicenseApplications_SelectByLocalDrivingLicenseApplicationID", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            try
            {
                await connection.OpenAsync();
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return new LocalDrivingLicenseApplicationFullOutputDTO
                    {
        LocalDrivingLicenseApplicationID = (int)reader["LocalDrivingLicenseApplicationID"],
        ApplicationID = (int)reader["ApplicationID"],
        LicenseClassID = (int)reader["LicenseClassID"]
                    };
                }
            }
            catch (Exception) { throw; }
            return null;
        }
        public static async Task<bool> updateLocalDrivingLicenseApplication(LocalDrivingLicenseApplicationFullOutputDTO dto)
        {
            int rowsAffected = 0;
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_LocalDrivingLicenseApplications_Update", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(@"LocalDrivingLicenseApplicationID", SqlDbType.Int).Value = dto.LocalDrivingLicenseApplicationID;
		    command.Parameters.Add(@"ApplicationID", SqlDbType.Int).Value = dto.ApplicationID;
		    command.Parameters.Add(@"LicenseClassID", SqlDbType.Int).Value = dto.LicenseClassID;
            try
            {
                await connection.OpenAsync();
                rowsAffected = await command.ExecuteNonQueryAsync();
            }
            catch (Exception) { throw; }
            return (rowsAffected > 0);
        }
        public static async Task<bool> deleteLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID)
        {
            int rowsAffected = 0;
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_LocalDrivingLicenseApplications_Delete", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            try
            {
                await connection.OpenAsync();
                rowsAffected = await command.ExecuteNonQueryAsync();
            }
            catch (Exception) { throw; }
            return (rowsAffected > 0);
        }
        public static async Task<bool> isLocalDrivingLicenseApplicationExistByID(int LocalDrivingLicenseApplicationID)
        {
            bool isFound = false;
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_LocalDrivingLicenseApplications_IsExistByLocalDrivingLicenseApplicationID", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
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
        public static async Task<List<LocalDrivingLicenseApplicationFullOutputDTO>> PagingDAL(int RowsPerPage, int PageNumber, string SortColumn, string Direction)
        {
            var list = new List<LocalDrivingLicenseApplicationFullOutputDTO>();
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_LocalDrivingLicenseApplications_Paging", connection);
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
                    list.Add(new LocalDrivingLicenseApplicationFullOutputDTO
                    {
        LocalDrivingLicenseApplicationID = (int)reader["LocalDrivingLicenseApplicationID"],
        ApplicationID = (int)reader["ApplicationID"],
        LicenseClassID = (int)reader["LicenseClassID"]
                    });
                }
            }
            catch (Exception) { throw; }
            return list;
        }
        public static async Task<List<LocalDrivingLicenseApplicationFullOutputDTO>> getAll()
        {
            var list = new List<LocalDrivingLicenseApplicationFullOutputDTO>();
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_LocalDrivingLicenseApplications_SelectAll", connection);
            command.CommandType = CommandType.StoredProcedure;
            try
            {
                await connection.OpenAsync();
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    list.Add(new LocalDrivingLicenseApplicationFullOutputDTO
                    {
        LocalDrivingLicenseApplicationID = (int)reader["LocalDrivingLicenseApplicationID"],
        ApplicationID = (int)reader["ApplicationID"],
        LicenseClassID = (int)reader["LicenseClassID"]
                    });
                }
            }
            catch (Exception) { throw; }
            return list;
        }
        public static async Task<LocalDrivingLicenseApplicationFullOutputDTO> getLocalDrivingLicenseApplicationByApplicationID(int ApplicationID)
        {
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_LocalDrivingLicenseApplications_SelectByApplicationID", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            try
            {
                await connection.OpenAsync();
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return new LocalDrivingLicenseApplicationFullOutputDTO
                    {
        LocalDrivingLicenseApplicationID = (int)reader["LocalDrivingLicenseApplicationID"],
        ApplicationID = (int)reader["ApplicationID"],
        LicenseClassID = (int)reader["LicenseClassID"]
                    };
                }
            }
            catch (Exception) { throw; }
            return null;
        }
        public static async Task<bool> isLocalDrivingLicenseApplicationExistByApplicationID(int ApplicationID)
        {
            bool isFound = false;
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_LocalDrivingLicenseApplications_IsExistByApplicationID", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
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
        public static async Task<List<LocalDrivingLicenseApplicationFullOutputDTO>> getAllByLicenseClassID(int LicenseClassID)
        {
            var list = new List<LocalDrivingLicenseApplicationFullOutputDTO>();
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_LocalDrivingLicenseApplications_SelectAllByLicenseClassID", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
            try
            {
                await connection.OpenAsync();
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    list.Add(new LocalDrivingLicenseApplicationFullOutputDTO
                    {
        LocalDrivingLicenseApplicationID = (int)reader["LocalDrivingLicenseApplicationID"],
        ApplicationID = (int)reader["ApplicationID"],
        LicenseClassID = (int)reader["LicenseClassID"]
                    });
                }
            }
            catch (Exception) { throw; }
            return list;
        }  
    }
}