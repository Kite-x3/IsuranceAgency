using InsuranceAgency.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace InsuranceAgency
{
    public partial class ClientWindow : Window
    {
        public ClientWindow(string username, int userId)
        {
            InitializeComponent();
            ClientWindowViewModel viewModel = new ClientWindowViewModel(username, userId);
            DataContext = viewModel;
            viewModel.OnLeaveRequested += Leave;
        }
        private void Leave(string window)
        {
            new LoginWindow().Show();
            Close();
        }
        protected override void OnClosed(EventArgs e)
        {
            if (DataContext is ClientWindowViewModel viewModel)
            {
                viewModel.OnLeaveRequested -= Leave;
            }
            base.OnClosed(e);
        }
    }
}
