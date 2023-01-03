﻿

namespace Namespace
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    class ArrayManipulator
    {
        static void Main()
        {
            List<int> nums = Console.ReadLine().Split().Select(int.Parse).ToList();

            string command;
            while ((command = Console.ReadLine()) != "print")
            {
                List<string> param = command.Split().ToList();
                string type = param[0];

                if (type == "add")
                {
                    int index = int.Parse(param[1]);
                    int element = int.Parse(param[2]);

                    if (index == nums.Count)
                    {
                        nums.Add(element);
                    }
                    else
                    {
                        nums.Insert(index, element);
                    }
                }
                else if (type == "addMany")
                {
                    int index = int.Parse(param[1]);
                    param.RemoveRange(0, 2);
                    var range = param.Select(int.Parse);
                    nums.InsertRange(index, range);
                }
                else if (type == "contains")
                {
                    int element = int.Parse(param[1]);
                    Console.WriteLine(nums.IndexOf(element));
                }
                else if (type == "remove")
                {
                    int index = int.Parse(param[1]);

                    nums.RemoveAt(index);
                }
                else if (type == "shift")
                {
                    int positions = int.Parse(param[1]);
                    positions %= nums.Count;

                    nums.AddRange(nums.GetRange(0, positions));
                    nums.RemoveRange(0, positions);
                }
                else if (type == "sumPairs")
                {
                    if (nums.Count % 2 != 0)
                    {
                        nums.Add(0);
                    }
                    nums = nums.Where((c, i) => i % 2 != 0)
                               .Zip(nums
                                   .Where((c, i) => i % 2 == 0),
                                          (x, y) => x + y)
                               .ToList();
                }
            }
            StringBuilder sb = new StringBuilder();

            sb.Append("[");
            for (int i = 0; i < nums.Count; i++)
            {
                sb.Append(nums[i]);
                if (i!=nums.Count-1)
                {
                    sb.Append(", ");
                }
            }
            sb.Append("]");

            Console.WriteLine(sb);
        }
    }
}