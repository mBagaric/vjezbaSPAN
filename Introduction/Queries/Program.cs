using System;
using System.Collections.Generic;
using System.Linq;

namespace Queries
{
    class Program
    {
        static void Main(string[] args)
        {
            var numbers = MyLinq.Random().Where(x => x > 0.5).Take(10);
            foreach(var numb in numbers)
            {
                Console.WriteLine(numb);
            }

            var movies = new List<Movie>
            {
                new Movie { Title = "The Dark Knight", Rating = 8.9f, Year = 2008},
                new Movie { Title = "The King's Speech", Rating = 8.0f, Year = 2010},
                new Movie { Title = "Casablanca", Rating = 8.5f, Year = 1942},
                new Movie { Title = "star Wars V", Rating = 8.7f, Year = 1980}
            };

            var query = movies.Where(m => m.Year > 2000)
                                .OrderByDescending(m => m.Rating);

            /* #1 ------------------------------------------------------
             * foreach (var movie in query)
             {
                 Console.WriteLine(movie.Title);
             }*/

            // #2 ------------------------------------------------------
            var enumerator = query.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Console.WriteLine(enumerator.Current.Title);
            }
        }
    }
}

/*
    #1 Početak obrade s queryjima i usporedba Linq i Custom filtera te njihovih ispisa

    #2 Problem je kada stavimo više puta pozivajne queryja, on onda i više puta prođe kroz
        kroz istu listu i time usporava rad programa. Zato Linq funkcije ne moraju uvijek
        biti najbolje
 */
