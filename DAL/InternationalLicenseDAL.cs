using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Shared;

namespace DAL
{
    public class InternationalLicenseDAL
    {  

        public static async Task<int> addInternationalLicense(InternationalLicenseFullOutputDTO dto)
        {
            int InternationalLicenseID = -1;
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_InternationalLicenses_Insert", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(@"ApplicationID", SqlDbType.Int).Value = dto.ApplicationID;
		    command.Parameters.Add(@"DriverID", SqlDbType.Int).Value = dto.DriverID;
		    command.Parameters.Add(@"IssuedUsingLocalLicenseID", SqlDbType.Int).Value = dto.IssuedUsingLocalLicenseID;
		    command.Parameters.Add(@"IssueDate", SqlDbType.DateTime).Value = dto.IssueDate;
		    command.Parameters.Add(@"ExpirationDate", SqlDbType.DateTime).Value = dto.ExpirationDate;
		    command.Parameters.Add(@"IsActive", SqlDbType.Bit).Value = dto.IsActive;
		    command.Parameters.Add(@"CreatedByUserID", SqlDbType.Int).Value = dto.CreatedByUserID;
            try
            {
                await connection.OpenAsync();
                object result = await command.ExecuteScalarAsync();
                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    InternationalLicenseID = insertedID;
                }
            }
            catch (Exception) { throw; }
            return InternationalLicenseID;
        }
        public static async Task<InternationalLicenseFullOutputDTO> getInternationalLicenseByID(int InternationalLicenseID)
        {
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_InternationalLicenses_SelectByInternationalLicenseID", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@InternationalLicenseID", InternationalLicenseID);
            try
            {
                await connection.OpenAsync();
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return new InternationalLicenseFullOutputDTO
                    {
        InternationalLicenseID = (int)reader["InternationalLicenseID"],
        ApplicationID = (int)reader["ApplicationID"],
        DriverID = (int)reader["DriverID"],
        IssuedUsingLocalLicenseID = (int)reader["IssuedUsingLocalLicenseID"],
        IssueDate = (DateTime)reader["IssueDate"],
        ExpirationDate = (DateTime)reader["ExpirationDate"],
        IsActive = (bool)reader["IsActive"],
        CreatedByUserID = (int)reader["CreatedByUserID"]
                    };
                }
            }
            catch (Exception) { throw; }
            return null;
        }
        public static async Task<bool> updateInternationalLicense(InternationalLicenseFullOutputDTO dto)
        {
            int rowsAffected = 0;
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_InternationalLicenses_Update", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(@"InternationalLicenseID", SqlDbType.Int).Value = dto.InternationalLicenseID;
		    command.Parameters.Add(@"ApplicationID", SqlDbType.Int).Value = dto.ApplicationID;
		    command.Parameters.Add(@"DriverID", SqlDbType.Int).Value = dto.DriverID;
		    command.Parameters.Add(@"IssuedUsingLocalLicenseID", SqlDbType.Int).Value = dto.IssuedUsingLocalLicenseID;
		    command.Parameters.Add(@"IssueDate", SqlDbType.DateTime).Value = dto.IssueDate;
		    command.Parameters.Add(@"ExpirationDate", SqlDbType.DateTime).Value = dto.ExpirationDate;
		    command.Parameters.Add(@"IsActive", SqlDbType.Bit).Value = dto.IsActive;
		    command.Parameters.Add(@"CreatedByUserID", SqlDbType.Int).Value = dto.CreatedByUserID;
            try
            {
                await connection.OpenAsync();
                rowsAffected = await command.ExecuteNonQueryAsync();
            }
            catch (Exception) { throw; }
            return (rowsAffected > 0);
        }
        public static async Task<bool> deleteInternationalLicense(int InternationalLicenseID)
        {
            int rowsAffected = 0;
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_InternationalLicenses_Delete", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@InternationalLicenseID", InternationalLicenseID);
            try
            {
                await connection.OpenAsync();
                rowsAffected = await command.ExecuteNonQueryAsync();
            }
            catch (Exception) { throw; }
            return (rowsAffected > 0);
        }
        public static async Task<bool> isInternationalLicenseExistByID(int InternationalLicenseID)
        {
            bool isFound = false;
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_InternationalLicenses_IsExistByInternationalLicenseID", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@InternationalLicenseID", InternationalLicenseID);
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
        public static async Task<List<InternationalLicenseFullOutputDTO>> PagingDAL(int RowsPerPage, int PageNumber, string SortColumn, string Direction)
        {
            var list = new List<InternationalLicenseFullOutputDTO>();
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_InternationalLicenses_Paging", connection);
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
                    list.Add(new InternationalLicenseFullOutputDTO
                    {
        InternationalLicenseID = (int)reader["InternationalLicenseID"],
        ApplicationID = (int)reader["ApplicationID"],
        DriverID = (int)reader["DriverID"],
        IssuedUsingLocalLicenseID = (int)reader["IssuedUsingLocalLicenseID"],
        IssueDate = (DateTime)reader["IssueDate"],
        ExpirationDate = (DateTime)reader["ExpirationDate"],
        IsActive = (bool)reader["IsActive"],
        CreatedByUserID = (int)reader["CreatedByUserID"]
                    });
                }
            }
            catch (Exception) { throw; }
            return list;
        }
        public static async Task<List<InternationalLicenseFullOutputDTO>> getAll()
        {
            var list = new List<InternationalLicenseFullOutputDTO>();
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_InternationalLicenses_SelectAll", connection);
            command.CommandType = CommandType.StoredProcedure;
            try
            {
                await connection.OpenAsync();
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    list.Add(new InternationalLicenseFullOutputDTO
                    {
        InternationalLicenseID = (int)reader["InternationalLicenseID"],
        ApplicationID = (int)reader["ApplicationID"],
        DriverID = (int)reader["DriverID"],
        IssuedUsingLocalLicenseID = (int)reader["IssuedUsingLocalLicenseID"],
        IssueDate = (DateTime)reader["IssueDate"],
        ExpirationDate = (DateTime)reader["ExpirationDate"],
        IsActive = (bool)reader["IsActive"],
        CreatedByUserID = (int)reader["CreatedByUserID"]
                    });
                }
            }
            catch (Exception) { throw; }
            return list;
        }
        public static async Task<InternationalLicenseFullOutputDTO> getInternationalLicenseByApplicationID(int ApplicationID)
        {
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_InternationalLicenses_SelectByApplicationID", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            try
            {
                await connection.OpenAsync();
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return new InternationalLicenseFullOutputDTO
                    {
        InternationalLicenseID = (int)reader["InternationalLicenseID"],
        ApplicationID = (int)reader["ApplicationID"],
        DriverID = (int)reader["DriverID"],
        IssuedUsingLocalLicenseID = (int)reader["IssuedUsingLocalLicenseID"],
        IssueDate = (DateTime)reader["IssueDate"],
        ExpirationDate = (DateTime)reader["ExpirationDate"],
        IsActive = (bool)reader["IsActive"],
        CreatedByUserID = (int)reader["CreatedByUserID"]
                    };
                }
            }
            catch (Exception) { throw; }
            return null;
        }
        public static async Task<InternationalLicenseFullOutputDTO> getInternationalLicenseByIssuedUsingLocalLicenseID(int IssuedUsingLocalLicenseID)
        {
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_InternationalLicenses_SelectByIssuedUsingLocalLicenseID", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", IssuedUsingLocalLicenseID);
            try
            {
                await connection.OpenAsync();
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return new InternationalLicenseFullOutputDTO
                    {
        InternationalLicenseID = (int)reader["InternationalLicenseID"],
        ApplicationID = (int)reader["ApplicationID"],
        DriverID = (int)reader["DriverID"],
        IssuedUsingLocalLicenseID = (int)reader["IssuedUsingLocalLicenseID"],
        IssueDate = (DateTime)reader["IssueDate"],
        ExpirationDate = (DateTime)reader["ExpirationDate"],
        IsActive = (bool)reader["IsActive"],
        CreatedByUserID = (int)reader["CreatedByUserID"]
                    };
                }
            }
            catch (Exception) { throw; }
            return null;
        }
        public static async Task<bool> isInternationalLicenseExistByApplicationID(int ApplicationID)
        {
            bool isFound = false;
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_InternationalLicenses_IsExistByApplicationID", connection);
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
        public static async Task<bool> isInternationalLicenseExistByIssuedUsingLocalLicenseID(int IssuedUsingLocalLicenseID)
        {
            bool isFound = false;
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_InternationalLicenses_IsExistByIssuedUsingLocalLicenseID", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", IssuedUsingLocalLicenseID);
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
        public static async Task<List<InternationalLicenseFullOutputDTO>> getAllByDriverID(int DriverID)
        {
            var list = new List<InternationalLicenseFullOutputDTO>();
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_InternationalLicenses_SelectAllByDriverID", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@DriverID", DriverID);
            try
            {
                await connection.OpenAsync();
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    list.Add(new InternationalLicenseFullOutputDTO
                    {
        InternationalLicenseID = (int)reader["InternationalLicenseID"],
        ApplicationID = (int)reader["ApplicationID"],
        DriverID = (int)reader["DriverID"],
        IssuedUsingLocalLicenseID = (int)reader["IssuedUsingLocalLicenseID"],
        IssueDate = (DateTime)reader["IssueDate"],
        ExpirationDate = (DateTime)reader["ExpirationDate"],
        IsActive = (bool)reader["IsActive"],
        CreatedByUserID = (int)reader["CreatedByUserID"]
                    });
                }
            }
            catch (Exception) { throw; }
            return list;
        }
        public static async Task<List<InternationalLicenseFullOutputDTO>> getAllByCreatedByUserID(int CreatedByUserID)
        {
            var list = new List<InternationalLicenseFullOutputDTO>();
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_InternationalLicenses_SelectAllByCreatedByUserID", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            try
            {
                await connection.OpenAsync();
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    list.Add(new InternationalLicenseFullOutputDTO
                    {
        InternationalLicenseID = (int)reader["InternationalLicenseID"],
        ApplicationID = (int)reader["ApplicationID"],
        DriverID = (int)reader["DriverID"],
        IssuedUsingLocalLicenseID = (int)reader["IssuedUsingLocalLicenseID"],
        IssueDate = (DateTime)reader["IssueDate"],
        ExpirationDate = (DateTime)reader["ExpirationDate"],
        IsActive = (bool)reader["IsActive"],
        CreatedByUserID = (int)reader["CreatedByUserID"]
                    });
                }
            }
            catch (Exception) { throw; }
            return list;
        }  
    }
}