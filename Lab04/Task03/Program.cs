using System;
using System.Collections.Generic;
using System.Text;

namespace Lab04;

public static class Program
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("===========================================");
        Console.WriteLine("     Laboratory Work No. 4 (Task 3)");
        Console.WriteLine("     Stack using Two Queues");
        Console.WriteLine("===========================================");

        StackTwoQueues<int> stack = new StackTwoQueues<int>();
        
        Console.WriteLine("Додаємо: 10, 20, 30");
        stack.Push(10);
        stack.Push(20);
        stack.Push(30);

        Console.WriteLine($"Видаляємо (Pop): {stack.Pop()}"); // 30 (LIFO - останній прийшов, перший вийшов)
        
        Console.WriteLine("Додаємо: 40");
        stack.Push(40);
        
        Console.WriteLine($"Вершина (Peek): {stack.Peek()}"); // 40
        Console.WriteLine($"Видаляємо: {stack.Pop()}"); // 40
        Console.WriteLine($"Видаляємо: {stack.Pop()}"); // 20
        Console.WriteLine($"Видаляємо: {stack.Pop()}"); // 10
    }
}

public class StackTwoQueues<T>
{
    private Queue<T> _q1;
    private Queue<T> _q2;

    public StackTwoQueues()
    {
        _q1 = new Queue<T>();
        _q2 = new Queue<T>();
    }

    public int Count => _q1.Count;
    public bool IsEmpty => Count == 0;

    /// <summary>
    /// Вставка елемента у стек.
    /// Працює за O(N), де N - кількість поточних елементів.
    /// </summary>
    public void Push(T item)
    {
        // 1. Спочатку додаємо новий елемент у порожню чергу _q2
        _q2.Enqueue(item);

        // 2. Перекидаємо всі елементи з _q1 до _q2.
        // Завдяки цьому новий елемент виявляється "на початку" черги _q2.
        while (_q1.Count > 0)
        {
            _q2.Enqueue(_q1.Dequeue());
        }

        // 3. Міняємо черги місцями.
        // Тепер _q1 містить всі елементи, останні додані елементи знаходяться попереду черги.
        // _q2 знову стає порожньою.
        (_q1, _q2) = (_q2, _q1);
    }

    /// <summary>
    /// Видалення верхнього елемента зі стеку.
    /// Працює зі складністю O(1).
    /// </summary>
    public T Pop()
    {
        if (IsEmpty)
        {
            throw new InvalidOperationException("Stack is empty");
        }

        // Оскільки елементи в _q1 відсортовані як у стеку (нові спереду),
        // ми можемо просто видалити перший елемент з черги.
        return _q1.Dequeue();
    }

    /// <summary>
    /// Перегляд верхнього елемента без видалення.
    /// Працює зі складністю O(1).
    /// </summary>
    public T Peek()
    {
        if (IsEmpty)
        {
            throw new InvalidOperationException("Stack is empty");
        }

        return _q1.Peek();
    }
}
