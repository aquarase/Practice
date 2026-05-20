using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Practice_DB_EntityFramework.Data;
using Practice_DB_EntityFramework.Services;

namespace Practice_DB_EntityFramework
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var services = new ServiceCollection();
            Console.WriteLine("Введите строку подключения к БД: ");
            string connectionString = Console.ReadLine();

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddScoped<CharacterService>();

            var serviceProvider = services.BuildServiceProvider();

            await RunMainMenuAsync(serviceProvider);
        }

        static async Task RunMainMenuAsync(ServiceProvider serviceProvider)
        {
            while (true)
            {
                Console.WriteLine("1. Создать нового персонажа");
                Console.WriteLine("2. Найти персонажа по имени");
                Console.WriteLine("3. Обновить уровень персонажа");
                Console.WriteLine("4. Удалить персонажа");
                Console.WriteLine("0. Выход\n");
                Console.Write("Выберите действие: ");

                var choice = Console.ReadLine();

                using (var scope = serviceProvider.CreateScope())
                {
                    var characterService = scope.ServiceProvider.GetRequiredService<CharacterService>();

                    try
                    {
                        switch (choice)
                        {
                            case "1":
                                await characterService.CreateCharacterAsync();
                                break;
                            case "2":
                                await characterService.SearchCharactersByNameAsync();
                                break;
                            case "3":
                                await characterService.UpdateCharacterLevelAsync();
                                break;
                            case "4":
                                await characterService.DeleteCharacterAsync();
                                break;
                            case "0":
                                return;
                            default:
                                Console.WriteLine("\nНеверный выбор");
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"\nОшибка: {ex.Message}");
                    }
                }

                Console.ReadKey();
            }
        }
    }
}