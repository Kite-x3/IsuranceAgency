using BLL.DTO;
using BLL.Services;
using DAL;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Principal;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace InsuranceAgency.ViewModel
{
    public class ClientWindowViewModel : INotifyPropertyChanged
    {
        private readonly ClientService _clientService;
        private readonly InsuranceProgrammContractService _programService;
        private readonly InsuranceProgrammService _insuranceProgrammService;
        private readonly ContractService _contractService;
        private readonly InsuranceSituationService _insuranceSituationService;
        private readonly InsurancePeriodService _insurancePeriodService;
        private readonly LifestyleOptionsService _lifestyleOptionsService;
        private readonly int userId;

        public event Action<string> OnLeaveRequested;

        public ObservableCollection<TimingOptions> InsurancePeriods { get; set; }
        public ObservableCollection<LifestyleOptions> LifestyleOptions { get; set; }


        private ObservableCollection<CaseType> _insuranceCaseTypes;
        public ObservableCollection<CaseType> InsuranceCaseTypes
        {
            get => _insuranceCaseTypes;
            set
            {
                _insuranceCaseTypes = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _caseDate;
        public DateTime? CaseDate
        {
            get => _caseDate;
            set
            {
                _caseDate = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Contract> _contracts;
        public ObservableCollection<Contract> Contracts
        {
            get => _contracts;
            set
            {
                _contracts = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Contract> _aceptWaitingContracts;
        public ObservableCollection<Contract> AceptWaitingContracts
        {
            get => _aceptWaitingContracts;
            set
            {
                _aceptWaitingContracts = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand CreateInsuranceSituationCommand { get; }

        private TimingOptions selectedInsurancePeriod;
        public TimingOptions SelectedInsurancePeriod
        {
            get => selectedInsurancePeriod;
            set
            {
                selectedInsurancePeriod = value;
                OnPropertyChanged();
            }
        }

        private LifestyleOptions selectedLifestyleOption;
        public LifestyleOptions SelectedLifestyleOption
        {
            get => selectedLifestyleOption;
            set
            {
                selectedLifestyleOption = value;
                OnPropertyChanged();
            }
        }


        private InsuranceProgram _selectedInsuranceProgram;
        public InsuranceProgram SelectedInsuranceProgram
        {
            get => _selectedInsuranceProgram;
            set
            {
                _selectedInsuranceProgram = value;
                OnPropertyChanged();
                UpdateVisibility();
            }
        }
        private bool _isLifeInsuranceVisible;
        public bool IsLifeInsuranceVisible
        {
            get => _isLifeInsuranceVisible;
            set
            {
                _isLifeInsuranceVisible = value;
                OnPropertyChanged();
            }
        }

        private bool _isPropertyInsuranceVisible;
        public bool IsPropertyInsuranceVisible
        {
            get => _isPropertyInsuranceVisible;
            set
            {
                _isPropertyInsuranceVisible = value;
                OnPropertyChanged();
            }
        }

        private int _calculatedCost;
        public int CalculatedCost
        {
            get => _calculatedCost;
            set
            {
                _calculatedCost = value;
                OnPropertyChanged();
            }
        }

        private void UpdateVisibility()
        {
            IsLifeInsuranceVisible = SelectedInsuranceProgram?.Property == false;
            IsPropertyInsuranceVisible = SelectedInsuranceProgram?.Property == true;
        }


        private ObservableCollection<InsuranceProgram> _insurancePrograms;
        public ObservableCollection<InsuranceProgram> InsurancePrograms
        {
            get => _insurancePrograms;
            set
            {
                _insurancePrograms = value;
                OnPropertyChanged();
            }
        }

        private int _x;
        public int X
        {
            get => _x;
            set
            {
                if (_x != value)
                {
                    _x = value;
                    OnPropertyChanged(nameof(X));
                }
            }
        }
        private string _address;
        public string Address
        {
            get => _address;
            set
            {
                if (_address != value)
                {
                    _address = value;
                    OnPropertyChanged(nameof(X));
                }
            }
        }

        private DateTime _startDate;
        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                _startDate = value;
                OnPropertyChanged();
            }
        }

        private DateTime _endDate;
        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                _endDate = value;
                OnPropertyChanged();
            }
        }


        private int _area;
        public int Area
        {
            get => _area;
            set
            {
                if (_area != value)
                {
                    _area = value;
                    OnPropertyChanged(nameof(X));
                }
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

        private ObservableCollection<InsuranceCaseDTO> _usersInsuranceCases;
        public ObservableCollection<InsuranceCaseDTO> UsersInsuranceCases
        {
            get => _usersInsuranceCases;
            set
            {
                _usersInsuranceCases = value;
                OnPropertyChanged();
            }
        }

        private string _insuranceSituation;
        public string InsuranceSituation
        {
            get => _insuranceSituation;
            set
            {
                _insuranceSituation = value;
                OnPropertyChanged();
            }
        }
        private Contract _selectedContract;
        public Contract SelectedContract
        {
            get => _selectedContract;
            set
            {
                _selectedContract = value;
                OnPropertyChanged();
                FilterInsuranceCaseTypes();

                StartDate = _selectedContract.StartDate;
                EndDate = _selectedContract.EndDate;
            }
        }

        
        private Contract _selectedAcceptContract;
        public Contract SelectedAcceptContract
        {
            get => _selectedAcceptContract;
            set
            {
                _selectedAcceptContract = value;
                OnPropertyChanged();
            }
        }

        private CaseType _selectedInsuranceCaseType;
        public CaseType SelectedInsuranceCaseType
        {
            get => _selectedInsuranceCaseType;
            set
            {
                _selectedInsuranceCaseType = value;
                OnPropertyChanged();
            }
        }


        public RelayCommand CalculateCostCommand { get; }
        public RelayCommand LeaveApp { get; }
        public RelayCommand SubmitCaseCommand { get; }
        public RelayCommand CalculateForRealCostCommand { get; }
        public RelayCommand CalculateRenewalCostCommand { get; }

        public RelayCommand AcceptContractCommand { get; }
        public RelayCommand RejectContractCommand { get; }

        public ClientWindowViewModel(string username, int userId)
        {
            this.userId = userId;
            _clientService = new ClientService();
            _programService = new InsuranceProgrammContractService();
            _contractService = new ContractService();
            _insuranceSituationService = new InsuranceSituationService();
            _lifestyleOptionsService = new LifestyleOptionsService();
            _insuranceProgrammService = new InsuranceProgrammService();
            _insurancePeriodService = new InsurancePeriodService();
            CalculateCostCommand = new RelayCommand(GetContractCost);
            CalculateForRealCostCommand = new RelayCommand(CalculateForRealCost);
            CalculateRenewalCostCommand = new RelayCommand(RenewContract);

            LeaveApp = new RelayCommand(LeaveApplication);
            SubmitCaseCommand = new RelayCommand(NewInsuranceCase);

            AcceptContractCommand = new RelayCommand(AcceptContract);
            RejectContractCommand = new RelayCommand(RejectContract);


            Username = username;
            LoadOptions();
        }

        private void AcceptContract(object parametr)
        {
            if (SelectedAcceptContract == null)
            {
                MessageBox.Show("Выберите договор для принятия.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBoxResult result = MessageBox.Show(
                $"Стоимость договор: {SelectedAcceptContract.Cost}.\nВы уверены, что хотите принять этот договор?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                _contractService.AcceptContract(SelectedAcceptContract.ContractID);
                MessageBox.Show("Договор принят!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadOptions();
            }
        }

        private void RejectContract(object parametr)
        {
            if (SelectedAcceptContract == null)
            {
                MessageBox.Show("Выберите Договор для отклонения.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBoxResult result = MessageBox.Show(
                "Вы уверены, что хотите отклонить этот договор?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                _contractService.RejectContract(SelectedAcceptContract.ContractID);
                MessageBox.Show("Договор отклонён.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadOptions();
            }
        }

        public void FilterInsuranceCaseTypes()
        {
            bool isPropertyContract = _contractService.ContractIsProperty(_selectedContract);
            var filtredCaseTypes = _insuranceSituationService.GetIsPropertyCaseTypes(isPropertyContract);
            InsuranceCaseTypes = new ObservableCollection<CaseType>(filtredCaseTypes);
        }

        public void LeaveApplication(object parameter)
        {
            OnLeaveRequested?.Invoke("Login"); 
        }

        public void GetContractCost(object parameter)
        {
            if (SelectedInsurancePeriod == null)
            {
                MessageBox.Show("Пожалуйста, выберите период страхования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (SelectedInsuranceProgram.Property == false && SelectedLifestyleOption == null)
            {
                MessageBox.Show("Пожалуйста, выберите образ жизни.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (X==0)
            {
                MessageBox.Show("Пожалуйста, введите верные значения", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            float insurancePeriodScale = (float)SelectedInsurancePeriod.scale;
            if (SelectedInsuranceProgram?.Property == false) {
                float lifestyleOptionScale = (float)SelectedLifestyleOption.Scale;
                CalculatedCost = _programService.CalculateLifeContractCost(SelectedInsuranceProgram.ProgramID, X, insurancePeriodScale, lifestyleOptionScale);
            }
            else
            {
                CalculatedCost = _programService.CalculateLifeContractCost(SelectedInsuranceProgram.ProgramID, X, insurancePeriodScale, 0);
            }
        }
        public void CalculateForRealCost(object parameter)
        {
            int cost;

            float insurancePeriodScale = (float)SelectedInsurancePeriod.scale;
            if (SelectedInsuranceProgram?.Property == false)
            {
                float lifestyleOptionScale = (float)SelectedLifestyleOption.Scale;
                int age = _clientService.GetAgeOfUser(userId);
                cost = _programService.CalculateLifeContractCost(SelectedInsuranceProgram.ProgramID, age, insurancePeriodScale, lifestyleOptionScale);
            }
            else
            {
                cost = _programService.CalculatePropertyContractCost(SelectedInsuranceProgram.ProgramID, X, insurancePeriodScale);
            }

            MessageBoxResult result = MessageBox.Show(
                $"Стоимость договора: {cost}.\nВы хотите отправить заявку?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);
            DateTime startDate = DateTime.Now;
            DateTime endDate = startDate.AddMonths((int)selectedInsurancePeriod.scale);

            if (result == MessageBoxResult.Yes)
            {
                if (SelectedInsuranceProgram?.Property == false)
                {
                    _contractService.AddNewLifeContractAplication(
                        clientId: userId,
                        cost: cost,
                        startDate: startDate,
                        endDate: endDate,
                        programId: SelectedInsuranceProgram.ProgramID);
                }
                else
                {
                    _contractService.AddNewPropertyAplication(
                        clientId: userId,
                        cost: cost,
                        startDate: startDate,
                        endDate: endDate,
                        objectCost: X,
                        objectAddress: Address,
                        area: Area,
                        programId: SelectedInsuranceProgram.ProgramID);
                }
                MessageBox.Show("Заявка отправлена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public void NewInsuranceCase(object parameter)
        {
            if (CaseDate != default && SelectedInsuranceCaseType != null && SelectedContract != null)
            {
                // Выводим подтверждение
                var result = MessageBox.Show(
                    "Вы уверены, что хотите отправить заявку на страховой случай?",
                    "Подтверждение",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question
                );

                // Если пользователь согласился, отправляем заявку
                if (result == MessageBoxResult.Yes)
                {
                    _insuranceSituationService.createInsuranceSituationAplication(
                        CaseDate.Value,
                        SelectedInsuranceCaseType.CaseTypeID,
                        SelectedContract.ContractID,
                        InsuranceSituation
                    );

                    MessageBox.Show(
                        "Заявка на страховой случай успешно создана!",
                        "Успех",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );
                }
            }
            else
            {
                MessageBox.Show(
                    "Заполните все поля.",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }


        public void RenewContract(object parameter)
        {
            float cost = 0;
            if (SelectedContract != null && SelectedInsurancePeriod!= null)
            {
                cost = _contractService.RenewContractCost(SelectedContract.ContractID);
                MessageBoxResult result = MessageBox.Show(
                $"Стоимость договора: {cost}.\nВы хотите отправить заявку?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    float insurancePeriodScale = (float)SelectedInsurancePeriod.scale;
                    DateTime startDate = DateTime.Now;
                    DateTime endDate = startDate.AddMonths((int)insurancePeriodScale);
                    _contractService.RenewContract(SelectedContract.ContractID, startDate, endDate);
                    MessageBox.Show("Заявка на продление успешно создана!",
                        "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Заполните все поля.",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void LoadOptions()
        {
            var LifeOptions = _lifestyleOptionsService.GetAllLifestyleOptions();
            LifestyleOptions = new ObservableCollection<LifestyleOptions>(LifeOptions);

            var Timig = _insurancePeriodService.GetInsurancePeriods();
            InsurancePeriods = new ObservableCollection<TimingOptions>(Timig);

            var programs = _insuranceProgrammService.GetInsuranceProgramList();
            InsurancePrograms = new ObservableCollection<InsuranceProgram>(programs);

            var situations = _insuranceSituationService.GetAllCaseTypes();
            InsuranceCaseTypes = new ObservableCollection<CaseType>(situations);

            var contracts = _contractService.GetAllUserContracts(userId);
            Contracts = new ObservableCollection<Contract>(contracts);

            var aceptContr = _contractService.GetAllAceptWaitingContracts(userId);
            AceptWaitingContracts = new ObservableCollection<Contract>(aceptContr);

            var cases = _insuranceSituationService.GetAllInsuranceCases(userId);
            UsersInsuranceCases = new ObservableCollection<InsuranceCaseDTO>(cases);
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
