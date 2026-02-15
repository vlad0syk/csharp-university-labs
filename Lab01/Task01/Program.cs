using System.Diagnostics;
using System.Text;

namespace Lab01; 

public class Program
{
    private static string[] algorithmsList = 
    { 
        "Linear search (brute force)",
        "Linear search with a barrier (sentinel)",
        "Binary search",
        "Binary search using the golden ratio rule"
    };
    
    private static int[] dataSizes = { 10, 100, 1000, 10000 };
    private static int dataSize = 10;
    private static int[] elements = [];
    
    private static Dictionary<int, int[]> generatedArrays = new Dictionary<int, int[]>();
    
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Greeting();
        ChooseMode();
    }

    private static void ChooseMode()
    {
        Console.WriteLine("\n===========================================");
        Console.WriteLine("  Choose mode:");
        Console.WriteLine("===========================================");
        Console.WriteLine("  1 - Manual search (choose algorithm)");
        Console.WriteLine("  2 - Run all algorithms comparison");
        Console.WriteLine("  0 - Exit");
        Console.WriteLine("===========================================");
        Console.Write("Your choice: ");

        if (!int.TryParse(Console.ReadLine(), out int choice))
        {
            Console.WriteLine("\n❌ Incorrect data. Please try again!");
            ChooseMode();
            return;
        }

        switch (choice)
        {
            case 1:
                ManualMode();
                break;
            case 2:
                ComparisonMode();
                break;
            case 0:
                Console.WriteLine("\n👋 Goodbye!");
                break;
            default:
                Console.WriteLine("\n❌ Invalid choice.");
                ChooseMode();
                break;
        }
    }

    private static void ManualMode()
    {
        ChooseDataSize();
        FillArray();
        PrintArray();
        Console.WriteLine();
        PrintAlgorithmsList();
        ChooseAlgorithm();
    }

    private static void ComparisonMode()
    {
        Console.Write("\n🔍 Enter value to search (0-100): ");
        if (!int.TryParse(Console.ReadLine(), out int searchedElement))
        {
            Console.WriteLine("❌ Incorrect data.");
            return;
        }

        Console.WriteLine("\n===========================================");
        Console.WriteLine("  Generating test arrays...");
        Console.WriteLine("===========================================\n");

        generatedArrays.Clear();
        foreach (int size in dataSizes)
        {
            Random random = new Random(8);
            int[] array = new int[size];
            for (int i = 0; i < size; i++)
            {
                array[i] = random.Next(101);
            }
            generatedArrays[size] = array;
            Console.WriteLine($"✅ Generated array of size {size}");
        }

        Console.WriteLine("\n===========================================");
        Console.WriteLine("  Running all algorithms comparison...");
        Console.WriteLine("===========================================\n");

        var tasks = new List<Task>();
        var results = new Dictionary<string, string>();
        var lockObj = new object();

        foreach (int size in dataSizes)
        {
            foreach (int algoIndex in new[] { 1, 2, 3, 4 })
            {
                int currentSize = size;
                int currentAlgo = algoIndex;
                
                var task = Task.Run(() =>
                {
                    string result = RunAlgorithmTest(currentAlgo, currentSize, searchedElement);
                    lock (lockObj)
                    {
                        results[$"Size {currentSize} - Algo {currentAlgo}"] = result;
                    }
                });
                
                tasks.Add(task);
            }
        }

        Console.Write("⏳ Running tests");
        while (!Task.WaitAll(tasks.ToArray(), 100))
        {
            Console.Write(".");
        }
        Console.WriteLine(" ✅ Done!\n");

        PrintGeneratedArrays(searchedElement);
        
        PrintComparisonResults(results, searchedElement);

        Console.WriteLine("\n💾 Do you want to save the report to a file? (y/n): ");
        if (Console.ReadLine()?.ToLower() == "y")
        {
            SaveReportToFile(results, searchedElement);
        }

        Console.WriteLine("\nPress any key to return to menu...");
        Console.ReadKey();
        ChooseMode();
    }

    private static void PrintGeneratedArrays(int searchedElement)
    {
        Console.WriteLine("📊 GENERATED TEST ARRAYS");
        Console.WriteLine("=".PadRight(100, '='));
        Console.WriteLine($"🔍 Searching for value: {searchedElement}\n");

        foreach (var kvp in generatedArrays.OrderBy(x => x.Key))
        {
            int size = kvp.Key;
            int[] array = kvp.Value;
            
            Console.WriteLine($"📦 Array size: {size} elements");
            Console.Write("   Original: ");
            
            int displayLimit = Math.Min(size, 30);
            for (int i = 0; i < displayLimit; i++)
            {
                if (array[i] == searchedElement)
                    Console.Write($"[{array[i]}] ");
                else
                    Console.Write($"{array[i]} ");
            }
            
            if (size > 30)
                Console.Write($"... (showing first 30 of {size})");
            
            bool found = Array.IndexOf(array, searchedElement) != -1;
            Console.WriteLine($"\n   Element {searchedElement} is {(found ? "✅ PRESENT" : "❌ NOT PRESENT")} in array");
            
            if (size <= 100)
            {
                int[] sorted = (int[])array.Clone();
                Array.Sort(sorted);
                Console.Write("   Sorted:   ");
                for (int i = 0; i < Math.Min(size, 30); i++)
                {
                    if (sorted[i] == searchedElement)
                        Console.Write($"[{sorted[i]}] ");
                    else
                        Console.Write($"{sorted[i]} ");
                }
                if (size > 30)
                    Console.Write($"... (showing first 30 of {size})");
                Console.WriteLine();
            }
            
            Console.WriteLine();
        }
        
        Console.WriteLine("=".PadRight(100, '='));
        Console.WriteLine("Note: Elements in [brackets] are the searched value\n");
    }

    private static string RunAlgorithmTest(int algoIndex, int size, int searchedElement)
    {
        int[] testArray = generatedArrays[size];

        int[] workingArray = testArray;
        if (algoIndex >= 3)
        {
            workingArray = (int[])testArray.Clone();
            Array.Sort(workingArray);
        }

        int iterations = size switch
        {
            <= 100 => 10000,
            <= 1000 => 1000,
            _ => 100
        };
        
        Stopwatch sw = Stopwatch.StartNew();
        
        int foundIndex = -1;
        for (int i = 0; i < iterations; i++)
        {
            foundIndex = algoIndex switch
            {
                1 => BruteForceSearch(searchedElement, workingArray),
                2 => SearchWithBarrier(searchedElement, workingArray),
                3 => BinarySearch(searchedElement, workingArray),
                4 => GoldenRatioBinarySearch(searchedElement, workingArray),
                _ => -1
            };
        }
        
        sw.Stop();
        
        double avgTimeMs = sw.Elapsed.TotalMilliseconds / iterations;
        long avgTicks = sw.ElapsedTicks / iterations;
        
        string foundStatus = foundIndex == -1 ? "NOT FOUND" : $"Index: {foundIndex}";
        return $"{avgTimeMs:F8} ms | {avgTicks,8} ticks | {foundStatus}";
    }

    private static void PrintComparisonResults(Dictionary<string, string> results, int searchedElement)
    {
        Console.WriteLine($"\n📈 PERFORMANCE COMPARISON RESULTS");
        Console.WriteLine("=".PadRight(100, '='));
        
        string[] algoNames = { "Linear (brute)", "Linear (barrier)", "Binary", "Binary (golden)" };
        
        foreach (int size in dataSizes)
        {
            Console.WriteLine($"\n📦 Array size: {size} elements");
            Console.WriteLine("-".PadRight(100, '-'));
            
            for (int algoIndex = 1; algoIndex <= 4; algoIndex++)
            {
                string key = $"Size {size} - Algo {algoIndex}";
                if (results.ContainsKey(key))
                {
                    Console.WriteLine($"  {algoNames[algoIndex - 1],-20} | {results[key]}");
                }
            }
        }
        
        Console.WriteLine("\n" + "=".PadRight(100, '='));
    }

    private static void SaveReportToFile(Dictionary<string, string> results, int searchedElement)
    {
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        string filename = $"Lab01_Report_{timestamp}.txt";
        
        using (StreamWriter writer = new StreamWriter(filename))
        {
            writer.WriteLine("╔════════════════════════════════════════════════════════════════╗");
            writer.WriteLine("║         Laboratory Work No. 1 - Search Algorithms              ║");
            writer.WriteLine("║         Performance Comparison Report                          ║");
            writer.WriteLine("╚════════════════════════════════════════════════════════════════╝");
            writer.WriteLine();
            writer.WriteLine($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            writer.WriteLine($"Performed by: Vlad Sapozhnyk (8)");
            writer.WriteLine($"Group: IPZ-13, Subgroup: 6");
            writer.WriteLine($"Searched value: {searchedElement}");
            writer.WriteLine();
            writer.WriteLine("================================================================");
            writer.WriteLine("GENERATED TEST ARRAYS");
            writer.WriteLine("================================================================");
            writer.WriteLine();
            
            foreach (var kvp in generatedArrays.OrderBy(x => x.Key))
            {
                int size = kvp.Key;
                int[] array = kvp.Value;
                
                writer.WriteLine($"Array size: {size} elements");
                writer.Write("Original: ");
                
                int displayLimit = Math.Min(size, 50);
                for (int i = 0; i < displayLimit; i++)
                {
                    writer.Write($"{array[i]} ");
                }
                
                if (size > 50)
                    writer.Write($"... (showing first 50 of {size})");
                
                writer.WriteLine();
                
                bool found = Array.IndexOf(array, searchedElement) != -1;
                writer.WriteLine($"Element {searchedElement} is {(found ? "PRESENT" : "NOT PRESENT")} in array");
                writer.WriteLine();
            }
            
            writer.WriteLine("================================================================");
            writer.WriteLine("PERFORMANCE RESULTS");
            writer.WriteLine("================================================================");
            writer.WriteLine();
            
            string[] algoNames = { "Linear (brute force)", "Linear (barrier)", "Binary search", "Binary (golden ratio)" };
            
            foreach (int size in dataSizes)
            {
                writer.WriteLine($"Array size: {size} elements");
                writer.WriteLine("----------------------------------------------------------------");
                
                for (int algoIndex = 1; algoIndex <= 4; algoIndex++)
                {
                    string key = $"Size {size} - Algo {algoIndex}";
                    if (results.ContainsKey(key))
                    {
                        writer.WriteLine($"  {algoNames[algoIndex - 1],-25} | {results[key]}");
                    }
                }
                writer.WriteLine();
            }
            
            writer.WriteLine("================================================================");
            writer.WriteLine("ANALYSIS");
            writer.WriteLine("================================================================");
            writer.WriteLine();
            writer.WriteLine("Linear search algorithms (1-2):");
            writer.WriteLine("  - Work on unsorted arrays");
            writer.WriteLine("  - Time complexity: O(n)");
            writer.WriteLine("  - Barrier method slightly faster due to one less comparison per iteration");
            writer.WriteLine();
            writer.WriteLine("Binary search algorithms (3-4):");
            writer.WriteLine("  - Require sorted arrays");
            writer.WriteLine("  - Time complexity: O(log n)");
            writer.WriteLine("  - Much faster on large arrays");
            writer.WriteLine("  - Golden ratio variant uses 0.618 split instead of 0.5");
            writer.WriteLine();
        }
        
        Console.WriteLine($"✅ Report saved to: {filename}");
    }

    private static void ChooseAlgorithm()
    {
        while (true)
        {
            Console.WriteLine("\n===========================================");
            Console.WriteLine("  Choose a search algorithm:");
            Console.WriteLine("===========================================");
            Console.WriteLine("  1 - Linear search (brute force)");
            Console.WriteLine("  2 - Linear search with a barrier");
            Console.WriteLine("  3 - Binary search");
            Console.WriteLine("  4 - Binary search (golden ratio)");
            Console.WriteLine("  0 - Back to menu");
            Console.WriteLine("===========================================");
            Console.Write("Your choice: ");

            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("\n❌ Incorrect data. Please try again!");
                continue;
            }

            if (choice == 0)
            {
                ChooseMode();
                break;
            }

            ExecuteSearch(choice);
        }
    }

    private static void ExecuteSearch(int choice)
    {
        if (choice < 1 || choice > 4)
        {
            Console.WriteLine("\n❌ Invalid choice.");
            return;
        }

        int searchedElement = GetSearchedElement();
        
        int[] workingArray = elements;
        if (choice >= 3)
        {
            workingArray = (int[])elements.Clone();
            Array.Sort(workingArray);
            Console.WriteLine("\nℹ️  Array has been sorted for binary search.");
            PrintSortedArray(workingArray);
        }

        Stopwatch stopwatch = Stopwatch.StartNew();
        
        int index = choice switch
        {
            1 => BruteForceSearch(searchedElement, workingArray),
            2 => SearchWithBarrier(searchedElement, workingArray),
            3 => BinarySearch(searchedElement, workingArray),
            4 => GoldenRatioBinarySearch(searchedElement, workingArray),
            _ => -1
        };
        
        stopwatch.Stop();

        Console.WriteLine("\n-------------------------------------------");
        if (index == -1)
            Console.WriteLine("❌ There is no such element in the array");
        else
        {
            Console.WriteLine($"✅ Index: {index}");
            if (choice >= 3)
                Console.WriteLine($"   Value at this position: {workingArray[index]}");
        }
            
        Console.WriteLine($"⏱️  Execution time: {stopwatch.Elapsed.TotalMilliseconds:F6} ms");
        Console.WriteLine($"⏱️  Execution time: {stopwatch.ElapsedTicks} ticks");
        Console.WriteLine("-------------------------------------------");
    }

    #region Helpers

    private static void Greeting()
    {
        Console.WriteLine("===========================================");
        Console.WriteLine("     📘 Laboratory Work No. 1");
        Console.WriteLine("     Search Algorithm Comparison");
        Console.WriteLine("===========================================");
        Console.WriteLine("💻 Performed by: Vlad Sapozhnyk (8)");
        Console.WriteLine("🎓 Taras Shevchenko National University of Kyiv");
        Console.WriteLine("🏫 Group: IPZ-13, Subgroup: 6");
        Console.WriteLine("===========================================");
    }

    private static void PrintAlgorithmsList()
    {
        Console.WriteLine("📋 Available algorithms:");
        for (int i = 0; i < algorithmsList.Length; i++)
        {
            Console.WriteLine($"   {i + 1}. {algorithmsList[i]}");
        }
    }

    private static void FillArray()
    {
        Console.Write("⏳ Filling array... ");
        Random random = new Random();
        elements = new int[dataSize];

        for (int i = 0; i < dataSize; i++)
        {
            elements[i] = random.Next(101);
        }

        Console.WriteLine("✅ Done!");
    }

    private static void PrintArray()
    {
        Console.WriteLine($"\n📊 Generated array ({dataSize} elements):");
        Console.Write("   ");
        
        int displayLimit = Math.Min(dataSize, 50);
        for (int i = 0; i < displayLimit; i++)
        {
            Console.Write(elements[i]);
            if (i < displayLimit - 1) Console.Write(", ");
        }
        
        if (dataSize > 50)
            Console.Write($" ... (showing first 50 of {dataSize})");
            
        Console.WriteLine();
    }

    private static void PrintSortedArray(int[] array)
    {
        Console.WriteLine($"Sorted array:");
        Console.Write("   ");
        
        int displayLimit = Math.Min(array.Length, 50);
        for (int i = 0; i < displayLimit; i++)
        {
            Console.Write(array[i]);
            if (i < displayLimit - 1) Console.Write(", ");
        }
        
        if (array.Length > 50)
            Console.Write($" ... (showing first 50 of {array.Length})");
            
        Console.WriteLine();
    }

    private static int GetSearchedElement()
    {
        while (true)
        {
            Console.Write("\n🔍 Enter value to search (0-100): ");

            if (int.TryParse(Console.ReadLine(), out int searchedElement))
            {
                return searchedElement;
            }

            Console.WriteLine("❌ Incorrect data. Please try again!");
        }
    }

    private static void ChooseDataSize()
    {
        Console.WriteLine("\nSelect array size:");
        Console.WriteLine("  1 - 10 elements");
        Console.WriteLine("  2 - 100 elements");
        Console.WriteLine("  3 - 1000 elements");
        Console.WriteLine("  4 - 10000 elements");
        Console.Write("\nYour choice: ");

        while (true)
        {
            if (!int.TryParse(Console.ReadLine(), out int choice)) 
            {
                Console.Write("❌ Incorrect input. Try again: ");
                continue;
            }
            
            dataSize = choice switch
            {
                1 => 10,
                2 => 100,
                3 => 1000,
                4 => 10000,
                _ => 0
            };

            if (dataSize > 0)
            {
                Console.WriteLine($"✅ Array size set to {dataSize} elements.\n");
                return;
            }

            Console.Write("❌ Invalid choice. Try again: ");
        }
    }

    #endregion 

    #region Algorithms

    private static int BruteForceSearch(int searchedElement, int[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] == searchedElement)
                return i;
        }
        return -1;
    }

    private static int SearchWithBarrier(int searchedElement, int[] array)
    {
        int n = array.Length;
        int[] temp = new int[n + 1];

        Array.Copy(array, temp, n);
        temp[n] = searchedElement;

        int i = 0;
        while (temp[i] != searchedElement)
        {
            i++;
        }

        return i < n ? i : -1;
    }

    private static int BinarySearch(int searchedElement, int[] array)
    {
        int left = 0;
        int right = array.Length - 1;
        
        while (left <= right)
        {
            int middle = left + (right - left) / 2;
            
            if (array[middle] == searchedElement)
                return middle;
            
            if (array[middle] < searchedElement)
                left = middle + 1;
            else
                right = middle - 1;
        }
        
        return -1;
    }

    private static int GoldenRatioBinarySearch(int searchedElement, int[] array)
    {
        int left = 0;
        int right = array.Length - 1;
        
        while (left <= right)
        {
            int middle = left + (int)((right - left) * 0.618);
            
            if (array[middle] == searchedElement)
                return middle;
            
            if (array[middle] < searchedElement)
                left = middle + 1;
            else
                right = middle - 1;
        }
        
        return -1;
    }

    #endregion
}