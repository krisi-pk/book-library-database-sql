using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    internal static class DbMigrate
    {
        public static void migrate() {
            SqlConnection sqlConnection = DbConnection.get();
            createDatabase(sqlConnection);
            createUsers(sqlConnection);
            createBooks(sqlConnection);
            createTakenBooks(sqlConnection);
        }

        private static void createDatabase(SqlConnection sqlConnection) {
            if (databaseExist(sqlConnection) == false) {
                string createDatabase = "create database LIBRARY";
                SqlCommand cmd = new SqlCommand(createDatabase,sqlConnection);
                cmd.ExecuteNonQuery();
            }
        }
        private static void createUsers(SqlConnection sqlConnection) {
            string createUsersTable = "create table USERS (" +
                    "ID INT PRIMARY KEY IDENTITY(1,1)," +
                    "USERNAME VARCHAR(10) UNIQUE," +                    
                    "PASS VARCHAR(20)," +
                    "FNAME VARCHAR(50)," +
                    "SNAME VARCHAR(50))";

            if (tableExist("USERS",sqlConnection) == false) {
                SqlCommand sqlCommand = new SqlCommand(createUsersTable, sqlConnection);
                sqlCommand.ExecuteNonQuery();
            }          
        }

        private static void createBooks(SqlConnection sqlConnection)
        {
            string createBooksTable = "create table BOOKS (" +
                    "ID INT PRIMARY KEY IDENTITY(1,1)," +
                    "TITLE VARCHAR(100)," +
                    "AUTHOR VARCHAR(100)," +
                    "QUANTITY INT, " +
                    "TAKEN INT)";

            if (tableExist("BOOKS",sqlConnection) == false)
            {
                SqlCommand sqlCommand = new SqlCommand(createBooksTable, sqlConnection);
                sqlCommand.ExecuteNonQuery();
            }           
        }

        private static void createTakenBooks(SqlConnection sqlConnection)
        {
            string createUsersBooksTable = "create table TAKEN_BOOKS (" +
                    "USER_ID INT," +
                    "BOOK_ID INT," +
                    "CONSTRAINT frg_users FOREIGN KEY(USER_ID) REFERENCES USERS(ID)," +
                    "CONSTRAINT frg_keys FOREIGN KEY(BOOK_ID) REFERENCES BOOKS(ID))";

            if (tableExist("TAKEN_BOOKS", sqlConnection) == false)
            {
                SqlCommand sqlCommand = new SqlCommand(createUsersBooksTable, sqlConnection);
                sqlCommand.ExecuteNonQuery();
            }          
        }

        private static bool databaseExist(SqlConnection sqlConnection) {
            bool exist = false;
            string isDbExist = @"
                IF EXISTS (
                    SELECT name 
                    FROM master.sys.databases 
                    WHERE name = 'LIBRARY'
                )
                SELECT 1
                ELSE
                SELECT 0";
            SqlCommand command = new SqlCommand(isDbExist, sqlConnection);
            int databaseExists = (int)command.ExecuteScalar();

            if (databaseExists == 1){
                exist = true;
            }
            return exist;
        }

        private static bool tableExist(string tableName,SqlConnection sqlConnection) {

            bool exists = false;
            string query = $@"
                IF EXISTS (
                    SELECT 1 
                    FROM INFORMATION_SCHEMA.TABLES 
                    WHERE TABLE_SCHEMA = 'dbo' 
                    AND TABLE_NAME = '{tableName}'
                )
                SELECT 1
                ELSE
                SELECT 0";

            SqlCommand command = new SqlCommand(query, sqlConnection);
            int tableExists = (int)command.ExecuteScalar();

            if (tableExists == 1)
            {
                exists = true;
            }
            return exists;
        }
    }
}
