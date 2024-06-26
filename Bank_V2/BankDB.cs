﻿namespace Bank;

static class BankDB
{
    private const int maxUserAccounts = 7;
    private const int maxAdminAccounts = 3;

    private const int maxAccounts = maxUserAccounts + maxAdminAccounts;

    public static List<Account> accounts = new List<Account>(maxAccounts);

    private const string fileName = "Data.txt";


    
    static BankDB()
    {
        // !!!ATTENTION!!!  Change This Directory to Yours
        Directory.SetCurrentDirectory("C:\\Users\\User\\source\\repos\\Bank\\Bank_V2\\Data");

        ReadFile();
    }

    public static void PrintFile()
    {
        using StreamWriter file = new(fileName);

        for (int i = 0; i < maxAccounts; i++) // Write Data
        {
            if (accounts[i] != null)
            {
                file.Write(accounts[i].Username + " ");
                file.Write(accounts[i].Password);

                if (accounts[i] is User)
                {
                    file.Write(" ");
                    file.Write(((User)accounts[i]).Card.ID + " ");
                    file.Write(((User)accounts[i]).Card.Balance + " ");
                    file.Write(((User)accounts[i]).Card.Salary + " ");
                    file.Write(((User)accounts[i]).Year);

                }
            }
            file.WriteLine();

        }
    }

    private static void ReadFile()
    {
        using StreamReader file = new(fileName);

        int maxInputLength = 6;
        string[] data = new string[maxInputLength];
        for (int i = 0; i < maxAccounts; i++)
        {
            string str = file.ReadLine() ?? " ";
            for (int j = 0; j < str.Split().Length; j++) // Transfer data into an array
                data[j] = str.Split()[j];

            if (i < maxAdminAccounts) // Read Admin Data
                accounts.Add(new Admin(data[0], data[1]));

            else // Read User Data 
            {
                Card card = new Card(data[2] ?? "", decimal.Parse(data[3] ?? "0"), int.Parse(data[4] ?? "0"));

                accounts.Add(new User(data[0], data[1], card, int.Parse(data[5] ?? "0")));
            }
        }
    }


    public static Account FindAccount(string username)
    {
        foreach (Account account in accounts)
            if (account.Username == username)
                return account;

        return null;
    }

    public static int FindEmptyAccount(bool isAdmin)
    {
        if (isAdmin)
        {
            for (int i = 0; i < maxAdminAccounts; i++)
                if (accounts[i].Username == "")
                    return i;
        }
        else
        {
            for (int i = maxAdminAccounts; i < maxAccounts; i++)
                if (accounts[i].Username == "")
                    return i;
        }

        return -1;
    }

    public static bool CreateAccount(string username, string password, bool isAdmin)
    {
        Console.Clear();

        if (isExist(username))
        {
            Console.WriteLine("Account with this Username already exist\n");
            return false;
        }
        
        Account account;

        if (isAdmin)    
            account = (Admin) new(username, password);
        else
            account = (User) new(username, password, new Card());

        int index = FindEmptyAccount(isAdmin);

        if (index != -1)
            accounts[index] = account;

        else
        {
            Console.WriteLine("There no avaliable space for this account\n");
            return false;
        }

        PrintFile();
        return true;
    }

    public static bool isExist(string username)
    {
        if(FindAccount(username) != null)
            return true;

        return false;
    }

    public static bool isCorrect(Account userAccount)
    {
        Account account = FindAccount(userAccount.Username);
        if (account != null && (account.Username == userAccount.Username && account.Password == userAccount.Password))
            return true;

        return false;
    }

}
