﻿namespace _3.Count_Uppercase_Words
{
    using System;

    public class Startup

            words.Where(x => char.IsUpper(x.First()))
                            .ToList()
                            .ForEach(x => Console.WriteLine(x));
        }
}