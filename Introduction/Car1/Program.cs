using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Linq;
using System.Data.Entity;

namespace Cars1
{
    class Program
    {
        static void Main(string[] args)
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<CarDb>());
            InsertData();
            queryData();
        }

        private static void queryData()
        {
            var db = new CarDb();
            db.Database.Log = Console.WriteLine;

            //Dohvaćanje iz baze podataka
            //var query =
            //    from car in db.Cars
            //    orderby car.Combined descending, car.Name ascending
            //    select car;

            //Dohvaćanje na drugi način
            var query =
                db.Cars.Where(c => c.Manufacturer == "BMW")
                       .OrderByDescending(c => c.Combined)
                       .ThenBy(c => c.Name)
                       .Take(10)
                       .ToList();
            //Korisno je staviti .ToList() ukoliko više puta trebam koristiti query
            //kako se ne bi svaki put morao izvoditi iz početka

            foreach(var car in query)
            {
                Console.WriteLine($"{car.Name}:  {car.Combined}");
            }
        }

        private static void InsertData()
        {
            var cars = ProcessCars("fuel.csv");
            var db = new CarDb();
            //db.Database.Log = Console.WriteLine;

            if (!db.Cars.Any())
            {
                foreach(var car in cars)
                {
                    db.Cars.Add(car);
                }
                db.SaveChanges();
            }
        }

        private static void QueryXml()
        {
            var document = XDocument.Load("fuel.xml");
            var ns = (XNamespace)"https://pluralsight.com/cars/2016";
            var ex = (XNamespace)"https://pluralsight.com/cars/2016/ex";

            var query =
                from element in document.Element(ns + "Cars").Elements(ex + "Car")
                //from element in document.Descendants("Car")

                //Ako nisi siguran na postoji započinješ s:
                //from element in document.Element("Cars")?.Elements(ex + "Car")
                                                        //?? Enumerable.Empty<XElement>()
                where element.Attribute("Manufacturer")?.Value == "BMW"
                select element.Attribute("Name").Value;

            foreach(var name in query)
            {
                Console.WriteLine(name);
            }


        }

        private static void CreateXml()
        {
            var records = ProcessCars("fuel.csv");

            var ns = (XNamespace)"https://pluralsight.com/cars/2016";
            var ex = (XNamespace)"https://pluralsight.com/cars/2016/ex";
            //Dodavanje xml dokumenta i uređivanje
            var document = new XDocument();
            var cars = new XElement(ns + "Cars");

            //Spremanje u xml file, ali loše jer ima dosta koda i mora proći kroz cijelu listu
            //foreach(var record in records)
            //{
            //    var car = new XElement("Car");
            //    var name = new XAttribute("Name", record.Name);
            //    var combined = new XAttribute("Combined", record.Combined);

            //    car.Add(name);
            //    car.Add(combined);

            //    cars.Add(car);
            //}

            //Nešto bolji način
            //foreach (var record in records)
            //{
            //    var name = new XAttribute("Name", record.Name);
            //    var combined = new XAttribute("Combined", record.Combined);
            //    var car = new XElement("Car", name, combined);

            //    cars.Add(car);
            //}

            //Valjd najbolja opcija ako i dalje želiš ptolazit kroz svaki element
            //foreach (var record in records)
            //{
            //    var car = new XElement("Car",
            //                    new XAttribute("Name", record.Name),
            //                    new XAttribute("Combined", record.Combined),
            //                    new XAttribute("Manufacturer", record.Manufacturer));

            //    cars.Add(car);
            //}

            var element =
                from record in records
                select new XElement(ex + "Car",
                                new XAttribute("Name", record.Name),
                                new XAttribute("Combined", record.Combined),
                                new XAttribute("Manufacturer", record.Manufacturer));

            cars.Add(new XAttribute(XNamespace.Xmlns + "ex", ex));

            cars.Add(element);
            document.Add(cars);
            document.Save("fuel.xml");
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
