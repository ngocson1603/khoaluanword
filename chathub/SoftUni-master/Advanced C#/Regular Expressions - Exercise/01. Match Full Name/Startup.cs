﻿namespace _01.Match_Full_Name
{
    using System;

    public class Startup

            while (true)
            {
                string input = Console.ReadLine();
                if (input == "end") break;

                var matches = regex.Matches(input);

                foreach (Match match in matches)
                {
                    Console.WriteLine(match.Groups[1].Value);
                }
            }
        }
}