using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab04.PracticalTasks;

public static class Task4
{
    /*
     * Завдання: Реалізуйте структуру даних SetOfStacks, що імітує реальну ситуацію.
     * Структура має складатися з кількох стеків, новий стек створюється, 
     * щойно розмір попереднього досягнув порогового значення.
     */
    public static void Execute()
    {
        Console.WriteLine("\n--- Завдання 4: SetOfStacks ---");

        SetOfStacks<int> setOfStacks = new SetOfStacks<int>(capacity: 3);
        
        Console.WriteLine("Додаємо числа від 1 до 8 (по 3 числа в стек).");
        for (int i = 1; i <= 8; i++)
        {
            setOfStacks.Push(i);
            Console.WriteLine($"Push({i}), поточна кількість стеків: {setOfStacks.StacksCount}");
        }

        Console.WriteLine("\nПочинаємо виймати елементи (Pop):");
        while (!setOfStacks.IsEmpty)
        {
            int val = setOfStacks.Pop();
            Console.WriteLine($"Pop: {val}, поточна кількість стеків: {setOfStacks.StacksCount}");
        }
    }
}

public class SetOfStacks<T>
{
    private List<Stack<T>> _stacks;
    private readonly int _capacity;

    public SetOfStacks(int capacity)
    {
        if (capacity <= 0) throw new ArgumentOutOfRangeException(nameof(capacity));
        
        _capacity = capacity;
        _stacks = new List<Stack<T>>();
    }

    public int StacksCount => _stacks.Count;
    public bool IsEmpty => _stacks.Count == 0;

    public void Push(T item)
    {
        // Якщо стеків немає, або останній стек вже заповнений, створюємо новий
        if (_stacks.Count == 0 || _stacks.Last().Count == _capacity)
        {
            _stacks.Add(new Stack<T>());
        }

        _stacks.Last().Push(item);
    }

    public T Pop()
    {
        if (IsEmpty)
        {
            throw new InvalidOperationException("SetOfStacks is empty");
        }

        Stack<T> lastStack = _stacks.Last();
        T value = lastStack.Pop();

        // Якщо після вилучення останній стек став порожнім, видаляємо його з колекції
        if (lastStack.Count == 0)
        {
            _stacks.RemoveAt(_stacks.Count - 1);
        }

        return value;
    }
}
