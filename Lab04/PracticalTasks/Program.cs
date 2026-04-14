using System;
using System.Text;

namespace Lab04.PracticalTasks;

public static class Program
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        
        while (true)
        {
            Console.WriteLine("\n===========================================");
            Console.WriteLine("     Практичні завдання (Lab04)");
            Console.WriteLine("===========================================");
            Console.WriteLine("  1 - Завдання 1 (Валідація дужок для виразу)");
            Console.WriteLine("  2 - Завдання 2 (Злиття двох впорядкованих черг)");
            Console.WriteLine("  3 - Завдання 3 (Видимі споруди)");
            Console.WriteLine("  4 - Завдання 4 (Set of Stacks)");
            Console.WriteLine("  0 - Вихід");
            Console.WriteLine("===========================================");
            Console.Write("Ваш вибір: ");

            if (!int.TryParse(Console.ReadLine(), out int choice)) continue;

            switch (choice)
            {
                case 1: Task1.Execute(); break;
                case 2: Task2.Execute(); break;
                case 3: Task3.Execute(); break;
                case 4: Task4.Execute(); break;
                case 0: return;
                default: Console.WriteLine("Невірний вибір."); break;
            }
        }
    }
}
