using System;
using System.Collections.Generic;
using System.Text;

namespace OOP.Encapsulation.ShoppingSpree
{
   public class Person //  створюємо приватні поля
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
            get => this.name;// це щоб не писати {return this.name}
         
            private set
            {
                if (value == null)
                {
                    throw new ArgumentException("Product name cannot be empty.");// виводить виключення про помилку
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
            if (this.money - product.Cost < 0) // умова яка вказує на те, що покупцю не вистачає на продукти
            {
                Console.WriteLine($"{this.name} can't afford {product.Name}"); // вивід про не купівлю
            }
            else
            {
                this.bag.Add(product); // грошей вистачило, додаємо продукт в сумку
                this.money -= product.Cost; // віднімаємо від загальної кількості грошей , ціну продукту
                Console.WriteLine($"{this.Name} bought {product.Name}"); // виводимо повідомлення, клієнт купив продукт
            }
        }

        public override string ToString() //override потрібний для того, щоб змінити рядок , реалізацію рядка, властивості рядка
        {
            var sb = new StringBuilder();
            sb.Append($"{this.Name} - "); // додає рядок до екземпляра sb

            if (this.bag.Count == 0) // якщо покупець нічого не купив
            {
                sb.Append("Nothing bought");
            }
            else
            {
                sb.Append(string.Join(", ", this.bag));//перераховується що купив покупець //join зчиплює всі введені елементи через кому
            }

            return sb.ToString(); //перетворює значення StringBuilder на значення string
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
            foreach (var parameter in parameters)// перераховує всі елементи і виконує цикл 
            {
                var person = parameter.Split("="); // повертає масив з розділенням =
                if (person.Length == 2)// якщо довжина рівна 2 елементам
                {
                    string name = person[0];// записує ім'я в змінну person
                    decimal money = decimal.Parse(person[1]);// записує гроші в змінну money
                    try
                    {
                        this.list.Add(new Person(name, money));// додає ім'я і гроші до list 
                    }
                    catch (ArgumentException e)
                    {
                        Console.WriteLine(e.Message);//повертає повідомлення про помилку
                        throw;
                    }
                }
            }
        }

        public void BuyProduct(string[] parameters, ProductCatalog products)
        {
            string personName = parameters[0]; // записуємо ім'я як перший елемент масиву
            string productName = parameters[1]; // записуємо продукт як 2 елем масиву

            Person currentPerson = this.list.FirstOrDefault(x => x.Name.Equals(personName)); // firstofdefault повертає перший елемент послідовності
            // Equals - перевіряє чи введене ім'я співпадає з першим введеним ім'ям(яке ви ввели на початку)
            Product currentProduct = products.Catalog.FirstOrDefault(x => x.Name.Equals(productName)); 
            currentPerson.AddToBag(currentProduct); // додає покуацю даний продукт в сумку
        }

        public void Print()
        {
            foreach (var person in this.list)
            {
                Console.WriteLine(person); //виводить покупців зі списку
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
                        this.catalog.Add(new Product(productName, productCost));// додаємо в каталог новий продукт та його ціну
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

            var peopleParameters = Console.ReadLine().Split(";");// вводимо покупців через ;
            var productsParameters = Console.ReadLine().Split(";");

            shoppers.AddMany(peopleParameters);
            products.AddMany(productsParameters);

            var input = Console.ReadLine();

            while (input != "END")
            {
                var parameters = input.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);// виключити елементи масиву, які мають пусті рядки
                shoppers.BuyProduct(parameters, products);// вивести покупців, і продукти які вони купили 
                input = Console.ReadLine();
            }

            shoppers.Print();
        }
    }
}
