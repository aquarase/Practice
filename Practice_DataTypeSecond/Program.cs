using System.Text;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Введите длину диагонали: ");
        int diagonalLength = int.Parse(Console.ReadLine());
        DrawingDiamond(diagonalLength);
    }


    static void DrawingDiamond(int diagonalLength)
    {
        if ((diagonalLength % 2 == 0) || (diagonalLength < 0) )
        {
            Console.WriteLine("Длины диагоналей должны быть нечётные и положительные");
            return;
        }
        int center = diagonalLength / 2;
        var drawing = new StringBuilder();

        for (int i = 0; i < diagonalLength; i++)
        {
            for (int j = 0; j < diagonalLength; j++)
            {
                if (i == center && j == center)
                {
                    drawing.Append(' ');
                    continue;
                }
                if (Math.Abs(i - center) + Math.Abs(j - center) == center)
                {
                    drawing.Append('X');
                }
                else
                {
                    drawing.Append(' ');
                }
            }
            drawing.AppendLine();
        }
        Console.Write(drawing.ToString());
    }

}