using System;
using System.Collections.Generic;
using System.Text;

namespace OOP.Encapsulation.ShoppingSpree
{
   public class Person
    {
        private string name;
        private decimal money;
        private List<Product> bag;

        public Person(string name, decimal money)
        {
            this.Name = name;
            this.Money = money;
            this.bag = new List<Product>();
        }

        public string Name
        {
            get => this.name;
            private set
            {
                if (value == null)
                {
                    throw new ArgumentException("Product name cannot be empty.");
                }

                this.name = value;
            }
        }

        public decimal Money
        {
            get => this.money;
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Money cannot be negative");
                }

                this.money = value;
            }
        }

        public void AddToBag(Product product)
        {
            if (this.money - product.Cost < 0)
            {
                Console.WriteLine($"{this.name} can't afford {product.Name}");
            }
            else
            {
                this.bag.Add(product);
                this.money -= product.Cost;
                Console.WriteLine($"{this.Name} bought {product.Name}");
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"{this.Name} - ");

            if (this.bag.Count == 0)
            {
                sb.Append("Nothing bought");
            }
            else
            {
                sb.Append(string.Join(", ", this.bag));
            }

            return sb.ToString();
        }
    }

    public class Product
    {
        private string name;
        private decimal cost;

        public Product(string name, decimal cost)
        {
            this.Name = name;
            this.Cost = cost;
        }

        public string Name
        {
            get => this.name;
            private set
            {
                if (value == null)
                {
                    throw new ArgumentException("Name cannot be empty.");
                }

                this.name = value;
            }
        }

        public decimal Cost
        {
            get => this.cost;
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Cost cannot be negative number.");
                }

                this.cost = value;
            }
        }

        public override string ToString()
        {
            return this.Name;
        }
    }

    public class Shoppers
    {
        private List<Person> list;

        public Shoppers()
        {
            this.list = new List<Person>();
        }

        public void AddMany(string[] parameters)
        {
            foreach (var parameter in parameters)
            {
                var person = parameter.Split("=");
                if (person.Length == 2)
                {
                    string name = person[0];
                    decimal money = decimal.Parse(person[1]);
                    try
                    {
                        this.list.Add(new Person(name, money));
                    }
                    catch (ArgumentException e)
                    {
                        Console.WriteLine(e.Message);
                        throw;
                    }
                }
            }
        }

        public void BuyProduct(string[] parameters, ProductCatalog products)
        {
            string personName = parameters[0];
            string productName = parameters[1];

            Person currentPerson = this.list.FirstOrDefault(x => x.Name.Equals(personName));
            Product currentProduct = products.Catalog.FirstOrDefault(x => x.Name.Equals(productName));

            currentPerson.AddToBag(currentProduct);
        }

        public void Print()
        {
            foreach (var person in this.list)
            {
                Console.WriteLine(person);
            }
        }
    }

    public class ProductCatalog
    {
        private List<Product> catalog;

        public ProductCatalog()
        {
            this.catalog = new List<Product>();
        }

        public IReadOnlyList<Product> Catalog
        {
            get => this.catalog;
        }

        public void AddMany(string[] parameters)
        {
            foreach (var parameter in parameters)
            {
                var product = parameter.Split("=");
                if (product.Length == 2)
                {
                    string productName = product[0];
                    decimal productCost = decimal.Parse(product[1]);
                    try
                    {
                        this.catalog.Add(new Product(productName, productCost));
                    }
                    catch (ArgumentException e)
                    {
                        Console.WriteLine(e.Message);
                        return;
                    }
                }
            }
        }
    }

    public class Startup 
    {
        public static void Main(string[] args)
        {
            var shoppers = new Shoppers();
            var products = new ProductCatalog();

            var peopleParameters = Console.ReadLine().Split(";");
            var productsParameters = Console.ReadLine().Split(";");

            shoppers.AddMany(peopleParameters);
            products.AddMany(productsParameters);

            var input = Console.ReadLine();

            while (input != "END")
            {
                var parameters = input.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                shoppers.BuyProduct(parameters, products);
                input = Console.ReadLine();
            }

            shoppers.Print();
        }
    }
}
