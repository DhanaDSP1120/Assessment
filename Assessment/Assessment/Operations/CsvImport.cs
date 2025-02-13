using Assessment.Enum;
using Assessment.Model;
using CsvHelper;
using Serilog;
using System.Globalization;
using static NHibernate.Engine.Query.CallableParser;
using System.Net;
using System.Xml.Linq;
using System.Drawing;

namespace Assessment.Operations
{
    public class CsvImport
    {
        public static void ImportCSV(string filepath , bool isOverwriteExiting = false)
        {
            Log.Information($"Importing record started");
            using (var reader = new StreamReader(filepath))
            {
                var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                var records = csv.GetRecords<dynamic>().ToList();
               
                foreach (var row in records)
                {
                    var record = (IDictionary<string, object>)row;
                    string customerId = record["Customer ID"].ToString();
                    string productId = record["Product ID"].ToString();
                    string orderId = record["Order ID"].ToString();

                    

                    Customer customer = Customer.GetById(customerId);
                    Order order = Order.GetById(orderId);
                    Product product = Product.GetById(productId);

                    int paymentmode = 0;

                    if (record["Payment Method"] == "PayPal")
                    {
                        paymentmode = (int)PaymentMode.PayPal;
                    }
                    else if (record["Payment Method"] == "Credit Card")
                    {
                        paymentmode = (int)PaymentMode.CreditCard;
                    }
                    else
                    {
                        paymentmode = (int)PaymentMode.DebitCard;
                    }

                    int category = 0;

                    if (record["Category"] == "Electronics")
                    {
                        category = (int)ProductCategory.Electronics;
                    }
                    else if (record["Category"] == "Clothing")
                    {
                        category = (int)ProductCategory.Clothing;
                    }
                    else
                    {
                        category = (int)ProductCategory.Shoes;
                    }

                    if (isOverwriteExiting)
                    {
                        if (customer != null)
                        {
                            customer.CustomerId = customerId;
                            customer.Name = record["Customer Name"].ToString();
                            customer.Email = record["Customer Email"].ToString();
                            customer.Address = record["Customer Address"].ToString();
                            Customer.Update(customer);
                        }

                        if (order != null)
                        {
                            order.OrderId = orderId;
                            order.Region = record["Region"].ToString();
                            order.DateOfSale = DateTime.Parse(record["Date of Sale"].ToString());
                            order.QuantitySold = int.Parse(record["Quantity Sold"].ToString());
                            order.UnitPrice = decimal.Parse(record["Unit Price"].ToString());
                            order.Discount = decimal.Parse(record["Discount"].ToString());
                            order.SellingPrice = decimal.Parse(record["Shipping Cost"].ToString());
                            order.PaymentMode = paymentmode;
                            Order.Update(order);
                        }

                        if(product != null)
                        {
                            product.ProductId = productId;
                            product.Name = record["Product Name"].ToString();
                            product.Category = category;
                            Product.Update(product);
                        }
                    }
                    else
                    {
                        Customer newCustomer = new Customer()
                        {
                            CustomerId = customerId,
                            Name = record["Customer Name"].ToString(),
                            Email = record["Customer Email"].ToString(),
                            Address = record["Customer Address"].ToString()
                        };
                        Order newOrder = new Order()
                        {
                            OrderId = orderId,
                            Region = record["Region"].ToString(),
                            DateOfSale = DateTime.Parse(record["Date of Sale"].ToString()),
                            QuantitySold = int.Parse(record["Quantity Sold"].ToString()),
                            UnitPrice = decimal.Parse(record["Unit Price"].ToString()),
                            Discount = decimal.Parse(record["Discount"].ToString()),
                            SellingPrice = decimal.Parse(record["Shipping Cost"].ToString()),
                            PaymentMode = paymentmode
                        };
                        Product newProduct = new Product()
                        {
                            ProductId = productId,
                            Name = record["Product Name"].ToString(),
                            Category = category,
                        };
                      int cId =  Customer.Add(newCustomer);
                       int pId = Product.Add(newProduct);
                        int oId = Order.Add(newOrder);
                        ReleationTable.Add(new ReleationTable { customerId = cId, OrderId = oId, ProductId = pId, });
                    }            
                    
                }
            }
            Log.Information($"Importing record ended");
        }
    }
}
