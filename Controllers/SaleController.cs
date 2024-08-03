using Microsoft.AspNetCore.Mvc;
using ProductSales.DTOs;
using ProductSales.Repositories;

namespace ProductSales.Controllers
{
    public class SaleController(IProductRepository productRepository, ISaleRepository saleRepository) : Controller
    {
        private readonly IProductRepository _productRepository = productRepository;
        private readonly ISaleRepository _saleRepository = saleRepository;

        public async Task<IActionResult> Index()
        {
            var sales = await _saleRepository.GetAllSalesAsync();
            return View(sales);
        }

        public async Task<IActionResult> Details(int id)
        {
            var sale = await _saleRepository.GetSaleByIdAsync(id);
            if (sale == null)
                return NotFound();

            var products = await _productRepository.GetAllProductsAsync();
            ViewBag.Products = products.ToList();

            return View(sale);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Products = await _productRepository.GetAllProductsAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaleDTO saleDTO)
        {
            if (ModelState.IsValid)
            {
                await _saleRepository.AddSaleAsync(saleDTO);
                return RedirectToAction("Index");
            }
            ViewBag.Products = await _productRepository.GetAllProductsAsync();
            return View(saleDTO);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var sale = await _saleRepository.GetSaleByIdAsync(id);
            if (sale == null)
                return NotFound();
            ViewBag.Products = await _productRepository.GetAllProductsAsync();
            return View(sale);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, SaleDTO saleDTO)
        {
            if (id != saleDTO.SaleID)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _saleRepository.UpdateSaleAsync(saleDTO);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }

            }
            ViewBag.Products = await _productRepository.GetAllProductsAsync();
            return View(saleDTO);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var sale = await _saleRepository.GetSaleByIdAsync(id);
            if (sale == null)
                return NotFound();
            ViewBag.Products = await _productRepository.GetAllProductsAsync();
            return View(sale);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _saleRepository.DeleteSaleAsync(id);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Price(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();
            return Json(product.Price);
        }
    }
}