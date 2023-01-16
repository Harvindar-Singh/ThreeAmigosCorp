using Customer.Web.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Customer.Web.Controllers
{
    public class OrdersController : Controller
    {
        HttpClient client;
        public OrdersController()
        {
            client = new HttpClient();
            client.BaseAddress = new System.Uri("https://localhost:7100/");
            client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
        }



        
        // GET: OrdersController
        public async Task<ActionResult> Index()
        {
            IEnumerable<OrderDto> orders = new List<OrderDto>();

            HttpResponseMessage response = await client.GetAsync("api/Orders");
            if (response.IsSuccessStatusCode)
            {
                orders = await response.Content.ReadAsAsync<IEnumerable<OrderDto>>();
            }
            else
            {
                Debug.WriteLine("Index received a bad response from the web service.");
            }
            return View(orders.ToList());
        }
        
        // GET: OrdersController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            OrderDto orders = new OrderDto();

            HttpResponseMessage response = await client.GetAsync("api/Orders/" + id);
            if (response.IsSuccessStatusCode)
            {
                orders = await response.Content.ReadAsAsync<OrderDto>();
            }
            else
            {
                Debug.WriteLine("Index received a bad response from the web service.");
            }
            return View(orders);
        }
        
        // GET: OrdersController/Create
        public ActionResult Create()
        {
            return View();
        }
        
        // POST: OrdersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("OrderId,ProductId,CustomerId,EmailAddress,DeliveryAddress,Description ")]OrderDto orders)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("api/Orders", orders);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                Debug.WriteLine("Index received a bad response from the web service.");
            }
            return View(orders);
        }
        
        // GET: OrdersController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            OrderDto orders = new OrderDto();

            HttpResponseMessage response = await client.GetAsync("api/Orders/" + id);
            if (response.IsSuccessStatusCode)
            {
                orders = await response.Content.ReadAsAsync<OrderDto>();
            }
            else
            {
                Debug.WriteLine("Index received a bad response from the web service.");
            }
            return View(orders);
        }
        
        // POST: OrdersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, OrderDto orders)
        {
            if (id != orders.OrderId)
            {
                return NotFound();
            }

            HttpResponseMessage response = await client.PutAsJsonAsync("api/Orders/" + id, orders);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                Debug.WriteLine("Index received a bad response from the web service.");
            }
            return View(orders);
        }
        
        // GET: OrdersController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            OrderDto orders = new OrderDto();

            HttpResponseMessage response = await client.GetAsync("api/Orders/" + id);
            if (response.IsSuccessStatusCode)
            {
                orders = await response.Content.ReadAsAsync<OrderDto>();
            }
            else
            {
                Debug.WriteLine("Index received a bad response from the web service.");
            }
            return View(orders);
        }
        
        // POST: OrdersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            HttpResponseMessage response = await client.DeleteAsync("api/Orders/" + id);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                Debug.WriteLine("Delete received a bad response from the web service.");
                return BadRequest();
            }
        }
    }
}
