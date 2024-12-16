using InsuranceAgency.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;

namespace InsuranceAgency
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            LoginViewModel viewModel = new LoginViewModel();
            viewModel.OnNavigationRequested += NavigateToWindow;
            DataContext = viewModel;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var viewModel = (LoginViewModel)DataContext;
            viewModel.Password = ((PasswordBox)sender).Password;
        }
        private void NavigateToWindow(string windowName, string username, int userId)
        {
            Window newWindow = null;

            switch (windowName)
            {
                case "Agent":
                    newWindow = new AgentWindow(username, userId);
                    break;
                case "Client":
                    newWindow = new ClientWindow(username, userId);
                    break;
            }

            if (newWindow != null)
            {
                newWindow.Show();
                this.Close();
            }
        }
        protected override void OnClosed(EventArgs e)
        {
            if (DataContext is LoginViewModel viewModel)
            {
                viewModel.OnNavigationRequested -= NavigateToWindow;
            }
            base.OnClosed(e);
        }
    }
}
