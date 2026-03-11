using System.Globalization;
using System.Text;

namespace Lab02;

public class Program
{
    private static Random random = new Random();

    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Greeting();
        ChooseTask();
    }

    private static void Greeting()
    {
        Console.WriteLine("===========================================");
        Console.WriteLine("     📘 Laboratory Work No. 2");
        Console.WriteLine("     SORT Algorithms");
        Console.WriteLine("===========================================");
        Console.WriteLine("💻 Performed by: Vlad Sapozhnyk (8)");
        Console.WriteLine("🎓 Taras Shevchenko National University of Kyiv");
        Console.WriteLine("🏫 Group: IPZ-13, Subgroup: 6");
        Console.WriteLine("===========================================");
    }

    private static void ChooseTask()
    {
        while (true)
        {
            Console.WriteLine("\n===========================================");
            Console.WriteLine("  Choose task to run:");
            Console.WriteLine("===========================================");
            Console.WriteLine("  1 - PARTITION illustration");
            Console.WriteLine("  2 - Modified PARTITION for equal elements");
            Console.WriteLine("  3 - QUICKSORT (descending)");
            Console.WriteLine("  4 - RANDOMIZED-QUICKSORT (descending)");
            Console.WriteLine("  5 - COUNTING-SORT illustration");
            Console.WriteLine("  6 - Preprocessing for interval queries [a..b]");
            Console.WriteLine("  7 - RADIX-SORT (words)");
            Console.WriteLine("  8 - BUCKET-SORT illustration");
            Console.WriteLine("  9 - Pipeline optimization");
            Console.WriteLine(" 10 - Theory: PARTITION complexity");
            Console.WriteLine(" 11 - Theory: invariant for descending sort");
            Console.WriteLine(" 12 - Theory: QUICKSORT when all elements equal");
            Console.WriteLine(" 13 - Theory: QUICKSORT on descending distinct array");
            Console.WriteLine(" 14 - Theory: why COUNTING-SORT is stable");
            Console.WriteLine(" 15 - Theory: COUNTING-SORT with j = 1..n");
            Console.WriteLine(" 16 - Theory: why RADIX needs stability");
            Console.WriteLine(" 17 - Run all tasks");
            Console.WriteLine("  0 - Exit");
            Console.WriteLine("===========================================");
            Console.Write("Your choice: ");

            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("\n❌ Incorrect data. Please try again!");
                continue;
            }

            if (choice == 0)
            {
                Console.WriteLine("\n👋 Goodbye!");
                return;
            }

            Console.WriteLine();
            switch (choice)
            {
                case 1:
                    Task01();
                    break;
                case 2:
                    Task02();
                    break;
                case 3:
                    Task03();
                    break;
                case 4:
                    Task04();
                    break;
                case 5:
                    Task05();
                    break;
                case 6:
                    Task06();
                    break;
                case 7:
                    Task07();
                    break;
                case 8:
                    Task08();
                    break;
                case 9:
                    Task09();
                    break;
                case 10:
                    Task10();
                    break;
                case 11:
                    Task11();
                    break;
                case 12:
                    Task12();
                    break;
                case 13:
                    Task13();
                    break;
                case 14:
                    Task14();
                    break;
                case 15:
                    Task15();
                    break;
                case 16:
                    Task16();
                    break;
                case 17:
                    RunAllTasks();
                    break;
                default:
                    Console.WriteLine("❌ Invalid choice.");
                    break;
            }
        }
    }

    private static void RunAllTasks()
    {
        Task01();
        Task02();
        Task03();
        Task04();
        Task05();
        Task06();
        Task07();
        Task08();
        Task09();
        Task10();
        Task11();
        Task12();
        Task13();
        Task14();
        Task15();
        Task16();
    }

    #region Tasks

    private static void Task01()
    {
        Console.WriteLine("===========================================");
        Console.WriteLine("Task 1: PARTITION illustration");
        Console.WriteLine("===========================================");

        int[] array = [13, 19, 9, 5, 12, 8, 7, 4, 21, 2, 6, 11];
        Console.WriteLine($"Input array:  {FormatArray(array)}");

        int[] copy = (int[])array.Clone();
        int q = PartitionDescending(copy, 0, copy.Length - 1);

        Console.WriteLine($"Pivot index q: {q}");
        Console.WriteLine($"After split:   {FormatArray(copy)}");
        Console.WriteLine($"Pivot value:   {copy[q]}");
    }

    private static void Task02()
    {
        Console.WriteLine("===========================================");
        Console.WriteLine("Task 2: Modified PARTITION for equal elements");
        Console.WriteLine("===========================================");

        int[] equalArray = [7, 7, 7, 7, 7, 7, 7, 7];
        Console.WriteLine($"Input array:   {FormatArray(equalArray)}");

        int[] copy = (int[])equalArray.Clone();
        int p = 0;
        int r = copy.Length - 1;
        int q = PartitionModified(copy, p, r);

        Console.WriteLine($"Returned q:    {q}");
        Console.WriteLine($"Expected q:    {(p + r) / 2}");
        Console.WriteLine($"After split:   {FormatArray(copy)}");
    }

    private static void Task03()
    {
        Console.WriteLine("===========================================");
        Console.WriteLine("Task 3: QUICKSORT (descending)");
        Console.WriteLine("===========================================");

        int[] array = GenerateRandomIntArray(15, 0, 100);
        Console.WriteLine($"Input array:   {FormatArray(array)}");

        QuickSortDescending(array, 0, array.Length - 1);
        Console.WriteLine($"Sorted array:  {FormatArray(array)}");
    }

    private static void Task04()
    {
        Console.WriteLine("===========================================");
        Console.WriteLine("Task 4: RANDOMIZED-QUICKSORT (descending)");
        Console.WriteLine("===========================================");

        int[] array = GenerateRandomIntArray(15, 0, 100);
        Console.WriteLine($"Input array:   {FormatArray(array)}");

        RandomizedQuickSortDescending(array, 0, array.Length - 1);
        Console.WriteLine($"Sorted array:  {FormatArray(array)}");
    }

    private static void Task05()
    {
        Console.WriteLine("===========================================");
        Console.WriteLine("Task 5: COUNTING-SORT illustration");
        Console.WriteLine("===========================================");

        int[] array = [6, 0, 2, 0, 1, 3, 4, 6, 1, 3, 2];
        int k = array.Max();

        Console.WriteLine($"Input array:   {FormatArray(array)}");

        int[] counts = BuildCounts(array, k);
        Console.WriteLine($"Count C[i]:    {FormatArray(counts)}");

        int[] prefix = BuildPrefixCounts(counts);
        Console.WriteLine($"Prefix C[i]:   {FormatArray(prefix)}");

        int[] sorted = CountingSort(array, k);
        Console.WriteLine($"Sorted array:  {FormatArray(sorted)}");
    }

    private static void Task06()
    {
        Console.WriteLine("===========================================");
        Console.WriteLine("Task 6: O(1) interval query after O(n+k) preprocessing");
        Console.WriteLine("===========================================");

        int[] source = [6, 0, 2, 0, 1, 3, 4, 6, 1, 3, 2];
        int k = 6;

        Console.WriteLine($"Input array:   {FormatArray(source)}");

        int[] prefixCounts = PreprocessForRangeQueries(source, k);
        Console.WriteLine($"Prefix table:  {FormatArray(prefixCounts)}");

        PrintRangeQuery(prefixCounts, 0, 2);
        PrintRangeQuery(prefixCounts, 2, 4);
        PrintRangeQuery(prefixCounts, 5, 6);
    }

    private static void Task07()
    {
        Console.WriteLine("===========================================");
        Console.WriteLine("Task 7: RADIX-SORT words");
        Console.WriteLine("===========================================");

        string[] words =
        [
            "COW", "DOG", "SEA", "RUG", "ROW", "MOB", "BOX", "TAB",
            "BAR", "EAR", "TAR", "DIG", "BIG", "TEA", "NOW", "FOX"
        ];

        Console.WriteLine($"Input words:   {string.Join(", ", words)}");
        RadixSortWords(words);
        Console.WriteLine($"Sorted words:  {string.Join(", ", words)}");
    }

    private static void Task08()
    {
        Console.WriteLine("===========================================");
        Console.WriteLine("Task 8: BUCKET-SORT illustration");
        Console.WriteLine("===========================================");

        double[] array = [0.79, 0.13, 0.16, 0.64, 0.39, 0.20, 0.89, 0.53, 0.71, 0.42];
        Console.WriteLine($"Input array:   {FormatArray(array)}");

        double[] sorted = BucketSort(array);
        Console.WriteLine($"Sorted array:  {FormatArray(sorted)}");
    }

    private static void Task09()
    {
        Console.WriteLine("===========================================");
        Console.WriteLine("Task 9: Pipeline optimization");
        Console.WriteLine("===========================================");

        (int x, int y)[] wells =
        [
            (2, 9), (4, 1), (8, 6), (10, 3), (13, 8), (14, 4), (18, 7)
        ];

        Console.WriteLine("Wells (x, y):");
        foreach (var well in wells)
        {
            Console.WriteLine($"  ({well.x}, {well.y})");
        }

        int[] ys = wells.Select(w => w.y).ToArray();
        int optimalY = SelectKthDeterministic((int[])ys.Clone(), ys.Length / 2);
        int totalLength = ys.Sum(y => Math.Abs(y - optimalY));

        Console.WriteLine($"\nOptimal pipeline level y*: {optimalY}");
        Console.WriteLine($"Total branch length:       {totalLength}");
        Console.WriteLine("Reason: minimum sum of |y_i - y*| is reached at median.");
    }

    private static void Task10()
    {
        Console.WriteLine("===========================================");
        Console.WriteLine("Task 10: Time of PARTITION on n elements");
        Console.WriteLine("===========================================");
        Console.WriteLine("PARTITION does one pass through subarray A[p..r].");
        Console.WriteLine("Comparisons: n - 1");
        Console.WriteLine("Extra swaps/assignments: O(n)");
        Console.WriteLine("Total: T(n) = a*n + b, so T(n) = O(n) and Θ(n).");
    }

    private static void Task11()
    {
        Console.WriteLine("===========================================");
        Console.WriteLine("Task 11: Invariant for descending partition");
        Console.WriteLine("===========================================");
        Console.WriteLine("For loop j = p..r-1 in PARTITION (pivot x = A[r]):");
        Console.WriteLine("1) A[p..i] contains elements >= x");
        Console.WriteLine("2) A[i+1..j-1] contains elements < x");
        Console.WriteLine("3) A[j..r-1] are unprocessed");
        Console.WriteLine("4) A[r] = x (pivot fixed until final swap)");
        Console.WriteLine("After loop and final swap, pivot is at correct index q.");
    }

    private static void Task12()
    {
        Console.WriteLine("===========================================");
        Console.WriteLine("Task 12: QUICKSORT if all elements are equal");
        Console.WriteLine("===========================================");
        Console.WriteLine("With classic PARTITION (pivot is last element):");
        Console.WriteLine("q becomes r each call, so recurrence is:");
        Console.WriteLine("T(n) = T(n - 1) + Θ(n)");
        Console.WriteLine("Therefore: T(n) = Θ(n^2).");
        Console.WriteLine("Note: with modified PARTITION from task 2, it becomes balanced.");
    }

    private static void Task13()
    {
        Console.WriteLine("===========================================");
        Console.WriteLine("Task 13: QUICKSORT on descending distinct array");
        Console.WriteLine("===========================================");
        Console.WriteLine("For descending sort with pivot A[r]:");
        Console.WriteLine("If array is already strictly descending, pivot is minimum.");
        Console.WriteLine("Then q = r, and recurrence is:");
        Console.WriteLine("T(n) = T(n - 1) + Θ(n)");
        Console.WriteLine("Therefore: T(n) = Θ(n^2).");
    }

    private static void Task14()
    {
        Console.WriteLine("===========================================");
        Console.WriteLine("Task 14: Why COUNTING-SORT is stable");
        Console.WriteLine("===========================================");
        Console.WriteLine("In stable COUNTING-SORT we traverse input from right to left.");
        Console.WriteLine("For equal keys, element appearing later is placed later in output.");
        Console.WriteLine("So relative order of equal keys is preserved.");
        Console.WriteLine("Hence COUNTING-SORT is stable.");
    }

    private static void Task15()
    {
        Console.WriteLine("===========================================");
        Console.WriteLine("Task 15: COUNTING-SORT with j = 1..n");
        Console.WriteLine("===========================================");

        KeyedItem[] source =
        [
            new KeyedItem(2, "A"),
            new KeyedItem(1, "B"),
            new KeyedItem(2, "C"),
            new KeyedItem(1, "D"),
            new KeyedItem(2, "E")
        ];

        KeyedItem[] stable = CountingSortKeyed(source, 2, true);
        KeyedItem[] forward = CountingSortKeyed(source, 2, false);

        Console.WriteLine($"Input:                 {FormatKeyed(source)}");
        Console.WriteLine($"Stable (j=n..1):       {FormatKeyed(stable)}");
        Console.WriteLine($"Modified (j=1..n):     {FormatKeyed(forward)}");
        Console.WriteLine("Result: keys are sorted correctly in both versions.");
        Console.WriteLine("But modified version is NOT stable (order of equal keys changes).");
    }

    private static void Task16()
    {
        Console.WriteLine("===========================================");
        Console.WriteLine("Task 16: Why RADIX-SORT needs stable inner sort");
        Console.WriteLine("===========================================");
        Console.WriteLine("RADIX processes digits from less significant to more significant.");
        Console.WriteLine("After sorting by next digit, previous digit order must be preserved");
        Console.WriteLine("inside each equal-digit group. Only stable sorting guarantees this.");
        Console.WriteLine("Without stability, prior passes are destroyed and final order may fail.");
    }

    #endregion

    #region Algorithms

    private static int PartitionDescending(int[] array, int p, int r)
    {
        int pivot = array[r];
        int i = p - 1;

        for (int j = p; j <= r - 1; j++)
        {
            if (array[j] >= pivot)
            {
                i++;
                Swap(array, i, j);
            }
        }

        Swap(array, i + 1, r);
        return i + 1;
    }

    private static int PartitionModified(int[] array, int p, int r)
    {
        bool allEqual = true;
        int first = array[p];
        for (int i = p + 1; i <= r; i++)
        {
            if (array[i] != first)
            {
                allEqual = false;
                break;
            }
        }

        if (allEqual)
            return (p + r) / 2;

        return PartitionDescending(array, p, r);
    }

    private static void QuickSortDescending(int[] array, int p, int r)
    {
        if (p < r)
        {
            int q = PartitionModified(array, p, r);
            QuickSortDescending(array, p, q - 1);
            QuickSortDescending(array, q + 1, r);
        }
    }

    private static void RandomizedQuickSortDescending(int[] array, int p, int r)
    {
        if (p < r)
        {
            int q = RandomizedPartitionDescending(array, p, r);
            RandomizedQuickSortDescending(array, p, q - 1);
            RandomizedQuickSortDescending(array, q + 1, r);
        }
    }

    private static int RandomizedPartitionDescending(int[] array, int p, int r)
    {
        int randomIndex = random.Next(p, r + 1);
        Swap(array, randomIndex, r);
        return PartitionModified(array, p, r);
    }

    private static int[] CountingSort(int[] array, int k)
    {
        int[] counts = BuildCounts(array, k);
        int[] prefix = BuildPrefixCounts(counts);
        int[] output = new int[array.Length];

        for (int j = array.Length - 1; j >= 0; j--)
        {
            int value = array[j];
            output[prefix[value] - 1] = value;
            prefix[value]--;
        }

        return output;
    }

    private static int[] BuildCounts(int[] array, int k)
    {
        int[] counts = new int[k + 1];
        foreach (int value in array)
        {
            counts[value]++;
        }
        return counts;
    }

    private static int[] BuildPrefixCounts(int[] counts)
    {
        int[] prefix = (int[])counts.Clone();
        for (int i = 1; i < prefix.Length; i++)
        {
            prefix[i] += prefix[i - 1];
        }
        return prefix;
    }

    private static int[] PreprocessForRangeQueries(int[] source, int k)
    {
        int[] counts = BuildCounts(source, k);
        return BuildPrefixCounts(counts);
    }

    private static int QueryCountInRange(int[] prefixCounts, int a, int b)
    {
        int left = Math.Max(0, a);
        int right = Math.Min(prefixCounts.Length - 1, b);
        if (left > right)
            return 0;

        if (left == 0)
            return prefixCounts[right];

        return prefixCounts[right] - prefixCounts[left - 1];
    }

    private static void RadixSortWords(string[] words)
    {
        int maxLength = words.Max(w => w.Length);
        for (int pos = maxLength - 1; pos >= 0; pos--)
        {
            CountingSortByChar(words, pos);
        }
    }

    private static void CountingSortByChar(string[] words, int charPosition)
    {
        int alphabet = 256;
        int[] counts = new int[alphabet];
        string[] output = new string[words.Length];

        for (int i = 0; i < words.Length; i++)
        {
            int code = GetCharCode(words[i], charPosition);
            counts[code]++;
        }

        for (int i = 1; i < alphabet; i++)
        {
            counts[i] += counts[i - 1];
        }

        for (int i = words.Length - 1; i >= 0; i--)
        {
            int code = GetCharCode(words[i], charPosition);
            output[counts[code] - 1] = words[i];
            counts[code]--;
        }

        Array.Copy(output, words, words.Length);
    }

    private static int GetCharCode(string text, int charPosition)
    {
        if (charPosition >= text.Length)
            return 0;
        return text[charPosition];
    }

    private static double[] BucketSort(double[] array)
    {
        int n = array.Length;
        List<double>[] buckets = new List<double>[n];
        for (int i = 0; i < n; i++)
        {
            buckets[i] = new List<double>();
        }

        foreach (double value in array)
        {
            int bucketIndex = Math.Min(n - 1, (int)(value * n));
            buckets[bucketIndex].Add(value);
        }

        for (int i = 0; i < n; i++)
        {
            buckets[i].Sort();
        }

        double[] result = new double[n];
        int idx = 0;
        for (int i = 0; i < n; i++)
        {
            foreach (double value in buckets[i])
            {
                result[idx++] = value;
            }
        }

        return result;
    }

    private static KeyedItem[] CountingSortKeyed(KeyedItem[] items, int k, bool stable)
    {
        int[] counts = new int[k + 1];
        foreach (KeyedItem item in items)
        {
            counts[item.Key]++;
        }

        for (int i = 1; i < counts.Length; i++)
        {
            counts[i] += counts[i - 1];
        }

        KeyedItem[] output = new KeyedItem[items.Length];

        if (stable)
        {
            for (int j = items.Length - 1; j >= 0; j--)
            {
                int key = items[j].Key;
                output[counts[key] - 1] = items[j];
                counts[key]--;
            }
        }
        else
        {
            for (int j = 0; j < items.Length; j++)
            {
                int key = items[j].Key;
                output[counts[key] - 1] = items[j];
                counts[key]--;
            }
        }

        return output;
    }

    private static int SelectKthDeterministic(int[] array, int k)
    {
        if (array.Length <= 5)
        {
            Array.Sort(array);
            return array[k];
        }

        List<int> medians = new List<int>();
        for (int i = 0; i < array.Length; i += 5)
        {
            int length = Math.Min(5, array.Length - i);
            int[] group = new int[length];
            Array.Copy(array, i, group, 0, length);
            Array.Sort(group);
            medians.Add(group[length / 2]);
        }

        int pivot = SelectKthDeterministic(medians.ToArray(), medians.Count / 2);

        List<int> lows = new List<int>();
        List<int> highs = new List<int>();
        List<int> pivots = new List<int>();

        foreach (int value in array)
        {
            if (value < pivot)
                lows.Add(value);
            else if (value > pivot)
                highs.Add(value);
            else
                pivots.Add(value);
        }

        if (k < lows.Count)
            return SelectKthDeterministic(lows.ToArray(), k);

        if (k < lows.Count + pivots.Count)
            return pivot;

        return SelectKthDeterministic(highs.ToArray(), k - lows.Count - pivots.Count);
    }

    #endregion

    #region Helpers

    private static void PrintRangeQuery(int[] prefixCounts, int a, int b)
    {
        int count = QueryCountInRange(prefixCounts, a, b);
        Console.WriteLine($"Elements in [{a}..{b}] = {count}");
    }

    private static int[] GenerateRandomIntArray(int size, int min, int maxInclusive)
    {
        int[] array = new int[size];
        for (int i = 0; i < size; i++)
        {
            array[i] = random.Next(min, maxInclusive + 1);
        }
        return array;
    }

    private static void Swap(int[] array, int i, int j)
    {
        if (i == j)
            return;

        int tmp = array[i];
        array[i] = array[j];
        array[j] = tmp;
    }

    private static string FormatArray(int[] array)
    {
        return "[" + string.Join(", ", array) + "]";
    }

    private static string FormatArray(double[] array)
    {
        return "[" + string.Join(", ", array.Select(x => x.ToString("0.00", CultureInfo.InvariantCulture))) + "]";
    }

    private static string FormatKeyed(KeyedItem[] items)
    {
        return "[" + string.Join(", ", items.Select(x => $"({x.Key},{x.Tag})")) + "]";
    }

    #endregion

    private readonly record struct KeyedItem(int Key, string Tag);
}
