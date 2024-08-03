using Microsoft.AspNetCore.Mvc;
using ProductSales.DTOs;
using ProductSales.Repositories;

namespace ProductSales.Controllers
{
    public class ProductController(IProductRepository productRepository) : Controller
    {
        private readonly IProductRepository _productRepository = productRepository;

        public async Task<IActionResult> Index()
        {
            var products = await _productRepository.GetAllProductsAsync();
            return View(products);
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            return View(product);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductDTO productDTO)
        {
            if (ModelState.IsValid)
            {
                await _productRepository.AddProductAsync(productDTO);
                return RedirectToAction("Index");
            }
            return View(productDTO);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductDTO productDTO)
        {
            if (id != productDTO.ProductID)
                return NotFound();

            if (ModelState.IsValid)
            {
                await _productRepository.UpdateProductAsync(productDTO);
                return RedirectToAction("Index");
            }
            return View(productDTO);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _productRepository.DeleteProductAsync(id);
            return RedirectToAction("Index");
        }

    }
}