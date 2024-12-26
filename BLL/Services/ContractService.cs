using BLL.DTO;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class ContractService
    {
        InsuranceDBEntities1 db;
        public ContractService()
        {
            db = new InsuranceDBEntities1();

        }
        public void AddNewLifeContractAplication(int clientId, decimal cost, DateTime startDate, DateTime endDate, 
            int programId)
        {
            var newContract = new Contract
            {
                ContractID = db.Contract.Max(c => c.ContractID) + 1,
                ClientID = clientId,
                Cost = cost,
                StartDate = startDate,
                EndDate = endDate,
                ProgramID = programId,
                ready = false
            };
            db.Contract.Add(newContract);

            db.SaveChanges();
        }
        public List<Contract> GetAllRenewableContracts(int userID)
        {
            DateTime currentDate = DateTime.Now;
            return db.Contract
                .Where(contract => contract.EndDate < currentDate && contract.ClientID == userID)
                .ToList();
        }
        public void AddNewPropertyAplication(int clientId, decimal cost, DateTime startDate, DateTime endDate, 
            int objectCost, string objectAddress, int programId, int area)
        {
            var existingProperty = db.Property.FirstOrDefault(p => p.Address == objectAddress);

            int propertyID;

            if (existingProperty == null)
            {
                var newProperty = new Property
                {
                    PropertyID = db.Property.Max(c => c.PropertyID) + 1,
                    Address = objectAddress,
                    EstimatedValue = objectCost,
                    TotalArea = area
                    
                };
                db.Property.Add(newProperty);
                db.SaveChanges();

                propertyID = newProperty.PropertyID;
            }
            else
            {
                propertyID = existingProperty.PropertyID;
            }

            var newContract = new Contract
            {
                ContractID = db.Contract.Max(c => c.ContractID) + 1,
                ClientID = clientId,
                Cost = cost,
                StartDate = startDate,
                EndDate = endDate,
                ObjectID = propertyID,
                ProgramID = programId,
                ready = false
            };

            db.Contract.Add(newContract);

            db.SaveChanges();
        }



        public List<ContractInfoDTO> GetUnsignedUserContracts()
        {
            var contracts = from contract in db.Contract
                            where contract.signed == null 
                            join client in db.Client on contract.ClientID equals client.ClientID
                            join property in db.Property on contract.ObjectID equals property.PropertyID into properties
                            from property in properties.DefaultIfEmpty()
                            select new ContractInfoDTO
                            {
                                ContractID = contract.ContractID,
                                Number = contract.Number,
                                ClientName = client.FullName,
                                ObjectAddress = property != null ? property.Address : "Страхование жизни",
                                Cost = contract.Cost,
                                StartDate = contract.StartDate,
                                EndDate = contract.EndDate,
                                Passport = client.Passport,
                            };

            return contracts.ToList();
        }

        public List<Contract> GetAllUserContracts(int userID)
        {
            return db.Contract.Where(c => c.ClientID == userID).ToList();
        }
        public List<Contract> GetAllUserSignedContracts(int userID)
        {
            return db.Contract
                .Where(c => c.ClientID == userID && c.signed == true && c.ready == true)
                .ToList();
        }
        public List<Contract> GetAllUserActiveContracts(int userID)
        {
            DateTime currentDate = DateTime.Now;
            return db.Contract
                .Where(c => c.ClientID == userID && c.EndDate > currentDate && c.signed == true && c.ready == true)
                .ToList();
        }
        public float RenewContractCost(int currentContractId)
        {
            var currentContract = db.Contract.FirstOrDefault(c => c.ContractID == currentContractId);

            bool hasInsuranceCases = db.InsuranceCase.Any(s => s.ContractID == currentContractId);
            decimal newCost = hasInsuranceCases
                ? currentContract.Cost * 1.5m
                : currentContract.Cost * 0.85m;
            return (float)newCost;
        }
        public void RenewContract(int currentContractId, DateTime newStartDate, DateTime newEndDate)
        {
            var currentContract = db.Contract.FirstOrDefault(c => c.ContractID == currentContractId);

            bool hasInsuranceCases = db.InsuranceCase.Any(s => s.ContractID == currentContractId);

            decimal newCost = hasInsuranceCases
                ? currentContract.Cost * 1.5m 
                : currentContract.Cost * 0.85m;

            var newContract = new Contract
            {
                ContractID = db.Contract.Max(c => c.ContractID) + 1,
                ClientID = currentContract.ClientID,
                InsuranceAgent = currentContract.InsuranceAgent,
                Cost = newCost,
                StartDate = newStartDate,
                EndDate = newEndDate,
                ProgramID = currentContract.ProgramID,
                ObjectID = currentContract.ObjectID,
                ready = false
            };

            db.Contract.Add(newContract);
            db.SaveChanges();
        }
        public bool ContractIsProperty(Contract contract)
        {
            if (contract.ObjectID == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public List<Contract> GetAllAceptWaitingContracts(int userID)
        {
            var contracts = db.Contract
                .Where(c => c.signed == true && c.ready == null && c.ClientID == userID)
                .ToList();

            foreach (var contract in contracts)
            {
                if (!string.IsNullOrEmpty(contract.Comment))
                {
                    contract.Comment = contract.Comment.TrimEnd();
                }
            }

            return contracts;
        }

        public void SignContract(int contractId, int cost, string comment, int userID)
        {
            var contract = db.Contract.FirstOrDefault(c => c.ContractID == contractId);
            if (contract != null)
            {
                contract.signed = true;
                contract.Cost = cost;
                contract.ready = null;
                contract.Comment = comment;
                contract.InsuranceAgentID = userID;
                db.SaveChanges();
            }
        }

        public void UnsignContract(int contractId, string comment, int userID)
        {
            var contract = db.Contract.FirstOrDefault(c => c.ContractID == contractId);
            if (contract != null)
            {
                contract.signed = false;
                contract.Cost = 0;
                contract.Comment = comment.TrimEnd();
                contract.InsuranceAgentID= userID;
                db.SaveChanges();
            }
        }

        public void AcceptContract(int contractID)
        {
            var contract = db.Contract.FirstOrDefault(c => c.ContractID == contractID);
            if (contract != null)
            {
                contract.ready = true;

                db.SaveChanges();
            }
        }
        public void RejectContract(int contractID)
        {
            var contract = db.Contract.FirstOrDefault(c => c.ContractID == contractID);
            if (contract != null)
            {
                contract.ready = false;

                db.SaveChanges();
            }
        }
        public List<string> GetSignedStatusOptions()
        {
            var signedStatusOptions = new List<string> { "Без фильтра" };

            // Подписан, Не подписан
            signedStatusOptions.AddRange(db.Contract
                .Select(c => c.signed)
                .Distinct()
                .Where(s => s.HasValue) //чтобы не добавлять null
                .Select(s => s.Value ? "Подписан" : "Не подписан"));

            // "Не определено", если есть записи с null в signed
            if (db.Contract.Any(c => !c.signed.HasValue))
            {
                signedStatusOptions.Add("Не определено");
            }

            return signedStatusOptions;
        }


        public List<ContractFilterDTO> GetAllContracts()
        {
            var contracts = from c in db.Contract
                            join p in db.InsuranceProgram on c.ProgramID equals p.ProgramID
                            join cl in db.Client on c.ClientID equals cl.ClientID
                            join prop in db.Property on c.ObjectID equals prop.PropertyID into properties
                            join a in db.InsuranceAgent on c.InsuranceAgentID equals a.InsuranceAgentID
                            from prop in properties.DefaultIfEmpty()
                            select new ContractFilterDTO
                            {
                                ContractID = c.ContractID,
                                Number = c.Number,
                                ClientName = cl.FullName,
                                ObjectAddress = prop != null ? prop.Address : "Страхование жизни",
                                Cost = c.Cost,
                                StartDate = c.StartDate,
                                EndDate = c.EndDate,
                                ProgramName = p.Name,
                                signed = c.signed,
                                ready = c.ready,
                                InsuranceAgent = a.FullName
                            };

            return contracts.ToList();
        }


        public List<string> GetReadyStatusOptions()
        {
            var readyStatusOptions = new List<string> { "Без фильтра" };

            // Добавляем варианты: Готов, Не готов, Не определено
            readyStatusOptions.AddRange(db.Contract
                .Select(c => c.ready)
                .Distinct()
                .Where(r => r.HasValue) // Фильтруем значения, чтобы не добавлять null
                .Select(r => r.Value ? "Готов" : "Не готов"));

            if (db.Contract.Any(c => !c.ready.HasValue))
            {
                readyStatusOptions.Add("Не определено");
            }

            return readyStatusOptions;
        }


        public List<ContractFilterDTO> FilterContracts(int? contractNumber, string signedStatus, string readyStatus, string programType)
        {
            var contracts = from c in db.Contract
                            join p in db.InsuranceProgram on c.ProgramID equals p.ProgramID
                            join a in db.InsuranceAgent on c.InsuranceAgentID equals a.InsuranceAgentID
                            where
                                (!contractNumber.HasValue || c.Number == contractNumber) &&

                                // Фильтрация по signedStatus (bit, с тремя состояниями)
                                (signedStatus == "Без фильтра" ||
                                 (signedStatus == "Не подписан" && c.signed == false) ||
                                 (signedStatus == "Подписан" && c.signed == true) ||
                                 (signedStatus == "Не определено" && !c.signed.HasValue)) &&

                                // Фильтрация по readyStatus (bit, с тремя состояниями)
                                (readyStatus == "Без фильтра" ||
                                 (readyStatus == "Не готов" && c.ready == false) ||
                                 (readyStatus == "Готов" && c.ready == true) ||
                                 (readyStatus == "Не определено" && !c.ready.HasValue)) &&

                                // Фильтрация по programType
                                (programType == "Без фильтра" || p.Name.Contains(programType))
                            select new ContractFilterDTO
                            {
                                ContractID = c.ContractID,
                                Number = c.Number,
                                ClientName = c.Client.FullName,
                                ObjectAddress = c.ObjectID.HasValue ? c.Property.Address : "Страхование жизни",
                                Cost = c.Cost,
                                StartDate = c.StartDate,
                                EndDate = c.EndDate,
                                ProgramName = p.Name,
                                signed = c.signed,
                                ready = c.ready,
                                InsuranceAgent = a.FullName
                            };

            return contracts.ToList();
        }




    }
}
