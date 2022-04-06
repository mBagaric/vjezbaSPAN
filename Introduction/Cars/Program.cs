using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Cars
{
    class Program
    {
        static void Main(string[] args)
        {
            var cars = ProcessCars("fuel.csv");
            var manufacturers = ProcessManufacturer("manufacturers.csv");

            //----------------------------------------------
            //Efficient Agregating data with extension method
            var query2 =
                cars.GroupBy(c => c.Manufacturer)
                    .Select(g =>
                    {
                        var results = g.Aggregate(new CarStatistics(),
                                        (acc, c) => acc.Accumilate(c),
                                        acc => acc.Compute());

                        return new
                        {
                            Name = g.Key,
                            Avg = results.Average,
                            Min = results.Min,
                            Max = results.Max
                        };

                    }).OrderByDescending(r => r.Max);


            //----------------------------------------------
            //Agregating data
            //var query =
            //    from car in cars
            //    group car by car.Manufacturer into carGroup
            //    select new
            //    {
            //        Name = carGroup.Key,
            //        Max = carGroup.Max(c => c.Combined),
            //        Min = carGroup.Min(c => c.Combined),
            //        Avg = carGroup.Average(c => c.Combined)
            //    } into result
            //    orderby result.Max descending
            //    select result;

            foreach (var result in query2)
            {
                Console.WriteLine($"{result.Name}");
                Console.WriteLine($"\t Max: {result.Max}");
                Console.WriteLine($"\t Min: {result.Min}");
                Console.WriteLine($"\t Avg: {result.Avg}");
            }


            //----------------------------------------------
            //Dvostruki grouping i kako funkcionira

            //var query =
            //    from manufacturer in manufacturers
            //    join car in cars on manufacturer.Name equals car.Manufacturer
            //        into carGroup
            //    select new
            //    {
            //        Manufacturer = manufacturer,
            //        Cars = carGroup
            //    } into result
            //    group result by result.Manufacturer.Headquarters;

            //var query2 =
            //    manufacturers.GroupJoin(cars, m => m.Name, c => c.Manufacturer, (m, g) =>
            //        new
            //        {
            //            Manufacturer = m,
            //            Cars = g
            //        }).GroupBy(m => m.Manufacturer.Headquarters);

            //----------------------------------------------
            //GroupJoin

            //var query =
            //    from manufacturer in manufacturers
            //    join car in cars on manufacturer.Name equals car.Manufacturer
            //        into carGroup
            //    select new
            //    {
            //        Manufacturer = manufacturer,
            //        Cars = cars
            //    };

            //var query2 =
            //    manufacturers.GroupJoin(cars, m => m.Name, c => c.Manufacturer, (m, g) =>
            //        new
            //        {
            //            Manufacturer = m,
            //            Cars = g
            //        }).OrderBy(m => m.Manufacturer.Name);

            //----------------------------------------------
            //Grouping Data

            //var query =
            //    from car in cars
            //    group car by car.Manufacturer.ToUpper()
            //    into manufacturer
            //    orderby manufacturer.Key
            //    select manufacturer;

            //var query2 =
            //    cars.GroupBy(c => c.Manufacturer.ToUpper())
            //        .OrderBy(g => g.Key);

            //foreach(var result in query)
            //{
            //    Console.WriteLine($"{result.Key} har {result.Count()} cars");
            //}

            //foreach(var group in query)
            //{
            //    Console.WriteLine($"{group.Key}");
            //    foreach (var car in group.SelectMany(g => g.Cars)
            //                            .OrderByDescending(c => c.Combined)
            //                            .Take(3))
            //    {
            //        Console.WriteLine($"\t{car.Name} : {car.Combined}");
            //    }
            //}

            //var query =
            //    from car in cars
            //    join manufacturer in manufacturers
            //        on new { car.Manufacturer, car.Year }
            //        equals
            //        new { Manufacturer = manufacturer.Name, manufacturer.Year }
            //    orderby car.Combined descending, car.Name ascending
            //    select new
            //    {
            //        manufacturer.Headquarters,
            //        car.Name,
            //        car.Combined
            //    };

            //var query2 =
            //    cars.Join(manufacturers,
            //                c => new { c.Manufacturer, c.Year },
            //                m => new { Manufacturer = m.Name, m.Year },
            //                (c, m) => new
            //                {
            //                    m.Headquarters,
            //                    c.Name,
            //                    c.Combined
            //                })
            //        .OrderByDescending(c => c.Combined)
            //        .ThenBy(c => c.Name);

            //var query2 =
            //    cars.Join(manufacturers,
            //                c => c.Manufacturer,
            //                m => m.Name,
            //                (c, m) => new
            //                {
            //                    Car = c,
            //                    Manufacturer = m
            //                })
            //        .OrderByDescending(c => c.Car.Combined)
            //        .ThenBy(c => c.Car.Name)
            //        .Select(c => new
            //        {
            //            c.Manufacturer.Headquarters,
            //            c.Car.Name,
            //            c.Car.Combined
            //        });

            //foreach (var car in query2.Take(10))
            //{
            //    Console.WriteLine($"{car.Headquarters} {car.Name} : {car.Combined}");
            //}
        }

        private static List<Car> ProcessCars(string path)
        {
            var query =
                File.ReadAllLines(path)
                    .Skip(1)
                    .Where(l => l.Length > 1)
                    .ToCar();

            return query.ToList();
        }

        private static List<Manufacturer> ProcessManufacturer(string path)
        {
            var query =
                File.ReadLines(path)
                    .Where(l => l.Length > 1)
                    .Select(l =>
                    {
                        var columns = l.Split(',');
                        return new Manufacturer
                        {
                            Name = columns[0],
                            Headquarters = columns[1],
                            Year = int.Parse(columns[2])
                        };
                    });
            return query.ToList();
        }
    }

    public class CarStatistics 
    {
        public CarStatistics()
        {
            Max = Int32.MinValue;
            Min = Int32.MaxValue;
        }
        public int Max { get; set; }
        public int Min { get; set; }
        public double Average { get; set; }
        public int Total { get; set; }
        public int Count { get; set; }

        public CarStatistics Accumilate(Car c)
        {
            Count += 1;
            Total += c.Combined;
            Max = Math.Max(Max, c.Combined);
            Min = Math.Min(Min, c.Combined);

            return this;
        }

        public CarStatistics Compute()
        {
            Average = Total / Count;
            return this;
        }
    }

    public static class CarExtensions
    {
        public static IEnumerable<Car> ToCar(this IEnumerable<string> source)
        {

            foreach (var line in source)
            {
                var column = line.Split(',');
                yield return new Car
                {
                    Year = int.Parse(column[0]),
                    Manufacturer = column[1],
                    Name = column[2],
                    Displacement = double.Parse(column[3]),
                    Cylinders = int.Parse(column[4]),
                    City = int.Parse(column[5]),
                    Highway = int.Parse(column[6]),
                    Combined = int.Parse(column[7]),
                };
            }
        }
    }
}

/*  Primjer 5. predavanja Filter, Ordering, Projecting
 * 
 * namespace Cars
{
    class Program
    {
        static void Main(string[] args)
        {
            var cars = ProcessFile("fuel.csv");

            //var query = cars.OrderByDescending(c => c.Combined)
            //                .ThenBy(c => c.Name);

            var query =
                from car in cars
                where car.Manufacturer == "BMW" && car.Year == 2016
                orderby car.Combined descending, car.Name ascending
                select new
                {
                    car.Manufacturer,
                    car.Name,
                    car.Combined
                };

            //var top = cars.Where(c => c.Manufacturer == "BMW" && c.Year == 2016)
            //                 .OrderByDescending(c => c.Combined)
            //                 .ThenBy(c => c.Name)
            //                 .Select(c => c)
            //                 .First();

            //Console.WriteLine(top.Name);

            //var top = cars.Any(c => c.Manufacturer == "Ford");
            //var result = cars.All(c => c.Manufacturer == "Ford");

            //Console.WriteLine(top);
            //Console.WriteLine(result);

            var result = cars.Select(c => c.Name);

            foreach(var item in result)
            {
                Console.WriteLine(item);
            }

            //foreach (var car in query.Take(10))
            //{
            //    Console.WriteLine($"{car.Manufacturer} {car.Name} : {car.Combined}");
            //}
        }

        private static List<Car> ProcessFile(string path)
        {
            //return File.ReadAllLines(path)
            //    .Skip(1)
            //    .Where(line => line.Length > 1)
            //    .Select(Car.ParseFromCsv)
            //    .ToList();

            //var query =
            //    from line in File.ReadAllLines(path).Skip(1)
            //    where line.Length > 1
            //    select Car.ParseFromCsv(line);

            var query =
                File.ReadAllLines(path)
                    .Skip(1)
                    .Where(l => l.Length > 1)
                    .ToCar();

            return query.ToList();
        }
    }
    public static class CarExtensions
    {
        public static IEnumerable<Car> ToCar(this IEnumerable<string> source)
        {

            foreach (var line in source)
            {
                var column = line.Split(',');
                yield return new Car
                {
                    Year = int.Parse(column[0]),
                    Manufacturer = column[1],
                    Name = column[2],
                    Displacement = double.Parse(column[3]),
                    Cylinders = int.Parse(column[4]),
                    City = int.Parse(column[5]),
                    Highway = int.Parse(column[6]),
                    Combined = int.Parse(column[7]),
                }; 
            }
        }
    }
}*/