using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using LibraryManagementSystem.DTOs;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.DAL
{

    public class MemberData {

        // 🔹 Get member by ID
        public static MemberDTO GetMemberByID(int memberID) {
            try {
                using (SqlConnection conn = new SqlConnection(DataAccessSettings.connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_GetMemberByID", conn)) {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MemberID", memberID);

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())  {
                        if (reader.Read()) {
                            return new MemberDTO(
                                PersonData.GetPersonInfoByID(reader.GetInt32(reader.GetOrdinal("PersonID"))),
                                reader.GetInt32(reader.GetOrdinal("MemberID")),
                                reader.GetInt32(reader.GetOrdinal("PersonID")),
                                reader.IsDBNull(reader.GetOrdinal("DateJoined")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("DateJoined"))
                            );
                        }
                    }
                }
            }catch (Exception ex) {
                Console.WriteLine("Error GetMemberByID: " + ex.ToString());
                // Optionally log the error
            }

            return null;
        }


        // 🔹 Get all people (optional enhancement: return List<PersonDTO>)
        public static List<MemberDTO> GetAllMembers() {
            var members = new List<MemberDTO>();

            try {
                using (SqlConnection conn = new SqlConnection(DataAccessSettings.connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_GetAllMembers2", conn)) {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader()) {
                        while (reader.Read()) {
                            var member = new MemberDTO(
                                PersonData.GetPersonInfoByID(reader.GetInt32(reader.GetOrdinal("PersonID"))),
                                reader.GetInt32(reader.GetOrdinal("MemberID")),
                                reader.GetInt32(reader.GetOrdinal("PersonID")),
                                reader.IsDBNull(reader.GetOrdinal("DateJoined")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("DateJoined"))
                            );
                            members.Add(member);
                        }
                    }
                }
            }catch (Exception ex) {
                Console.WriteLine("Error GetAllMembers: " + ex.ToString());
                // Log exception if needed
            }

            return members;
        }

        // 🔹 Add new member (using DTO, returns new ID)
        public static int AddNewMember(MemberDTO member) {
            try {
                using (SqlConnection conn = new SqlConnection(DataAccessSettings.connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_AddNewMember", conn)) {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@PersonID", member.PersonID);

                    conn.Open();
                    var result = cmd.ExecuteScalar();
                    return result != null && int.TryParse(result.ToString(), out int id) ? id : -1;
                }
            } catch(Exception ex) {
                Console.WriteLine("Error AddNewMember: " + ex.Message);
                return -1;
            }
        }

        // 🔹 Update member (using DTO)
        //public static bool Updatemember(MemberDTO member) {
        //    try {
        //        using (SqlConnection conn = new SqlConnection(DataAccessSettings.connectionString))
        //        using (SqlCommand cmd = new SqlCommand("sp_UpdateMember", conn)) {
        //            cmd.CommandType = CommandType.StoredProcedure;

        //            cmd.Parameters.AddWithValue("@memberID", member.ID);
        //            cmd.Parameters.AddWithValue("@PersonID", member.DateJoined);

        //            conn.Open();
        //            return cmd.ExecuteNonQuery() > 0;
        //        }
        //    } catch {
        //        return false;
        //    }
        //}

        // 🔹 Delete member
        
        public static bool DeleteMember(int memberID) {
            try {
                using (SqlConnection conn = new SqlConnection(DataAccessSettings.connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_DeleteMember", conn)) {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MemberID", memberID);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            } catch {
                return false;
            }
        }

        // 🔹 Check if member exists
        public static bool IsMemberExist(int memberID) {
            try {
                using (SqlConnection conn = new SqlConnection(DataAccessSettings.connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_IsMemberExist", conn)) {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MemberID", memberID);

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
