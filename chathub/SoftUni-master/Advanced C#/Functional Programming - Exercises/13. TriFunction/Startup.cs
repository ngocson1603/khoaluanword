﻿namespace _13.TriFunction
{
    using System;

            string[] names = Console.ReadLine().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);


            Func<string, int, bool> test = (x, i) => SumChars(x) > i;

            Func<string[], int, Func<string, int, bool>, string> fiter = (arr, i, check) =>
            {
                foreach (var name in arr)
                {
                    if (check(name,i))
                    {
                        return name;
                    };
                }
                return null;
            };

            string result = fiter(names, number, test);

            Console.WriteLine(result);
        }

        static int SumChars(string name)
        {
            int sum = 0;
            foreach (var ch in name)
            {
                sum += ch;
            }
            return sum;
        }


}