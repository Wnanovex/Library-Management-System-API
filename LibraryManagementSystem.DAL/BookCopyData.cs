using LibraryManagementSystem.DTOs;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.DAL
{

    public class BookCopyData {

        // 🔹 Get book by ID
        public static BookCopyDTO GetBookCopyInfoByID(int copyID) {
            try {
                using (SqlConnection conn = new SqlConnection(DataAccessSettings.connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_GetBookCopyByID", conn)) {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CopyID", copyID);

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())  {
                        if (reader.Read()) {
                            return new BookCopyDTO(
                                reader.GetInt32(reader.GetOrdinal("CopyID")),
                                reader.GetInt32(reader.GetOrdinal("BookID")),
                                (BookCopyDTO.enStatus)reader.GetInt16(reader.GetOrdinal("Status")),
                                reader.IsDBNull(reader.GetOrdinal("DateAdded")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("DateAdded"))
                            );
                        }
                    }
                }
            }catch{
                // Optionally log the error
            }

            return null;
        }

        // 🔹 Get all books (optional enhancement: return List<BookCopyDTO>)
        public static List<BookCopyDTO> GetAllBookCopies() {
            var copies = new List<BookCopyDTO>();

            try {
                using (SqlConnection conn = new SqlConnection(DataAccessSettings.connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_GetAllBooksCopies2", conn)) {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader()) {
                        while (reader.Read()) {
                            var copy = new BookCopyDTO(
                                reader.GetInt32(reader.GetOrdinal("CopyID")),
                                reader.GetInt32(reader.GetOrdinal("BookID")),
                                (BookCopyDTO.enStatus)reader.GetInt16(reader.GetOrdinal("Status")),
                                reader.IsDBNull(reader.GetOrdinal("DateAdded")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("DateAdded"))
                            );
                            copy.Book = BookData.GetBookInfoByID(copy.BookID);
                            copies.Add(copy);
                        }
                    }
                }
            }catch (Exception ex) {
                Console.WriteLine("Error GetAllBookCopies: " + ex.Message);
                // Log exception if needed
            }

            return copies;
        }

        // 🔹 Add new book (using DTO, returns new ID)
        public static int AddNewBookCopy(BookCopyDTO copy) {
            try {
                using (SqlConnection conn = new SqlConnection(DataAccessSettings.connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_AddNewBookCopy", conn)) {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@BookID", copy.BookID);
                    cmd.Parameters.AddWithValue("@Status", (byte)copy.Status);

                    conn.Open();
                    var result = cmd.ExecuteScalar();
                    return result != null && int.TryParse(result.ToString(), out int id) ? id : -1;
                }
            } catch {
                return -1;
            }
        }

        // 🔹 Update book (using DTO)
        public static bool UpdateBookCopyStatus(BookCopyDTO copy) {
            try {
                using (SqlConnection conn = new SqlConnection(DataAccessSettings.connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_UpdateBookCopyStatus", conn)) {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@CopyID", copy.ID);
                    cmd.Parameters.AddWithValue("@Status", (byte)copy.Status);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            } catch {
                return false;
            }
        }

        // 🔹 Delete book
        public static bool DeleteBookCopy(int copyID) {
            try {
                using (SqlConnection conn = new SqlConnection(DataAccessSettings.connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_DeleteCopy", conn)) {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CopyID", copyID);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            } catch {
                return false;
            }
        }

        public static bool IsBookIssued(int copyID) {
            using (SqlConnection conn = new SqlConnection(DataAccessSettings.connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_IsBookIssued", conn)) {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CopyID", copyID);
                var returnParameter = cmd.Parameters.Add("@RETURN_VALUE", SqlDbType.Int);
                returnParameter.Direction = ParameterDirection.ReturnValue;

                conn.Open();
                cmd.ExecuteNonQuery();

                return (int)returnParameter.Value == 1;
            }
        }

        public static bool IsBookAvailable(int copyID) {
            using (SqlConnection conn = new SqlConnection(DataAccessSettings.connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_IsBookAvailable", conn)) {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CopyID", copyID);
                var returnParameter = cmd.Parameters.Add("@RETURN_VALUE", SqlDbType.Int);
                returnParameter.Direction = ParameterDirection.ReturnValue;

                conn.Open();
                cmd.ExecuteNonQuery();

                return (int)returnParameter.Value == 1;
            }
        }

    }
}
