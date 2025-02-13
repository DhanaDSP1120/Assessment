using Assessment.Model;
using Assessment.Operations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using NHibernate.Criterion;
using Serilog;

namespace Assessment.Controllers
{
    [Route("api/action")]
    [ApiController]
    public class ActionController : ControllerBase
    {
        [HttpGet("Test")]
        public IActionResult test()
        {
            return Ok(ApiResponse.Success("success", "ok"));
        }
        [HttpPost("RefreshData")]
        public IActionResult RefreshData(bool isModifyExisting = false)
        {
            try
            {
                Log.Information("Data Refersh Triggered");
                Task.Run(() =>
                {
                    CsvImport.ImportCSV("config/assessment.csv" , isModifyExisting);
                    Log.Information("Data Refersh Completed");

                });

                return Ok(ApiResponse.Success("success", "ok"));
            }
            catch (Exception ex)
            {
                Log.Error("Error in Data Refersh - " + ex.Message);

                return BadRequest(ApiResponse.Error(ex.Message));
            }
        }
        [HttpGet("GetAllProducts")]
        public IActionResult GetAllProducts()
        {
            try
            {
                List<Product> products = Product.GetAll().ToList();
                return Ok(ApiResponse.Success(products, "ok"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse.Error(ex.Message));
            }
        }
        [HttpGet("GetAllOrders")]
        public IActionResult GetAllOrders()
        {
            try
            {
                List<Model.Order> orders = Model.Order.GetAll().ToList();
                return Ok(ApiResponse.Success(orders, "ok"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse.Error(ex.Message));
            }
        }
        [HttpGet("GetAllCustomers")]
        public IActionResult GetAllCustomers()
        {
            try
            {
                List<Customer> customers = Customer.GetAll().ToList();
                return Ok(ApiResponse.Success(customers, "ok"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse.Error(ex.Message));
            }
        }

        [HttpGet("GetTotalOrders")]
        public IActionResult GettotalOrders(DateTime startDate , DateTime EndTime)
        {
            try
            {
                var criteria1 = Restrictions.Ge(nameof(Model.Order.DateOfSale), startDate);
                var criteria2 = Restrictions.Le(nameof(Model.Order.DateOfSale), EndTime);
                int count = DBService.FetchRowCount<Model.Order>(criteria1,criteria2);
         
                return Ok(ApiResponse.Success(new { TotalOrder = count }, "ok"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse.Error(ex.Message));
            }
        }

        [HttpGet("GetOrdersAverageValue")]
        public IActionResult GetOrdersAverageValue(DateTime startDate, DateTime EndTime)
        {
            try
            {
                var criteria1 = Restrictions.Ge(nameof(Model.Order.DateOfSale), startDate);
                var criteria2 = Restrictions.Le(nameof(Model.Order.DateOfSale), EndTime);
                int count = DBService.FetchRowCount<Model.Order>(criteria1, criteria2);
                decimal sum = DBService.SumSingleCol<Model.Order>(nameof(Model.Order.SellingPrice),criteria1, criteria2);

                decimal totalAvg = sum / count;
                return Ok(ApiResponse.Success(new { TotalAvg = totalAvg }, "ok"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse.Error(ex.Message));
            }
        }

        [HttpGet("GetTotalCustomers")]
        public IActionResult GetTotalCustomers(DateTime startDate, DateTime EndTime)
        {
            try
            {
                var criteria1 = Restrictions.Ge(nameof(Model.Order.DateOfSale), startDate);
                var criteria2 = Restrictions.Le(nameof(Model.Order.DateOfSale), EndTime);
                IList<Model.Order> orders = DBService.FetchAllBycriteria<Model.Order>(criteria1, criteria2); 
                List<int> orderIds = orders.Select(o=>o.Id).ToList();

                int customersCount = DBService.FetchRowCount<ReleationTable>(Restrictions.In(nameof(ReleationTable.OrderId),orderIds));

                
                return Ok(ApiResponse.Success(new { CustomersCount = customersCount }, "ok"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse.Error(ex.Message));
            }
        }
    }

    public class ApiResponse
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        public ApiResponse()
        {
            Status = 200;
            Message = "OK";
        }

        public static ApiResponse Success (object data, string msg)
        {
            return new ApiResponse { Status = 200 , Message = msg , Data=data };
        }
        public static ApiResponse Error(string msg)
        {
            return new ApiResponse { Status = 500, Message = msg };
        }
    }
}
