using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Customer.Web.Product.Services;

namespace Customer.Web.Controllers;

public class ProductsController : Controller
{
    private readonly ILogger _logger;
    private readonly IProductServices _productsService;

    public ProductsController(ILogger<ReviewsController> logger,
                             IProductServices productsService)
    {
        _logger = logger;
        _productsService = productsService;
    }

    // GET: /reviews/
    public async Task<IActionResult> Index([FromQuery] string? subject)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        IEnumerable<ProductDto> products = null;
        try
        {
            products = await _productsService.GetProductsAsync(subject);
        }
        catch
        {
            _logger.LogWarning("Exception occurred using Reviews service.");
            products = Array.Empty<ProductDto>();
        }
        return View(products.ToList());
    }

    // GET: /reviews/details/{id}
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return BadRequest();
        }

        try
        {
            var product = await _productsService.GetProductAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        catch
        {
            _logger.LogWarning("Exception occurred using Products service.");
            return StatusCode(StatusCodes.Status503ServiceUnavailable);
        }
    }
}