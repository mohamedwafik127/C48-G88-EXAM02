using System;
using System.Collections.Generic;
using System.Text;
public class HelperClass
{
    public static int ReadInt(string prompt)
    {
        int result;
        while (true)
        {
            Console.Write(prompt);
            string input = Console.ReadLine();

            if (int.TryParse(input, out result))
            {
                return result;
            }

            Console.WriteLine("Invalid number, please try again.");
        }
    }

    public static string ReadNonEmptyString(string prompt)
    {
        string input;
        while (true)
        {
            Console.Write(prompt);
            input = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(input))
            {
                return input;
            }

            Console.WriteLine("Input cannot be empty, please try again.");
        }
    }
}