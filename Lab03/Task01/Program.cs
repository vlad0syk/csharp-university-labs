using System.Text;

namespace Lab03;

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
        Console.WriteLine("     Laboratory Work No. 3");
        Console.WriteLine("     STRING SEARCH Algorithms");
        Console.WriteLine("===========================================");
        Console.WriteLine("Performed by: Vlad Sapozhnyk (8)");
        Console.WriteLine("Group: IPZ-13, Subgroup: 6");
        Console.WriteLine("===========================================");
    }

    private static void ChooseMode()
    {
        while (true)
        {
            Console.WriteLine("\n===========================================");
            Console.WriteLine("  Choose task to run:");
            Console.WriteLine("===========================================");
            Console.WriteLine("  1 - Direct substring search");
            Console.WriteLine("  2 - Knuth-Morris-Pratt search");
            Console.WriteLine("  3 - Run both algorithms");
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
                    RunDirectSearch();
                    break;
                case 2:
                    RunKmpSearch();
                    break;
                case 3:
                    RunBothAlgorithms();
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

    private static void RunDirectSearch()
    {
        Console.WriteLine("\n------ Direct search ------");
        var (text, pattern) = ReadSearchInput();

        int index = DirectSearch(text, pattern);
        PrintSearchResult("Direct search", index);
    }

    private static void RunKmpSearch()
    {
        Console.WriteLine("\n------ Knuth-Morris-Pratt search ------");
        var (text, pattern) = ReadSearchInput();

        int[] prefix = BuildPrefixFunction(pattern);
        PrintPrefixFunction(pattern, prefix);

        int index = KnuthMorrisPrattSearch(text, pattern, prefix);
        PrintSearchResult("Knuth-Morris-Pratt search", index);
    }

    private static void RunBothAlgorithms()
    {
        Console.WriteLine("\n------ Direct search + KMP ------");
        var (text, pattern) = ReadSearchInput();

        int directIndex = DirectSearch(text, pattern);
        int[] prefix = BuildPrefixFunction(pattern);
        int kmpIndex = KnuthMorrisPrattSearch(text, pattern, prefix);

        PrintPrefixFunction(pattern, prefix);
        PrintSearchResult("Direct search", directIndex);
        PrintSearchResult("Knuth-Morris-Pratt search", kmpIndex);

        Console.WriteLine(directIndex == kmpIndex
            ? "Both algorithms returned the same result."
            : "The algorithms returned different results.");
    }

    private static (string Text, string Pattern) ReadSearchInput()
    {
        string text = ReadNonEmptyString("Enter your string: ");
        string pattern = ReadNonEmptyString("Enter your substring: ");

        while (pattern.Length > text.Length)
        {
            Console.WriteLine("The substring must not be longer than the string.");
            pattern = ReadNonEmptyString("Enter your substring again: ");
        }

        return (text, pattern);
    }

    private static string ReadNonEmptyString(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? value = Console.ReadLine();

            if (!string.IsNullOrEmpty(value))
            {
                return value;
            }

            Console.WriteLine("Input cannot be empty.");
        }
    }

    private static int DirectSearch(string text, string pattern)
    {
        for (int i = 0; i <= text.Length - pattern.Length; i++)
        {
            int j = 0;

            while (j < pattern.Length && text[i + j] == pattern[j])
            {
                j++;
            }

            if (j == pattern.Length)
            {
                return i;
            }
        }

        return -1;
    }

    private static int[] BuildPrefixFunction(string pattern)
    {
        int[] prefix = new int[pattern.Length];
        int j = 0;

        for (int i = 1; i < pattern.Length; i++)
        {
            while (j > 0 && pattern[i] != pattern[j])
            {
                j = prefix[j - 1];
            }

            if (pattern[i] == pattern[j])
            {
                j++;
            }

            prefix[i] = j;
        }

        return prefix;
    }

    private static int KnuthMorrisPrattSearch(string text, string pattern, int[]? prefix = null)
    {
        prefix ??= BuildPrefixFunction(pattern);

        int textIndex = 0;
        int patternIndex = 0;

        while (textIndex < text.Length)
        {
            if (text[textIndex] == pattern[patternIndex])
            {
                textIndex++;
                patternIndex++;

                if (patternIndex == pattern.Length)
                {
                    return textIndex - patternIndex;
                }
            }
            else if (patternIndex == 0)
            {
                textIndex++;
            }
            else
            {
                patternIndex = prefix[patternIndex - 1];
            }
        }

        return -1;
    }

    private static void PrintSearchResult(string algorithmName, int index)
    {
        if (index >= 0)
        {
            Console.WriteLine($"{algorithmName}: substring found at position {index + 1} (index {index}).");
            return;
        }

        Console.WriteLine($"{algorithmName}: substring not found.");
    }

    private static void PrintPrefixFunction(string pattern, int[] prefix)
    {
        Console.WriteLine("\nPrefix function for the substring:");
        Console.WriteLine($"Pattern: {string.Join(' ', pattern.ToCharArray())}");
        Console.WriteLine($"Prefix : {string.Join(' ', prefix)}");
    }
}
