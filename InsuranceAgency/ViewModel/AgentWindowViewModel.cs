using BLL.DTO;
using BLL.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Windows.Input;

namespace InsuranceAgency.ViewModel
{
    public class AgentWindowViewModel : INotifyPropertyChanged
    {
        private readonly int userId;

        public event Action<string> OnLeaveRequested;

        private readonly ContractService _contractService;
        private readonly InsuranceSituationService _insuranceSituationService;
        private readonly ReportService _reportService;
        private readonly InsuranceProgrammService _insuranceProgrammService;
        private readonly InsuranceTypeService _insuranceTypeService;
        private readonly ClientService _clientService;

        private string _selectedReadyStatus;
        public string SelectedReadyStatus
        {
            get => _selectedReadyStatus;
            set { _selectedReadyStatus = value; OnPropertyChanged(); }
        }

        private string _selectedProgramType;
        public string SelectedProgramType
        {
            get => _selectedProgramType;
            set { _selectedProgramType = value; OnPropertyChanged(); }
        }

        private string _selectedCaseType;
        public string SelectedCaseType
        {
            get => _selectedCaseType;
            set { _selectedCaseType = value; OnPropertyChanged(); }
        }

        private ObservableCollection<ContractFilterDTO> _filteredContracts;
        public ObservableCollection<ContractFilterDTO> FilteredContracts
        {
            get => _filteredContracts;
            set { _filteredContracts = value; OnPropertyChanged(); }
        }

        private ObservableCollection<InsuranceCaseInfoDTO> _filteredCases;
        public ObservableCollection<InsuranceCaseInfoDTO> FilteredCases
        {
            get => _filteredCases;
            set { _filteredCases = value; OnPropertyChanged(); }
        }

        private ObservableCollection<ReportItemDTO> _reportData;
        public ObservableCollection<ReportItemDTO> ReportData
        {
            get => _reportData;
            set { _reportData = value; OnPropertyChanged(); }
        }



        private ObservableCollection<InsuranceCaseInfoDTO> _insuranceCases;
        public ObservableCollection<InsuranceCaseInfoDTO> InsuranceCases
        {
            get => _insuranceCases;
            set
            {
                _insuranceCases = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<ContractInfoDTO> _insuranceContracts;
        public ObservableCollection<ContractInfoDTO> InsuranceContracts
        {
            get => _insuranceContracts;
            set
            {
                _insuranceContracts = value;
                OnPropertyChanged();
            }
        }

        private InsuranceCaseInfoDTO _selectedCase;
        public InsuranceCaseInfoDTO SelectedCase
        {
            get => _selectedCase;
            set
            {
                _selectedCase = value;
                OnPropertyChanged();

                if (_selectedCase != null)
                    CustomCost = _selectedCase.Cost;
            }
        }
        private ContractInfoDTO _selectedContract;
        public ContractInfoDTO SelectedContract
        {
            get => _selectedContract;
            set
            {
                _selectedContract = value;
                OnPropertyChanged();

                if (_selectedContract != null)
                    CustomCost = _selectedContract.Cost;
            }
        }

        private decimal? _customCost;
        public decimal? CustomCost
        {
            get => _customCost;
            set
            {
                _customCost = value;
                OnPropertyChanged();
            }
        }

        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<string> _caseSignedStatusOptions;
        public ObservableCollection<string> CaseSignedStatusOptions
        {
            get => _caseSignedStatusOptions;
            set { _caseSignedStatusOptions = value; OnPropertyChanged(); }
        }

        private ObservableCollection<string> _contractSignedStatusOptions;
        public ObservableCollection<string> ContractSignedStatusOptions
        {
            get => _contractSignedStatusOptions;
            set { _contractSignedStatusOptions = value; OnPropertyChanged(); }
        }

        private string _selectedCaseSignedStatus;
        public string SelectedCaseSignedStatus
        {
            get => _selectedCaseSignedStatus;
            set { _selectedCaseSignedStatus = value; OnPropertyChanged(); }
        }

        private string _selectedContractSignedStatus;
        public string SelectedContractSignedStatus
        {
            get => _selectedContractSignedStatus;
            set { _selectedContractSignedStatus = value; OnPropertyChanged(); }
        }

        private ObservableCollection<string> _readyStatusOptions;
        public ObservableCollection<string> ReadyStatusOptions
        {
            get => _readyStatusOptions;
            set { _readyStatusOptions = value; OnPropertyChanged(); }
        }

        private ObservableCollection<string> _programTypeOptions;
        public ObservableCollection<string> ProgramTypeOptions
        {
            get => _programTypeOptions;
            set { _programTypeOptions = value; OnPropertyChanged(); }
        }

        private ObservableCollection<string> _caseTypeOptions;
        public ObservableCollection<string> CaseTypeOptions
        {
            get => _caseTypeOptions;
            set { _caseTypeOptions = value; OnPropertyChanged(); }
        }

        private string _comment;
        public string Comment
        {
            get => _comment;
            set
            {
                _comment = value;
                OnPropertyChanged();
                if (SelectedCase != null)
                    SelectedCase.Comment = value;
                if (SelectedContract != null)
                    SelectedContract.Comment = value;
            }
        }
        private int? _contractNumberFilter;
        public int? ContractNumberFilter
        {
            get => _contractNumberFilter;
            set { _contractNumberFilter = value; OnPropertyChanged(); }
        }

        private string _newClientName;
        public string NewClientName
        {
            get => _newClientName;
            set { _newClientName = value; OnPropertyChanged(); }
        }

        private string _newClientPassport;
        public string NewClientPassport
        {
            get => _newClientPassport;
            set { _newClientPassport = value; OnPropertyChanged(); }
        }

        private DateTime? _newClientBirthDate;
        public DateTime? NewClientBirthDate
        {
            get => _newClientBirthDate;
            set { _newClientBirthDate = value; OnPropertyChanged(); }
        }

        private string _newClientPassword;
        public string NewClientPassword
        {
            get => _newClientPassword;
            set { _newClientPassword = value; OnPropertyChanged(); }
        }

        private string _newClientEmail;
        public string NewClientEmail
        {
            get => _newClientEmail;
            set { _newClientEmail = value; OnPropertyChanged(); }
        }


        private int? _caseNumberFilter;
        public int? CaseNumberFilter
        {
            get => _caseNumberFilter;
            set { _caseNumberFilter = value; OnPropertyChanged(); }
        }

        private DateTime? _reportStartDate;
        public DateTime? ReportStartDate
        {
            get => _reportStartDate;
            set { _reportStartDate = value; OnPropertyChanged(); }
        }

        private DateTime? _reportEndDate;
        public DateTime? ReportEndDate
        {
            get => _reportEndDate;
            set { _reportEndDate = value; OnPropertyChanged(); }
        }
        public RelayCommand LeaveApp { get; }

        public RelayCommand GenerateReportCommand { get; }
        public RelayCommand ApplyContractFilterCommand { get; }
        public RelayCommand ApplyCaseFilterCommand { get; }
        public RelayCommand AddClientCommand { get; }


        public void LoadData()
        {
            var contracts = _contractService.GetUnsignedUserContracts();
            InsuranceContracts = new ObservableCollection<ContractInfoDTO>(contracts);

            var cases = _insuranceSituationService.GetUnsignedInsuranceCases();
            InsuranceCases = new ObservableCollection<InsuranceCaseInfoDTO>(cases);

            var signedStatusOptions = _contractService.GetSignedStatusOptions();
            ContractSignedStatusOptions = new ObservableCollection<string>(signedStatusOptions);

            var caseSignedStatusOptions = _insuranceSituationService.GetSignedStatusOptions();
            CaseSignedStatusOptions = new ObservableCollection<string>(caseSignedStatusOptions);

            var readyStatusOptions = _contractService.GetReadyStatusOptions();
            ReadyStatusOptions = new ObservableCollection<string>(readyStatusOptions);

            var programTypeOptions = _insuranceProgrammService.GetProgramTypeOptions();
            ProgramTypeOptions = new ObservableCollection<string>(programTypeOptions);

            var filtredContracts = _contractService.GetAllContracts();
            FilteredContracts = new ObservableCollection<ContractFilterDTO>(filtredContracts);

            var caseTypeOptions = _insuranceTypeService.GetAllInsuranceTypes();
            CaseTypeOptions = new ObservableCollection<string>(caseTypeOptions);

            var filtredcases = _insuranceSituationService.GetAllOfInsuranceCases();
            FilteredCases = new ObservableCollection<InsuranceCaseInfoDTO>(filtredcases);

            SelectedContractSignedStatus = ContractSignedStatusOptions.FirstOrDefault();
            SelectedReadyStatus = ReadyStatusOptions.FirstOrDefault();
            SelectedProgramType = ProgramTypeOptions.FirstOrDefault();
            SelectedCaseSignedStatus = CaseSignedStatusOptions.FirstOrDefault();
            SelectedCaseType = CaseTypeOptions.FirstOrDefault();
        }

        public AgentWindowViewModel(string username, int userId) {
            this.userId = userId;
            _contractService = new ContractService();
            _insuranceSituationService = new InsuranceSituationService();
            _reportService = new ReportService();
            _insuranceProgrammService = new InsuranceProgrammService();
            _insuranceTypeService = new InsuranceTypeService();
            _clientService = new ClientService();
            LoadData();

            LeaveApp = new RelayCommand(LeaveApplication);

            Username = username;

            AcceptCaseCommand = new RelayCommand(AcceptCase);
            RejectCaseCommand = new RelayCommand(RejectCase);
            AcceptContractCommand = new RelayCommand(AcceptContract);
            RejectContractCommand = new RelayCommand(RejectContract);
            PrintReportCommand = new RelayCommand(PrintReport);
            AddClientCommand = new RelayCommand(AddClient);

            GenerateReportCommand = new RelayCommand(GenerateReport);
            ApplyContractFilterCommand = new RelayCommand(ApplyContractFilter);
            ApplyCaseFilterCommand = new RelayCommand(ApplyCaseFilter);
            ResetFilterCommand = new RelayCommand(ResetFilter);
        }

        public RelayCommand AcceptCaseCommand { get; }
        public RelayCommand RejectCaseCommand { get; }
        public RelayCommand AcceptContractCommand { get; }
        public RelayCommand RejectContractCommand { get; }
        public RelayCommand PrintReportCommand { get; }
        public RelayCommand ResetFilterCommand { get; }

        private void AddClient(object parameter)
        {
            if (string.IsNullOrWhiteSpace(NewClientName) ||
                string.IsNullOrWhiteSpace(NewClientPassport) ||
                NewClientBirthDate == null ||
                string.IsNullOrWhiteSpace(NewClientPassword) ||
                string.IsNullOrWhiteSpace(NewClientEmail))
            {
                MessageBox.Show("Все поля должны быть заполнены.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var result = MessageBox.Show(
                $"Вы уверены, что хотите добавить нового клиента?\n\n" +
                $"Имя: {NewClientName}\n" +
                $"Паспорт: {NewClientPassport}\n" +
                $"Дата рождения: {NewClientBirthDate:dd.MM.yyyy}\n" +
                $"Email: {NewClientEmail}",
                "Подтверждение",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                _clientService.AddClient(
                name: NewClientName,
                passport: NewClientPassport,
                birthDate: NewClientBirthDate.Value,
                password: NewClientPassword,
                email: NewClientEmail
                );

                MessageBox.Show("Клиент успешно добавлен.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Очистка полей после добавления клиента
                NewClientName = string.Empty;
                NewClientPassport = string.Empty;
                NewClientBirthDate = null;
                NewClientPassword = string.Empty;
                NewClientEmail = string.Empty;
            }

        }

        private void PrintReport(object parameter)
        {
            if (ReportData == null || ReportData.Count == 0)
            {
                return;
            }

            var saveFileDialog = new SaveFileDialog
            {
                Filter = "PDF files (*.pdf)|*.pdf",
                DefaultExt = "pdf",
                FileName = "Отчёт.pdf"
            };

            var dialogResult = saveFileDialog.ShowDialog();
            if (dialogResult == System.Windows.Forms.DialogResult.OK)
            {
                _reportService.GeneratePdfReport(ReportData.ToList(), ReportStartDate.Value, ReportEndDate.Value, saveFileDialog.FileName);
            }
        }
        public void ResetFilter(object parameter)
        {
            var filtredContracts = _contractService.GetAllContracts();
            FilteredContracts = new ObservableCollection<ContractFilterDTO>(filtredContracts);


            var filtredcases = _insuranceSituationService.GetAllOfInsuranceCases();
            FilteredCases = new ObservableCollection<InsuranceCaseInfoDTO>(filtredcases);
        }


        private void GenerateReport(object parameter)
        {
            if (ReportStartDate == null || ReportEndDate == null)
                return;

            var reportData = _reportService.GetReportData(ReportStartDate.Value, ReportEndDate.Value);
            ReportData = new ObservableCollection<ReportItemDTO> { reportData };
        }
        private void ApplyContractFilter(object parameter)
        {
            var filteredContracts = _contractService.FilterContracts(ContractNumberFilter, 
                SelectedContractSignedStatus, SelectedReadyStatus, SelectedProgramType);

            FilteredContracts = new ObservableCollection<ContractFilterDTO>(filteredContracts);
        }

        private void ApplyCaseFilter(object parameter)
        {
            var filteredCases = _insuranceSituationService.FilterCases(CaseNumberFilter, 
                SelectedCaseType, SelectedCaseSignedStatus);

            FilteredCases = new ObservableCollection<InsuranceCaseInfoDTO>(filteredCases);
        }
        public void LeaveApplication(object parameter)
        {
            OnLeaveRequested?.Invoke("Login");
        }

        public void AcceptContract(object parametr)
        {
            if (SelectedContract != null)
            {
                _contractService.SignContract(SelectedContract.ContractID, 
                    (int)CustomCost, Comment, userId);
                InsuranceContracts.Remove(SelectedContract);
                CustomCost = 0;
                Comment = string.Empty;
                LoadData();
            }
        }

        public void RejectContract(object parametr)
        {
            if (SelectedContract != null)
            {
                _contractService.UnsignContract(SelectedContract.ContractID, SelectedContract.Comment,
                    userId);
                InsuranceContracts.Remove(SelectedContract);
                CustomCost = 0;
                Comment = string.Empty;
                LoadData();
            }
        }

        public void AcceptCase(object parametr)
        {
            if (SelectedCase != null)
            {
                _insuranceSituationService.SignCase(SelectedCase.CaseID, (int)CustomCost, Comment);
                InsuranceCases.Remove(SelectedCase);
                CustomCost = 0;
                Comment = string.Empty;
                LoadData();
            }
        }

        public void RejectCase(object parametr)
        {
            if (SelectedCase != null)
            {
                _insuranceSituationService.UnsignCase(SelectedCase.CaseID, SelectedCase.Comment);
                InsuranceCases.Remove(SelectedCase);
                CustomCost = 0;
                Comment = string.Empty;
                LoadData();
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
