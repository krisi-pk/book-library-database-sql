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
                    if (UserRepo.LogUser(sqlConnection, user, pass) == true) {
                        inSystem = true;
                        if (user.Equals("admin")){
                            isAdmin = true;
                        }
                    }
                }
                else if (choosen.Equals("R"))
                {
                    Console.WriteLine("Username (Up to 10 characters):");
                    string user = Console.ReadLine();
                    Console.WriteLine("Password (Up to 20 characters):");
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

            if (isAdmin == true) {
                Console.WriteLine("За да видите всички книги натиснете 1");
                Console.WriteLine("За да видите всички потребители натиснете 2");
                Console.WriteLine("За да добавите нова книга натиснете 3");
                Console.WriteLine("За да премахнете книга натиснете 4");
                int num = Int32.Parse(Console.ReadLine());

                if (num == 1) {
                    BookRepo.ShowBooks(sqlConnection);
                }
                else if (num == 2) {
                    UserRepo.ShowUsers(sqlConnection);                   
                }
                else if (num == 3) {
                    Console.WriteLine("Title:");
                    string title = Console.ReadLine();
                    Console.WriteLine("Author:");
                    string author = Console.ReadLine();
                    Console.WriteLine("Quantity:");
                    int quantity = Int32.Parse(Console.ReadLine());
                    BookRepo.CreateBook(sqlConnection, title, author, quantity);
                }
                else if (num == 4) { 
                
                }
            }

        }
    }
}
