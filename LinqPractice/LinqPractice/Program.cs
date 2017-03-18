using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqPractice
{
    public class Person
    {
        public string FN { get; set; }
        public string LN { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Problem1();
            Problem2();
            Problem3();
            Problem4();
            Problem5();
            var pleb = new
            {
                Leader = "Caesar",
                Language = "Roman",
                Gear = "Gladius and water bottle",
                God = "Jupiter"
            };
        }

        private static void Problem5()
        {
            List<Person> people = new List<Person>
            {
                new Person { FN = "Wingus", LN = "McCree" },
                new Person { FN = "Dingus", LN = "McGee" }
            };

            people.GroupBy((p) => p.FN).ToDictionary(grp => grp.Key, grp => grp.ToList());
        }

        private static void Problem4()
        {
            var data = "dog,dog,cat,cat,rabbit";
            var query = data.Split(',')
                .GroupBy(str => str)
                .Select(grp => new { Name = grp.Key, Count = grp.Count() });

            foreach (var q in query)
            {
                Console.WriteLine($"{q.Name} : {q.Count}");
            }
        }

        private static void Problem2()
        {
            // bishop starts on c6, enumerate the squares the bishop can move to
            // enumeration should include b5, a4 b7 a8     
            List<string> chessSquares = (from lett in Enumerable.Range('a', 8)
                                         from num in Enumerable.Range('1', 8)
                                         where Math.Abs(lett - 'c') == Math.Abs(num - '6')
                                         select $"{(char)lett}{(char)num}").ToList();

            foreach (var square in chessSquares)
                Console.WriteLine(square);
        }

        private static void Problem3()
        {
            string data = "Joey, Brenton, Sherry, Eugene, Kate, Sue, Daniel";
            var query = data.Split(',')
                .Select(str => new
                {
                    Name = str.Trim(),
                    IsOk = str.Length > 13
                });
            //.All(anony => /*IsAnonyOkay(anony)*/);
            foreach (var whatisthis in query)
            {
                if (whatisthis.Name == "Joey")
                {

                }
            }
            Console.WriteLine(query);
        }

        private static void Problem1()
        {
            string data = "Joe DiMaggio, 20/7/1984; Rona Rodgers, 8/4/1936";

            var res = data.Split(';') /// make enumerable
                .Select(str => str.Trim().Split(',')) // clean it and make it List<List<string>>
                .Select(lstStr => new
                {
                    Name = lstStr[0].Trim(),
                    DoB = DateTime.Parse(lstStr[1].Trim())
                })
                .OrderByDescending(anony => anony.DoB)
                .Select(anony => new
                {
                    Name = anony.Name,
                    Age = Math.Round((DateTime.Now - anony.DoB).TotalDays / 365) // convert day to years
                });

            foreach (var anony in res)
            {
                Console.WriteLine($"{anony.Name}, {anony.Age}");
            }
        }

        static bool IsAnonyOkay(dynamic anony) => anony.IsOk;
    }
}
