using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class OrderDetails
    {
        public string ProductName { get; }
        public decimal Price { get; }
        public int Quantity { get; }

        public OrderDetails(string productName, decimal price, int quantity)
        {
            ProductName = productName;
            Price = price;
            Quantity = quantity;
        }

        public override bool Equals(object obj)
        {
            return obj is OrderDetails details &&
                   ProductName == details.ProductName;
        }

        public override int GetHashCode()
        {
            return ProductName.GetHashCode();
        }

        public override string ToString()
        {
            return $"{ProductName} (Price: {Price:C}, Quantity: {Quantity})";
        }
    }

    public class Order
    {
        public int OrderId { get; }
        public string Customer { get; set; }
        private List<OrderDetails> details;

        public Order(int orderId, string customer)
        {
            OrderId = orderId;
            Customer = customer;
            details = new List<OrderDetails>();
        }

        public void AddDetail(OrderDetails detail)
        {
            if (details.Contains(detail))
                throw new ArgumentException($"Product '{detail.ProductName}' already exists in order.");
            details.Add(detail);
        }

        public void ReplaceDetails(IEnumerable<OrderDetails> newDetails)
        {
            var temp = new List<OrderDetails>();
            foreach (var d in newDetails)
            {
                if (temp.Any(t => t.Equals(d)))
                    throw new ArgumentException($"Duplicate product '{d.ProductName}' in new details.");
                temp.Add(d);
            }
            details.Clear();
            details.AddRange(temp);
        }

        public decimal TotalAmount => details.Sum(d => d.Price * d.Quantity);

        public override bool Equals(object obj)
        {
            return obj is Order order &&
                   OrderId == order.OrderId;
        }

        public override int GetHashCode()
        {
            return OrderId.GetHashCode();
        }

        public override string ToString()
        {
            return $"Order {OrderId}\nCustomer: {Customer}\nTotal: {TotalAmount:C}\nDetails:\n{string.Join("\n", details)}\n";
        }

        public IEnumerable<OrderDetails> Details => details.AsReadOnly();
    }

    public class OrderService
    {
        private List<Order> orders = new List<Order>();

        public void AddOrder(Order order)
        {
            if (orders.Contains(order))
                throw new ArgumentException($"Order {order.OrderId} already exists.");
            orders.Add(order);
        }

        public void RemoveOrder(int orderId)
        {
            var order = orders.FirstOrDefault(o => o.OrderId == orderId);
            if (order == null)
                throw new ArgumentException($"Order {orderId} not found.");
            orders.Remove(order);
        }

        public void UpdateOrder(int orderId, string newCustomer, IEnumerable<OrderDetails> newDetails)
        {
            var order = orders.FirstOrDefault(o => o.OrderId == orderId) ??
                throw new ArgumentException($"Order {orderId} not found.");

            order.Customer = newCustomer;
            order.ReplaceDetails(newDetails);
        }

        public IEnumerable<Order> QueryByOrderId(int orderId) =>
            orders.Where(o => o.OrderId == orderId).OrderBy(o => o.TotalAmount);

        public IEnumerable<Order> QueryByProductName(string productName) =>
            orders.Where(o => o.Details.Any(d => d.ProductName == productName))
                 .OrderBy(o => o.TotalAmount);

        public IEnumerable<Order> QueryByCustomer(string customer) =>
            orders.Where(o => o.Customer == customer)
                 .OrderBy(o => o.TotalAmount);

        public IEnumerable<Order> QueryByTotalAmount(decimal minAmount) =>
            orders.Where(o => o.TotalAmount >= minAmount)
                 .OrderBy(o => o.TotalAmount);

        public void SortOrders() => orders = orders.OrderBy(o => o.OrderId).ToList();

        public void SortOrders<TKey>(Func<Order, TKey> keySelector) =>
            orders = orders.OrderBy(keySelector).ToList();
    }

    public class OrderServiceTests
    {
        public void AddOrder_DuplicateOrder_ThrowsException()
        {
            var service = new OrderService();
            var order = new Order(1, "Test");
            service.AddOrder(order);

            try
            {
                service.AddOrder(order);
                throw new Exception("Expected exception not thrown");
            }
            catch (ArgumentException) { }
        }

        public void RemoveOrder_NonExisting_ThrowsException()
        {
            var service = new OrderService();
            try
            {
                service.RemoveOrder(999);
                throw new Exception("Expected exception not thrown");
            }
            catch (ArgumentException) { }
        }

        public void QueryByProduct_ReturnsCorrectOrders()
        {
            var service = new OrderService();
            var order = new Order(1, "Test");
            order.AddDetail(new OrderDetails("Apple", 10m, 2));
            service.AddOrder(order);

            var result = service.QueryByProductName("Apple");
            if (result.Count() != 1)
                throw new Exception($"Expected 1 order, got {result.Count()}");
        }

        public void UpdateOrder_ChangesCustomer()
        {
            var service = new OrderService();
            service.AddOrder(new Order(1, "Old"));
            service.UpdateOrder(1, "New", Array.Empty<OrderDetails>());

            var order = service.QueryByOrderId(1).First();
            if (order.Customer != "New")
                throw new Exception($"Customer not updated. Expected New, got {order.Customer}");
        }
    }

    class Program
    {
        static void Main()
        {

            //RunTests();//测试
            //return;

            OrderService service = new OrderService();
            while (true)
            {
                try
                {
                    Console.WriteLine("\n1. Add Order\n2. Remove Order\n3. Update Order\n4. Query Orders\n5. Exit");
                    Console.Write("Choice: ");
                    switch (Console.ReadLine())
                    {
                        case "1":
                            AddOrder(service);
                            break;
                        case "2":
                            RemoveOrder(service);
                            break;
                        case "3":
                            UpdateOrder(service);
                            break;
                        case "4":
                            QueryOrders(service);
                            break;
                        case "5":
                            return;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        static void RunTests()
        {
            var tests = new OrderServiceTests();
            var testMethods = new Action[]
            {
                tests.AddOrder_DuplicateOrder_ThrowsException,
                tests.RemoveOrder_NonExisting_ThrowsException,
                tests.QueryByProduct_ReturnsCorrectOrders,
                tests.UpdateOrder_ChangesCustomer
            };

            int passed = 0;
            foreach (var test in testMethods)
            {
                try
                {
                    test();
                    Console.WriteLine($"[PASS] {test.Method.Name}");
                    passed++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[FAIL] {test.Method.Name}");
                    Console.WriteLine($"\t{ex.Message}");
                }
            }

            Console.WriteLine($"\nTest Results: {passed}/{testMethods.Length} passed");
        }

        static void AddOrder(OrderService service)
        {
            var order = new Order(
                InputInt("Order ID: "),
                InputString("Customer: "));

            while (true)
            {
                try
                {
                    var product = InputString("Product Name (empty to finish): ");
                    if (string.IsNullOrEmpty(product)) break;

                    order.AddDetail(new OrderDetails(
                        product,
                        InputDecimal("Price: "),
                        InputInt("Quantity: ")));
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            service.AddOrder(order);
            Console.WriteLine("Order added!");
        }

        static void RemoveOrder(OrderService service) =>
            service.RemoveOrder(InputInt("Order ID to remove: "));

        static void UpdateOrder(OrderService service)
        {
            int id = InputInt("Order ID to update: ");
            var details = new List<OrderDetails>();

            while (true)
            {
                try
                {
                    var product = InputString("Product Name (empty to finish): ");
                    if (string.IsNullOrEmpty(product)) break;

                    details.Add(new OrderDetails(
                        product,
                        InputDecimal("Price: "),
                        InputInt("Quantity: ")));
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            service.UpdateOrder(id, InputString("New Customer: "), details);
            Console.WriteLine("Order updated!");
        }

        static void QueryOrders(OrderService service)
        {
            Console.WriteLine("1. By ID\n2. By Product\n3. By Customer\n4. By Min Amount");
            IEnumerable<Order> result = Enumerable.Empty<Order>();
            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    result = service.QueryByOrderId(InputInt("Order ID: "));
                    break;
                case "2":
                    result = service.QueryByProductName(InputString("Product Name: "));
                    break;
                case "3":
                    result = service.QueryByCustomer(InputString("Customer: "));
                    break;
                case "4":
                    result = service.QueryByTotalAmount(InputDecimal("Min Amount: "));
                    break;
            }

            Console.WriteLine(string.Join("\n", result));
        }

        static int InputInt(string prompt)
        {
            Console.Write(prompt);
            return int.Parse(Console.ReadLine());
        }

        static decimal InputDecimal(string prompt)
        {
            Console.Write(prompt);
            return decimal.Parse(Console.ReadLine());
        }

        static string InputString(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }
    }


}
