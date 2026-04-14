using System;
using System.Collections.Generic;

namespace Lab04.PracticalTasks;

public static class Task1
{
    /*
     * Завдання: Написати програму для перевірки, чи можна додати до неї цифри 
     * та знаки арифметичних дій так, щоб вийшов правильний арифметичний вираз.
     * 
     * Пояснення: Це класична задача на правильність дужкової послідовності.
     * Якщо баланс дужок правильний (наприклад "()()", "(())"), то між ними
     * завжди можна вставити цифри та знаки (наприклад "(1+2)*(3)"). 
     * Якщо ж дужки не збалансовані (наприклад ")(", "(()"), то з них неможливо
     * створити правильний арифметичний вираз.
     */
    public static void Execute()
    {
        Console.WriteLine("\n--- Завдання 1: Валідація майбутнього виразу ---");
        Console.Write("Введіть рядок (тільки дужки, наприклад '(()())'): ");
        string? input = Console.ReadLine();

        if (string.IsNullOrEmpty(input))
        {
            Console.WriteLine("Рядок порожній.");
            return;
        }

        bool isValid = CheckIfCanFormExpression(input);

        if (isValid)
            Console.WriteLine("Так, до цього рядка можна додати цифри і знаки, щоб утворити правильний вираз.");
        else
            Console.WriteLine("Ні, з цього рядка неможливо утворити правильний вираз (дужки не збалансовані).");
    }

    private static bool CheckIfCanFormExpression(string s)
    {
        Stack<char> stack = new Stack<char>();

        foreach (char c in s)
        {
            if (c == '(' || c == '{' || c == '[')
            {
                stack.Push(c);
            }
            else if (c == ')' || c == '}' || c == ']')
            {
                if (stack.Count == 0) return false;
                
                char top = stack.Pop();
                if (!IsMatchingPair(top, c))
                    return false;
            }
        }

        return stack.Count == 0;
    }

    private static bool IsMatchingPair(char open, char close)
    {
        return (open == '(' && close == ')') ||
               (open == '{' && close == '}') ||
               (open == '[' && close == ']');
    }
}
