﻿namespace _03IlluminatiLock
{
    using System;

    public class IlluminatiLock
    {
        public static void Main()
        {
            int n = int.Parse(Console.ReadLine());
            int length = 3 * n;

            string sideStart = new string('.', n);
            string middleStart = new string('#', n);
            Console.WriteLine("{0}{1}{0}", sideStart, middleStart);

            for (int i = 1; i <= n / 2; i++)
            {
                int edgeSpace = n - (2 * i);
                int midSpace = n - 2;
                int sideSpace = (length - (2 * edgeSpace) - midSpace - 6) / 2;
                string edge = new string('.', edgeSpace);
                string middle = new string('.', midSpace);
                string side = new string('.', sideSpace);
                Console.WriteLine("{0}##{1}#{2}#{1}##{0}", edge, side, middle);
            }

            for (int i = n / 2; i > 0; i--)
            {
                int edgeSpace = n - (2 * i);
                int midSpace = n - 2;
                int sideSpace = (length - (2 * edgeSpace) - midSpace - 6) / 2;
                string edge = new string('.', edgeSpace);
                string middle = new string('.', midSpace);
                string side = new string('.', sideSpace);
                Console.WriteLine("{0}##{1}#{2}#{1}##{0}", edge, side, middle);
            }

            Console.WriteLine("{0}{1}{0}", sideStart, middleStart);
			
			// Variant 2
			//
			//int height = n + 2;
            //for (int i = 0; i < height; i++)
            //{
            //    if (i == 0 || i == height - 1)
            //    {
            //        string side = new string('.', n);
            //        string middle = new string('#', n);
            //        Console.WriteLine("{0}{1}{0}", side, middle);
            //    }
            //    else if (i < height / 2)
            //    {
            //        int edgeSpace = n - (2 * i);
            //        int midspace = n - 2;
            //        int sideSpace = (length - (2 * edgeSpace) - 6 - midspace) / 2;
            //        string edge = new string('.', edgeSpace);
            //        string middle = new string('.', midspace);
            //        string side = new string('.', sideSpace);
            //        Console.WriteLine("{0}##{1}#{2}#{1}##{0}", edge, side, middle);
            //    }
            //    else if (i > height / 2)
            //    {
            //        int edgeSpace = n - ((height - i - 1) * 2);
            //        int midspace = n - 2;
            //        int sideSpace = (length - (2 * edgeSpace) - 6 - midspace) / 2;
            //        string edge = new string('.', edgeSpace);
            //        string middle = new string('.', midspace);
            //        string side = new string('.', sideSpace);
            //        Console.WriteLine("{0}##{1}#{2}#{1}##{0}", edge, side, middle);
            //    }
            //}

        }
    }
}
