using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab04.PracticalTasks;

public static class Task3
{
    /*
     * Завдання: У першому рядку дано розмір масиву n. Далі йдуть n чисел (висоти споруд).
     * Визначити, скільки таких споруд можна побачити з позиції перед першою спорудою.
     * Вивести висоти цих споруд. (Приклад: 6 \n 8 2 3 11 11 10 -> Бачимо 8, 11).
     */
    public static void Execute()
    {
        Console.WriteLine("\n--- Завдання 3: Видимі споруди ---");

        Console.Write("Введіть розмір масиву n (або натисніть Enter для демо-даних): ");
        string? nStr = Console.ReadLine();

        int[] buildings;
        if (string.IsNullOrWhiteSpace(nStr))
        {
            buildings = new[] { 8, 2, 3, 11, 11, 10 };
            Console.WriteLine($"Демо висоти: {string.Join(" ", buildings)}");
        }
        else
        {
            if (!int.TryParse(nStr, out int _)) return;
            
            Console.Write("Введіть висоти через пробіл: ");
            string? hStr = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(hStr)) return;

            buildings = hStr.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                            .Select(int.Parse)
                            .ToArray();
        }

        List<int> visibleBuildings = GetVisibleBuildings(buildings);

        Console.WriteLine($"\nКількість видимих споруд: {visibleBuildings.Count}");
        Console.WriteLine($"Висоти видимих споруд: {string.Join(" ", visibleBuildings)}");
    }

    private static List<int> GetVisibleBuildings(int[] heights)
    {
        List<int> visible = new List<int>();
        int currentMaxHeight = -1;

        // Споруду видно тільки тоді, коли вона строго вища за ВСІ попередні споруди
        foreach (int h in heights)
        {
            if (h > currentMaxHeight)
            {
                visible.Add(h);
                currentMaxHeight = h;
            }
        }

        return visible;
    }
}
