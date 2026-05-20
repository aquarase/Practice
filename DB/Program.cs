using RpgApp.Database;
using RpgApp.Models;
using System.Configuration;
class Program
{
    static async Task Main(string[] args)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        var connectionManager = new ConnectionManager(connectionString);
        var commandExecutor = new CommandExecutor(connectionManager);
        connectionManager.PrintConnectionStatistics();

        Console.WriteLine("\nSelect");
        Console.WriteLine("Введите имя персонажа");
        var heroName = Console.ReadLine();
        await Select(commandExecutor, heroName);

        Console.WriteLine("\nCreate");
        await Create(commandExecutor);

        Console.WriteLine("\nUpdate");
        Console.WriteLine("Введите имя персонажа для повышения уровня");
        heroName = Console.ReadLine();
        Console.WriteLine("Введите на сколько хотите повысить уровень");
        int lvl = int.Parse(Console.ReadLine());
        Console.WriteLine("\nИзначальные данные");
        await Select(commandExecutor, heroName);
        await Update(commandExecutor, heroName, lvl);
        Console.WriteLine("\nНовые данные");
        await Select(commandExecutor, heroName);

        Console.WriteLine("\nDelete");
        Console.WriteLine("Введите имя персонажа для удаления");
        heroName = Console.ReadLine();
        await Delete(commandExecutor, heroName);
    }

    
    static public async Task Create (CommandExecutor commandExecutor)
    {
        Console.WriteLine("Введите Id персонажа: ");
        int Id = int.Parse(Console.ReadLine());
        Console.WriteLine("Введите имя персонажа: ");
        string Name = Console.ReadLine();
        Console.WriteLine("Введите LVL персонажа: ");
        int Lvl = int.Parse(Console.ReadLine());
        Console.WriteLine("Жив-ли персонаж персонажа (1 - Да, 0 - Нет): ");
        int isAlive = int.Parse(Console.ReadLine());
        bool isAliveBool = (isAlive == 1);


        bool isCreated = await commandExecutor.CreateCharacterAsync(Id, Name, Lvl, isAliveBool);
        Console.WriteLine(isCreated ? "Результат: Успешно создан в БД!" : "Результат: Ошибка создания.");

        if (isCreated == true)
        {
            Character hero = await commandExecutor.GetCharacterByNameAsync(Name);
            Console.WriteLine($"Ваш персонаж - {hero.CharacterName}");
            Console.WriteLine($"ID: {hero.CharacterId}");
            Console.WriteLine($"Уровень: {hero.CharacterLevel}");
            Console.WriteLine($"Статус: {(hero.IsAlive ? "Жив" : "Мертв")}");
        }
    }
    static public async Task Select(CommandExecutor commandExecutor, string heroName)
    {
        Character hero = await commandExecutor.GetCharacterByNameAsync(heroName);
        if (hero != null)
        {
            Console.WriteLine($"\n[Успешно найдено]:");
            Console.WriteLine($" - ID: {hero.CharacterId}");
            Console.WriteLine($" - Имя: {hero.CharacterName}");
            Console.WriteLine($" - Уровень: {hero.CharacterLevel}");
            Console.WriteLine($" - Статус: {(hero.IsAlive ? "Жив" : "Мертв")}");
        }
        else
        {
            Console.WriteLine("\n[Результат]: Персонаж с таким именем не найден.");
        }
    }

    static public async Task Update(CommandExecutor commandExecutor, string heroName, int lvlUp)
    {
        Character hero = await commandExecutor.GetCharacterByNameAsync(heroName);
        if (hero != null)
        {
            bool isUpdated = await commandExecutor.AddCharacterLevelsAsync(heroName, lvlUp);
            Console.WriteLine(isUpdated ? "Результат: Уровень успешно обновлен!" : "Результат: Персонаж не найден.");
        }
        else
        {
            Console.WriteLine("Герой не найден");
        }
    }

    static public async Task Delete (CommandExecutor commandExecutor, string Name)
    {
        bool isDeleted = await commandExecutor.DeleteCharacterByNameAsync(Name);
        Console.WriteLine(isDeleted ? "Результат: Персонаж удален из базы данных." : "Результат: ID не найден.");
    }
}
