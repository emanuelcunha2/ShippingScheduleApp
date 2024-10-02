using Microsoft.Win32;
using ShippingScheduleMVVM.Services;
using System;
using System.Collections.Generic;
using System.Windows;

namespace ShippingScheduleMVVM.Models
{
    public class User
    {
        private int _id { get; set; }
        private int? _authenticationStatus { get; set; }
        private string _username { get; set; }
        private string _password { get; set; }
        private string _email { get; set; }
        private List<string>? _roles { get; set; }
        public int? AuthenticationStatus { get => _authenticationStatus; }
        public string Username { get => _username; }
        public string Password { get => _password; }
        public string Email { get => _email; }
        public int Id { get => _id; }
        public List<string>? Roles { get => _roles; }

        // Creating new user
        public User()
        {
            _id = -1;
            _username = string.Empty;
            _password = string.Empty;
            _email = string.Empty;
            _authenticationStatus = null;
            _roles = null;
        }
        // Getting Login Database data
        public User(string username, string password)
        {
            _id = -1;
            _username = username;
            _password = password;
            _email = string.Empty;
            _authenticationStatus = null;
            _roles = null;
        }
        // Getting error data
        public User(int authenticationStatus)
        {
            _id = -1;
            _username = string.Empty;
            _password = string.Empty;
            _email = string.Empty;
            _authenticationStatus = authenticationStatus;
            _roles = null;
        }
        public User(string username, string email, int id, int authenticationStatus)
        {
            _id = id;
            _username = username;
            _password = string.Empty;
            _email = email;
            _authenticationStatus = authenticationStatus;
            _roles = null;
        }
        // Register new user
        public User(string username, string password, string email)
        {
            _id = -1;
            _username = username;
            _password = password;
            _email = email;
            _authenticationStatus = null;
            _roles = null;
        }
        /**
            <summary>
            This function retrieves saved user information from the Windows Registry.
            </summary>
            <returns>
            Returns a boolean value indicating whether the operation was successful.
            </returns>
        */
        public bool GetSavedUserInformation()
        {
            string username = Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\ShippingSchedule", "Username", "NULL")?.ToString() ?? "Null";
            string password = Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\ShippingSchedule", "Password", "NULL")?.ToString() ?? "Null";

            // Check if user information was found in the Registry
            if (username != "Null" && password != "Null")
            {
                _username = username;
                _password = password;
                return true;
            }
            // If user information was not found, return false
            return false;
        }
        public Tuple<bool, string, User?> Login()
        {
            DatabaseOperations database = new();
            User? retrievedUser = database.LoginUser(_username, _password);
  
            if(retrievedUser.AuthenticationStatus == 200) { retrievedUser._roles = database.GetUserRoles(retrievedUser._id); }
            switch (retrievedUser.AuthenticationStatus)
            {
                case 200:
                    return Tuple.Create(true, "Success.", retrievedUser ?? null);
                case 401:
                    return Tuple.Create(false, "A password que inseriu é inválida, tente outra vez.", retrievedUser ?? null);
                case 403:
                    return Tuple.Create(false, "O utilizador que inseriu não foi encontrado.", retrievedUser ?? null);
                case 500:
                    return Tuple.Create(false, "Erro interno da aplicação.", retrievedUser ?? null);
                default:
                    break;
            }
            return Tuple.Create(false, "Undefined error.", retrievedUser ?? null);
        }
        public bool Register()
        {
            DatabaseOperations database = new();

            if (database.UsernameExists(_username))
            {
                MessageBox.Show("Username already exists");
                return false;
            }

            database.RegisterUser(_username, _password, _email);
            return true;
        }
        public void SetUserCredentials(bool rememberPassword)
        {
            if (rememberPassword)
            {
                Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\ShippingSchedule", "Username", _username);
                Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\ShippingSchedule", "Password", _password);
                return;
            }
            Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\ShippingSchedule", "Username", "Null");
            Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\ShippingSchedule", "Password", "Null");
        }
    }
}
