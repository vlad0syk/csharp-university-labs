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
        Console.WriteLine("     Laboratory Work No. 4 (Task 2)");
        Console.WriteLine("     Queue using Two Stacks");
        Console.WriteLine("===========================================");

        QueueTwoStacks<int> queue = new QueueTwoStacks<int>();
        
        Console.WriteLine("Додаємо: 10, 20, 30");
        queue.Enqueue(10);
        queue.Enqueue(20);
        queue.Enqueue(30);

        Console.WriteLine($"Видаляємо: {queue.Dequeue()}"); // 10
        
        Console.WriteLine("Додаємо: 40");
        queue.Enqueue(40);
        
        Console.WriteLine($"Видаляємо: {queue.Dequeue()}"); // 20
        Console.WriteLine($"Витягуємо без видалення (Peek): {queue.Peek()}"); // 30
        Console.WriteLine($"Видаляємо: {queue.Dequeue()}"); // 30
        Console.WriteLine($"Видаляємо: {queue.Dequeue()}"); // 40
    }
}

public class QueueTwoStacks<T>
{
    // Стек для вхідних даних
    private readonly Stack<T> _inStack;
    
    // Стек для вихідних даних (де перші додані елементи стають на вершині)
    private readonly Stack<T> _outStack;

    public QueueTwoStacks()
    {
        _inStack = new Stack<T>();
        _outStack = new Stack<T>();
    }

    public int Count => _inStack.Count + _outStack.Count;
    public bool IsEmpty => Count == 0;

    /// <summary>
    /// Додавання елемента до черги (натискання у вхідний стек).
    /// </summary>
    public void Enqueue(T item)
    {
        _inStack.Push(item);
    }

    /// <summary>
    /// Видалення першого доданого елемента з черги.
    /// </summary>
    public T Dequeue()
    {
        if (IsEmpty)
        {
            throw new InvalidOperationException("Queue is empty");
        }

        MoveInToOutIfNeeded();
        return _outStack.Pop();
    }

    /// <summary>
    /// Перегляд першого доданого елемента без видалення.
    /// </summary>
    public T Peek()
    {
        if (IsEmpty)
        {
            throw new InvalidOperationException("Queue is empty");
        }

        MoveInToOutIfNeeded();
        return _outStack.Peek();
    }

    /// <summary>
    /// Якщо вихідний стек порожній, перекидаємо всі елементи з вхідного стеку до вихідного.
    /// Таким чином, найстаріші елементи опиняться на вершині вихідного стеку.
    /// </summary>
    private void MoveInToOutIfNeeded()
    {
        if (_outStack.Count == 0)
        {
            while (_inStack.Count > 0)
            {
                _outStack.Push(_inStack.Pop());
            }
        }
    }
}
