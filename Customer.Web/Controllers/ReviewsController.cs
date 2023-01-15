using Customer.Web.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Customer.Web.Controllers
{
    public class ReviewsController : Controller
    {
        HttpClient client;

        public ReviewsController()
        {
            client = new HttpClient();
            client.BaseAddress = new System.Uri("https://localhost:7066/");
            client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
        }

        // GET: ReviewsController
        public async Task<ActionResult> Index()
        {
            IEnumerable<ReviewDto> reviews = new List<ReviewDto>();

            HttpResponseMessage response = await client.GetAsync("api/Reviews");
            if (response.IsSuccessStatusCode)
            {
                reviews = await response.Content.ReadAsAsync<IEnumerable<ReviewDto>>();
            }
            else
            {
                Debug.WriteLine("Index received a bad response from the web service.");
            }
            return View(reviews.ToList());
        }

        // GET: ReviewsController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            ReviewDto review = new ReviewDto();

            HttpResponseMessage response = await client.GetAsync("api/Reviews/" + id);
            if (response.IsSuccessStatusCode)
            {
                review = await response.Content.ReadAsAsync<ReviewDto>();
            }
            else
            {
                Debug.WriteLine("Details received a bad response from the web service.");
                return NotFound();
            }
            return View(review);
        }

        // GET: ReviewsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ReviewsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ReviewDto review)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("api/Reviews", review);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                Debug.WriteLine("Create received a bad response from the web service.");
            }
            return View(review);
        }

        // GET: ReviewsController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            ReviewDto review = new ReviewDto();

            HttpResponseMessage response = await client.GetAsync("api/Reviews/" + id);
            if (response.IsSuccessStatusCode)
            {
                review = await response.Content.ReadAsAsync<ReviewDto>();
            }
            else
            {
                Debug.WriteLine("Edit received a bad response from the web service.");
                return NotFound();
            }
            return View(review);
        }

        // POST: ReviewsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, ReviewDto review)
        {
            if (id != review.ReviewId)
            {
                return NotFound();
            }

            HttpResponseMessage response = await client.PutAsJsonAsync("api/Reviews/" + id, review);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                Debug.WriteLine("Edit received a bad response from the web service.");
            }
            return View(review);
        }

        // GET: ReviewsController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            ReviewDto review = new ReviewDto();

            HttpResponseMessage response = await client.GetAsync("api/Reviews/" + id);
            if (response.IsSuccessStatusCode)
            {
                review = await response.Content.ReadAsAsync<ReviewDto>();
            }
            else
            {
                Debug.WriteLine("Delete received a bad response from the web service.");
                return NotFound();
            }
            return View(review);
        }

        // POST: ReviewsController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            HttpResponseMessage response = await client.DeleteAsync("api/Reviews/" + id);
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