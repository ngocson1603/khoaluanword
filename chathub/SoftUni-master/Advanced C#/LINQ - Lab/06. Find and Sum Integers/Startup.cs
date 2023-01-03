﻿namespace _06.Find_and_Sum_longegers
{
    using System;
    using System.Linq;

    public class Startup

            var result = args.Select(
                            s =>
                            {
                                long n;
                                if (!long.TryParse((s ?? string.Empty), out n))
                                {
                                    return (long?)null;
                                }
                                return (long?)n;
                            }
                        )
                        .Where(n => n != null)
                        .Select(n => n.Value)
                        .ToList();

            if (result.Count>0)
            {
                Console.WriteLine(result.Sum());
            }
            else
            {
                Console.WriteLine("No match");
            }
        }
}