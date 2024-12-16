using BLL.DTO;
using BLL.Services;
using DAL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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

        private bool _selectedSignedStatus;
        public bool SelectedSignedStatus
        {
            get => _selectedSignedStatus;
            set { _selectedSignedStatus = value; OnPropertyChanged(); }
        }

        private bool _selectedReadyStatus;
        public bool SelectedReadyStatus
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

        private decimal _customCost;
        public decimal CustomCost
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

        private ObservableCollection<bool?> _signedStatusOptions;
        public ObservableCollection<bool?> SignedStatusOptions
        {
            get => _signedStatusOptions;
            set { _signedStatusOptions = value; OnPropertyChanged(); }
        }

        private ObservableCollection<bool?> _readyStatusOptions;
        public ObservableCollection<bool?> ReadyStatusOptions
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
        private int _contractNumberFilter;
        public int ContractNumberFilter
        {
            get => _contractNumberFilter;
            set { _contractNumberFilter = value; OnPropertyChanged(); }
        }

        private int _caseNumberFilter;
        public int CaseNumberFilter
        {
            get => _caseNumberFilter;
            set { _caseNumberFilter = value; OnPropertyChanged(); }
        }

        private DateTime _reportStartDate;
        public DateTime ReportStartDate
        {
            get => _reportStartDate;
            set { _reportStartDate = value; OnPropertyChanged(); }
        }

        private DateTime _reportEndDate;
        public DateTime ReportEndDate
        {
            get => _reportEndDate;
            set { _reportEndDate = value; OnPropertyChanged(); }
        }
        public RelayCommand LeaveApp { get; }

        public RelayCommand GenerateReportCommand { get; }
        public RelayCommand ApplyContractFilterCommand { get; }
        public RelayCommand ApplyCaseFilterCommand { get; }


        public void LoadData()
        {
            var contracts = _contractService.GetUnsignedUserContracts();
            InsuranceContracts = new ObservableCollection<ContractInfoDTO>(contracts);

            var cases = _insuranceSituationService.GetUnsignedInsuranceCases();
            InsuranceCases = new ObservableCollection<InsuranceCaseInfoDTO>(cases);

            var signedStatusOptions = _contractService.GetSignedStatusOptions();
            SignedStatusOptions = new ObservableCollection<bool?>(signedStatusOptions);

            var readyStatusOptions = _contractService.GetReadyStatusOptions();
            ReadyStatusOptions = new ObservableCollection<bool?>(readyStatusOptions);

            var programTypeOptions = _insuranceProgrammService.GetProgramTypeOptions();
            ProgramTypeOptions = new ObservableCollection<string>(programTypeOptions);

            var filtredContracts = _contractService.GetAllContracts();
            FilteredContracts = new ObservableCollection<ContractFilterDTO>(filtredContracts);

            var caseType = _insuranceTypeService.GetAllInsuranceTypes();
            CaseTypeOptions = new ObservableCollection<string>(caseType);

            var filtredcases = _insuranceSituationService.GetAllOfInsuranceCases();
            FilteredCases = new ObservableCollection<InsuranceCaseInfoDTO>(filtredcases);
        }

        public AgentWindowViewModel(string username, int userId) {
            this.userId = userId;
            _contractService = new ContractService();
            _insuranceSituationService = new InsuranceSituationService();
            _reportService = new ReportService();
            _insuranceProgrammService = new InsuranceProgrammService();
            _insuranceTypeService = new InsuranceTypeService();
            LoadData();

            LeaveApp = new RelayCommand(LeaveApplication);

            Username = username;

            AcceptCaseCommand = new RelayCommand(AcceptCase);
            RejectCaseCommand = new RelayCommand(RejectCase);
            AcceptContractCommand = new RelayCommand(AcceptContract);
            RejectContractCommand = new RelayCommand(RejectContract);

            GenerateReportCommand = new RelayCommand(GenerateReport);
            ApplyContractFilterCommand = new RelayCommand(ApplyContractFilter);
            ApplyCaseFilterCommand = new RelayCommand(ApplyCaseFilter);
            ResetFilterCommand = new RelayCommand(ResetFilter);
        }

        public RelayCommand AcceptCaseCommand { get; }
        public RelayCommand RejectCaseCommand { get; }
        public RelayCommand AcceptContractCommand { get; }
        public RelayCommand RejectContractCommand { get; }

        public RelayCommand ResetFilterCommand { get; }

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

            var reportData = _reportService.GetReportData(ReportStartDate, ReportEndDate);
            ReportData = new ObservableCollection<ReportItemDTO> { reportData };
        }
        private void ApplyContractFilter(object parameter)
        {
            var filteredContracts = _contractService.FilterContracts(ContractNumberFilter, 
                SelectedSignedStatus, SelectedReadyStatus, SelectedProgramType);

            FilteredContracts = new ObservableCollection<ContractFilterDTO>(filteredContracts);
        }

        private void ApplyCaseFilter(object parameter)
        {
            var filteredCases = _insuranceSituationService.FilterCases(CaseNumberFilter, SelectedCaseType, SelectedSignedStatus);

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
                _contractService.SignContract(SelectedContract.ContractID, (int)CustomCost, Comment);
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
                _contractService.UnsignContract(SelectedContract.ContractID, SelectedContract.Comment);
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
