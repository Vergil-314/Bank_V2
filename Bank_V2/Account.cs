﻿namespace Bank;

class Account
{

    protected string username;
    protected string password;

    private const int usernameMinLength = 4;
    private const int passwordMinLength = 4;

    public string Username
    {
        get => username;
        set
        {
            if (value == "")
                throw new ArgumentException("Username Can't Be Empty");

            if (value.Length < usernameMinLength)
                throw new ArgumentException("Username Can't Be Less Than " + usernameMinLength + " Digits");

            if (value.Contains(' '))
                throw new ArgumentException("Username Can't Contain Spaces");

            username = value;
        }
    }

    public string Password
    {
        get => password;
        set
        {
            if (value == "")
                throw new ArgumentException("Password Can't Be Empty");

            if (value.Length < passwordMinLength)
                throw new ArgumentException("Password Can't Be Less Than " + passwordMinLength + " Digits");

            if (value.Contains(' '))
                throw new ArgumentException("Password Can't Contain Spaces");

            password = value;

        }
    }

    public bool IsAdmin { get; protected set; }


    public Account(string? username = null, string? password = null)
    {
        this.username = username ?? "";
        this.password = password ?? "";
    }

    public static void EnterAccount(Account account)
    {
        account = BankDB.FindAccount(account.Username);

        if (account is Admin)
            ((Admin)account).MainMenu();

        else
            ((User)account).MainMenu();
    }

    protected void ChangePassword()
    {
        Password = Credentials.GetPassword("Enter new Password: ");
        Console.Clear();
        BankDB.PrintFile();
    }

    protected void DeleteAccount(string username)
    {

        for (int i = 0; i < BankDB.accounts.Capacity; i++)
            if (BankDB.accounts[i].Username == username)
            {
                if (IsAdmin)
                    BankDB.accounts[i] = new Admin();
                else
                    BankDB.accounts[i] = new User();

                BankDB.PrintFile();

                Console.Clear();
                Console.WriteLine("Account has been Deleted\n");
                return;
            }
    }

}
