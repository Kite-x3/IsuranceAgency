using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace BLL.Services
{
    public class InsuranceProgrammContractService
    {
        private const int BaseCost = 1000;
        InsuranceDBEntities1 db;
        public InsuranceProgrammContractService()
        {
            db = new InsuranceDBEntities1();

        }
        public int CalculateLifeContractCost(int insuranceProgramId, int x, float y, float z)
        {
            var insuranceProgram = db.InsuranceProgram.FirstOrDefault(p => p.ProgramID == insuranceProgramId);
            var formula = insuranceProgram.CostFormula;
            float xMultiplier = ParseMultiplier(formula, 'x');
            float yMultiplier = ParseMultiplier(formula, 'y');
            float zMultiplier = ParseMultiplier(formula, 'z');

            int calculatedCost = BaseCost + (int)(x * xMultiplier) + (int)(y * yMultiplier) + (int)(z * zMultiplier);

            return calculatedCost;

        }
        public int CalculatePropertyContractCost(int insuranceProgramId, float x, float y)
        {
            var insuranceProgram = db.InsuranceProgram.FirstOrDefault(p => p.ProgramID == insuranceProgramId);
            var formula = insuranceProgram.CostFormula;

            float xMultiplier = ParseMultiplier(formula, 'x');
            float yMultiplier = ParseMultiplier(formula, 'y');

            int calculatedCost = BaseCost + (int)(x * xMultiplier) + (int)(y * yMultiplier);

            return calculatedCost;
        }

        private float ParseMultiplier(string formula, char variable)
        {
            int variableIndex = formula.IndexOf(variable);
            if (variableIndex == -1)
                return 0;

            int startIndex = variableIndex - 1;
            while (startIndex >= 0 && (char.IsDigit(formula[startIndex]) || formula[startIndex] == '.' || formula[startIndex] == '-'))
            {
                startIndex--;
            }

            startIndex++;

            int endIndex = variableIndex + 1;
            while (endIndex < formula.Length && !char.IsLetter(formula[endIndex]))
            {
                endIndex++;
            }

            string multiplierStr = formula.Substring(startIndex, variableIndex - startIndex);

            return float.TryParse(multiplierStr, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out float multiplier) ? multiplier : 0;
        }



    }
}
