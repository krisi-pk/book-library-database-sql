using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library;

namespace Library
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // admin admin
            // krisiK kisiK

            SqlConnection sqlConnection = DbConnection.get();
            sqlConnection.Open();
            DbMigrate.migrate();
            DbSeed.seed();

            bool inSystem = false;
            bool isAdmin = false;

            while (inSystem == false) {
                Console.WriteLine("L - for login and R - for new registration");
                string choosen = Console.ReadLine();

                if (choosen.Equals("L")) {
                    Console.WriteLine("Username:");
                    string user = Console.ReadLine();
                    Console.WriteLine("Password:");
                    string pass = Console.ReadLine();
                    if (UserRepo.LogUser(sqlConnection, user, pass) == true)
                    {
                        inSystem = true;
                        if (user.Equals("admin"))
                        {
                            isAdmin = true;
                        }
                    }
                }
                else if (choosen.Equals("R"))
                {
                    Console.WriteLine("Username (5 - 10 characters):");
                    string user = Console.ReadLine();
                    Console.WriteLine("Password (5 - 20 characters):");
                    string pass = Console.ReadLine();
                    Console.WriteLine("First name:");
                    string fname = Console.ReadLine();
                    Console.WriteLine("Second name:");
                    string sname = Console.ReadLine();
                    if (UserRepo.RegisterUser(sqlConnection, user, pass, fname, sname) == true){
                        Console.WriteLine("Registration is done. You can log in now.");
                    }
                    else {
                        Console.WriteLine("This username already exist or incorrect input");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input");
                }
            }


            if (isAdmin == true)
            {
                Console.WriteLine("За да видите всички книги натиснете 1");
                Console.WriteLine("За да видите всички потребители натиснете 2");
                Console.WriteLine("За да добавите нова книга натиснете 3");
                Console.WriteLine("За да премахнете книга натиснете 4");
                Console.WriteLine("За да проверите кой потребител коя книга е взел натиснете 5");
                Console.WriteLine("За да напуснете натиснете 0");
                int num = Int32.Parse(Console.ReadLine());
                while (num != 0) {
                    if (num == 1)
                    {
                        BookRepo.ShowBooks(sqlConnection);
                    }
                    else if (num == 2)
                    {
                        UserRepo.ShowUsers(sqlConnection);
                    }
                    else if (num == 3)
                    {
                        Console.WriteLine("Title:");
                        string title = Console.ReadLine();
                        Console.WriteLine("Author:");
                        string author = Console.ReadLine();
                        Console.WriteLine("Quantity:");
                        int quantity = Int32.Parse(Console.ReadLine());
                        BookRepo.CreateBook(sqlConnection, title, author, quantity);
                    }
                    else if (num == 4)
                    {
                        Console.WriteLine("Title of book to delete:");
                        string title = Console.ReadLine();
                        BookRepo.DeleteBook(sqlConnection, title);
                    }
                    else if (num == 5)
                    {
                        Console.WriteLine("User to check all his takens books:");
                        string user = Console.ReadLine();
                        UserRepo.UserTakensBook(sqlConnection,user);
                    }
                    num = Int32.Parse(Console.ReadLine());
                }
            }
            else {
                Console.WriteLine("За да видите всички свободни книги натиснете 1");
                Console.WriteLine("За да вземете книга натиснете 2");
                Console.WriteLine("За да видите всички книги които сте взимали натиснете 3");
                Console.WriteLine("За да върнете книга натиснете 4");
                Console.WriteLine("За да напуснете натиснете 0");
                int num = Int32.Parse(Console.ReadLine());
                while (num != 0) {
                    if (num == 1)
                    {
                        BookRepo.ShowFreeBooks(sqlConnection);
                    }
                    else if (num == 2) {
                        Console.WriteLine("Title of the book you want");
                        string title = Console.ReadLine();
                        Console.WriteLine("Your username:");
                        string user = Console.ReadLine();
                        BookRepo.TakeBook(sqlConnection, title, user);
                    }
                    else if (num == 3) {
                        Console.WriteLine("Your username: ");
                        string user = Console.ReadLine();
                        UserRepo.UserTakensBook(sqlConnection, user);
                    }
                    else if (num == 4)
                    {
                        Console.WriteLine("Your username: ");
                        string user = Console.ReadLine();
                        Console.WriteLine("Title of book: ");
                        string title = Console.ReadLine();
                        BookRepo.ReturnBook(sqlConnection, title, user);
                    }

                    num = Int32.Parse(Console.ReadLine());
                }
            }
        }
    }
}
