using System;
using System.Collections.Generic;

namespace Lab04.PracticalTasks;

public static class Task2
{
    /*
     * Завдання: Дано дві непорожні черги. Елементи кожної з черг упорядковані за зростанням. 
     * Об'єднати черги в одну із збереженням упорядкованості елементів.
     */
    public static void Execute()
    {
        Console.WriteLine("\n--- Завдання 2: Злиття впорядкованих черг ---");

        Queue<int> q1 = new Queue<int>(new[] { 1, 4, 7, 15 });
        Queue<int> q2 = new Queue<int>(new[] { 2, 3, 9, 20, 25 });

        Console.WriteLine("Черга 1: " + string.Join(", ", q1));
        Console.WriteLine("Черга 2: " + string.Join(", ", q2));

        Queue<int> merged = MergeQueues(q1, q2);

        Console.WriteLine("Об'єднана черга: " + string.Join(", ", merged));
    }

    private static Queue<int> MergeQueues(Queue<int> q1, Queue<int> q2)
    {
        Queue<int> result = new Queue<int>();

        // Доки в обох чергах є елементи, порівнюємо їх верхівки
        while (q1.Count > 0 && q2.Count > 0)
        {
            if (q1.Peek() <= q2.Peek())
            {
                result.Enqueue(q1.Dequeue());
            }
            else
            {
                result.Enqueue(q2.Dequeue());
            }
        }

        // Якщо в одній з черг залишились елементи, просто докидаємо їх у хвіст результату
        while (q1.Count > 0)
        {
            result.Enqueue(q1.Dequeue());
        }

        while (q2.Count > 0)
        {
            result.Enqueue(q2.Dequeue());
        }

        return result;
    }
}
