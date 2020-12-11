﻿using System;
using System.Collections.Generic;
using System.Text;

namespace IntroductionToLinq
{
    public class Movie
    {
        public string Title { get; set; }
        public float Rating { get; set; }

        int _year;
        public int Year { get {
                Console.WriteLine($"Returning {Title}: {_year}");
                return _year;
            }
            set
            {
                _year = value;
            }
        }
    }
}
