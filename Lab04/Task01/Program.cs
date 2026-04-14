using System.Text;

namespace Lab04;

public static class Program
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Greeting();
        ChooseMode();
    }

    private static void Greeting()
    {
        Console.WriteLine("===========================================");
        Console.WriteLine("     Laboratory Work No. 4");
        Console.WriteLine("     Deque (Array Implementation)");
        Console.WriteLine("===========================================");
        Console.WriteLine("Performed by: Vlad Sapozhnyk (8)");
        Console.WriteLine("Group: IPZ-13, Subgroup: 6");
        Console.WriteLine("===========================================");
    }

    private static void ChooseMode()
    {
        Deque deque = new(capacity: 10);

        while (true)
        {
            Console.WriteLine("\n===========================================");
            Console.WriteLine("  Deque menu:");
            Console.WriteLine("===========================================");
            Console.WriteLine("  1 - Push front");
            Console.WriteLine("  2 - Push back");
            Console.WriteLine("  3 - Pop front");
            Console.WriteLine("  4 - Pop back");
            Console.WriteLine("  5 - Print deque");
            Console.WriteLine("  0 - Exit");
            Console.WriteLine("===========================================");
            Console.Write("Your choice: ");

            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("\nIncorrect input. Please enter a number.");
                continue;
            }

            switch (choice)
            {
                case 1:
                    int frontValue = ReadInt("Enter value to insert at front: ");
                    if (deque.PushFront(frontValue))
                    {
                        Console.WriteLine("Inserted to front.");
                    }
                    else
                    {
                        Console.WriteLine("Deque is full.");
                    }

                    break;

                case 2:
                    int backValue = ReadInt("Enter value to insert at back: ");
                    if (deque.PushBack(backValue))
                    {
                        Console.WriteLine("Inserted to back.");
                    }
                    else
                    {
                        Console.WriteLine("Deque is full.");
                    }

                    break;

                case 3:
                    if (deque.PopFront(out int removedFront))
                    {
                        Console.WriteLine($"Removed from front: {removedFront}");
                    }
                    else
                    {
                        Console.WriteLine("Deque is empty.");
                    }

                    break;

                case 4:
                    if (deque.PopBack(out int removedBack))
                    {
                        Console.WriteLine($"Removed from back: {removedBack}");
                    }
                    else
                    {
                        Console.WriteLine("Deque is empty.");
                    }

                    break;

                case 5:
                    PrintDeque(deque);
                    break;

                case 0:
                    Console.WriteLine("\nGoodbye!");
                    return;

                default:
                    Console.WriteLine("\nInvalid choice.");
                    break;
            }
        }
    }

    private static int ReadInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine(), out int value))
            {
                return value;
            }

            Console.WriteLine("Incorrect input. Please enter an integer.");
        }
    }

    private static void PrintDeque(Deque deque)
    {
        Console.WriteLine($"Deque size: {deque.Count}/{deque.Capacity}");

        if (deque.IsEmpty)
        {
            Console.WriteLine("Deque: [empty]");
            return;
        }

        Console.Write("Deque: ");
        foreach (int value in deque.AsEnumerable())
        {
            Console.Write($"{value} ");
        }

        Console.WriteLine();
    }
}

public sealed class Deque
{
    private readonly int[] _buffer;
    private int _front;
    private int _back;

    public Deque(int capacity)
    {
        if (capacity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be greater than zero.");
        }

        _buffer = new int[capacity];
        _front = 0;
        _back = capacity - 1;
        Count = 0;
    }

    public int Capacity => _buffer.Length;

    public int Count { get; private set; }

    public bool IsEmpty => Count == 0;

    public bool IsFull => Count == Capacity;

    // O(1): move front index in circular buffer and write value.
    public bool PushFront(int value)
    {
        if (IsFull)
        {
            return false;
        }

        _front = PreviousIndex(_front);
        _buffer[_front] = value;
        Count++;
        return true;
    }

    // O(1): move back index in circular buffer and write value.
    public bool PushBack(int value)
    {
        if (IsFull)
        {
            return false;
        }

        _back = NextIndex(_back);
        _buffer[_back] = value;
        Count++;
        return true;
    }

    // O(1): read front value and move front index forward.
    public bool PopFront(out int value)
    {
        if (IsEmpty)
        {
            value = default;
            return false;
        }

        value = _buffer[_front];
        _front = NextIndex(_front);
        Count--;
        return true;
    }

    // O(1): read back value and move back index backward.
    public bool PopBack(out int value)
    {
        if (IsEmpty)
        {
            value = default;
            return false;
        }

        value = _buffer[_back];
        _back = PreviousIndex(_back);
        Count--;
        return true;
    }

    public IEnumerable<int> AsEnumerable()
    {
        int index = _front;

        for (int i = 0; i < Count; i++)
        {
            yield return _buffer[index];
            index = NextIndex(index);
        }
    }

    private int NextIndex(int index) => (index + 1) % Capacity;

    private int PreviousIndex(int index) => (index - 1 + Capacity) % Capacity;
}
