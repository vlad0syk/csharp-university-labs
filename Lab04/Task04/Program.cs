using System;
using System.Text;

namespace Lab04;

public static class Program
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("===========================================");
        Console.WriteLine("     Laboratory Work No. 4 (Task 4)");
        Console.WriteLine("     Stack using a Singly Linked List");
        Console.WriteLine("===========================================");

        LinkedListStack<int> stack = new LinkedListStack<int>();
        
        Console.WriteLine("Додаємо (Push): 10, 20, 30");
        stack.Push(10);
        stack.Push(20);
        stack.Push(30);

        Console.WriteLine($"Вершина (Peek): {stack.Peek()}"); // 30
        Console.WriteLine($"Поточний розмір стеку: {stack.Count}"); // 3
        Console.WriteLine($"Видаляємо (Pop): {stack.Pop()}"); // 30
        
        Console.WriteLine("Додаємо (Push): 40");
        stack.Push(40);
        
        Console.WriteLine($"Видаляємо (Pop): {stack.Pop()}"); // 40
        Console.WriteLine($"Видаляємо (Pop): {stack.Pop()}"); // 20
        Console.WriteLine($"Видаляємо (Pop): {stack.Pop()}"); // 10
        
        // Відловлюємо помилку порожнього стеку
        try
        {
            stack.Pop();
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine($"Помилка при видаленні з порожнього стеку: {e.Message}");
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

// Клас стеку на основі лінійного зв'язаного списку
public class LinkedListStack<T>
{
    private Node<T>? _head; // _head грає роль "вершини" стеку (Top)
    private int _count;

    public LinkedListStack()
    {
        _head = null;
        _count = 0;
    }

    public int Count => _count;
    public bool IsEmpty => _head == null;

    /// <summary>
    /// Вставка елемента у стек.
    /// Працює за O(1), оскільки ми просто додаємо завжди на початок списку.
    /// </summary>
    public void Push(T item)
    {
        Node<T> newNode = new Node<T>(item);
        
        // Вказуємо для нового елемента, що наступним за ним іде поточна вершина
        newNode.Next = _head;
        
        // Робимо новий елемент новою вершиною стеку
        _head = newNode;
        
        _count++;
    }

    /// <summary>
    /// Видалення верхнього елемента зі стеку.
    /// Працює за O(1), бо ми просто відрізаємо "голову" і робимо "головою" наступний елемент.
    /// </summary>
    public T Pop()
    {
        if (IsEmpty)
        {
            throw new InvalidOperationException("Stack is empty");
        }

        // Зберігаємо значення, щоб його повернути
        T value = _head!.Data;

        // Зміщуємо "голову" стеку на наступний вузол
        _head = _head.Next;
        
        _count--;
        
        return value;
    }

    /// <summary>
    /// Перегляд верхнього елемента без його видалення.
    /// Працює за O(1).
    /// </summary>
    public T Peek()
    {
        if (IsEmpty)
        {
            throw new InvalidOperationException("Stack is empty");
        }

        return _head!.Data;
    }
}
