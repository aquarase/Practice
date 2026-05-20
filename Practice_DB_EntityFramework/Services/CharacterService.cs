using Microsoft.EntityFrameworkCore;
using Practice_DB_EntityFramework.Data;
using Practice_DB_EntityFramework.Models;

namespace Practice_DB_EntityFramework.Services
{
    public class CharacterService
    {
        private readonly AppDbContext _context;

        public CharacterService(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateCharacterAsync()
        {
            Console.WriteLine("\n=== Создание нового персонажа ===");

            var character = new Character();

            Console.Write("Введите имя персонажа (до 30 символов): ");
            character.CharacterName = Console.ReadLine();

            Console.Write("Введите уровень персонажа: ");
            if (int.TryParse(Console.ReadLine(), out int level))
                character.CharacterLevel = level;
            else
                character.CharacterLevel = null;

            Console.Write("Персонаж жив? (y/n): ");
            var isAlive = Console.ReadLine()?.ToLower();
            character.IsAlive = isAlive == "y" || isAlive == "yes";

            try
            {
                _context.Characters.Add(character);
                await _context.SaveChangesAsync();
                Console.WriteLine($"✓ Персонаж успешно создан с ID: {character.CharacterID}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Ошибка при создании: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Внутренняя ошибка: {ex.InnerException.Message}");
                }
            }
        }

        public async Task SearchCharactersByNameAsync()
        {
            Console.Write("\nВведите имя для поиска: ");
            var searchTerm = Console.ReadLine() ?? string.Empty;

            var characters = await _context.Characters
                .Where(c => c.CharacterName != null && c.CharacterName.Contains(searchTerm))
                .OrderBy(c => c.CharacterName)
                .ToListAsync();

            if (!characters.Any())
            {
                Console.WriteLine($"Персонажи с именем '{searchTerm}' не найдены.");
                return;
            }

            Console.WriteLine($"\n=== Результаты поиска '{searchTerm}' ===");
            Console.WriteLine("\n{0,-5} {1,-20} {2,-10} {3,-10}",
                "ID", "Имя", "Уровень", "Статус", "GuildID");
            Console.WriteLine(new string('-', 45));

            foreach (var character in characters)
            {
                string status = character.IsAlive.HasValue && character.IsAlive.Value ? "Жив" : "Мертв";
                string level = character.CharacterLevel.HasValue ? character.CharacterLevel.Value.ToString() : "N/A";

                Console.WriteLine("{0,-5} {1,-20} {2,-10} {3,-10}",
                    character.CharacterID,
                    character.CharacterName ?? "N/A",
                    level,
                    status);
            }
            Console.WriteLine($"\nНайдено персонажей: {characters.Count}");
        }

        public async Task UpdateCharacterLevelAsync()
        {
            Console.Write("\nВведите ID персонажа для обновления уровня: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Неверный ID!");
                return;
            }

            var character = await _context.Characters.FindAsync(id);
            if (character == null)
            {
                Console.WriteLine($"Персонаж с ID {id} не найден.");
                return;
            }

            string currentLevel = character.CharacterLevel.HasValue ? character.CharacterLevel.Value.ToString() : "не задан";
            Console.WriteLine($"\nТекущий уровень персонажа '{character.CharacterName}': {currentLevel}");
            Console.Write("Введите новый уровень: ");

            if (int.TryParse(Console.ReadLine(), out int newLevel))
            {
                character.CharacterLevel = newLevel;
            }
            else
            {
                Console.WriteLine("Неверный формат уровня! Уровень не изменен.");
                return;
            }

            try
            {
                await _context.SaveChangesAsync();
                Console.WriteLine($"✓ Уровень персонажа '{character.CharacterName}' успешно обновлен на {newLevel}!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Ошибка при обновлении: {ex.Message}");
            }
        }

        public async Task DeleteCharacterAsync()
        {
            Console.Write("\nВведите ID персонажа для удаления: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Неверный ID!");
                return;
            }

            var character = await _context.Characters.FindAsync(id);

            if (character == null)
            {
                Console.WriteLine($"Персонаж с ID {id} не найден.");
                return;
            }

            string level = character.CharacterLevel.HasValue ? character.CharacterLevel.Value.ToString() : "N/A";
            string status = character.IsAlive.HasValue && character.IsAlive.Value ? "Жив" : "Мертв";

            Console.WriteLine($"\nВы уверены, что хотите удалить персонажа '{character.CharacterName}'?");
            Console.WriteLine($"ID: {character.CharacterID}, Уровень: {level}, Статус: {status}");
            Console.Write("Подтвердите удаление (y/n): ");

            if (Console.ReadLine()?.ToLower() != "y")
            {
                Console.WriteLine("Удаление отменено.");
                return;
            }

            try
            {
                _context.Characters.Remove(character);
                await _context.SaveChangesAsync();
                Console.WriteLine("✓ Персонаж успешно удален!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Ошибка при удалении: {ex.Message}");
            }
        }
    }
}