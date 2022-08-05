using Loan_Calculater.Models;

namespace Loan_Calculater.Helpers
{
    public class LoanHelper
    {
        public Loan GetPayments(Loan loan)
        {
            //Caluculate Monthly Payment
            loan.Payment = CalcPayment(loan.Amount, loan.Rate, loan.Term);

            //Create a loop from 1 to the term
            var balance = loan.Amount;
            var totalInterest = 0.0m;
            var monthlyInterest = 0.0m;
            var monthlyPrincipal = 0.0m;
            var monthlyRate = MonthlyCalc(loan.Rate);


            //loop over each month until term is reached
            for (int month = 1; month <= loan.Term; month++)
            {
                //Calculate a payment schedule
                monthlyInterest = CalcMonthlyInterest(balance, monthlyRate);
                totalInterest += monthlyInterest;
                monthlyPrincipal = loan.Payment - monthlyInterest;
                balance -= monthlyPrincipal;

                //push the payments into the loan
                LoanPayment loanPayment = new();

                loanPayment.Month = month;
                loanPayment.Payment = loan.Payment;
                loanPayment.MonthlyPrincipal = monthlyPrincipal;
                loanPayment.MonthlyInterest = monthlyInterest;
                loanPayment.TotalInterest = totalInterest;
                loanPayment.Balance = balance;


                loan.Payments.Add(loanPayment);

            }


            loan.TotalInterest = totalInterest;
            loan.TotalCost = loan.Amount + totalInterest;



            
            
            
           

            //return the loan to the view

            return loan;
        }

        private decimal CalcPayment(decimal amount, decimal rate, int term)
        {
          
            var monthlyrate = MonthlyCalc(rate);
            var rateD = Convert.ToDouble(monthlyrate);
            var amountD = Convert.ToDouble(amount);

            var paymentD = (amountD * rateD) / (1 - Math.Pow(1 + rateD, -term));

            return Convert.ToDecimal(paymentD);
        }

        private decimal MonthlyCalc(decimal rate)
        {
            return rate / 1200;
        }

        private decimal CalcMonthlyInterest(decimal balance, decimal monthlyRate)
        {
            return balance * monthlyRate;
        }

    }
}
