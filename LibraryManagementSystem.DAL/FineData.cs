using LibraryManagementSystem.DTOs;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.DAL
{

    public class FineData {

        // 🔹 Get Fine by ID
        public static FineDTO GetFineInfoByID(int fineID) {
            try {
                using (SqlConnection conn = new SqlConnection(DataAccessSettings.connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_GetFineByID", conn)) {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FineID", fineID);

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())  {
                        if (reader.Read()) {
                            return new FineDTO(
                                reader.GetInt32(reader.GetOrdinal("FineID")),
                                reader.GetInt32(reader.GetOrdinal("IssueID")),
                                (float)reader.GetDecimal(reader.GetOrdinal("Amount")),
                                reader.GetBoolean(reader.GetOrdinal("Paid")),
                                reader.IsDBNull(reader.GetOrdinal("DatePaid")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("DatePaid"))
                            );
                        }
                    }
                }
            }catch{
                // Optionally log the error
            }

            return null;
        }

        // 🔹 Get all Fines (optional enhancement: return List<FineDTO>)
        public static List<FineDTO> GetAllFines() {
            var Fines = new List<FineDTO>();

            try {
                using (SqlConnection conn = new SqlConnection(DataAccessSettings.connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_GetAllFines", conn)) {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader()) {
                        while (reader.Read()) {
                            var Fine = new FineDTO(
                                reader.GetInt32(reader.GetOrdinal("FineID")),
                                reader.GetInt32(reader.GetOrdinal("IssueID")),
                                (float)reader.GetDecimal(reader.GetOrdinal("Amount")),
                                reader.GetBoolean(reader.GetOrdinal("Paid")),
                                reader.IsDBNull(reader.GetOrdinal("DatePaid")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("DatePaid"))
                            );
                            Fines.Add(Fine);
                        }
                    }
                }
            }catch (Exception ex) {
                Console.WriteLine("Error GetAllFines: " + ex.Message);
                // Log exception if needed
            }

            return Fines;
        }

        // 🔹 Add new fine (using DTO, returns new ID)
        public static int AddNewFine(FineDTO fine) {
            try {
                using (SqlConnection conn = new SqlConnection(DataAccessSettings.connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_AddNewFine", conn)) {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IssueID", fine.IssueID);

                    conn.Open();
                    var result = cmd.ExecuteScalar();
                    return result != null && int.TryParse(result.ToString(), out int id) ? id : -1;
                }
            } catch {
                return -1;
            }
        }

        // 🔹 Update fine (using DTO)
        //public static bool UpdateFine(FineDTO Fine) {
        //    try {
        //        using (SqlConnection conn = new SqlConnection(DataAccessSettings.connectionString))
        //        using (SqlCommand cmd = new SqlCommand("sp_UpdateFine", conn)) {
        //            cmd.CommandType = CommandType.StoredProcedure;

        //            cmd.Parameters.AddWithValue("@FineID", fineID);
        //            cmd.Parameters.AddWithValue("@Amount", amount);
        //            cmd.Parameters.AddWithValue("@Paid", paid);
        //            cmd.Parameters.AddWithValue("@DatePaid", datePaid);

        //            conn.Open();
        //            return cmd.ExecuteNonQuery() > 0;
        //        }
        //    } catch {
        //        return false;
        //    }
        //}

        // 🔹 Delete book
        public static bool DeleteFine(int fineID) {
            try {
                using (SqlConnection conn = new SqlConnection(DataAccessSettings.connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_DeleteFine", conn)) {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FineID", fineID);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            } catch {
                return false;
            }
        }

        public static bool PayFine(int fineID) {
            try {
                using (SqlConnection conn = new SqlConnection(DataAccessSettings.connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_PayFine", conn)) {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@FineID", fineID);

                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();

                    return rows > 0;
                }
            }catch {
                // Optionally log the error
                return false;
            }
        }


    }
}
