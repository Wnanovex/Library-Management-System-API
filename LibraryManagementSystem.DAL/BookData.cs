using LibraryManagementSystem.DTOs;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace LibraryManagementSystem.DAL
{

    public class BookData {

        // 🔹 Get book by ID
        public static BookDTO GetBookInfoByID(int bookID) {
            try {
                using (SqlConnection conn = new SqlConnection(DataAccessSettings.connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_GetBookByID", conn)) {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BookID", bookID);

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())  {
                        if (reader.Read()) {
                            return new BookDTO(
                                reader.GetInt32(reader.GetOrdinal("BookID")),
                                reader.GetString(reader.GetOrdinal("Title")),
                                reader.GetString(reader.GetOrdinal("ISBN")),
                                reader.GetInt32(reader.GetOrdinal("CategoryID")),
                                reader.GetInt32(reader.GetOrdinal("AuthorID")),
                                reader.GetString(reader.GetOrdinal("Edition")),
                                reader.GetInt32(reader.GetOrdinal("PublishedYear")),
                                reader.GetString(reader.GetOrdinal("Language")),
                                reader.GetString(reader.GetOrdinal("Description")),
                                reader.GetString(reader.GetOrdinal("ShelfLocation")),
                                (float)reader.GetDecimal(reader.GetOrdinal("DailyRate")),
                                reader.IsDBNull(reader.GetOrdinal("ImageName")) ? "" : reader.GetString(reader.GetOrdinal("ImageName")),
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

        // 🔹 Get all books (optional enhancement: return List<BookDTO>)
        public static List<BookDTO> GetAllBooks() {
            var books = new List<BookDTO>();

            try {
                using (SqlConnection conn = new SqlConnection(DataAccessSettings.connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_GetAllBooks2", conn)) {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader()) {
                        while (reader.Read()) {
                            var book = new BookDTO(
                                reader.GetInt32(reader.GetOrdinal("BookID")),
                                reader.GetString(reader.GetOrdinal("Title")),
                                reader.GetString(reader.GetOrdinal("ISBN")),
                                reader.GetInt32(reader.GetOrdinal("CategoryID")),
                                reader.GetInt32(reader.GetOrdinal("AuthorID")),
                                reader.GetString(reader.GetOrdinal("Edition")),
                                reader.GetInt32(reader.GetOrdinal("PublishedYear")),
                                reader.GetString(reader.GetOrdinal("Language")),
                                reader.GetString(reader.GetOrdinal("Description")),
                                reader.GetString(reader.GetOrdinal("ShelfLocation")),
                                (float)reader.GetDecimal(reader.GetOrdinal("DailyRate")),
                                reader.IsDBNull(reader.GetOrdinal("ImageName")) ? "" : reader.GetString(reader.GetOrdinal("ImageName")),
                                reader.IsDBNull(reader.GetOrdinal("DateAdded")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("DateAdded"))
                            );
                            book.Category = CategoryData.GetCategoryInfoByID(book.CategoryID);
                            book.Author = AuthorData.GetAuthorByID(book.AuthorID);
                            books.Add(book);
                        }
                    }
                }
            }catch (Exception ex) {
                Console.WriteLine("Error GetAllBooks: " + ex.Message);
                // Log exception if needed
            }

            return books;
        }

        // 🔹 Add new book (using DTO, returns new ID)
        public static int AddNewBook(BookDTO book) {
            try {
                using (SqlConnection conn = new SqlConnection(DataAccessSettings.connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_AddNewBook", conn)) {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Title", book.Title);
                    cmd.Parameters.AddWithValue("@ISBN", book.ISBN);
                    cmd.Parameters.AddWithValue("@CategoryID", book.CategoryID);
                    cmd.Parameters.AddWithValue("@AuthorID", book.AuthorID);
                    cmd.Parameters.AddWithValue("@Edition", book.Edition);
                    cmd.Parameters.AddWithValue("@PublishedYear", book.PublishedYear);
                    cmd.Parameters.AddWithValue("@Language", book.Language);
                    cmd.Parameters.AddWithValue("@Description", book.Description);
                    cmd.Parameters.AddWithValue("@ShelfLocation", book.ShelfLocation);
                    cmd.Parameters.AddWithValue("@DailyRate", book.DailyRate);
                    if (book.ImageName != "" && book.ImageName != null)
                        cmd.Parameters.AddWithValue("@ImageName", book.ImageName);
                    else
                        cmd.Parameters.AddWithValue("@ImageName", System.DBNull.Value);

                    conn.Open();
                    var result = cmd.ExecuteScalar();
                    return result != null && int.TryParse(result.ToString(), out int id) ? id : -1;
                }
            } catch {
                return -1;
            }
        }

        // 🔹 Update book (using DTO)
        public static bool UpdateBook(BookDTO book) {
            try {
                using (SqlConnection conn = new SqlConnection(DataAccessSettings.connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_UpdateBook", conn)) {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@BookID", book.ID);
                    cmd.Parameters.AddWithValue("@Title", book.Title);
                    cmd.Parameters.AddWithValue("@ISBN", book.ISBN);
                    cmd.Parameters.AddWithValue("@CategoryID", book.CategoryID);
                    cmd.Parameters.AddWithValue("@AuthorID", book.AuthorID);
                    cmd.Parameters.AddWithValue("@Edition", book.Edition);
                    cmd.Parameters.AddWithValue("@PublishedYear", book.PublishedYear);
                    cmd.Parameters.AddWithValue("@Language", book.Language);
                    cmd.Parameters.AddWithValue("@Description", book.Description);
                    cmd.Parameters.AddWithValue("@ShelfLocation", book.ShelfLocation);
                    cmd.Parameters.AddWithValue("@DailyRate", book.DailyRate);
                    if (book.ImageName != "" && book.ImageName != null)
                        cmd.Parameters.AddWithValue("@ImageName", book.ImageName);
                    else
                        cmd.Parameters.AddWithValue("@ImageName", System.DBNull.Value);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            } catch {
                return false;
            }
        }

        // 🔹 Delete book
        public static bool DeleteBook(int bookID) {
            try {
                using (SqlConnection conn = new SqlConnection(DataAccessSettings.connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_DeleteBook", conn)) {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BookID", bookID);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            } catch {
                return false;
            }
        }


    }
}
