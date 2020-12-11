using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

namespace IntroductionToLinq
{
    class Program
    {
        static void Main(string[] args)
        {
            //string path = @"C:\windows";
            //ShowLageFilesWithoutLinq(path);
            //Console.WriteLine("******");
            //ShowLageFilesWithLinq(path);

            Func<int, int> square = x => x * x;
            //Func<int, int, int> Add = (x, y) => x + y;
            Func<int, int, int> Add = (x, y) => {
                var temp = x + y;
                return temp;
            };
            Console.WriteLine(square(3));
            Console.WriteLine(Add(3, 4));

            //Action method take one param and return void

            Action<int> Write=x=>Console.WriteLine(x);
            Write(2);


            IEnumerable<Employee> developers = new Employee[]
            {
                new Employee {Id=1,Name="Scott"},
                new Employee{Id=2,Name="Zuko"}
            };

            IEnumerable<Employee> sales = new List<Employee>()
            {
                new Employee{Id=3,Name="Zzuko"}
            };

            Console.WriteLine(sales.Count());

            IEnumerator<Employee> enumerator = developers.GetEnumerator();
            while(enumerator.MoveNext())
            {
                Console.WriteLine(enumerator.Current.Name);
            }
           
            //use named method
            foreach(var employee in developers.Where(NameStartsWithS))
            {
                Console.WriteLine(employee.Name);
            }

            //use anonymous method

            foreach (var employee in developers.Where(
                delegate (Employee employee)
            {
                return employee.Name.StartsWith("S");
            }))
            {
                Console.WriteLine(employee.Name);
            }

            //use lambda exp

            foreach (var employee in developers.Where(
                e=>e.Name.StartsWith("S")))
            {
                Console.WriteLine(employee.Name);
            }

            var query2 = from developer in developers
                         where developer.Name.Length == 5
                         orderby developer.Name descending
                         select developer;
            Console.WriteLine(query2.Count());
            foreach(var dev in query2)
            {
                Console.WriteLine(dev.Name);
            }


            Console.WriteLine("****************************************************************");

            var movies = new List<Movie>
            {
                new Movie{Title="The Dark Knight",Rating=8.9f,Year=2021}    ,
                  new Movie{Title="The Dark Hallow",Rating=8.5f,Year=2021} ,
                    new Movie{Title="The Light Knight",Rating=8.4f,Year=2000}

            };
            var query = movies.Where(m => m.Year > 2000);

            foreach(var m in query)
            {
                Console.WriteLine(m.Title);
            }

            Console.WriteLine("*****************");

            var query3= movies.Filter(m => m.Year > 2000);

            //foreach (var m in query3)
            //{
            //    Console.WriteLine(m.Title);
            //}

            var enumerator1 = query3.GetEnumerator();
            while (enumerator1.MoveNext())
            {
                Console.WriteLine(enumerator1.Current.Title);
            }


        }

      

        //Namedmethod
        private static bool NameStartsWithS(Employee arg)
        {
            return arg.Name.StartsWith("S");
        }

        private static void ShowLageFilesWithLinq(string path)
        {
            //var query = from file in new DirectoryInfo(path).GetFiles()
            //            orderby file.Length descending
            //            select file;
            var query = new DirectoryInfo(path).GetFiles()
                      .OrderByDescending(f => f.Length)
                      .Take(5);
            foreach (var file in query)
            {
                Console.WriteLine($"{file.Name,-20} : {file.Length,10:N0}");
            }

            //foreach(var file in query.Take(5))
            //{
            //    Console.WriteLine($"{file.Name,-20} : {file.Length,10:N0}");
            //}

        }

        private static void ShowLageFilesWithoutLinq(string path)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            FileInfo[] files=dir.GetFiles();
            Array.Sort(files, new FileInfoComparer());
            //foreach(FileInfo file in files)
            //{
            //    Console.WriteLine($"{file.Name} : {file.Length}");
            //}
            for (int i=0;i<5;i++)
            {
                FileInfo file = files[i];
                Console.WriteLine($"{file.Name, -20} : {file.Length,10:N0}");
            }
        }
    }
    public class FileInfoComparer:IComparer<FileInfo>
    {
        public int Compare (FileInfo x,FileInfo y)
        {
            return y.Length.CompareTo(x.Length);
        }

       
    }
}
