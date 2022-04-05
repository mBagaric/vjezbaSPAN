using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features
{
    class Programm
    {
        static void Main(string[] args)
        {
            //#1 Employee[] developers = new Employee[]
            var developers = new Employee[]
            {
                new Employee{Id = 1, Name = "Scott" },
                new Employee{Id = 2, Name = "Chris" }
            };

            //#1 List<Employee> sales = new List<Employee>()
            var sales = new List<Employee>()
            {
                new Employee{Id = 3, Name = "Alex" }
            };

            /* #2 ---------------------------------------------------------------
             * foreach (var person in developers)
            {
                Console.WriteLine(person.Name);
            }*/

            /* #2 ---------------------------------------------------------------
            * Console.WriteLine(developers.Count());
            IEnumerator<Employee> enumerator = developers.GetEnumerator();
            while (enumerator.MoveNext()) 
            {
                Console.WriteLine(enumerator.Current.Name);
            }*/

            /* #3 ---------------------------------------------------------------
             *      Named Method
            foreach(var employee in developers.Where(NameStartsWithS))
            {
                Console.WriteLine(employee.Name);
            }
            */

            /* #3 ---------------------------------------------------------------
                    Anonymus Method
            foreach (var employee in developers.Where(
                delegate (Employee employee)
                {
                    return employee.Name.StartsWith("S");
                }))
            {
                Console.WriteLine(employee.Name);
            }*/

            /* #3 ---------------------------------------------------------------
             * Lambda expression
            foreach(var employee in developers.Where(
                e => e.Name.Length == 5).OrderBy(e => e.Name))
            {
                Console.WriteLine(employee.Name);
            }*/

            /* #4 ---------------------------------------------------------------
            Func<int, int> f = x => x * x;
            Console.WriteLine(f(3));

            Func<int, int, int> add = (x, y) =>
            {
                int temp = x + y;
                return temp;
            };
            Console.WriteLine(f(add(3, 5)));

            Action<int> write = x => Console.WriteLine(x);
            write(f(add(3, 5)));*/

            /* #5 ---------------------------------------------------------------
             * */
            var query = developers.Where(e => e.Name.Length == 5)
                                    .OrderByDescending(e => e.Name)
                                    .Select(e => e);

            var query2 = from developer in developers
                         where developer.Name.Length == 5
                         orderby developer.Name descending
                         select developer;

            foreach(var employee in query2)
            {
                Console.WriteLine(employee.Name);
            }

        }

        private static bool NameStartsWithS(Employee employee)
        {
            return employee.Name.StartsWith("S");
        }
    }
}

/*
    #1 Pokazivanje što je zapravo IEnumerable

    #2 Pokazivanje starog načina ispisa pomoću for petlje i korisnijeg načina preko
        enumerator.MoveNext() koja nije ograničena o broju atributa unutar petlje

    #3 Filtriranje podataka i dolazak do lambda expressiona

    #4 Proučavanje Func<> typa i kako funkcionira

    #5 Rad sa queryjem
 */
