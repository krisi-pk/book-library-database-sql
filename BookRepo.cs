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
        }

        public static void CreateBook(SqlConnection sqlConnection,string title,string author,int quantity) {
            string newBook = $"INSERT INTO BOOKS(TITLE,AUTHOR,QUANTITY,TAKEN) " +
                    "VALUES ('{title}','{author}','{quantity}',0')";
            SqlCommand cmd = new SqlCommand(newBook, sqlConnection);
            cmd.ExecuteNonQuery();
        }
    }
}
