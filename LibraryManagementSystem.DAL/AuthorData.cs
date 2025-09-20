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

    public class AuthorData {

        // 🔹 Get author by ID
        public static AuthorDTO GetAuthorByID(int authorID) {
            try {
                using (SqlConnection conn = new SqlConnection(DataAccessSettings.connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_GetAuthorByID", conn)) {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@AuthorID", authorID);

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())  {
                        if (reader.Read()) {
                            return new AuthorDTO(
                                PersonData.GetPersonInfoByID(reader.GetInt32(reader.GetOrdinal("PersonID"))),
                                reader.GetInt32(reader.GetOrdinal("AuthorID")),
                                reader.GetInt32(reader.GetOrdinal("PersonID")),
                                reader.GetString(reader.GetOrdinal("Biography"))
                            );
                        }
                    }
                }
            }catch (Exception ex) {
                Console.WriteLine("Error GetAuthorByID: " + ex.Message);
                // Optionally log the error
            }

            return null;
        }
      

        // 🔹 Get all author (optional enhancement: return List<PersonDTO>)
        public static List<AuthorDTO> GetAllAuthors() {
            var authors = new List<AuthorDTO>();

            try {
                using (SqlConnection conn = new SqlConnection(DataAccessSettings.connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_GetAllAuthors2", conn)) {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader()) {
                        while (reader.Read()) {
                            var author = new AuthorDTO(
                                PersonData.GetPersonInfoByID(reader.GetInt32(reader.GetOrdinal("PersonID"))),
                                reader.GetInt32(reader.GetOrdinal("AuthorID")),
                                reader.GetInt32(reader.GetOrdinal("PersonID")),
                                reader.GetString(reader.GetOrdinal("Biography"))
                            );
                            authors.Add(author);
                        }
                    }
                }
            }catch (Exception ex) {
                Console.WriteLine("Error GetAllAuthors: " + ex.ToString());
                // Log exception if needed
            }

            return authors;
        }

        // 🔹 Add new author (using DTO, returns new ID)
        public static int AddNewAuthor(AuthorDTO author) {
            try {
                using (SqlConnection conn = new SqlConnection(DataAccessSettings.connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_AddNewAuthor", conn)) {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@PersonID", author.PersonID);
                    cmd.Parameters.AddWithValue("@Biography", author.Biography);

                    conn.Open();
                    var result = cmd.ExecuteScalar();
                    return result != null && int.TryParse(result.ToString(), out int id) ? id : -1;
                }
            } catch {
                return -1;
            }
        }

        // 🔹 Update author (using DTO)
        public static bool UpdateAuthor(AuthorDTO author) {
            try {
                using (SqlConnection conn = new SqlConnection(DataAccessSettings.connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_UpdateAuthor", conn)) {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@AuthorID", author.ID);
                    cmd.Parameters.AddWithValue("@Biography", author.Biography);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            } catch {
                return false;
            }
        }

        // 🔹 Delete author
        public static bool DeleteAuthor(int authorID) {
            try {
                using (SqlConnection conn = new SqlConnection(DataAccessSettings.connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_DeleteAuthor", conn)) {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@AuthorID", authorID);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            } catch {
                return false;
            }
        }

        // 🔹 Check if author exists
        public static bool IsAuthorExist(int authorID) {
            try {
                using (SqlConnection conn = new SqlConnection(DataAccessSettings.connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_IsAuthorExist", conn)) {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@AuthorID", authorID);

                    var returnParameter = cmd.Parameters.Add("@RETURN_VALUE", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    return (int)returnParameter.Value == 1;
                }
            } catch {
                return false;
            }
        }


    }
}
