# Лабораторна робота №3

## Пошук підрядка в рядку

У цій лабораторній роботі реалізовано два алгоритми пошуку підрядка в рядку:

1. **Прямий пошук**  
   Ідея проста: беремо підрядок і по черзі прикладаємо його до кожної можливої позиції в рядку.  
   Якщо всі символи збіглися, то підрядок знайдено.

2. **Алгоритм Кнута-Морріса-Пратта (KMP)**  
   Цей алгоритм працює швидше, бо не починає перевірку заново після кожної невдачі.  
   Він використовує **префікс-функцію**, яка підказує, з якої позиції в підрядку треба продовжити порівняння.

---

## Що робить програма

Програма:

- показує меню;
- дозволяє запустити прямий пошук;
- дозволяє запустити KMP-пошук;
- дозволяє одразу порівняти обидва алгоритми;
- перевіряє коректність введення;
- виводить позицію знайденого підрядка.

---

## Як запустити

```bash
dotnet run --project /Users/sapozhnyk.vladyslav/Desktop/uni-projects/csharp-university-labs/Lab03/Task01/Lab03.Task01.csproj
```

---

## Приклад роботи

```text
Choose task to run:
1 - Direct substring search
2 - Knuth-Morris-Pratt search
3 - Run both algorithms
0 - Exit

Your choice: 3
Enter your string: ababdjjsdjfryalgorithmsduqwp
Enter your substring: algorithms

Direct search: substring found at position 14 (index 13).
Knuth-Morris-Pratt search: substring found at position 14 (index 13).
```

---

## Складність алгоритмів

- **Прямий пошук**: у гіршому випадку `O(n * m)`, де `n` — довжина рядка, `m` — довжина підрядка.
- **KMP**: `O(n + m)`, бо префікс-функція дозволяє не робити зайвих повторних перевірок.

---

## Пояснення логіки програми

### 1. `Main`

Це точка входу в програму.  
Спочатку встановлюється UTF-8 для консолі, потім виводиться заголовок, після чого запускається меню.

### 2. `ChooseMode`

Цей метод відповідає за меню:

- читає вибір користувача;
- перевіряє, чи число введене правильно;
- запускає потрібний алгоритм;
- або завершує програму.

### 3. `ReadSearchInput`

Цей метод зчитує:

- рядок, у якому шукаємо;
- підрядок, який шукаємо.

Також він перевіряє, щоб підрядок не був довшим за сам рядок.

### 4. `DirectSearch`

Це класичний прямий пошук:

- зовнішній цикл перебирає всі можливі позиції початку;
- внутрішній цикл порівнює символи рядка і підрядка;
- якщо весь підрядок співпав, повертається індекс;
- якщо ніде не знайдено, повертається `-1`.

### 5. `BuildPrefixFunction`

Це побудова префікс-функції для KMP.

`prefix[i]` показує, яка довжина найбільшого власного префікса підрядка `pattern[0..i]` одночасно є його суфіксом.

Наприклад, для рядка `ababa` префікс-функція буде:

```text
0 0 1 2 3
```

### 6. `KnuthMorrisPrattSearch`

Тут відбувається сам пошук KMP:

- `textIndex` рухається по рядку;
- `patternIndex` рухається по підрядку;
- якщо символи збігаються, обидва індекси збільшуються;
- якщо символи не збігаються, KMP не повертається в початок, а бере наступну позицію з префікс-функції.

### 7. `PrintSearchResult`

Метод просто красиво виводить:

- або позицію знайденого підрядка;
- або повідомлення, що збіг відсутній.

### 8. `PrintPrefixFunction`

Цей метод виводить:

- сам підрядок посимвольно;
- масив префікс-функції.

Це корисно для розуміння роботи KMP.

---

## Код з детальними коментарями

Нижче наведений той самий код, але з поясненням майже кожного робочого рядка.

```csharp
// Підключаємо простір імен, у якому є клас Encoding.
using System.Text;

// Оголошуємо namespace для третьої лабораторної роботи.
namespace Lab03;

// Основний клас програми. Він static, бо всі методи в ньому теж static.
public static class Program
{
    // Точка входу в програму.
    public static void Main(string[] args)
    {
        // Встановлюємо UTF-8, щоб консоль коректно показувала текстові символи.
        Console.OutputEncoding = Encoding.UTF8;

        // Виводимо інформацію про лабораторну роботу.
        Greeting();

        // Переходимо в головне меню програми.
        ChooseMode();
    }

    // Метод виводить заголовок програми.
    private static void Greeting()
    {
        // Друкуємо верхню межу блоку.
        Console.WriteLine("===========================================");

        // Друкуємо номер лабораторної роботи.
        Console.WriteLine("     Laboratory Work No. 3");

        // Друкуємо тему лабораторної роботи.
        Console.WriteLine("     STRING SEARCH Algorithms");

        // Друкуємо розділювач.
        Console.WriteLine("===========================================");

        // Друкуємо ім'я виконавця.
        Console.WriteLine("Performed by: Vlad Sapozhnyk (8)");

        // Друкуємо групу і підгрупу.
        Console.WriteLine("Group: IPZ-13, Subgroup: 6");

        // Друкуємо нижню межу блоку.
        Console.WriteLine("===========================================");
    }

    // Метод показує меню і виконує вибір користувача.
    private static void ChooseMode()
    {
        // Безкінечний цикл потрібен, щоб після виконання алгоритму меню з'являлось знову.
        while (true)
        {
            // Починаємо новий блок меню з нового рядка.
            Console.WriteLine("\n===========================================");

            // Пояснюємо користувачу, що треба вибрати режим.
            Console.WriteLine("  Choose task to run:");

            // Друкуємо розділювач.
            Console.WriteLine("===========================================");

            // Варіант 1: запустити прямий пошук.
            Console.WriteLine("  1 - Direct substring search");

            // Варіант 2: запустити алгоритм Кнута-Морріса-Пратта.
            Console.WriteLine("  2 - Knuth-Morris-Pratt search");

            // Варіант 3: запустити обидва алгоритми на одних і тих самих даних.
            Console.WriteLine("  3 - Run both algorithms");

            // Варіант 0: вийти з програми.
            Console.WriteLine("  0 - Exit");

            // Ще один розділювач для візуальної зручності.
            Console.WriteLine("===========================================");

            // Просимо ввести номер пункту меню.
            Console.Write("Your choice: ");

            // Пробуємо перетворити введений текст у число.
            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                // Якщо введено не число, показуємо повідомлення про помилку.
                Console.WriteLine("\nIncorrect input. Please enter a number.");

                // Переходимо до наступної ітерації циклу і показуємо меню ще раз.
                continue;
            }

            // Вибираємо дію за номером пункту меню.
            switch (choice)
            {
                // Якщо введено 1, запускаємо прямий пошук.
                case 1:
                    RunDirectSearch();
                    break;

                // Якщо введено 2, запускаємо KMP.
                case 2:
                    RunKmpSearch();
                    break;

                // Якщо введено 3, запускаємо обидва алгоритми.
                case 3:
                    RunBothAlgorithms();
                    break;

                // Якщо введено 0, завершуємо програму.
                case 0:
                    Console.WriteLine("\nGoodbye!");
                    return;

                // Якщо число є, але такого пункту меню немає.
                default:
                    Console.WriteLine("\nInvalid choice.");
                    break;
            }
        }
    }

    // Метод запускає тільки прямий пошук.
    private static void RunDirectSearch()
    {
        // Показуємо, який саме алгоритм зараз виконується.
        Console.WriteLine("\n------ Direct search ------");

        // Зчитуємо рядок і підрядок.
        var (text, pattern) = ReadSearchInput();

        // Викликаємо алгоритм прямого пошуку і зберігаємо знайдений індекс.
        int index = DirectSearch(text, pattern);

        // Виводимо результат пошуку.
        PrintSearchResult("Direct search", index);
    }

    // Метод запускає тільки KMP-пошук.
    private static void RunKmpSearch()
    {
        // Показуємо назву алгоритму в консолі.
        Console.WriteLine("\n------ Knuth-Morris-Pratt search ------");

        // Зчитуємо текст і підрядок з консолі.
        var (text, pattern) = ReadSearchInput();

        // Будуємо префікс-функцію для підрядка.
        int[] prefix = BuildPrefixFunction(pattern);

        // Виводимо масив префікс-функції, щоб було видно, як він виглядає.
        PrintPrefixFunction(pattern, prefix);

        // Виконуємо KMP-пошук, передаючи вже готову префікс-функцію.
        int index = KnuthMorrisPrattSearch(text, pattern, prefix);

        // Виводимо підсумок пошуку.
        PrintSearchResult("Knuth-Morris-Pratt search", index);
    }

    // Метод запускає обидва алгоритми для порівняння.
    private static void RunBothAlgorithms()
    {
        // Виводимо заголовок комбінованого режиму.
        Console.WriteLine("\n------ Direct search + KMP ------");

        // Зчитуємо текст і підрядок лише один раз, щоб обидва алгоритми працювали з однаковими даними.
        var (text, pattern) = ReadSearchInput();

        // Виконуємо прямий пошук.
        int directIndex = DirectSearch(text, pattern);

        // Будуємо префікс-функцію для KMP.
        int[] prefix = BuildPrefixFunction(pattern);

        // Виконуємо KMP-пошук.
        int kmpIndex = KnuthMorrisPrattSearch(text, pattern, prefix);

        // Показуємо префікс-функцію.
        PrintPrefixFunction(pattern, prefix);

        // Показуємо результат прямого пошуку.
        PrintSearchResult("Direct search", directIndex);

        // Показуємо результат KMP.
        PrintSearchResult("Knuth-Morris-Pratt search", kmpIndex);

        // Перевіряємо, чи обидва алгоритми дали однакову відповідь.
        Console.WriteLine(directIndex == kmpIndex
            ? "Both algorithms returned the same result."
            : "The algorithms returned different results.");
    }

    // Метод читає коректні вхідні дані для пошуку.
    private static (string Text, string Pattern) ReadSearchInput()
    {
        // Зчитуємо основний рядок.
        string text = ReadNonEmptyString("Enter your string: ");

        // Зчитуємо підрядок.
        string pattern = ReadNonEmptyString("Enter your substring: ");

        // Перевіряємо, щоб підрядок не був довшим за рядок.
        while (pattern.Length > text.Length)
        {
            // Повідомляємо користувача про помилку.
            Console.WriteLine("The substring must not be longer than the string.");

            // Просимо ввести підрядок ще раз.
            pattern = ReadNonEmptyString("Enter your substring again: ");
        }

        // Повертаємо обидва рядки як кортеж.
        return (text, pattern);
    }

    // Метод зчитує непорожній рядок.
    private static string ReadNonEmptyString(string prompt)
    {
        // Цикл повторюється, поки користувач не введе нормальне значення.
        while (true)
        {
            // Виводимо запрошення до вводу.
            Console.Write(prompt);

            // Зчитуємо рядок з консолі.
            string? value = Console.ReadLine();

            // Якщо значення не null і не порожній рядок, повертаємо його.
            if (!string.IsNullOrEmpty(value))
            {
                return value;
            }

            // Якщо значення порожнє, повідомляємо про це.
            Console.WriteLine("Input cannot be empty.");
        }
    }

    // Прямий пошук підрядка в рядку.
    private static int DirectSearch(string text, string pattern)
    {
        // Перебираємо всі позиції, з яких підрядок ще може повністю поміститися в тексті.
        for (int i = 0; i <= text.Length - pattern.Length; i++)
        {
            // j показує, скільки символів підрядка вже збіглося.
            int j = 0;

            // Порівнюємо символи доти, доки не закінчився підрядок і символи однакові.
            while (j < pattern.Length && text[i + j] == pattern[j])
            {
                // Переходимо до наступного символу підрядка.
                j++;
            }

            // Якщо j дорівнює довжині підрядка, то всі символи співпали.
            if (j == pattern.Length)
            {
                // Повертаємо індекс початку знайденого входження.
                return i;
            }
        }

        // Якщо цикл завершився без успіху, повертаємо -1.
        return -1;
    }

    // Побудова префікс-функції для KMP.
    private static int[] BuildPrefixFunction(string pattern)
    {
        // Створюємо масив prefix тієї ж довжини, що і підрядок.
        int[] prefix = new int[pattern.Length];

        // j зберігає поточну довжину збігу префікса і суфікса.
        int j = 0;

        // Починаємо з другого символу, бо для першого prefix[0] завжди 0.
        for (int i = 1; i < pattern.Length; i++)
        {
            // Якщо символи не збігаються, відступаємо за вже обчисленими значеннями prefix.
            while (j > 0 && pattern[i] != pattern[j])
            {
                // Переходимо до коротшого префікса.
                j = prefix[j - 1];
            }

            // Якщо після відступу або одразу символи збіглись.
            if (pattern[i] == pattern[j])
            {
                // Збільшуємо довжину поточного префікса.
                j++;
            }

            // Записуємо знайдене значення для позиції i.
            prefix[i] = j;
        }

        // Повертаємо готову префікс-функцію.
        return prefix;
    }

    // Пошук підрядка алгоритмом Кнута-Морріса-Пратта.
    private static int KnuthMorrisPrattSearch(string text, string pattern, int[]? prefix = null)
    {
        // Якщо масив prefix не передали, будуємо його всередині методу.
        prefix ??= BuildPrefixFunction(pattern);

        // Індекс поточного символу тексту.
        int textIndex = 0;

        // Індекс поточного символу підрядка.
        int patternIndex = 0;

        // Поки ми не дійшли до кінця тексту.
        while (textIndex < text.Length)
        {
            // Якщо поточні символи тексту і підрядка однакові.
            if (text[textIndex] == pattern[patternIndex])
            {
                // Переходимо до наступного символу тексту.
                textIndex++;

                // Переходимо до наступного символу підрядка.
                patternIndex++;

                // Якщо patternIndex дійшов до кінця підрядка, значить весь підрядок знайдено.
                if (patternIndex == pattern.Length)
                {
                    // Повертаємо індекс початку входження.
                    return textIndex - patternIndex;
                }
            }
            // Якщо символи не збіглись і ми стоїмо на початку підрядка.
            else if (patternIndex == 0)
            {
                // Просто рухаємось далі по тексту.
                textIndex++;
            }
            // Якщо символи не збіглись, але частина підрядка вже збіглася.
            else
            {
                // Не повертаємось у початок, а беремо нову позицію з prefix-функції.
                patternIndex = prefix[patternIndex - 1];
            }
        }

        // Якщо дійшли до кінця тексту і збіг не знайдено, повертаємо -1.
        return -1;
    }

    // Метод красиво виводить результат пошуку.
    private static void PrintSearchResult(string algorithmName, int index)
    {
        // Якщо індекс не від'ємний, то підрядок знайдено.
        if (index >= 0)
        {
            // Виводимо позицію у двох форматах: людську (з 1) і програмну (з 0).
            Console.WriteLine($"{algorithmName}: substring found at position {index + 1} (index {index}).");

            // Завершуємо метод, щоб не виконувати код нижче.
            return;
        }

        // Якщо index = -1, виводимо повідомлення, що збіг відсутній.
        Console.WriteLine($"{algorithmName}: substring not found.");
    }

    // Метод друкує підрядок і його префікс-функцію.
    private static void PrintPrefixFunction(string pattern, int[] prefix)
    {
        // Друкуємо заголовок блоку.
        Console.WriteLine("\nPrefix function for the substring:");

        // Виводимо символи підрядка через пробіл, щоб їх було зручно зіставити з числами prefix.
        Console.WriteLine($"Pattern: {string.Join(' ', pattern.ToCharArray())}");

        // Виводимо сам масив prefix.
        Console.WriteLine($"Prefix : {string.Join(' ', prefix)}");
    }
}
```

---

## Найважливіше, що треба запам'ятати

- **Прямий пошук** простіший для розуміння, але повільніший.
- **KMP** складніший, зате ефективніший.
- **Префікс-функція** дозволяє не починати порівняння підрядка з нуля після кожної помилки.
- Якщо метод повертає `-1`, це означає, що підрядок у рядку не знайдено.
- У програмі виводяться дві позиції:
  - `position` — зручна для людини нумерація з `1`;
  - `index` — стандартна для C# нумерація з `0`.

---

## Висновок

У цій лабораторній роботі показано два різні підходи до пошуку підрядка:

- простий і зрозумілий;
- швидший і алгоритмічно сильніший.

Тобто лабораторна не просто шукає слово в рядку, а ще й показує різницю між наївним підходом і оптимізованим алгоритмом.
