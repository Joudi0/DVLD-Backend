using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Shared;

namespace DAL
{
    public class ApplicationDAL
    {  

        public static async Task<int> addApplication(ApplicationFullOutputDTO dto)
        {
            int ApplicationID = -1;
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_Applications_Insert", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(@"ApplicantPersonID", SqlDbType.Int).Value = dto.ApplicantPersonID;
		    command.Parameters.Add(@"ApplicationDate", SqlDbType.DateTime).Value = dto.ApplicationDate;
		    command.Parameters.Add(@"ApplicationTypeID", SqlDbType.Int).Value = dto.ApplicationTypeID;
		    command.Parameters.Add(@"ApplicationStatus", SqlDbType.TinyInt).Value = dto.ApplicationStatus;
		    command.Parameters.Add(@"LastStatusDate", SqlDbType.DateTime).Value = dto.LastStatusDate;
		    command.Parameters.Add(@"PaidFees", SqlDbType.Decimal).Value = dto.PaidFees;
		    command.Parameters.Add(@"CreatedByUserID", SqlDbType.Int).Value = dto.CreatedByUserID;
		    command.Parameters.Add(@"IsDeleted", SqlDbType.Bit).Value = (dto.IsDeleted == null) ? DBNull.Value : (object)dto.IsDeleted;
            try
            {
                await connection.OpenAsync();
                object result = await command.ExecuteScalarAsync();
                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    ApplicationID = insertedID;
                }
            }
            catch (Exception) { throw; }
            return ApplicationID;
        }
        public static async Task<ApplicationFullOutputDTO> getApplicationByID(int ApplicationID)
        {
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_Applications_SelectByApplicationID", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            try
            {
                await connection.OpenAsync();
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return new ApplicationFullOutputDTO
                    {
        ApplicationID = (int)reader["ApplicationID"],
        ApplicantPersonID = (int)reader["ApplicantPersonID"],
        ApplicationDate = (DateTime)reader["ApplicationDate"],
        ApplicationTypeID = (int)reader["ApplicationTypeID"],
        ApplicationStatus = (byte)reader["ApplicationStatus"],
        LastStatusDate = (DateTime)reader["LastStatusDate"],
        PaidFees = (decimal)reader["PaidFees"],
        CreatedByUserID = (int)reader["CreatedByUserID"],
        IsDeleted = (reader["IsDeleted"] == DBNull.Value) ? null : (bool?)reader["IsDeleted"]
                    };
                }
            }
            catch (Exception) { throw; }
            return null;
        }
        public static async Task<bool> updateApplication(ApplicationFullOutputDTO dto)
        {
            int rowsAffected = 0;
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_Applications_Update", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(@"ApplicationID", SqlDbType.Int).Value = dto.ApplicationID;
		    command.Parameters.Add(@"ApplicantPersonID", SqlDbType.Int).Value = dto.ApplicantPersonID;
		    command.Parameters.Add(@"ApplicationDate", SqlDbType.DateTime).Value = dto.ApplicationDate;
		    command.Parameters.Add(@"ApplicationTypeID", SqlDbType.Int).Value = dto.ApplicationTypeID;
		    command.Parameters.Add(@"ApplicationStatus", SqlDbType.TinyInt).Value = dto.ApplicationStatus;
		    command.Parameters.Add(@"LastStatusDate", SqlDbType.DateTime).Value = dto.LastStatusDate;
		    command.Parameters.Add(@"PaidFees", SqlDbType.Decimal).Value = dto.PaidFees;
		    command.Parameters.Add(@"CreatedByUserID", SqlDbType.Int).Value = dto.CreatedByUserID;
		    command.Parameters.Add(@"IsDeleted", SqlDbType.Bit).Value = (dto.IsDeleted == null) ? DBNull.Value : (object)dto.IsDeleted;
            try
            {
                await connection.OpenAsync();
                rowsAffected = await command.ExecuteNonQueryAsync();
            }
            catch (Exception) { throw; }
            return (rowsAffected > 0);
        }
        public static async Task<bool> deleteApplication(int ApplicationID)
        {
            int rowsAffected = 0;
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_Applications_Delete", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            try
            {
                await connection.OpenAsync();
                rowsAffected = await command.ExecuteNonQueryAsync();
            }
            catch (Exception) { throw; }
            return (rowsAffected > 0);
        }
        public static async Task<bool> isApplicationExistByID(int ApplicationID)
        {
            bool isFound = false;
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_Applications_IsExistByApplicationID", connection);
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
        public static async Task<List<ApplicationFullOutputDTO>> PagingDAL(int RowsPerPage, int PageNumber, string SortColumn, string Direction)
        {
            var list = new List<ApplicationFullOutputDTO>();
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_Applications_Paging", connection);
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
                    list.Add(new ApplicationFullOutputDTO
                    {
        ApplicationID = (int)reader["ApplicationID"],
        ApplicantPersonID = (int)reader["ApplicantPersonID"],
        ApplicationDate = (DateTime)reader["ApplicationDate"],
        ApplicationTypeID = (int)reader["ApplicationTypeID"],
        ApplicationStatus = (byte)reader["ApplicationStatus"],
        LastStatusDate = (DateTime)reader["LastStatusDate"],
        PaidFees = (decimal)reader["PaidFees"],
        CreatedByUserID = (int)reader["CreatedByUserID"],
        IsDeleted = (reader["IsDeleted"] == DBNull.Value) ? null : (bool?)reader["IsDeleted"]
                    });
                }
            }
            catch (Exception) { throw; }
            return list;
        }
        public static async Task<List<ApplicationFullOutputDTO>> getAll()
        {
            var list = new List<ApplicationFullOutputDTO>();
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_Applications_SelectAll", connection);
            command.CommandType = CommandType.StoredProcedure;
            try
            {
                await connection.OpenAsync();
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    list.Add(new ApplicationFullOutputDTO
                    {
        ApplicationID = (int)reader["ApplicationID"],
        ApplicantPersonID = (int)reader["ApplicantPersonID"],
        ApplicationDate = (DateTime)reader["ApplicationDate"],
        ApplicationTypeID = (int)reader["ApplicationTypeID"],
        ApplicationStatus = (byte)reader["ApplicationStatus"],
        LastStatusDate = (DateTime)reader["LastStatusDate"],
        PaidFees = (decimal)reader["PaidFees"],
        CreatedByUserID = (int)reader["CreatedByUserID"],
        IsDeleted = (reader["IsDeleted"] == DBNull.Value) ? null : (bool?)reader["IsDeleted"]
                    });
                }
            }
            catch (Exception) { throw; }
            return list;
        }
        public static async Task<List<ApplicationFullOutputDTO>> getAllByApplicantPersonID(int ApplicantPersonID)
        {
            var list = new List<ApplicationFullOutputDTO>();
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_Applications_SelectAllByApplicantPersonID", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);
            try
            {
                await connection.OpenAsync();
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    list.Add(new ApplicationFullOutputDTO
                    {
        ApplicationID = (int)reader["ApplicationID"],
        ApplicantPersonID = (int)reader["ApplicantPersonID"],
        ApplicationDate = (DateTime)reader["ApplicationDate"],
        ApplicationTypeID = (int)reader["ApplicationTypeID"],
        ApplicationStatus = (byte)reader["ApplicationStatus"],
        LastStatusDate = (DateTime)reader["LastStatusDate"],
        PaidFees = (decimal)reader["PaidFees"],
        CreatedByUserID = (int)reader["CreatedByUserID"],
        IsDeleted = (reader["IsDeleted"] == DBNull.Value) ? null : (bool?)reader["IsDeleted"]
                    });
                }
            }
            catch (Exception) { throw; }
            return list;
        }
        public static async Task<List<ApplicationFullOutputDTO>> getAllByApplicationTypeID(int ApplicationTypeID)
        {
            var list = new List<ApplicationFullOutputDTO>();
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_Applications_SelectAllByApplicationTypeID", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
            try
            {
                await connection.OpenAsync();
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    list.Add(new ApplicationFullOutputDTO
                    {
        ApplicationID = (int)reader["ApplicationID"],
        ApplicantPersonID = (int)reader["ApplicantPersonID"],
        ApplicationDate = (DateTime)reader["ApplicationDate"],
        ApplicationTypeID = (int)reader["ApplicationTypeID"],
        ApplicationStatus = (byte)reader["ApplicationStatus"],
        LastStatusDate = (DateTime)reader["LastStatusDate"],
        PaidFees = (decimal)reader["PaidFees"],
        CreatedByUserID = (int)reader["CreatedByUserID"],
        IsDeleted = (reader["IsDeleted"] == DBNull.Value) ? null : (bool?)reader["IsDeleted"]
                    });
                }
            }
            catch (Exception) { throw; }
            return list;
        }
        public static async Task<List<ApplicationFullOutputDTO>> getAllByCreatedByUserID(int CreatedByUserID)
        {
            var list = new List<ApplicationFullOutputDTO>();
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_Applications_SelectAllByCreatedByUserID", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            try
            {
                await connection.OpenAsync();
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    list.Add(new ApplicationFullOutputDTO
                    {
        ApplicationID = (int)reader["ApplicationID"],
        ApplicantPersonID = (int)reader["ApplicantPersonID"],
        ApplicationDate = (DateTime)reader["ApplicationDate"],
        ApplicationTypeID = (int)reader["ApplicationTypeID"],
        ApplicationStatus = (byte)reader["ApplicationStatus"],
        LastStatusDate = (DateTime)reader["LastStatusDate"],
        PaidFees = (decimal)reader["PaidFees"],
        CreatedByUserID = (int)reader["CreatedByUserID"],
        IsDeleted = (reader["IsDeleted"] == DBNull.Value) ? null : (bool?)reader["IsDeleted"]
                    });
                }
            }
            catch (Exception) { throw; }
            return list;
        }  
    }
}