using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;

[Serializable]
public class employee
{
    static Dictionary<int, employee> employeeDetails = new Dictionary<int, employee>();
    string fname, lname, emailId, address, post;
    long phoneNumber;
    static int empId = 0;

    //method to take the employee details from the user and add them to dictionary and then save to file
    public void addContact()
    {
        employee emp = new employee();
        bool retVal;
        Console.WriteLine("Enter employee details.");

        Console.Write("Enter first name: ");
        emp.fname = Console.ReadLine();
        //to check that first name is not blank and only characters are entered

        retVal = IsAllLetters(emp.fname);
        if (retVal == false)
        {
            while (retVal != true)
            {
                Console.Write("Enter first name: ");
                emp.fname = Console.ReadLine();
                retVal = IsAllLetters(emp.fname);
            }
        }

        Console.Write("Enter last name: ");
        emp.lname = Console.ReadLine();
        //to check that last name is not blank and only characters are entered

        retVal = IsAllLetters(emp.lname);
        if (retVal == false)
        {
            while (retVal != true)
            {
                Console.Write("Enter last name: ");
                emp.lname = Console.ReadLine();
                retVal = IsAllLetters(emp.lname);
            }
        }

        //code to check that phone number has only digits and length is 10
        retVal = false;
        const int maxPhoneLength = 10;
        while (retVal != true)
        {
            try
            {
                Console.Write("Enter phone number: ");
                emp.phoneNumber = long.Parse(Console.ReadLine());
                retVal = true;
                while (emp.phoneNumber.ToString().Length != maxPhoneLength)
                {
                    Console.WriteLine("Phone number should have 10 digits");
                    Console.Write("Enter phone number: ");
                    emp.phoneNumber = long.Parse(Console.ReadLine());
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Phone number should contain digits only");
                retVal = false;
            }
        }

        //to check email address
        retVal = false;
        Console.Write("Enter email address: ");
        emp.emailId = Console.ReadLine();
        retVal = RegexEmailCheck(emp.emailId);
        while (retVal != true)
        {
            Console.WriteLine("Email address is invalid");
            Console.Write("Enter email address: ");
            emp.emailId = Console.ReadLine();
            retVal = RegexEmailCheck(emp.emailId);
        }

        Console.Write("Enter address: ");
        emp.address = Console.ReadLine();
        Console.Write("Enter post: ");
        emp.post = Console.ReadLine();

        if (File.Exists("employeeData.bin"))
        {
            BinaryFormatter bf1 = new BinaryFormatter();
            FileStream file1 = File.OpenRead("employeeData.bin");
            employeeDetails = (Dictionary<int, employee>)bf1.Deserialize(file1);
            file1.Close();
            List<int> keys = new List<int>(employeeDetails.Keys);
            empId = keys[keys.Count - 1];
        }

        employeeDetails.Add(++empId, emp);
        saveContact();
    }

    //Code to check if the string has only characters taken from:
    //"Only Letters", Gaff
    //https://stackoverflow.com/questions/1181419/verifying-that-a-string-contains-only-letters-in-c-sharp
    //[Accessed 26/12/22]
    public static bool IsAllLetters(string s)
    {
        if (s == "")
        {
            Console.WriteLine("First name cannot be blank");
            return false;
        }
        else
        {

            foreach (char c in s)
            {
                if (!Char.IsLetter(c))
                {
                    Console.WriteLine("First name can have characters only");
                    return false;
                }
            }
        }
        return true;
    }

    //Code to check if email is valid or not taken from:
    //"Validate email address with System.Text.RegularExpressions", Jagger, E
    //https://www.abstractapi.com/guides/validate-emails-in-c
    //[Accessed 26/12/22]
    public static bool RegexEmailCheck(string input)
    {
        // returns true if the input is a valid email
        return Regex.IsMatch(input, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
    }

    //method to save the dictionary values into a file using automatic serialisation
    public void saveContact()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream saveFile = File.Create("employeeData.bin");
        bf.Serialize(saveFile, employeeDetails);
        saveFile.Close();
    }

    //method to load the employee details from the file to the dictionary using automatic de-serialisation and then display the dictionary values
    public void loadAndView()
    {
        BinaryFormatter bf1 = new BinaryFormatter();
        try
        {
            FileStream file1 = File.OpenRead("employeeData.bin");
            employeeDetails = (Dictionary<int, employee>)bf1.Deserialize(file1);
            file1.Close();
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("File does not exist");
        }

        //Code to navigate through dictionary items and print the values taken from:
        //"Example #2"
        //https://www.educba.com/c-object-to-dictionary/
        //[Accessed 21/12/22]
        foreach (KeyValuePair<int, employee> vars1 in employeeDetails)
        {
            Console.WriteLine("\nEmp Id: " + vars1.Key);
            employee e1 = vars1.Value;
            Console.WriteLine("Emp name: " + e1.fname + " " + e1.lname + "\nEmp phone number: " + e1.phoneNumber + "\nEmp email: " + e1.emailId + "\nEmp address: " + e1.address + "\nEmp post: " + e1.post);
        }
    }

    //method to delete a contact
    public void deleteContact()
    {
        Console.Write("Enter Emp id of contact you want to delete: ");
        int delEmp = int.Parse(Console.ReadLine());
        bool recFound = false;
        try
        {
            BinaryFormatter bf1 = new BinaryFormatter();
            FileStream file1 = File.OpenRead("employeeData.bin");
            employeeDetails = (Dictionary<int, employee>)bf1.Deserialize(file1);
            file1.Close();
        }

        catch (FileNotFoundException)
        {
            Console.WriteLine("File does not exist");
        }

        List<int> keys = new List<int>(employeeDetails.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            if (keys[i] == delEmp)
            {
                employeeDetails.Remove(keys[i]);
                Console.WriteLine("Employee details for Emp id: " + keys[i] + " deleted");
                saveContact();
                recFound = true;
                break;
            }
        }

        if (recFound == false)
        {
            Console.WriteLine("Record not found");
        }
    }

    //method to search a contact
    public void searchContact()
    {
        Console.Write("Enter Emp id of contact you want to search: ");
        int searchEmp = int.Parse(Console.ReadLine());
        bool recFound = false;
        try
        {
            BinaryFormatter bf1 = new BinaryFormatter();
            FileStream file1 = File.OpenRead("employeeData.bin");
            employeeDetails = (Dictionary<int, employee>)bf1.Deserialize(file1);
            file1.Close();
        }

        catch (FileNotFoundException)
        {
            Console.WriteLine("File does not exist");
        }

        List<int> keys = new List<int>(employeeDetails.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            if (keys[i] == searchEmp)
            {
                Console.WriteLine("\nEmp Id: " + keys[i]);
                employee e1 = employeeDetails[keys[i]];
                Console.WriteLine("Emp name: " + e1.fname + " " + e1.lname + "\nEmp phone number: " + e1.phoneNumber + "\nEmp email: " + e1.emailId + "\nEmp address: " + e1.address + "\nEmp post: " + e1.post);
                recFound = true;
                break;
            }
        }
        if (recFound == false)
        {
            Console.WriteLine("Record not found");
        }
    }

    //method to update a contact
    public void updateContact()
    {
        Console.Write("Enter Emp id of contact you want to update: ");
        int updateEmp = int.Parse(Console.ReadLine());
        bool recFound = false;
        try
        {
            BinaryFormatter bf1 = new BinaryFormatter();
            FileStream file1 = File.OpenRead("employeeData.bin");
            employeeDetails = (Dictionary<int, employee>)bf1.Deserialize(file1);
            file1.Close();
        }

        catch (FileNotFoundException)
        {
            Console.WriteLine("File does not exist");
        }

        List<int> keys = new List<int>(employeeDetails.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            if (keys[i] == updateEmp)
            {
                recFound = true;
                employee e1 = employeeDetails[keys[i]];
                Console.WriteLine("Enter the field to be updated: ");
                Console.WriteLine("1.First name\n2.Last name\n3.Phone number\n4.Email id\n5.Address\n6.Post");
                int updateChoice = int.Parse(Console.ReadLine());

                switch (updateChoice)
                {
                    case 1:
                        Console.Write("Enter first name: ");
                        e1.fname = Console.ReadLine();
                        //to check that first name is not blank and only characters are entered
                        bool retVal = IsAllLetters(e1.fname);
                        if (retVal == false)
                        {
                            while (retVal != true)
                            {
                                Console.Write("Enter first name: ");
                                e1.fname = Console.ReadLine();
                                retVal = IsAllLetters(e1.fname);
                            }
                        }
                        saveContact();
                        Console.WriteLine("Details for employee id: " + updateEmp + " updated");
                        break;
                    case 2:
                        Console.Write("Enter last name: ");
                        e1.lname = Console.ReadLine();
                        //to check that last name is not blank and only characters are entered

                        retVal = IsAllLetters(e1.lname);
                        if (retVal == false)
                        {
                            while (retVal != true)
                            {
                                Console.Write("Enter last name: ");
                                e1.lname = Console.ReadLine();
                                retVal = IsAllLetters(e1.lname);
                            }
                        }
                        saveContact();
                        Console.WriteLine("Details for employee id: " + updateEmp + " updated");
                        break;
                    case 3:
                        retVal = false;
                        while (retVal != true)
                        {
                            try
                            {
                                Console.Write("Enter phone number: ");
                                e1.phoneNumber = long.Parse(Console.ReadLine());
                                retVal = true;
                                while (e1.phoneNumber.ToString().Length != 10)
                                {
                                    Console.WriteLine("Phone number should have 10 digits");
                                    Console.Write("Enter phone number: ");
                                    e1.phoneNumber = long.Parse(Console.ReadLine());
                                }
                            }
                            catch (FormatException)
                            {
                                Console.WriteLine("Phone number should contain digits only");
                                retVal = false;
                            }
                        }
                        saveContact();
                        Console.WriteLine("Details for employee id: " + updateEmp + " updated");
                        break;
                    case 4:
                        retVal = false;
                        Console.Write("Enter email address: ");
                        e1.emailId = Console.ReadLine();
                        retVal = RegexEmailCheck(e1.emailId);
                        while (retVal != true)
                        {
                            Console.WriteLine("Email address is invalid");
                            Console.Write("Enter email address: ");
                            e1.emailId = Console.ReadLine();
                            retVal = RegexEmailCheck(e1.emailId);
                        }
                        saveContact();
                        Console.WriteLine("Details for employee id: " + updateEmp + " updated");
                        break;
                    case 5:
                        Console.Write("Enter address: ");
                        e1.address = Console.ReadLine();
                        saveContact();
                        Console.WriteLine("Details for employee id: " + updateEmp + " updated");
                        break;
                    case 6:
                        Console.Write("Enter post: ");
                        e1.post = Console.ReadLine();
                        saveContact();
                        Console.WriteLine("Details for employee id: " + updateEmp + " updated");
                        break;
                    default:
                        Console.WriteLine("Enter a valid choice");
                        break;
                }
                
                break;
            }
        }
        if (recFound == false)
        {
            Console.WriteLine("Record not found");
        }
    }

    //method to generate a report
    public void generateReport()
    {
        Console.WriteLine("Enter choice based on which you want to generate a report: ");
        Console.WriteLine("1.Address\n2.Post");
        int choice = int.Parse(Console.ReadLine());
        switch (choice)
        {
            case 1:
                Console.Write("Enter the address for which you want to generate a report: ");
                string requiredCity = Console.ReadLine();
                bool recFound = false;
                StreamWriter cityReport = new StreamWriter("Report-" + requiredCity + ".txt");
                try
                {
                    BinaryFormatter bf1 = new BinaryFormatter();
                    FileStream file1 = File.OpenRead("employeeData.bin");
                    employeeDetails = (Dictionary<int, employee>)bf1.Deserialize(file1);
                    file1.Close();
                }

                catch (FileNotFoundException)
                {
                    Console.WriteLine("File does not exist");
                }
                List<int> keys = new List<int>(employeeDetails.Keys);
                keys.Sort();
                for (int i = 0; i < keys.Count; i++)
                {
                    employee e1 = employeeDetails[keys[i]];
                    if (e1.address == requiredCity)
                    {
                        recFound = true;
                        cityReport.WriteLine("\nEmp Id: " + keys[i]);
                        cityReport.WriteLine("Emp name: " + e1.fname + " " + e1.lname + "\nEmp phone number: " + e1.phoneNumber + "\nEmp email: " + e1.emailId + "\nEmp address: " + e1.address + "\nEmp post: " + e1.post);
                    }
                }
                cityReport.Close();
                if (recFound == true)
                {
                    Console.WriteLine("Report generated : Report-" + requiredCity + ".txt");
                }
                if (recFound == false)
                {
                    Console.WriteLine("Record not found");
                }
                break;
            case 2:
                Console.Write("Enter the post for which you want to generate a report: ");
                string requiredPost = Console.ReadLine();
                recFound = false;
                StreamWriter postReport = new StreamWriter("Report-" + requiredPost + ".txt");
                try
                {
                    BinaryFormatter bf1 = new BinaryFormatter();
                    FileStream file1 = File.OpenRead("employeeData.bin");
                    employeeDetails = (Dictionary<int, employee>)bf1.Deserialize(file1);
                    file1.Close();
                }

                catch (FileNotFoundException)
                {
                    Console.WriteLine("File does not exist");
                }
                List<int> keys1 = new List<int>(employeeDetails.Keys);
                keys1.Sort();
                for (int i = 0; i < keys1.Count; i++)
                {
                    employee e1 = employeeDetails[keys1[i]];
                    if (e1.post == requiredPost)
                    {
                        recFound = true;
                        postReport.WriteLine("\nEmp Id: " + keys1[i]);
                        postReport.WriteLine("Emp name: " + e1.fname + " " + e1.lname + "\nEmp phone number: " + e1.phoneNumber + "\nEmp email: " + e1.emailId + "\nEmp address: " + e1.address + "\nEmp post: " + e1.post);
                    }
                }
                postReport.Close();
                if (recFound == true)
                {
                    Console.WriteLine("Report generated : Report-" + requiredPost + ".txt");
                }
                if (recFound == false)
                {
                    Console.WriteLine("Record not found");
                }
                break;
            default:
                Console.WriteLine("Enter valid choice");
                break;
        }
    }
}
