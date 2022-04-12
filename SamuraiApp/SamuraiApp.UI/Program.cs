using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;
using SamuraiApp.Domain;

namespace SamuraiApp.UI
{
    public class Program
    {
        private static SamuraiContext _context = new SamuraiContext();
        private static SamuraiContextNoTracking _contextNT = new SamuraiContextNoTracking();

        private static void Main(string[] args)
        {
            //AddSamuraisByName("Shimada", "Okamoto", "Kikuchio", "Hayashida");
            //AddVariousTypes();
            //GetSamurais("After Add:");
            //Console.Write("Press any key...");
            //Console.ReadKey();
            //QueryFilters();
            //RetriveAndUpdateMultipleSamurais();
            //MultipleDatabaseOperations();
            //RetriveAndDeleteSamurai();
            //QueryAndUpdateBattles_Disconnected();
            //InsertNewSamuraiwithQuote();
            //InsertNewSamuraiWithManyQuotes();
            //AddQuoteToExistingSamuraiWhileTracked();
            //AddQuoteToExistingSamuraiNotTracked(5);
            //Simpler_AddQuoteToExistingSamuraiNotTracked(2);
            //EagerLoadSamuraiWithQuotes();
            //ProjectSomeProperties();
            //FilteringWithRelatedData();
            //ModifyingRelatedDataWhenTracked();
            ModifyingRelatedDataWhenNotTracked();
        }

        private static void AddSamuraisByName(params string[] names)
        {
            foreach (string name in names)
            {
                _context.Samurais.Add(new Samurai { Name = name });
            }
            _context.SaveChanges();
        }

        private static void GetSamurais(string text)
        {
            var samurais = _context.Samurais.ToList();
            Console.WriteLine($"{text}: samurai count is {samurais.Count}");
            foreach (var samurai in samurais)
            {
                Console.WriteLine(samurai.Name);
            }
        }

        private static void AddVariousTypes()
        {
            _context.AddRange(new Samurai { Name = "Shimada" },
                              new Samurai { Name = "Okamoto" },
                              new Battle { Name = "Battle of Anegawa" },
                              new Battle { Name = "Battle of Nagashino" });

            _context.SaveChanges();
        }

        private static void QueryFilters()
        {
            var name = "Sampson";
            //var samurais = _context.Samurais.Where(s => s.Name == name).ToList();

            /*var samurai = _context.Samurais.Where(samurais =>
                    EF.Functions.Like(samurais.Name, "J%")).FirstOrDefault();*/

            //var samurai = _context.Samurais.FirstOrDefault(s => s.Id == 2);

            //TRaženje preko id-a
            var samurai = _context.Samurais.Find(2);

            Console.WriteLine(samurai.Name);
        }

        private static void RetriveAndUpdateSamurai()
        {
            var samurai = _context.Samurais.FirstOrDefault();
            samurai.Name += "San";
            _context.SaveChanges();
        }

        private static void RetriveAndUpdateMultipleSamurais()
        {
            var samurais = _context.Samurais.Skip(1).Take(4).ToList();
            samurais.ForEach(s => s.Name += "San");
            _context.SaveChanges();
        }

        private static void MultipleDatabaseOperations()
        {
            var samurais = _context.Samurais.FirstOrDefault();
            samurais.Name += "San";
            _context.Samurais.Add(new Samurai { Name = "Shino" });
            _context.SaveChanges();
        }

        private static void RetriveAndDeleteSamurai()
        {
            var samurai = _context.Samurais.Find(15);
            _context.Samurais.Remove(samurai);
            _context.SaveChanges();
        }

        private static void QueryAndUpdateBattles_Disconnected()
        {
            List<Battle> disconnectedBattles;
            using (var context1 = new SamuraiContext())
            {
                disconnectedBattles = _context.Battles.ToList();
            }
            disconnectedBattles.ForEach(b =>
            {
                b.StartDate = new DateTime(1570, 01, 01);
                b.EndDate = new DateTime(1570, 12, 1);
            });

            using (var context2 = new SamuraiContext())
            {
                context2.UpdateRange(disconnectedBattles);
                context2.SaveChanges();
            }
        }

        private static void InsertNewSamuraiwithQuote()
        {
            var samurai = new Samurai
            {
                Name = "Kambei Shimada",
                Quotes = new List<Quote>
                {
                    new Quote { Text = "i've come to save you"}
                }
            };
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }

        private static void InsertNewSamuraiWithManyQuotes()
        {
            var samurai = new Samurai
            {
                Name = "Kyuzo",
                Quotes = new List<Quote>
                {
                    new Quote { Text = "Watch out for my sharp sword"},
                    new Quote { Text = "I told you to wathc out for the sharp sword! Oh well!"}
                }
            };
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }

        private static void AddQuoteToExistingSamuraiWhileTracked()
        {
            var samurai = _context.Samurais.FirstOrDefault();
            samurai.Quotes.Add(new Quote
            {
                Text = "I bet you're happy that I've saved you!"
            });
            _context.SaveChanges();
        }

        private static void AddQuoteToExistingSamuraiNotTracked(int samuraiId)
        {
            var samurai = _context.Samurais.Find(samuraiId);
            samurai.Quotes.Add(new Quote
            {
                Text = "Now that I saved you, will you feed me dinner?"
            });
            using (var newContext = new SamuraiContext())
            {
                newContext.Samurais.Attach(samurai);
                newContext.SaveChanges();
            }
        }

        private static void Simpler_AddQuoteToExistingSamuraiNotTracked(int samuraiId)
        {
            var quote = new Quote { Text = "Thanks for dinner!", SamuraiId = samuraiId };
            using var newContext = new SamuraiContext();
            newContext.Quotes.Add(quote);
            newContext.SaveChanges();
        }

        private static void EagerLoadSamuraiWithQuotes()
        {
            var samuraiwithQuotes = _context.Samurais.Include(s => s.Quotes).ToList();
        }

        private static void ProjectSomeProperties()
        {
            var someProperties = _context.Samurais
                .Select(s => new { s.Id, s.Name, NumberOfQuotes = s.Quotes }).ToList();
        }

        private static void FilteringWithRelatedData()
        {
            var samurais = _context.Samurais
                            .Where(s => s.Quotes.Any(q => q.Text.Contains("happy")))
                            .ToList();
        }

        private static void ModifyingRelatedDataWhenTracked()
        {
            var samurai = _context.Samurais.Include(s => s.Quotes)
                                           .FirstOrDefault(s => s.Id == 2);
            samurai.Quotes[0].Text = "Did you hear that?";
            _context.SaveChanges();
        }

        private static void ModifyingRelatedDataWhenNotTracked()
        {
            var samurai = _context.Samurais.Include(s => s.Quotes)
                                    .FirstOrDefault(s => s.Id == 2);
            var quote = samurai.Quotes[0];
            quote.Text = "Did you hear that again?";

            using var newContext = new SamuraiContext();
            newContext.Quotes.Update(quote);
            newContext.SaveChanges();
        }
    }
}
