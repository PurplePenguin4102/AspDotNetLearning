using NinjaDomain.Classes;
using NinjaDomain.DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Database.SetInitializer(new NullDatabaseInitializer<NinjaContext>());
            //InsertNinja();
            //InsertNinjas();
            //SimpleNinjaQueries();
            InsertRelatedData();
            Console.ReadLine();
        }

        private static void InsertNinja()
        {
            var shinobi = new Ninja
            {
                Name = "Jonnu Digguru",
                ServedInOniwaban = true,
                DateOfBirth = new DateTime(1692, 7, 12),
                ClanId = 1
            };
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                context.Ninjas.Add(shinobi);
                context.SaveChanges();
            }
        }

        private static void InsertNinjas()
        {
            var shinobi1 = new Ninja
            {
                Name = "Masahito Yammeru",
                ServedInOniwaban = false,
                DateOfBirth = new DateTime(1934, 3, 1),
                ClanId = 1
            };
            var shinobi2 = new Ninja
            {
                Name = "Sengo Farahito",
                ServedInOniwaban = false,
                DateOfBirth = new DateTime(1936, 5, 30),
                ClanId = 1
            };
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                context.Ninjas.AddRange(new List<Ninja>() { shinobi1, shinobi2 });
                context.SaveChanges();
            }
        }

        private static void SimpleNinjaQueries()
        {
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                // we can pull out the DbSet and treat it as a variable
                var query = context.Ninjas;
                // we can directly call a linq method on a DbSet object. This will query all data in table
                var ninjas = query.ToList();
                // we can enumerate across the DbSet (considered dangerous)
                Console.WriteLine("Enumerating across DbSet");
                foreach (var ninja in query)
                {
                    Console.WriteLine(ninja.Name);
                }

                // This query will grab all ninjas named Ryu
                // here is the query
                var ninjasNamedRyuQuery = context.Ninjas.Where(n => n.Name == "Ryu");
                // here is the execution of the query
                var ninjasNamedRyu = ninjasNamedRyuQuery.FirstOrDefault();
                // result
                Console.WriteLine($"ninjasNamedRyu : {ninjasNamedRyu.Name}, {ninjasNamedRyu.DateOfBirth}");

                // This query will grab the first ninja born after 1500
                DateTime comparison = new DateTime(1500, 1, 1);
                var OldNinjas = context.Ninjas.Where(n => n.DateOfBirth >= comparison).OrderBy(n => n.DateOfBirth).FirstOrDefault();
                Console.WriteLine($"OldNinjas : {OldNinjas.Name}");
            }
        }

        private static void QueryAndUpdateNinja()
        {
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                // grab the first ninja in the DB
                var ninja = context.Ninjas.FirstOrDefault();
                // this update will do nothing because no changes were made
                context.SaveChanges();
                // edit the field and then the query will run
                ninja.ServedInOniwaban = !ninja.ServedInOniwaban;
                context.SaveChanges();
            }
        }

        private static void QueryAndUpdateNinjaDisconnected()
        {
            Ninja ninja;
            // client queries for the ninja
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                ninja = context.Ninjas.FirstOrDefault();
            }

            // client updates the ninja
            ninja.ServedInOniwaban = !ninja.ServedInOniwaban;

            using (var context = new NinjaContext())
            {
                // now we don't have the context for the ninja because we lost context after query
                context.Database.Log = Console.WriteLine;
                // we attach to context first instead of add to avoid inserting duplicate data
                context.Ninjas.Attach(ninja);
                // we make EF aware of the fact that this is a modification to existing data
                context.Entry(ninja).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        private static void RetrieveDataWithFind()
        {
            var keyval = 1;
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                // queries memory first, very fast!!
                var ninja = context.Ninjas.Find(keyval);
                Console.WriteLine($"After Find#1 : {ninja.Name}");

                var newNinja = context.Ninjas.Find(keyval);
                Console.WriteLine($"After Find#1 : {newNinja.Name}");
            }
        }

        private static void DeleteNinja()
        {
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                var ninja = context.Ninjas.FirstOrDefault();
                context.Ninjas.Remove(ninja);
                context.SaveChanges();
            }
        }

        private static void DeleteNinjaAcrossContexts()
        {
            Ninja ninja;
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                ninja = context.Ninjas.FirstOrDefault();
            }
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                // attach to our active context
                context.Ninjas.Attach(ninja);
                // and remove
                context.Ninjas.Remove(ninja);

                // alternatively delete using Entry
                // context.Entry(ninja).State = EntityState.Deleted
                context.SaveChanges();
            }
        }

        private static void InsertRelatedData()
        {
            var ninja = new Ninja()
            {
                Name = "Kevin Sakomoto",
                ClanId = 1,
                DateOfBirth = new DateTime(1978, 9, 12),
                ServedInOniwaban = true
            };

            var tool = new NinjaEquipment()
            {
                Name = "Grappling Hook",
                Type = NinjaDomain.Classes.Enums.EquipmentType.Tool
            };

            var weapon = new NinjaEquipment()
            {
                Name = "Katana",
                Type = NinjaDomain.Classes.Enums.EquipmentType.Weapon
            };

            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                // add our Ninja to context
                context.Ninjas.Add(ninja);
                // add equipment to our ninja
                ninja.EquipmentOwned.Add(tool);
                ninja.EquipmentOwned.Add(weapon);
                // apply to database
                context.SaveChanges();
            }
        }

        private static void RetrieveRelatedData()
        {
            using (var Context = new NinjaContext())
            {
                Context.Database.Log = Console.WriteLine;

                // by using include we can greedily load related data
                var ninjas = Context.Ninjas
                    .Include(n => n.EquipmentOwned)
                    .FirstOrDefault(n => n.Name.StartsWith("Kevin"));

                // by using explicit loading we can get just equipment for a particular ninja instead of all
                var kevin = Context.Ninjas
                    .FirstOrDefault(n => n.Name.StartsWith("Kevin"));
                Context.Entry(kevin).Collection(n => n.EquipmentOwned).Load();

                // Lazy loading example, mentioning the property gets the property
                kevin.EquipmentOwned.Count();
            }
        }

        private static void ProjectionQuery()
        {
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                // Ninjas is now an anonymous type
                var Ninjas = context.Ninjas
                    .Select(n => new { n.Name, n.DateOfBirth, n.EquipmentOwned })
                    .ToList();
            }
        }
    }
}
