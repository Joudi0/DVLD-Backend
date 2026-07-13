using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Shared;

namespace DAL
{
    public class CountryDAL
    {  

        public static async Task<CountryFullOutputDTO> getCountryByID(int CountryID)
        {
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_Countries_SelectByCountryID", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@CountryID", CountryID);
            try
            {
                await connection.OpenAsync();
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return new CountryFullOutputDTO
                    {
        CountryID = (int)reader["CountryID"],
        CountryName = (string)reader["CountryName"]
                    };
                }
            }
            catch (Exception) { throw; }
            return null;
        }
        public static async Task<List<CountryFullOutputDTO>> getAll()
        {
            var list = new List<CountryFullOutputDTO>();
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_Countries_SelectAll", connection);
            command.CommandType = CommandType.StoredProcedure;
            try
            {
                await connection.OpenAsync();
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    list.Add(new CountryFullOutputDTO
                    {
        CountryID = (int)reader["CountryID"],
        CountryName = (string)reader["CountryName"]
                    });
                }
            }
            catch (Exception) { throw; }
            return list;
        }  
    }
}