﻿namespace _05.Min_Even_Number
{
    using System;
    using System.Linq;

    public class Startup
                                        .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                                        .Select(double.Parse)
                                        .Where(x=>x%2==0);

            {
                Console.WriteLine($"{numbers.Min():f2}");
            }
            {
                Console.WriteLine("No match");
            }
}