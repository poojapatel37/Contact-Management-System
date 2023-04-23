using System;
using System.Collections.Generic;
using System.IO;
internal class CMS
{
    static void Main(string[] args)
    {
        //displaying welcome message and calling login method for the Admin to login
        Console.WriteLine("--------------------------------------");
        Console.WriteLine("Welcome to Contact Management System");
        Console.WriteLine("--------------------------------------");
        login();
    }

    //login procedure to ask user to enter his credentials and compare with the values from config file
    public static void login()
    {
        Console.WriteLine("Enter your credentials to login");
        bool loginSuccess = false;
        int count = 0;
        try
        {
            StreamReader numberOfLoginAttemptsFile = new StreamReader("numberOfLoginAttempts.txt");
            int maxLoginAttempts = int.Parse(numberOfLoginAttemptsFile.ReadLine());
            for (count = 0; count < maxLoginAttempts && loginSuccess == false; count++)
            {
                StreamReader loginInputFile = new StreamReader("login.txt");
                Console.Write("Enter your username: ");
                String userName = Console.ReadLine();

                //to check if username is blank
                if (userName.Length == 0)
                {
                    Console.WriteLine("Username cannot be empty");
                }

                Console.Write("Enter your password: ");
                String passWord = Console.ReadLine();

                //to check if password is blank
                if (passWord.Length == 0)
                {
                    Console.WriteLine("Password cannot be empty");
                }

                //to compare credentials entered with the values from config file
                while (!loginInputFile.EndOfStream)
                {
                    string[] loginTextInput = (loginInputFile.ReadLine()).Split(':');

                    if (loginTextInput[0] == userName && loginTextInput[1] == passWord)
                    {
                        Console.WriteLine("Login successful");
                        loginSuccess = true;
                        displayMenu();
                    }
                }
                loginInputFile.Close();
            }

            //to check if maximum number of unsuccessful login attempts is reached
            if (count == maxLoginAttempts && loginSuccess == false)
            {
                Console.WriteLine("You have reached maximum login attempts");
            }
        }

        //to handle file not found exceptions for config files
        catch (FileNotFoundException)
        {
            Console.WriteLine("Config file not found");
        }
    }

    //procedure to display the menu options in case login is successful
    static void displayMenu()
    {
        int choice=0;
        const int exitChoice = 7;
        do
        {
            Console.WriteLine("\n--------------------------------------");
            Console.WriteLine("1.Add new contact\n2.Delete contact\n3.Update contact\n4.Search contact\n5.Generate a report\n6.View contacts\n7.Exit");
            Console.WriteLine("--------------------------------------");
            try
            {
                Console.Write("Enter the choice: ");
                choice = int.Parse(Console.ReadLine());
                employee e = new employee();
                //take action based on option selected
                switch (choice)
                {
                    case 1:
                        e.addContact();
                        break;
                    case 2:
                        e.deleteContact();
                        break;
                    case 3:
                        e.updateContact();
                        break;
                    case 4:
                        e.searchContact();
                        break;
                    case 5:
                        e.generateReport();
                        break;
                    case 6:
                        e.loadAndView();
                        break;
                    case 7:
                        break;
                    default:
                        Console.WriteLine("Enter a valid choice");
                        break;
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Enter the choice in int format");
            }
        } while (choice != exitChoice);
    }
}





