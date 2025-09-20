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

    public class PersonData {

        // 🔹 Get person by ID
        public static PersonDTO GetPersonInfoByID(int personID) {
            try {
                using (SqlConnection conn = new SqlConnection(DataAccessSettings.connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_GetPersonByID", conn)) {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PersonID", personID);

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())  {
                        if (reader.Read()) {
                            return new PersonDTO(
                                reader.GetInt32(reader.GetOrdinal("PersonID")),
                                reader.GetString(reader.GetOrdinal("FirstName")),
                                reader.GetString(reader.GetOrdinal("LastName")),
                                reader.IsDBNull(reader.GetOrdinal("DateOfBirth")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                                reader.IsDBNull(reader.GetOrdinal("Gender")) ? null : reader.GetString(reader.GetOrdinal("Gender")),
                                reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")),
                                reader.IsDBNull(reader.GetOrdinal("Phone")) ? null : reader.GetString(reader.GetOrdinal("Phone")),
                                reader.IsDBNull(reader.GetOrdinal("Address")) ? null : reader.GetString(reader.GetOrdinal("Address")),
                                reader.IsDBNull(reader.GetOrdinal("City")) ? null : reader.GetString(reader.GetOrdinal("City")),
                                reader.IsDBNull(reader.GetOrdinal("CreatedAt")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                                reader.IsDBNull(reader.GetOrdinal("UpdatedAt")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("UpdatedAt"))
                            );
                        }
                    }
                }
            }catch{
                // Optionally log the error
            }

            return null;
        }

        // 🔹 Get all people (optional enhancement: return List<PersonDTO>)
        public static List<PersonDTO> GetAllPeople() {
            var people = new List<PersonDTO>();

            try {
                using (SqlConnection conn = new SqlConnection(DataAccessSettings.connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_GetAllPeople", conn)) {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader()) {
                        while (reader.Read()) {
                            var person = new PersonDTO(
                                reader.GetInt32(reader.GetOrdinal("PersonID")),
                                reader.GetString(reader.GetOrdinal("FirstName")),
                                reader.GetString(reader.GetOrdinal("LastName")),
                                reader.IsDBNull(reader.GetOrdinal("DateOfBirth")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                                reader.IsDBNull(reader.GetOrdinal("Gender")) ? null : reader.GetString(reader.GetOrdinal("Gender")),
                                reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")),
                                reader.IsDBNull(reader.GetOrdinal("Phone")) ? null : reader.GetString(reader.GetOrdinal("Phone")),
                                reader.IsDBNull(reader.GetOrdinal("Address")) ? null : reader.GetString(reader.GetOrdinal("Address")),
                                reader.IsDBNull(reader.GetOrdinal("City")) ? null : reader.GetString(reader.GetOrdinal("City")),
                                reader.IsDBNull(reader.GetOrdinal("CreatedAt")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                                reader.IsDBNull(reader.GetOrdinal("UpdatedAt")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("UpdatedAt"))
                            );
                            people.Add(person);
                        }
                        throw new Exception("Test Exception!");
                    }
                }
            }catch (Exception ex) {
                Console.WriteLine("Error GetAllPeople: " + ex.ToString());
                // Log exception if needed
            }

            return people;
        }

        // 🔹 Add new person (using DTO, returns new ID)
        public static int AddNewPerson(PersonDTO person) {
            try {
                using (SqlConnection conn = new SqlConnection(DataAccessSettings.connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_AddNewPerson", conn)) {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@FirstName", person.FirstName ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@LastName", person.LastName ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@DateOfBirth", person.DateOfBirth != DateTime.MinValue ? (object)person.DateOfBirth : DBNull.Value);
                    cmd.Parameters.AddWithValue("@Gender", person.Gender ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Email", person.Email ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Phone", person.Phone ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Address", person.Address ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@City", person.City ?? (object)DBNull.Value);

                    conn.Open();
                    var result = cmd.ExecuteScalar();
                    return result != null && int.TryParse(result.ToString(), out int id) ? id : -1;
                }
            } catch {
                return -1;
            }
        }

        // 🔹 Update person (using DTO)
        public static bool UpdatePerson(PersonDTO person) {
            try {
                using (SqlConnection conn = new SqlConnection(DataAccessSettings.connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_UpdatePerson", conn)) {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@PersonID", person.ID);
                    cmd.Parameters.AddWithValue("@FirstName", person.FirstName ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@LastName", person.LastName ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@DateOfBirth", person.DateOfBirth != DateTime.MinValue ? (object)person.DateOfBirth : DBNull.Value);
                    cmd.Parameters.AddWithValue("@Gender", person.Gender ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Email", person.Email ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Phone", person.Phone ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Address", person.Address ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@City", person.City ?? (object)DBNull.Value);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            } catch {
                return false;
            }
        }

        // 🔹 Delete person
        public static bool DeletePerson(int personID) {
            try {
                using (SqlConnection conn = new SqlConnection(DataAccessSettings.connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_DeletePerson", conn)) {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PersonID", personID);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            } catch {
                return false;
            }
        }

        // 🔹 Check if person exists
        public static bool IsPersonExist(int personID) {
            try {
                using (SqlConnection conn = new SqlConnection(DataAccessSettings.connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_IsPersonExist", conn)) {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PersonID", personID);

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
