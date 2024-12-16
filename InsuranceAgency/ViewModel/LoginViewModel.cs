using BLL.Services;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace InsuranceAgency.ViewModel
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly LoginService _loginService;

        public event Action<string, string, int> OnNavigationRequested;

        public ICommand LoginCommand { get; }
        public LoginViewModel() {
            _loginService = new LoginService();
            LoginCommand = new RelayCommand(ExecuteLogin);
        }

        private string _username;
        public string Username
        {
            get => _username;
            set {
                _username = value;
                OnPropertyChanged();
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        private void ExecuteLogin(object parameter)
        {
            string _userRoleWithId = _loginService.CheckLoginAndPassword(Username, Password);

            if (!string.IsNullOrEmpty(_userRoleWithId))
            {
                var parts = _userRoleWithId.Split(':');
                string role = parts[0];
                int userId = int.Parse(parts[1]);

                if (role == "agent")
                {
                    OnNavigationRequested?.Invoke("Agent", Username, userId);
                    System.Windows.MessageBox.Show($"Добро пожаловать {Username}");
                }
                else if (role == "client")
                {
                    OnNavigationRequested?.Invoke("Client", Username, userId);
                    System.Windows.MessageBox.Show($"Добро пожаловать {Username}");
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Неверный логин или пароль");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
