using System;
using System.Collections.Generic;

// Address class - holds street, city, state/province, and country
public class Address
{
    private string _street;
    private string _city;
    private string _stateOrProvince;
    private string _country;

    public Address(string street, string city, string stateOrProvince, string country)
    {
        _street = street;
        _city = city;
        _stateOrProvince = stateOrProvince;
        _country = country;
    }

    public string Street
    {
        get { return _street; }
        set { _street = value; }
    }

    public string City
    {
        get { return _city; }
        set { _city = value; }
    }

    public string StateOrProvince
    {
        get { return _stateOrProvince; }
        set { _stateOrProvince = value; }
    }

    public string Country
    {
        get { return _country; }
        set { _country = value; }
    }

    // Returns true if the address is in the USA
    public bool IsInUSA()
    {
        return _country.Trim().ToLower() == "usa" || _country.Trim().ToLower() == "united states";
    }

    // Returns all fields as a single formatted string
    public string GetFullAddress()
    {
        return $"{_street}\n{_city}, {_stateOrProvince}\n{_country}";
    }
}

// Customer class - holds a name and an Address
public class Customer
{
    private string _name;
    private Address _address;

    public Customer(string name, Address address)
    {
        _name = name;
        _address = address;
    }

    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }

    public Address Address
    {
        get { return _address; }
        set { _address = value; }
    }

    // Calls the method on Address to determine if the customer lives in the USA
    public bool IsInUSA()
    {
        return _address.IsInUSA();
    }
}

// Product class - holds name, product id, price per unit, and quantity
public class Product
{
    private string _name;
    private string _productId;
    private double _pricePerUnit;
    private int _quantity;

    public Product(string name, string productId, double pricePerUnit, int quantity)
    {
        _name = name;
        _productId = productId;
        _pricePerUnit = pricePerUnit;
        _quantity = quantity;
    }

    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }

    public string ProductId
    {
        get { return _productId; }
        set { _productId = value; }
    }

    public double PricePerUnit
    {
        get { return _pricePerUnit; }
        set { _pricePerUnit = value; }
    }

    public int Quantity
    {
        get { return _quantity; }
        set { _quantity = value; }
    }

    // Total cost = price per unit * quantity
    public double GetTotalCost()
    {
        return _pricePerUnit * _quantity;
    }
}

// Order class - holds a list of products and a customer
public class Order
{
    private List<Product> _products = new List<Product>();
    private Customer _customer;

    public Order(Customer customer)
    {
        _customer = customer;
    }

    public Customer Customer
    {
        get { return _customer; }
        set { _customer = value; }
    }

    public void AddProduct(Product product)
    {
        _products.Add(product);
    }

    // Sum of all product costs + one-time shipping cost
    public double GetTotalPrice()
    {
        double total = 0;
        foreach (Product product in _products)
        {
            total += product.GetTotalCost();
        }
        total += GetShippingCost();
        return total;
    }

    // Shipping is $5 for USA, $35 for everywhere else
    public double GetShippingCost()
    {
        return _customer.IsInUSA() ? 5.0 : 35.0;
    }

    // Packing label: name and product id of each product
    public string GetPackingLabel()
    {
        string label = "Packing Label:\n";
        foreach (Product product in _products)
        {
            label += $"  {product.Name} (ID: {product.ProductId})\n";
        }
        return label;
    }

    // Shipping label: name and address of the customer
    public string GetShippingLabel()
    {
        string label = "Shipping Label:\n";
        label += $"  {_customer.Name}\n";
        label += "  " + _customer.Address.GetFullAddress().Replace("\n", "\n  ") + "\n";
        return label;
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        // ----- Order 1: Customer in the USA -----
        Address address1 = new Address("123 Maple Street", "Springfield", "IL", "USA");
        Customer customer1 = new Customer("John Smith", address1);

        Order order1 = new Order(customer1);
        order1.AddProduct(new Product("Wireless Mouse", "WM-101", 24.99, 2));
        order1.AddProduct(new Product("Mechanical Keyboard", "MK-205", 89.50, 1));
        order1.AddProduct(new Product("USB-C Cable", "UC-310", 9.99, 3));

        // ----- Order 2: Customer outside the USA -----
        Address address2 = new Address("45 King's Road", "London", "England", "United Kingdom");
        Customer customer2 = new Customer("Emily Johnson", address2);

        Order order2 = new Order(customer2);
        order2.AddProduct(new Product("Noise-Cancelling Headphones", "NH-500", 199.99, 1));
        order2.AddProduct(new Product("Laptop Stand", "LS-420", 34.75, 2));

        // ----- Display results for Order 1 -----
        Console.WriteLine("################ ORDER 1 ################");
        Console.WriteLine(order1.GetPackingLabel());
        Console.WriteLine(order1.GetShippingLabel());
        Console.WriteLine($"Total Price: ${order1.GetTotalPrice():F2}");
        Console.WriteLine();

        // ----- Display results for Order 2 -----
        Console.WriteLine("################ ORDER 2 ################");
        Console.WriteLine(order2.GetPackingLabel());
        Console.WriteLine(order2.GetShippingLabel());
        Console.WriteLine($"Total Price: ${order2.GetTotalPrice():F2}");
        Console.WriteLine();
    }
}