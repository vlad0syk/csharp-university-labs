using System;
using System.Text;

namespace Lab04;

public static class Program
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("===========================================");
        Console.WriteLine("     Laboratory Work No. 4 (Task 5)");
        Console.WriteLine("     Queue using a Singly Linked List");
        Console.WriteLine("===========================================");

        LinkedListQueue<int> queue = new LinkedListQueue<int>();
        
        Console.WriteLine("Додаємо (Enqueue): 10, 20, 30");
        queue.Enqueue(10);
        queue.Enqueue(20);
        queue.Enqueue(30);

        Console.WriteLine($"Перший елемент (Peek): {queue.Peek()}"); // 10
        Console.WriteLine($"Поточний розмір черги: {queue.Count}"); // 3
        Console.WriteLine($"Вилучаємо (Dequeue): {queue.Dequeue()}"); // 10
        
        Console.WriteLine("Додаємо (Enqueue): 40");
        queue.Enqueue(40);
        
        Console.WriteLine($"Вилучаємо (Dequeue): {queue.Dequeue()}"); // 20
        Console.WriteLine($"Вилучаємо (Dequeue): {queue.Dequeue()}"); // 30
        Console.WriteLine($"Вилучаємо (Dequeue): {queue.Dequeue()}"); // 40
        
        // Відловлюємо помилку порожньої черги
        try
        {
            queue.Dequeue();
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine($"Помилка при видаленні з порожньої черги: {e.Message}");
        }
    }
}

// Вузол зв'язаного списку
public class Node<T>
{
    public T Data { get; set; }
    public Node<T>? Next { get; set; }

    public Node(T data)
    {
        Data = data;
        Next = null;
    }
}

// Клас черги на основі лінійного зв'язаного списку
public class LinkedListQueue<T>
{
    private Node<T>? _head; // _head вказує на початок черги (звідси ми вилучаємо елементи - Dequeue)
    private Node<T>? _tail; // _tail вказує на кінець черги (сюди ми додаємо нові елементи - Enqueue)
    private int _count;      

    public LinkedListQueue()
    {
        _head = null;
        _tail = null;
        _count = 0;
    }

    public int Count => _count;
    public bool IsEmpty => _head == null;

    /// <summary>
    /// Додавання елемента до черги.
    /// Працює за O(1), бо ми маємо пряме посилання на хвіст списку (_tail).
    /// </summary>
    public void Enqueue(T item)
    {
        Node<T> newNode = new Node<T>(item);
        
        // Якщо черга порожня, то об'єкт стає одночасно і головою, і хвостом черги
        if (_tail == null)
        {
            _head = newNode;
            _tail = newNode;
        }
        else
        {
            // Якщо черга не порожня, ми просто "чіпляємо" новий вузол до хвоста
            _tail.Next = newNode;
            // І робимо новий вузол оновленим хвостом черги
            _tail = newNode;
        }
        
        _count++;
    }

    /// <summary>
    /// Вилучення елемента з черги (зняття 1-го в черзі).
    /// Працює за O(1), бо ми видаляємо перший вузол списку (_head), маючи на нього посилання.
    /// </summary>
    public T Dequeue()
    {
        if (IsEmpty)
        {
            throw new InvalidOperationException("Queue is empty");
        }

        // Беремо дані з голови
        T value = _head!.Data;

        // Зміщуємо "голову" черги на наступний елемент
        _head = _head.Next;
        
        // Якщо після видалення голова стала null (тобто черга стала порожньою), 
        // треба також обнулити хвіст
        if (_head == null)
        {
            _tail = null;
        }
        
        _count--;
        
        return value;
    }

    /// <summary>
    /// Перегляд першого елемента черги без вилучення.
    /// Працює за O(1).
    /// </summary>
    public T Peek()
    {
        if (IsEmpty)
        {
            throw new InvalidOperationException("Queue is empty");
        }

        return _head!.Data;
    }
}
