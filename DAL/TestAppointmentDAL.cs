using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Shared;

namespace DAL
{
    public class TestAppointmentDAL
    {  

        public static async Task<int> addTestAppointment(TestAppointmentFullOutputDTO dto)
        {
            int TestAppointmentID = -1;
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_TestAppointments_Insert", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(@"TestTypeID", SqlDbType.Int).Value = dto.TestTypeID;
		    command.Parameters.Add(@"LocalDrivingLicenseApplicationID", SqlDbType.Int).Value = dto.LocalDrivingLicenseApplicationID;
		    command.Parameters.Add(@"AppointmentDate", SqlDbType.DateTime).Value = dto.AppointmentDate;
		    command.Parameters.Add(@"PaidFees", SqlDbType.Decimal).Value = dto.PaidFees;
		    command.Parameters.Add(@"CreatedByUserID", SqlDbType.Int).Value = dto.CreatedByUserID;
		    command.Parameters.Add(@"IsLocked", SqlDbType.Bit).Value = dto.IsLocked;
		    command.Parameters.Add(@"RetakeTestApplicationID", SqlDbType.Int).Value = (dto.RetakeTestApplicationID == null) ? DBNull.Value : (object)dto.RetakeTestApplicationID;
            try
            {
                await connection.OpenAsync();
                object result = await command.ExecuteScalarAsync();
                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    TestAppointmentID = insertedID;
                }
            }
            catch (Exception) { throw; }
            return TestAppointmentID;
        }
        public static async Task<TestAppointmentFullOutputDTO> getTestAppointmentByID(int TestAppointmentID)
        {
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_TestAppointments_SelectByTestAppointmentID", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
            try
            {
                await connection.OpenAsync();
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return new TestAppointmentFullOutputDTO
                    {
        TestAppointmentID = (int)reader["TestAppointmentID"],
        TestTypeID = (int)reader["TestTypeID"],
        LocalDrivingLicenseApplicationID = (int)reader["LocalDrivingLicenseApplicationID"],
        AppointmentDate = (DateTime)reader["AppointmentDate"],
        PaidFees = (decimal)reader["PaidFees"],
        CreatedByUserID = (int)reader["CreatedByUserID"],
        IsLocked = (bool)reader["IsLocked"],
        RetakeTestApplicationID = (reader["RetakeTestApplicationID"] == DBNull.Value) ? null : (int?)reader["RetakeTestApplicationID"]
                    };
                }
            }
            catch (Exception) { throw; }
            return null;
        }
        public static async Task<bool> updateTestAppointment(TestAppointmentFullOutputDTO dto)
        {
            int rowsAffected = 0;
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_TestAppointments_Update", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(@"TestAppointmentID", SqlDbType.Int).Value = dto.TestAppointmentID;
		    command.Parameters.Add(@"TestTypeID", SqlDbType.Int).Value = dto.TestTypeID;
		    command.Parameters.Add(@"LocalDrivingLicenseApplicationID", SqlDbType.Int).Value = dto.LocalDrivingLicenseApplicationID;
		    command.Parameters.Add(@"AppointmentDate", SqlDbType.DateTime).Value = dto.AppointmentDate;
		    command.Parameters.Add(@"PaidFees", SqlDbType.Decimal).Value = dto.PaidFees;
		    command.Parameters.Add(@"CreatedByUserID", SqlDbType.Int).Value = dto.CreatedByUserID;
		    command.Parameters.Add(@"IsLocked", SqlDbType.Bit).Value = dto.IsLocked;
		    command.Parameters.Add(@"RetakeTestApplicationID", SqlDbType.Int).Value = (dto.RetakeTestApplicationID == null) ? DBNull.Value : (object)dto.RetakeTestApplicationID;
            try
            {
                await connection.OpenAsync();
                rowsAffected = await command.ExecuteNonQueryAsync();
            }
            catch (Exception) { throw; }
            return (rowsAffected > 0);
        }
        public static async Task<bool> deleteTestAppointment(int TestAppointmentID)
        {
            int rowsAffected = 0;
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_TestAppointments_Delete", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
            try
            {
                await connection.OpenAsync();
                rowsAffected = await command.ExecuteNonQueryAsync();
            }
            catch (Exception) { throw; }
            return (rowsAffected > 0);
        }
        public static async Task<bool> isTestAppointmentExistByID(int TestAppointmentID)
        {
            bool isFound = false;
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_TestAppointments_IsExistByTestAppointmentID", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
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
        public static async Task<List<TestAppointmentFullOutputDTO>> PagingDAL(int RowsPerPage, int PageNumber, string SortColumn, string Direction)
        {
            var list = new List<TestAppointmentFullOutputDTO>();
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_TestAppointments_Paging", connection);
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
                    list.Add(new TestAppointmentFullOutputDTO
                    {
        TestAppointmentID = (int)reader["TestAppointmentID"],
        TestTypeID = (int)reader["TestTypeID"],
        LocalDrivingLicenseApplicationID = (int)reader["LocalDrivingLicenseApplicationID"],
        AppointmentDate = (DateTime)reader["AppointmentDate"],
        PaidFees = (decimal)reader["PaidFees"],
        CreatedByUserID = (int)reader["CreatedByUserID"],
        IsLocked = (bool)reader["IsLocked"],
        RetakeTestApplicationID = (reader["RetakeTestApplicationID"] == DBNull.Value) ? null : (int?)reader["RetakeTestApplicationID"]
                    });
                }
            }
            catch (Exception) { throw; }
            return list;
        }
        public static async Task<List<TestAppointmentFullOutputDTO>> getAll()
        {
            var list = new List<TestAppointmentFullOutputDTO>();
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_TestAppointments_SelectAll", connection);
            command.CommandType = CommandType.StoredProcedure;
            try
            {
                await connection.OpenAsync();
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    list.Add(new TestAppointmentFullOutputDTO
                    {
        TestAppointmentID = (int)reader["TestAppointmentID"],
        TestTypeID = (int)reader["TestTypeID"],
        LocalDrivingLicenseApplicationID = (int)reader["LocalDrivingLicenseApplicationID"],
        AppointmentDate = (DateTime)reader["AppointmentDate"],
        PaidFees = (decimal)reader["PaidFees"],
        CreatedByUserID = (int)reader["CreatedByUserID"],
        IsLocked = (bool)reader["IsLocked"],
        RetakeTestApplicationID = (reader["RetakeTestApplicationID"] == DBNull.Value) ? null : (int?)reader["RetakeTestApplicationID"]
                    });
                }
            }
            catch (Exception) { throw; }
            return list;
        }
        public static async Task<TestAppointmentFullOutputDTO> getTestAppointmentByLocalDrivingLicenseApplicationID(int LocalDrivingLicenseApplicationID)
        {
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_TestAppointments_SelectByLocalDrivingLicenseApplicationID", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            try
            {
                await connection.OpenAsync();
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return new TestAppointmentFullOutputDTO
                    {
        TestAppointmentID = (int)reader["TestAppointmentID"],
        TestTypeID = (int)reader["TestTypeID"],
        LocalDrivingLicenseApplicationID = (int)reader["LocalDrivingLicenseApplicationID"],
        AppointmentDate = (DateTime)reader["AppointmentDate"],
        PaidFees = (decimal)reader["PaidFees"],
        CreatedByUserID = (int)reader["CreatedByUserID"],
        IsLocked = (bool)reader["IsLocked"],
        RetakeTestApplicationID = (reader["RetakeTestApplicationID"] == DBNull.Value) ? null : (int?)reader["RetakeTestApplicationID"]
                    };
                }
            }
            catch (Exception) { throw; }
            return null;
        }
        public static async Task<TestAppointmentFullOutputDTO> getTestAppointmentByRetakeTestApplicationID(int? RetakeTestApplicationID)
        {
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_TestAppointments_SelectByRetakeTestApplicationID", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@RetakeTestApplicationID", RetakeTestApplicationID);
            try
            {
                await connection.OpenAsync();
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return new TestAppointmentFullOutputDTO
                    {
        TestAppointmentID = (int)reader["TestAppointmentID"],
        TestTypeID = (int)reader["TestTypeID"],
        LocalDrivingLicenseApplicationID = (int)reader["LocalDrivingLicenseApplicationID"],
        AppointmentDate = (DateTime)reader["AppointmentDate"],
        PaidFees = (decimal)reader["PaidFees"],
        CreatedByUserID = (int)reader["CreatedByUserID"],
        IsLocked = (bool)reader["IsLocked"],
        RetakeTestApplicationID = (reader["RetakeTestApplicationID"] == DBNull.Value) ? null : (int?)reader["RetakeTestApplicationID"]
                    };
                }
            }
            catch (Exception) { throw; }
            return null;
        }
        public static async Task<bool> isTestAppointmentExistByLocalDrivingLicenseApplicationID(int LocalDrivingLicenseApplicationID)
        {
            bool isFound = false;
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_TestAppointments_IsExistByLocalDrivingLicenseApplicationID", connection);
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
        public static async Task<bool> isTestAppointmentExistByRetakeTestApplicationID(int? RetakeTestApplicationID)
        {
            bool isFound = false;
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_TestAppointments_IsExistByRetakeTestApplicationID", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@RetakeTestApplicationID", RetakeTestApplicationID);
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
        public static async Task<List<TestAppointmentFullOutputDTO>> getAllByTestTypeID(int TestTypeID)
        {
            var list = new List<TestAppointmentFullOutputDTO>();
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_TestAppointments_SelectAllByTestTypeID", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            try
            {
                await connection.OpenAsync();
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    list.Add(new TestAppointmentFullOutputDTO
                    {
        TestAppointmentID = (int)reader["TestAppointmentID"],
        TestTypeID = (int)reader["TestTypeID"],
        LocalDrivingLicenseApplicationID = (int)reader["LocalDrivingLicenseApplicationID"],
        AppointmentDate = (DateTime)reader["AppointmentDate"],
        PaidFees = (decimal)reader["PaidFees"],
        CreatedByUserID = (int)reader["CreatedByUserID"],
        IsLocked = (bool)reader["IsLocked"],
        RetakeTestApplicationID = (reader["RetakeTestApplicationID"] == DBNull.Value) ? null : (int?)reader["RetakeTestApplicationID"]
                    });
                }
            }
            catch (Exception) { throw; }
            return list;
        }
        public static async Task<List<TestAppointmentFullOutputDTO>> getAllByCreatedByUserID(int CreatedByUserID)
        {
            var list = new List<TestAppointmentFullOutputDTO>();
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_TestAppointments_SelectAllByCreatedByUserID", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            try
            {
                await connection.OpenAsync();
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    list.Add(new TestAppointmentFullOutputDTO
                    {
        TestAppointmentID = (int)reader["TestAppointmentID"],
        TestTypeID = (int)reader["TestTypeID"],
        LocalDrivingLicenseApplicationID = (int)reader["LocalDrivingLicenseApplicationID"],
        AppointmentDate = (DateTime)reader["AppointmentDate"],
        PaidFees = (decimal)reader["PaidFees"],
        CreatedByUserID = (int)reader["CreatedByUserID"],
        IsLocked = (bool)reader["IsLocked"],
        RetakeTestApplicationID = (reader["RetakeTestApplicationID"] == DBNull.Value) ? null : (int?)reader["RetakeTestApplicationID"]
                    });
                }
            }
            catch (Exception) { throw; }
            return list;
        }  
    }
}