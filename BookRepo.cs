using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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
                Console.WriteLine($"{reader.GetInt32(0)} {reader.GetString(1)} {reader.GetString(2)} {reader.GetInt32(3)} {reader.GetInt32(4)}");
            }
            reader.Close();
        }

        public static void ShowFreeBooks(SqlConnection sqlConnection)
        {
            string allFreeBooks = "select * from BOOKS " +
                "where QUANTITY > 0";
            SqlCommand cmd = new SqlCommand(allFreeBooks, sqlConnection);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine($"{reader.GetInt32(0)} {reader.GetString(1)} {reader.GetString(2)} {reader.GetInt32(3)} {reader.GetInt32(4)}");
            }
            reader.Close();
        }

        public static void CreateBook(SqlConnection sqlConnection,string title,string author,int quantity) {
            string newBook = @"INSERT INTO BOOKS(TITLE,AUTHOR,QUANTITY,TAKEN) " +
                    "VALUES (@title,@author,@quantity,0)";
            SqlCommand cmd = new SqlCommand(newBook, sqlConnection);
            cmd.Parameters.AddWithValue("@title", title);
            cmd.Parameters.AddWithValue("@author", author);
            cmd.Parameters.AddWithValue("@quantity", quantity);
            cmd.ExecuteNonQuery();
            Console.WriteLine("You added the book");
        }

        public static void DeleteBook(SqlConnection sqlConnection,string title) {
            bool isDeleted = false;
            string delBook = @"
                DELETE FROM BOOKS
                WHERE TITLE = @value";

            SqlCommand cmd = new SqlCommand(delBook, sqlConnection);
            cmd.Parameters.AddWithValue("@value", title);
            int rowsAffected = cmd.ExecuteNonQuery();
            if (rowsAffected == 1) {
                Console.WriteLine("You deleted the book");
            }
        }

        public static void TakeBook(SqlConnection sqlConnection, string title, string user) {
            string selectBook = @"select id from BOOKS " +
                "where TITLE = @title";
            SqlCommand cmd = new SqlCommand(selectBook, sqlConnection);
            cmd.Parameters.AddWithValue("@title", title);
            object result = cmd.ExecuteScalar();
            int idBook = Convert.ToInt32(result);


            string selectUser = @"select id from USERS " +
                "where USERNAME = @user";
            SqlCommand cmd2 = new SqlCommand(selectUser, sqlConnection);
            cmd2.Parameters.AddWithValue("@user", user);
            object result2 = cmd2.ExecuteScalar();
            int idUser = Convert.ToInt32(result2);


            string take = @"INSERT INTO TAKEN_BOOKS(USER_ID,BOOK_ID) " +
                    "VALUES (@user,@book)";
            SqlCommand cmd3 = new SqlCommand(take, sqlConnection);
            cmd3.Parameters.AddWithValue("@user", idUser);
            cmd3.Parameters.AddWithValue("@book", idBook);
            cmd3.ExecuteNonQuery();


            string takeQuantity = @"select QUANTITY from BOOKS " +
                "where TITLE = @title";
            SqlCommand cmd4 = new SqlCommand(takeQuantity, sqlConnection);
            cmd4.Parameters.AddWithValue("@title", title);
            object result3 = cmd4.ExecuteScalar();
            int quantity = Convert.ToInt32(result3);

            string takeTakens = @"select TAKEN from BOOKS " +
                "where TITLE = @title";
            SqlCommand cmd5 = new SqlCommand(takeTakens, sqlConnection);
            cmd5.Parameters.AddWithValue("@title", title);
            object result4 = cmd5.ExecuteScalar();
            int taken = Convert.ToInt32(result4);

            string update = @"UPDATE BOOKS " +
                    "SET QUANTITY = @quantity, TAKEN = @taken " +
                    "WHERE TITLE = @title";
            SqlCommand cmd6 = new SqlCommand(update, sqlConnection);
            cmd6.Parameters.AddWithValue("@quantity", (quantity - 1));
            cmd6.Parameters.AddWithValue("@taken", (taken + 1));
            cmd6.Parameters.AddWithValue("@title", title);
            cmd6.ExecuteNonQuery();

            Console.WriteLine("You take the book");
        }


        public static void ReturnBook(SqlConnection sqlConnection, string title, string user){
            string checkIfExist = @"select TITLE from TAKEN_BOOKS " +
                "where USERNAME = @user";
            SqlCommand cmd = new SqlCommand(checkIfExist, sqlConnection);
            cmd.Parameters.AddWithValue("@user", user);
            object result = cmd.ExecuteScalar();
            int idBook = Convert.ToInt32(result);

        }

    }
}
