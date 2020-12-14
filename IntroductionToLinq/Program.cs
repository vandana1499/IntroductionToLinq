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


            Console.WriteLine("------------------------------------------------------------------");

            var cars = ProcessFile("fuel.csv");
            foreach(var car in cars)
            {
                Console.WriteLine(car.Name);
            }
            Console.WriteLine("fuel efiicient --------------");
            var queryCars = cars.OrderByDescending(c => c.Combined)
                             .ThenBy(c => c.Name);
            //then by gives the capability of secondary sort

            foreach (var car in queryCars.Take(10))
            {
                Console.WriteLine($"{car.Name}-{car.Combined}");
            }

            Console.WriteLine("--------------");
            //with query
            var manufacturers = ProcessManufacturer("manufacturers.csv");
            var queryCars2 = from car in cars
                             join manufacturer in manufacturers
                                on car.Manufacturer equals manufacturer.Name
                             orderby car.Combined descending, car.Name ascending
                             select new
                             {
                                 manufacturer.Headquarters,
                                 car.Name,
                                 car.Combined
                             };
            foreach (var car in queryCars2.Take(10))
            {
                Console.WriteLine($"{car.Name}-{car.Combined}-{car.Headquarters}");
            }

            Console.WriteLine("-------------------------------------------------------");

            var queryCars3 =
                            cars.Join(manufacturers, c => c.Manufacturer, m => m.Name, (c, m) => new
                            {
                                Car = c,
                                Manufacturer = m
                            })
                            .OrderByDescending(c => c.Car.Combined)
                            .ThenBy(c => c.Car.Name)
                            .Select(c => new
                            {
                                c.Manufacturer.Headquarters,
                                c.Car.Name,
                                c.Car.Combined
                            });

            foreach (var car in queryCars3.Take(10))
            {
                Console.WriteLine($"{car.Name}-{car.Combined}-{car.Headquarters}");
            }
            Console.WriteLine("---------------------------------------------------");

            //to join on two things 
            //to check the equality both side should have same property like year and Manufacturer
            var queryCars4 = from car in cars
                             join manufacturer in manufacturers
                                on new { car.Manufacturer,car.Year } equals new { Manufacturer= manufacturer.Name,manufacturer.Year }
                             orderby car.Combined descending, car.Name ascending
                             select new
                             {
                                 manufacturer.Headquarters,
                                 car.Name,
                                 car.Combined
                             };
            foreach (var car in queryCars4.Take(10))
            {
                Console.WriteLine($"{car.Name}-{car.Combined}-{car.Headquarters}");
            }

            Console.WriteLine("-------------------------------------------------------");

            var queryCars5 =
                            cars.Join(manufacturers,
                            c => new { c.Manufacturer, c.Year },
                            m => new { Manufacturer=m.Name,m.Year }, 
                            (c, m) => new
                            {
                                Car = c,
                                Manufacturer = m
                            })
                            .OrderByDescending(c => c.Car.Combined)
                            .ThenBy(c => c.Car.Name)
                            .Select(c => new
                            {
                                c.Manufacturer.Headquarters,
                                c.Car.Name,
                                c.Car.Combined
                            });

            foreach (var car in queryCars5.Take(10))
            {
                Console.WriteLine($"{car.Name}-{car.Combined}-{car.Headquarters}");
            }
            Console.WriteLine("---------------------------------------------------");


            var queryCars6 =
                           from car in cars
                           group car by car.Manufacturer.ToUpper() into manufacture
                           orderby manufacture.Key
                           select manufacture;
                           


            foreach (var group in queryCars6)
            {
                Console.WriteLine(group.Key);
                foreach(var car in group.OrderByDescending(c=>c.Combined).Take(2))
                {
                    Console.WriteLine($"\t{car.Name}:{car.Combined}");
                }
            }

            Console.WriteLine("---------------------------------------------------");


            var queryCars7 =
                          cars.GroupBy(c => c.Manufacturer.ToUpper())
                          .OrderBy(g => g.Key);




            foreach (var group in queryCars7)
            {
                Console.WriteLine(group.Key);
                foreach (var car in group.OrderByDescending(c => c.Combined).Take(2))
                {
                    Console.WriteLine($"\t{car.Name}:{car.Combined}");
                }
            }


            Console.WriteLine("-----------------------Group join----------------------------");


            var queryCars8 =
                          from manufacturer in manufacturers
                          join car in cars on manufacturer.Name equals car.Manufacturer
                            into carGroup
                          select new
                          {
                              Manufacturer = manufacturer,
                              Cars = carGroup
                          };




            foreach (var group in queryCars8)
            {
                Console.WriteLine(group.Manufacturer.Name);
                foreach (var car in group.Cars.OrderByDescending(c => c.Combined).Take(2))
                {
                    Console.WriteLine($"\t{car.Name}:{car.Combined}");
                }
            }


            Console.WriteLine("-----------------------Group join----------------------------");


            var queryCars9 =
                          manufacturers.GroupJoin(cars, m => m.Name, c => c.Manufacturer, (m, g) => new
                          {
                              Cars = g,
                              Manufacturer = m
                          })
                          .OrderBy(m => m.Manufacturer.Name);




            foreach (var group in queryCars9)
            {
                Console.WriteLine(group.Manufacturer.Name);
                foreach (var car in group.Cars.OrderByDescending(c => c.Combined).Take(2))
                {
                    Console.WriteLine($"\t{car.Name}:{car.Combined}");
                }
            }


            Console.WriteLine("-----------------------Group join----------------------------");


            var queryCars10 =
                        from car in cars
                        group car by car.Manufacturer into carGroup
                        select new
                        {
                            Name = carGroup.Key,
                            Max = carGroup.Max(c => c.Combined),
                            Min = carGroup.Min(c => c.Combined),
                            Avg=carGroup.Average(c=>c.Combined)
                        };


            foreach (var result3 in queryCars10)
            {
                Console.WriteLine(result3.Name);
                Console.WriteLine(result3.Max);
                Console.WriteLine(result3.Min);
                Console.WriteLine(result3.Avg);

            }

            var top =
                 cars.Where(c => c.Manufacturer == "BMW" && c.Year == 2016)
                 .OrderByDescending(c => c.Combined)
                 .ThenBy(c => c.Name)
                 .Select(c => c)
                 .First();
            Console.WriteLine("-------------");
            Console.WriteLine(top.Name);

            var top1 =
               cars
               .OrderByDescending(c => c.Combined)
               .ThenBy(c => c.Name)
               .Select(c => c)
               .First(c => c.Manufacturer == "BMW" && c.Year == 2016);
            Console.WriteLine("-------------");
            Console.WriteLine(top1.Name);

            var result =
              cars.Any(c => c.Manufacturer == "Ford");
            Console.WriteLine("-------------");
            Console.WriteLine(result);
            var result2 =
          cars.All (c => c.Manufacturer == "Ford");
            Console.WriteLine("-------------");
            Console.WriteLine(result2);
        
    }

        private static List<Car> ProcessFile(string path)
        {
           return File.ReadAllLines(path)
                .Skip(1)
                .Where(line => line.Length >1)
                .Select(Car.ParseFromCsv)
                .ToList();
        }

        private static List<Manufacturer> ProcessManufacturer(string path)
        {
            var query = File.ReadAllLines(path)
                        .Where(l => l.Length > 1)
                        .Select(l =>
                        {
                            var col = l.Split(",");
                            return new Manufacturer
                            {
                                Name = col[0],
                                Headquarters = col[1],
                                Year = int.Parse(col[2])
                            };
                        });
            return query.ToList();
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
