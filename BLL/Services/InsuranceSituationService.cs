using DAL;
using BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class InsuranceSituationService
    {
        InsuranceDBEntities1 db;
        public InsuranceSituationService()
        {
            db = new InsuranceDBEntities1();
        }
        public void createInsuranceSituationAplication(DateTime date, int TypeID, int contractID, string description)
        {
            var Case = new InsuranceCase{
                CaseID = db.InsuranceCase.Max(c => c.CaseID)+1,
                ContractID = contractID,
                Date = date,
                CaseTypeID = TypeID,
                PayoutAmount = 0,
                description = description
            };
            db.InsuranceCase.Add(Case);

            db.SaveChanges();
        }
        public List<CaseType> GetAllCaseTypes()
        {
            return db.CaseType.ToList();
        }
        public List<CaseType> GetIsPropertyCaseTypes(bool isProperty)
        {
            return db.CaseType.Where(c => c.Property == isProperty).ToList();
        }

        public List<InsuranceCaseDTO> GetAllInsuranceCases(int userID)
        {
            var cases = from insuranceCase in db.InsuranceCase
                        join contract in db.Contract on insuranceCase.ContractID equals contract.ContractID
                        join caseType in db.CaseType on insuranceCase.CaseTypeID equals caseType.CaseTypeID
                        where contract.ClientID == userID && insuranceCase.signed == true
                        select new InsuranceCaseDTO
                        {
                            CaseID = insuranceCase.CaseID,
                            ContractID = insuranceCase.ContractID,
                            ContractNumber = contract.Number, 
                            CaseTypeName = caseType.Situation, 
                            Date = insuranceCase.Date,
                            Description = insuranceCase.description.TrimEnd(),
                            Payout = insuranceCase.PayoutAmount
                        };

            return cases.ToList();
        }
        public List<string> GetSignedStatusOptions()
        {
            var signedStatusOptions = new List<string> { "Без фильтра" };

            // Подписан, Не подписан
            signedStatusOptions.AddRange(db.InsuranceCase
                .Select(c => c.signed)
                .Distinct()
                .Where(s => s.HasValue) // чтобы не добавлять null
                .Select(s => s.Value ? "Подписан" : "Не подписан"));

            // "Не определено", если есть записи с null в signed
            if (db.InsuranceCase.Any(c => !c.signed.HasValue))
            {
                signedStatusOptions.Add("Не определено");
            }

            return signedStatusOptions;
        }

        public List<InsuranceCaseInfoDTO> GetAllOfInsuranceCases()
        {
            var cases = from insuranceCase in db.InsuranceCase
                        join contract in db.Contract on insuranceCase.ContractID equals contract.ContractID
                        join client in db.Client on contract.ClientID equals client.ClientID
                        join caseType in db.CaseType on insuranceCase.CaseTypeID equals caseType.CaseTypeID
                        select new InsuranceCaseInfoDTO
                        {
                            CaseID = insuranceCase.CaseID,
                            ContractNumber = contract.Number,
                            ClientName = client.FullName,
                            Date = insuranceCase.Date,
                            CaseTypeName = caseType.Situation,
                            Description = insuranceCase.description.TrimEnd(),
                            Cost = caseType.BaseCost,
                            signed = insuranceCase.signed,
                            Comment = insuranceCase.Comment.TrimEnd(),
                        };

            return cases.ToList();
        }

        public List<InsuranceCaseInfoDTO> GetUnsignedInsuranceCases()
        {
            var cases = from insuranceCase in db.InsuranceCase
                        join contract in db.Contract on insuranceCase.ContractID equals contract.ContractID
                        join client in db.Client on contract.ClientID equals client.ClientID
                        join caseType in db.CaseType on insuranceCase.CaseTypeID equals caseType.CaseTypeID
                        where insuranceCase.signed == null
                        select new InsuranceCaseInfoDTO
                        {
                            CaseID = insuranceCase.CaseID,
                            ContractNumber = contract.Number,
                            ClientName = client.FullName,
                            Date = insuranceCase.Date,
                            CaseTypeName = caseType.Situation,
                            Description = insuranceCase.description.TrimEnd(),
                            Cost = caseType.BaseCost
                        };

            return cases.ToList();
        }

        public List<InsuranceCaseInfoDTO> FilterCases(int? caseNumber, string caseType, string signedStatus)
        {
            var cases = from ic in db.InsuranceCase
                        join contract in db.Contract on ic.ContractID equals contract.ContractID
                        join client in db.Client on contract.ClientID equals client.ClientID
                        join caseTypeInDb in db.CaseType on ic.CaseTypeID equals caseTypeInDb.CaseTypeID
                        where
                            (!caseNumber.HasValue || contract.Number == caseNumber) &&
                            (caseType == "Без фильтра" || caseTypeInDb.Situation == caseType) &&
                            (signedStatus == "Без фильтра" ||
                                (signedStatus == "Подписан" && ic.signed == true) ||
                                (signedStatus == "Не подписан" && ic.signed == false))
                        select new InsuranceCaseInfoDTO
                        {
                            CaseID = ic.CaseID,
                            ContractNumber = contract.Number,
                            ClientName = client.FullName,
                            Date = ic.Date,
                            Description = ic.description,
                            Cost = ic.PayoutAmount,
                            CaseTypeName = caseTypeInDb.Situation
                        };

            return cases.ToList();
        }




        public void SignCase(int caseId, int cost, string comment)
        {
            var insuranceCase = db.InsuranceCase.FirstOrDefault(c => c.CaseID == caseId);
            if (insuranceCase != null)
            {
                insuranceCase.signed = true;
                insuranceCase.PayoutAmount = cost;
                insuranceCase.Comment = comment.TrimEnd();
                

                db.SaveChanges();
            }
        }

        public void UnsignCase(int caseId, string comment)
        {
            var insuranceCase = db.InsuranceCase.FirstOrDefault(c => c.CaseID == caseId);
            if (insuranceCase != null)
            {
                insuranceCase.signed = false;
                insuranceCase.PayoutAmount = 0;
                insuranceCase.Comment = comment.TrimEnd();
                db.SaveChanges();
            }
        }
    }
}
