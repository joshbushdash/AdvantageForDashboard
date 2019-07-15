
using System;
using System.Linq;
using System.Collections.Generic;
using Advantage.API.Models;

namespace Advantage.API
{
    public class DataSeed
    {
        private readonly ApiContext _ctx;

        public DataSeed(ApiContext ctx)
        {
            _ctx = ctx;
        }

        public void SeedData(int nCustomers, int nOrders)
        {
            if (!_ctx.Customers.Any())
            {
                SeedCustomers(nCustomers);
                _ctx.SaveChanges();
            }
            if (!_ctx.Orders.Any())
            {
                SeedOrders(nOrders);
                _ctx.SaveChanges();
            }
            if (!_ctx.Servers.Any())
            {
                SeedServers();
                _ctx.SaveChanges();
            }
        }

        private void SeedCustomers(int n)
        {
            List<Customer> customers = BuildCustomerList(n);

            foreach (var customer in customers)
            {
                _ctx.Customers.Add(customer);
            }
        }

        private void SeedOrders(int n)
        {
            List<Order> orders = BuildOrderList(n);

            foreach (var order in orders)
            {
                _ctx.Orders.Add(order);
            }
        }

        private void SeedServers()
        {
            List<Server> servers = BuildServerList();

            foreach (var server in servers)
            {
                _ctx.Servers.Add(server);
            }
        }

        private List<Customer> BuildCustomerList(int nCustomers)
        {
            var customers = new List<Customer>();
            var names = new List<string>();

            for (int i = 1; i <= nCustomers; i++)
            {
                var name = Helpers.MakeUniqueCustomerName(names);
                names.Add(name);

                _ctx.Customers.Add(new Customer
                {
                    Id = i,
                    Name = name,
                    Email = Helpers.MakeCustomerEmail(name),
                    State = Helpers.GetRandomState()
                });
            }

            return customers;
        }

        private List<Order> BuildOrderList(int nOrders)
        {
            var orders = new List<Order>();
            var rand = new Random();

            for (var i = 1; i <= nOrders; i++)
            {
                var randCustomerId = rand.Next(_ctx.Customers.Count()) + 1;
                var placed = Helpers.GetRandomOrderPlaced();
                var complated = Helpers.GetRandomOrderCompleted(placed);

                orders.Add(new Order
                {
                    Id = i,
                    Customer = _ctx.Customers.First(c => c.Id == randCustomerId),
                    Total = Helpers.GitRandomOrderTotal(),
                    Placed = placed,
                    Completed=complated
                });
            }

            return orders;
        }


        private List<Server> BuildServerList()
        {
            return new List<Server>()
                {
                    new Server{
                        Id=1,
                        Name="Dev_Web",
                        IsOnline=true
                    },
                    new Server{
                        Id=2,
                        Name="Dev_Mail",
                        IsOnline=false
                    },
                    new Server{
                        Id=3,
                        Name="Dev_Services",
                        IsOnline=true
                    },
                    new Server{
                        Id=4,
                        Name="QA_Web",
                        IsOnline=true
                    },
                    new Server{
                        Id=5,
                        Name="QA_Mail",
                        IsOnline=true
                    },
                    new Server{
                        Id=6,
                        Name="QA_Services",
                        IsOnline=true
                    },
                    new Server{
                        Id=7,
                        Name="Prod_Web",
                        IsOnline=true
                    },
                    new Server{
                        Id=8,
                        Name="Prod_Mail",
                        IsOnline=true
                    },
                    new Server{
                        Id=9,
                        Name="Prod_Services",
                        IsOnline=true
                    }
                };
        }

        public class Helpers
        {
            private static Random _rand = new Random();
            private static string GetRandom(IList<string> items)
            {
                return items[_rand.Next(items.Count)];
            }

            internal static string MakeUniqueCustomerName(List<string> names)
            {
                var maxNames = bizPrefix.Count * bizSuffix.Count;

                if (names.Count >= maxNames)
                {
                    throw new System.InvalidOperationException("Maximum number of unique names exceeded");
                }

                var prefix = GetRandom(bizPrefix);
                var suffix = GetRandom(bizSuffix);
                var bizName = prefix + suffix;

                if (names.Contains(bizName))
                {
                    MakeUniqueCustomerName(names);
                }

                return bizName;
            }

            internal static string MakeCustomerEmail(string customerName)
            {
                return $"constact@{customerName.ToLower()}.com";
            }
            internal static string GetRandomState()
            {
                return (GetRandom(usStateCodes));
            }

            internal static decimal GitRandomOrderTotal()
            {
                return _rand.Next(100, 10000);
            }

            internal static DateTime GetRandomOrderPlaced()
            {
                var end = DateTime.Now;
                var start = end.AddDays(-90);

                TimeSpan possibleSpan = end - start;
                TimeSpan newSpan = new TimeSpan(0, _rand.Next(0, (int)possibleSpan.TotalMinutes), 0);

                return start + newSpan;
            }

            internal static DateTime? GetRandomOrderCompleted(DateTime orderPlaced)
            {
                var now = DateTime.Now;
                var timePassed = now - orderPlaced;
                var minLeadTime = TimeSpan.FromDays(_rand.Next(7, 21));

                if (timePassed < minLeadTime)
                {
                    return null;
                }

                return orderPlaced.AddDays(_rand.Next(7, minLeadTime.Days));
            }

            private static readonly List<string> usStateCodes = new List<string>(){
                "CA", "CO", "DE", "HI", "KS", "MI", "MS", "MA", "NE", "WA", "WI", "MP"
            };

            private static readonly List<string> bizPrefix = new List<string>()
            {
                "A",
                "B",
                "C",
                "AA",
                "CC",
                "AC",
                "AG",
                "GG",
                "FA",
                "BB"
            };
            private static readonly List<string> bizSuffix = new List<string>()
            {
                "Z",
                "YY",
                "Y",
                "YZ",
                "XY",
                "XZ",
                "Z",
                "OZ",
                "O",
                "P"
            };
        }
    }
}