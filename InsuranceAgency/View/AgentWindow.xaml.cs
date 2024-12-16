using InsuranceAgency.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
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
    public partial class AgentWindow : Window
    {
        public AgentWindow(string username, int userId)
        {
            InitializeComponent();
            AgentWindowViewModel viewModel = new AgentWindowViewModel(username, userId);
            DataContext = viewModel;
            viewModel.OnLeaveRequested += Leave;
        }

        private void Leave(string window)
        {
            new LoginWindow().Show();
            Close();
        }
    }
}
