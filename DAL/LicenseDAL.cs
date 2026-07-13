using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Shared;

namespace DAL
{
    public class LicenseDAL
    {  

        public static async Task<int> addLicense(LicenseFullOutputDTO dto)
        {
            int LicenseID = -1;
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_Licenses_Insert", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(@"ApplicationID", SqlDbType.Int).Value = dto.ApplicationID;
		    command.Parameters.Add(@"DriverID", SqlDbType.Int).Value = dto.DriverID;
		    command.Parameters.Add(@"LicenseClass", SqlDbType.Int).Value = dto.LicenseClass;
		    command.Parameters.Add(@"IssueDate", SqlDbType.DateTime).Value = dto.IssueDate;
		    command.Parameters.Add(@"ExpirationDate", SqlDbType.DateTime).Value = dto.ExpirationDate;
		    command.Parameters.Add(@"Notes", SqlDbType.NVarChar).Value = string.IsNullOrEmpty(dto.Notes) ? DBNull.Value : (object)dto.Notes;
		    command.Parameters.Add(@"PaidFees", SqlDbType.Decimal).Value = dto.PaidFees;
		    command.Parameters.Add(@"IsActive", SqlDbType.Bit).Value = dto.IsActive;
		    command.Parameters.Add(@"IssueReason", SqlDbType.TinyInt).Value = dto.IssueReason;
		    command.Parameters.Add(@"CreatedByUserID", SqlDbType.Int).Value = dto.CreatedByUserID;
            try
            {
                await connection.OpenAsync();
                object result = await command.ExecuteScalarAsync();
                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    LicenseID = insertedID;
                }
            }
            catch (Exception) { throw; }
            return LicenseID;
        }
        public static async Task<LicenseFullOutputDTO> getLicenseByID(int LicenseID)
        {
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_Licenses_SelectByLicenseID", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@LicenseID", LicenseID);
            try
            {
                await connection.OpenAsync();
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return new LicenseFullOutputDTO
                    {
        LicenseID = (int)reader["LicenseID"],
        ApplicationID = (int)reader["ApplicationID"],
        DriverID = (int)reader["DriverID"],
        LicenseClass = (int)reader["LicenseClass"],
        IssueDate = (DateTime)reader["IssueDate"],
        ExpirationDate = (DateTime)reader["ExpirationDate"],
        Notes = (reader["Notes"] == DBNull.Value) ? "" : (string)reader["Notes"],
        PaidFees = (decimal)reader["PaidFees"],
        IsActive = (bool)reader["IsActive"],
        IssueReason = (byte)reader["IssueReason"],
        CreatedByUserID = (int)reader["CreatedByUserID"]
                    };
                }
            }
            catch (Exception) { throw; }
            return null;
        }
        public static async Task<bool> updateLicense(LicenseFullOutputDTO dto)
        {
            int rowsAffected = 0;
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_Licenses_Update", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(@"LicenseID", SqlDbType.Int).Value = dto.LicenseID;
		    command.Parameters.Add(@"ApplicationID", SqlDbType.Int).Value = dto.ApplicationID;
		    command.Parameters.Add(@"DriverID", SqlDbType.Int).Value = dto.DriverID;
		    command.Parameters.Add(@"LicenseClass", SqlDbType.Int).Value = dto.LicenseClass;
		    command.Parameters.Add(@"IssueDate", SqlDbType.DateTime).Value = dto.IssueDate;
		    command.Parameters.Add(@"ExpirationDate", SqlDbType.DateTime).Value = dto.ExpirationDate;
		    command.Parameters.Add(@"Notes", SqlDbType.NVarChar).Value = string.IsNullOrEmpty(dto.Notes) ? DBNull.Value : (object)dto.Notes;
		    command.Parameters.Add(@"PaidFees", SqlDbType.Decimal).Value = dto.PaidFees;
		    command.Parameters.Add(@"IsActive", SqlDbType.Bit).Value = dto.IsActive;
		    command.Parameters.Add(@"IssueReason", SqlDbType.TinyInt).Value = dto.IssueReason;
		    command.Parameters.Add(@"CreatedByUserID", SqlDbType.Int).Value = dto.CreatedByUserID;
            try
            {
                await connection.OpenAsync();
                rowsAffected = await command.ExecuteNonQueryAsync();
            }
            catch (Exception) { throw; }
            return (rowsAffected > 0);
        }
        public static async Task<bool> deleteLicense(int LicenseID)
        {
            int rowsAffected = 0;
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_Licenses_Delete", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@LicenseID", LicenseID);
            try
            {
                await connection.OpenAsync();
                rowsAffected = await command.ExecuteNonQueryAsync();
            }
            catch (Exception) { throw; }
            return (rowsAffected > 0);
        }
        public static async Task<bool> isLicenseExistByID(int LicenseID)
        {
            bool isFound = false;
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_Licenses_IsExistByLicenseID", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@LicenseID", LicenseID);
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
        public static async Task<List<LicenseFullOutputDTO>> PagingDAL(int RowsPerPage, int PageNumber, string SortColumn, string Direction)
        {
            var list = new List<LicenseFullOutputDTO>();
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_Licenses_Paging", connection);
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
                    list.Add(new LicenseFullOutputDTO
                    {
        LicenseID = (int)reader["LicenseID"],
        ApplicationID = (int)reader["ApplicationID"],
        DriverID = (int)reader["DriverID"],
        LicenseClass = (int)reader["LicenseClass"],
        IssueDate = (DateTime)reader["IssueDate"],
        ExpirationDate = (DateTime)reader["ExpirationDate"],
        Notes = (reader["Notes"] == DBNull.Value) ? "" : (string)reader["Notes"],
        PaidFees = (decimal)reader["PaidFees"],
        IsActive = (bool)reader["IsActive"],
        IssueReason = (byte)reader["IssueReason"],
        CreatedByUserID = (int)reader["CreatedByUserID"]
                    });
                }
            }
            catch (Exception) { throw; }
            return list;
        }
        public static async Task<List<LicenseFullOutputDTO>> getAll()
        {
            var list = new List<LicenseFullOutputDTO>();
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_Licenses_SelectAll", connection);
            command.CommandType = CommandType.StoredProcedure;
            try
            {
                await connection.OpenAsync();
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    list.Add(new LicenseFullOutputDTO
                    {
        LicenseID = (int)reader["LicenseID"],
        ApplicationID = (int)reader["ApplicationID"],
        DriverID = (int)reader["DriverID"],
        LicenseClass = (int)reader["LicenseClass"],
        IssueDate = (DateTime)reader["IssueDate"],
        ExpirationDate = (DateTime)reader["ExpirationDate"],
        Notes = (reader["Notes"] == DBNull.Value) ? "" : (string)reader["Notes"],
        PaidFees = (decimal)reader["PaidFees"],
        IsActive = (bool)reader["IsActive"],
        IssueReason = (byte)reader["IssueReason"],
        CreatedByUserID = (int)reader["CreatedByUserID"]
                    });
                }
            }
            catch (Exception) { throw; }
            return list;
        }
        public static async Task<LicenseFullOutputDTO> getLicenseByApplicationID(int ApplicationID)
        {
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_Licenses_SelectByApplicationID", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            try
            {
                await connection.OpenAsync();
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return new LicenseFullOutputDTO
                    {
        LicenseID = (int)reader["LicenseID"],
        ApplicationID = (int)reader["ApplicationID"],
        DriverID = (int)reader["DriverID"],
        LicenseClass = (int)reader["LicenseClass"],
        IssueDate = (DateTime)reader["IssueDate"],
        ExpirationDate = (DateTime)reader["ExpirationDate"],
        Notes = (reader["Notes"] == DBNull.Value) ? "" : (string)reader["Notes"],
        PaidFees = (decimal)reader["PaidFees"],
        IsActive = (bool)reader["IsActive"],
        IssueReason = (byte)reader["IssueReason"],
        CreatedByUserID = (int)reader["CreatedByUserID"]
                    };
                }
            }
            catch (Exception) { throw; }
            return null;
        }
        public static async Task<bool> isLicenseExistByApplicationID(int ApplicationID)
        {
            bool isFound = false;
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_Licenses_IsExistByApplicationID", connection);
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
        public static async Task<List<LicenseFullOutputDTO>> getAllByDriverID(int DriverID)
        {
            var list = new List<LicenseFullOutputDTO>();
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_Licenses_SelectAllByDriverID", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@DriverID", DriverID);
            try
            {
                await connection.OpenAsync();
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    list.Add(new LicenseFullOutputDTO
                    {
        LicenseID = (int)reader["LicenseID"],
        ApplicationID = (int)reader["ApplicationID"],
        DriverID = (int)reader["DriverID"],
        LicenseClass = (int)reader["LicenseClass"],
        IssueDate = (DateTime)reader["IssueDate"],
        ExpirationDate = (DateTime)reader["ExpirationDate"],
        Notes = (reader["Notes"] == DBNull.Value) ? "" : (string)reader["Notes"],
        PaidFees = (decimal)reader["PaidFees"],
        IsActive = (bool)reader["IsActive"],
        IssueReason = (byte)reader["IssueReason"],
        CreatedByUserID = (int)reader["CreatedByUserID"]
                    });
                }
            }
            catch (Exception) { throw; }
            return list;
        }
        public static async Task<List<LicenseFullOutputDTO>> getAllByCreatedByUserID(int CreatedByUserID)
        {
            var list = new List<LicenseFullOutputDTO>();
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_Licenses_SelectAllByCreatedByUserID", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            try
            {
                await connection.OpenAsync();
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    list.Add(new LicenseFullOutputDTO
                    {
        LicenseID = (int)reader["LicenseID"],
        ApplicationID = (int)reader["ApplicationID"],
        DriverID = (int)reader["DriverID"],
        LicenseClass = (int)reader["LicenseClass"],
        IssueDate = (DateTime)reader["IssueDate"],
        ExpirationDate = (DateTime)reader["ExpirationDate"],
        Notes = (reader["Notes"] == DBNull.Value) ? "" : (string)reader["Notes"],
        PaidFees = (decimal)reader["PaidFees"],
        IsActive = (bool)reader["IsActive"],
        IssueReason = (byte)reader["IssueReason"],
        CreatedByUserID = (int)reader["CreatedByUserID"]
                    });
                }
            }
            catch (Exception) { throw; }
            return list;
        }  
    }
}