using System.Drawing;

class Program
{
    private class Rectangle()
    {
        private int _width;
        private int _height;
        public int posX { get; set; }
        public int posY { get; set; }

        public required int Width
        {
            get => _width;
            init
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Ширина не может быть отрицательной");
                }
                _width = value;
            }
        }
        public required int Height
        {
            get => _height;
            init
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Высота не может быть отрицательной");
                }
                _height = value;
            }
        }
        public int Area => Width* Height;
        public int Perimetr => (Width + Height) * 2;
    }

    static void Main(string[] args)
    {
        try
        {   
            Console.WriteLine("Создание прямоугольника");
            var rectangle = new Rectangle { Width = 10, Height = 10 };
            Console.WriteLine($"Его координаты: X = {rectangle.posX}, Y = {rectangle.posY}");
            Console.WriteLine($"Его площадь = {rectangle.Area}, его периметр = {rectangle.Perimetr}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex}");
        }
        try
        {
            Console.WriteLine("Попытка создания прямоугольника с некорректными данными (Отрицательная высота): ");
            var rectangleWithError = new Rectangle { Width = 5, Height = -15 };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }
       
}
