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

    public class UserData {

        // 🔹 Get person by ID
        public static UserDTO GetUserByID(int userID) {
            try {
                using (SqlConnection conn = new SqlConnection(DataAccessSettings.connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_GetUserByID", conn)) {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserID", userID);

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())  {
                        if (reader.Read()) {
                            return new UserDTO(
                                PersonData.GetPersonInfoByID(reader.GetInt32(reader.GetOrdinal("PersonID"))),
                                reader.GetInt32(reader.GetOrdinal("UserID")),
                                reader.GetInt32(reader.GetOrdinal("PersonID")),
                                reader.GetString(reader.GetOrdinal("Username")),
                                reader.GetString(reader.GetOrdinal("Password")),
                                (byte)reader.GetInt16(reader.GetOrdinal("Role")),
                                reader.IsDBNull(reader.GetOrdinal("LastLogin")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("LastLogin")),
                                reader.GetBoolean(reader.GetOrdinal("IsActive"))
                            );
                        }
                    }
                }
            }catch (Exception ex) {
                Console.WriteLine("Error GetUserByID: " + ex.ToString());
                // Optionally log the error
            }

            return null;
        }

        public static UserDTO GetUserByUsernameAndPassword(string username, string password) {
            try {
                using (SqlConnection conn = new SqlConnection(DataAccessSettings.connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_GetUserByUsernameAndPassword", conn)) {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", password);

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())  {
                        if (reader.Read()) {
                            return new UserDTO(
                                PersonData.GetPersonInfoByID(reader.GetInt32(reader.GetOrdinal("PersonID"))),
                                reader.GetInt32(reader.GetOrdinal("UserID")),
                                reader.GetInt32(reader.GetOrdinal("PersonID")),
                                reader.GetString(reader.GetOrdinal("Username")),
                                reader.GetString(reader.GetOrdinal("Password")),
                                (byte)reader.GetInt16(reader.GetOrdinal("Role")),
                                reader.IsDBNull(reader.GetOrdinal("LastLogin")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("LastLogin")),
                                reader.GetBoolean(reader.GetOrdinal("IsActive"))
                            );
                        }
                    }
                }
            }catch(Exception ex) {
                Console.WriteLine("Error GetUserByUsernameAndPassword: " + ex.Message);           
            }

            return null;
        }

        // 🔹 Get all people (optional enhancement: return List<PersonDTO>)
        public static List<UserDTO> GetAllUsers() {
            var users = new List<UserDTO>();

            try {
                using (SqlConnection conn = new SqlConnection(DataAccessSettings.connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_GetAllUsers2", conn)) {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    //Console.WriteLine("Error GetAllUsers: ");

                    using (SqlDataReader reader = cmd.ExecuteReader()) {
                        while (reader.Read()) {
                            var user = new UserDTO(
                                PersonData.GetPersonInfoByID(reader.GetInt32(reader.GetOrdinal("PersonID"))),
                                reader.GetInt32(reader.GetOrdinal("UserID")),
                                reader.GetInt32(reader.GetOrdinal("PersonID")),
                                reader.GetString(reader.GetOrdinal("Username")),
                                null, //reader.GetString(reader.GetOrdinal("Password")),
                                (byte)reader.GetInt16(reader.GetOrdinal("Role")),
                                reader.IsDBNull(reader.GetOrdinal("LastLogin")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("LastLogin")),
                                reader.GetBoolean(reader.GetOrdinal("IsActive"))
                            );
                            users.Add(user);
                        }
                    }
                }
            }catch (Exception ex) {
                Console.WriteLine("Error GetAllUsers: " + ex.ToString());
                // Log exception if needed
            }

            return users;
        }

        // 🔹 Add new user (using DTO, returns new ID)
        public static int AddNewUser(UserDTO user) {
            try {
                using (SqlConnection conn = new SqlConnection(DataAccessSettings.connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_AddNewUser", conn)) {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@PersonID", user.PersonID);
                    cmd.Parameters.AddWithValue("@Username", user.Username);
                    cmd.Parameters.AddWithValue("@Password", user.Password);
                    cmd.Parameters.AddWithValue("@Role", user.Role);
                    cmd.Parameters.AddWithValue("@IsActive", user.isActive);

                    conn.Open();
                    var result = cmd.ExecuteScalar();
                    return result != null && int.TryParse(result.ToString(), out int id) ? id : -1;
                }
            } catch {
                return -1;
            }
        }

        // 🔹 Update user (using DTO)
        public static bool UpdateUser(UserDTO user) {
            try {
                using (SqlConnection conn = new SqlConnection(DataAccessSettings.connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_UpdateUser", conn)) {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@UserID", user.ID);
                    cmd.Parameters.AddWithValue("@PersonID", user.PersonID);
                    cmd.Parameters.AddWithValue("@Username", user.Username);
                    cmd.Parameters.AddWithValue("@Password", user.Password);
                    cmd.Parameters.AddWithValue("@Role", user.Role);
                    cmd.Parameters.AddWithValue("@IsActive", user.isActive);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            } catch {
                return false;
            }
        }

        // 🔹 Delete user
        public static bool DeleteUser(int userID) {
            try {
                using (SqlConnection conn = new SqlConnection(DataAccessSettings.connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_DeleteUser", conn)) {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserID", userID);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            } catch {
                return false;
            }
        }

        // 🔹 Check if user exists
        public static bool IsUserExist(int userID) {
            try {
                using (SqlConnection conn = new SqlConnection(DataAccessSettings.connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_IsUserExist", conn)) {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserID", userID);

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


        public static bool ChangePassword(int userID, string password)  {
            try {
                using (SqlConnection conn = new SqlConnection(DataAccessSettings.connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_ChangePassword", conn)) {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@UserID", userID);
                    cmd.Parameters.AddWithValue("@Password", password);

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
