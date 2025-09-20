using LibraryManagementSystem.DTOs;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.DAL
{

    public class IssuedBookData {

        // 🔹 Get book by ID
        public static IssuedBookDTO GetIssuedBookInfoByID(int issueID) {
            try {
                using (SqlConnection conn = new SqlConnection(DataAccessSettings.connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_GetIssuedBookByID", conn)) {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IssueID", issueID);

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())  {
                        if (reader.Read()) {
                            return new IssuedBookDTO(
                                reader.GetInt32(reader.GetOrdinal("IssueID")),
                                reader.GetInt32(reader.GetOrdinal("CopyID")),
                                reader.GetInt32(reader.GetOrdinal("MemberID")),
                                reader.GetInt32(reader.GetOrdinal("IssuedBy")),
                                reader.IsDBNull(reader.GetOrdinal("IssueDate")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("IssueDate")),
                                reader.IsDBNull(reader.GetOrdinal("DueDate")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("DueDate")),
                                reader.IsDBNull(reader.GetOrdinal("ReturnDate")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("ReturnDate")),
                                reader.GetBoolean(reader.GetOrdinal("IsReturned"))
                            );
                        }
                    }
                }
            }catch{
                // Optionally log the error
            }

            return null;
        }

        // 🔹 Get all Issued Books (optional enhancement: return List<IssuedBookDTO>)
        public static List<IssuedBookDTO> GetAllIssuedBooks() {
            var issuedBooks = new List<IssuedBookDTO>();

            try {
                using (SqlConnection conn = new SqlConnection(DataAccessSettings.connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_GetAllIssuedBooks", conn)) {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader()) {
                        while (reader.Read()) {
                            var issuedBook = new IssuedBookDTO(
                                reader.GetInt32(reader.GetOrdinal("IssueID")),
                                reader.GetInt32(reader.GetOrdinal("CopyID")),
                                reader.GetInt32(reader.GetOrdinal("MemberID")),
                                reader.GetInt32(reader.GetOrdinal("IssuedBy")),
                                reader.IsDBNull(reader.GetOrdinal("IssueDate")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("IssueDate")),
                                reader.IsDBNull(reader.GetOrdinal("DueDate")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("DueDate")),
                                reader.IsDBNull(reader.GetOrdinal("ReturnDate")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("ReturnDate")),
                                reader.GetBoolean(reader.GetOrdinal("IsReturned"))
                            );
                            issuedBook.Copy = BookCopyData.GetBookCopyInfoByID(issuedBook.CopyID);
                            issuedBook.MemberInfo = MemberData.GetMemberByID(issuedBook.MemberID);
                            issuedBook.IssuedUser = UserData.GetUserByID(issuedBook.IssuedBy);
                            issuedBooks.Add(issuedBook);
                        }
                    }
                }
            }catch (Exception ex) {
                Console.WriteLine("Error GetAllBookCopies: " + ex.Message);
                // Log exception if needed
            }

            return issuedBooks;
        }

        // 🔹 Add new book (using DTO, returns new ID)
        public static int AddNewIssuedBook(IssuedBookDTO issuedBook) {
            try {
                using (SqlConnection conn = new SqlConnection(DataAccessSettings.connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_AddNewIssuedBook", conn)) {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@CopyID", issuedBook.CopyID);
                    cmd.Parameters.AddWithValue("@MemberID", issuedBook.MemberID);
                    cmd.Parameters.AddWithValue("@IssuedBy", issuedBook.IssuedBy);
                    cmd.Parameters.AddWithValue("@DueDate", issuedBook.DueDate);

                    conn.Open();
                    var result = cmd.ExecuteScalar();
                    return result != null && int.TryParse(result.ToString(), out int id) ? id : -1;
                }
            } catch {
                return -1;
            }
        }

        // 🔹 Update book (using DTO)
        public static bool UpdateIssuedBook(IssuedBookDTO issuedBook) {
            try {
                using (SqlConnection conn = new SqlConnection(DataAccessSettings.connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_UpdateIssuedBook", conn)) {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IssueID", issuedBook.ID);
                    cmd.Parameters.AddWithValue("@CopyID", issuedBook.CopyID);
                    cmd.Parameters.AddWithValue("@MemberID", issuedBook.MemberID);
                    cmd.Parameters.AddWithValue("@DueDate", issuedBook.DueDate);

                    if (issuedBook.ReturnDate != DateTime.MinValue)
                        cmd.Parameters.AddWithValue("@ReturnDate", issuedBook.ReturnDate);
                    else
                        cmd.Parameters.AddWithValue("@ReturnDate", System.DBNull.Value);

                    cmd.Parameters.AddWithValue("@IsReturned", issuedBook.IsReturned);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            } catch {
                return false;
            }
        }

        // 🔹 Delete book
        public static bool DeleteIssuedBook(int issueID) {
            try {
                using (SqlConnection conn = new SqlConnection(DataAccessSettings.connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_DeleteIssuedBook", conn)) {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IssueID", issueID);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            } catch {
                return false;
            }
        }

        public static bool MarkBookAsReturned(int issueID, DateTime? returnDate = null) {
            try {
                using (SqlConnection conn = new SqlConnection(DataAccessSettings.connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_MarkBookAsReturned", conn)) {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IssueID", issueID);

                    if (returnDate.HasValue)
                        cmd.Parameters.AddWithValue("@ReturnDate", returnDate.Value);
                    else
                        cmd.Parameters.AddWithValue("@ReturnDate", DBNull.Value);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }catch (Exception ex) {
                // Log the error if needed
                Console.WriteLine("Error in MarkBookAsReturned: " + ex.Message);
                return false;
            }
        }

        public static bool IsBookReturned(int issueID) {
            using (SqlConnection conn = new SqlConnection(DataAccessSettings.connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_IsBookReturned", conn)) {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IssueID", issueID);
                var returnParameter = cmd.Parameters.Add("@RETURN_VALUE", SqlDbType.Int);
                returnParameter.Direction = ParameterDirection.ReturnValue;

                conn.Open();
                cmd.ExecuteNonQuery();

                return (int)returnParameter.Value == 1;
            }
        }


    }
}
