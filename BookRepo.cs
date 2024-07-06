using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    internal class BookRepo
    {
        public static void ShowBooks(SqlConnection sqlConnection) {
            string allBooks = "select * from BOOKS";
            SqlCommand cmd = new SqlCommand(allBooks, sqlConnection);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine($"{reader.GetInt32(0)}{reader.GetString(1)} {reader.GetString(2)}{reader.GetInt32(3)}{reader.GetInt32(4)}");
            }
            reader.Close();
        }

        public static void CreateBook(SqlConnection sqlConnection,string title,string author,int quantity) {
            string newBook = $"INSERT INTO BOOKS(TITLE,AUTHOR,QUANTITY,TAKEN) " +
                    "VALUES ('{title}','{author}','{quantity}',0')";
            SqlCommand cmd = new SqlCommand(newBook, sqlConnection);
            cmd.ExecuteNonQuery();
        }

        public static void DeleteBook(SqlConnection sqlConnection,string title) {
            bool isDeleted = false;
            string delBook = @"
                DELETE FROM BOOKS
                WHERE TITLE = @value
                )";

            SqlCommand cmd = new SqlCommand(delBook, sqlConnection);
            cmd.Parameters.AddWithValue("@value", title);
            int rowsAffected = cmd.ExecuteNonQuery();
            if (rowsAffected == 1) {
                Console.WriteLine("You deleted the book");
            }
        }
    }
}
