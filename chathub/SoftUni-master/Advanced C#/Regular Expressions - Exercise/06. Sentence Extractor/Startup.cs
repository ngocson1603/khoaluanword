﻿namespace _06.Sentence_Extractor
{
    using System;

    public class Startup
            string input = Console.ReadLine();

            Regex regex = new Regex($"[^\\.!\\?]*\\b{keyword}\\b[^\\.!\\?]*(\\.|!|\\?)");

            var matches = regex.Matches(input);

            foreach (Match match in matches)
            {
                Console.WriteLine(match.Value);
            }
        }
}