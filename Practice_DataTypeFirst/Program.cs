using System.Text;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Введите размер депозита:");
        int deposit = int.Parse(Console.ReadLine());
        Console.WriteLine("Введите количество лет:");
        int years = int.Parse(Console.ReadLine());
        Console.WriteLine("Введите процентную ставку:");
        int interestRate = int.Parse(Console.ReadLine());
        Console.WriteLine(СalculationOfCompoundInterest(deposit, years, interestRate));
    }


    static string СalculationOfCompoundInterest(int initial_deposit, int years, int interest_rate)
    {

        if ((initial_deposit <= 0) || (years <= 0) || (interest_rate <= 0))
        {
            return "Ошибка: Переданы неверные данные для расчета!";
        }

        double deposit = initial_deposit;
        double currentRate = interest_rate / 100.0;
        var CalculationInfo = new StringBuilder();
        for (int i = 1; i < years+1; i++)
        {
            deposit = deposit + (deposit * currentRate);
            CalculationInfo.AppendLine($"Год {i}: {deposit:F2} руб.");

        }

        return CalculationInfo.ToString();
    }

}
