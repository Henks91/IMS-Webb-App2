using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PäronWebbApp.Data;
using PäronWebbApp.Models;

namespace PäronWebbApp.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly AppDbContext _context;

        public TransactionsController(AppDbContext context)
        {
            _context = context;
        }

        
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Transactions.Include(t => t.Warehouse).Include(t => t.Product);

            return View(await appDbContext.ToListAsync());
        }

        
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Transactions == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .Include(t => t.Warehouse)
                .Include(i => i.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        
        public IActionResult Create()
        {
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "WarehouseId", "City");
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName");
            return View();
        }

        
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Create transaction and update existing inventoryBlance / if not existing error message
        public async Task<IActionResult> Create([Bind("Id,Quantity,TransactionDate,ProductId,WarehouseId")] Transaction transaction)
        {

            if (ModelState.IsValid)
            {
                
                InventoryBalance updateInventoryBalance = await FindInventoryBalanceAsync(transaction);
                if (updateInventoryBalance != null)
                {
                    // Update the TotalAmount
                    updateInventoryBalance.TotalAmount += transaction.Quantity;


                    _context.Add(transaction);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    HandleInventoryBalanceNotFound(transaction);
                    return View(transaction);
                }
            }
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "WarehouseId", "City", transaction.WarehouseId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName", transaction.ProductId);
            return View(transaction);

        }

        
        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Transactions == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "WarehouseId", "City", transaction.WarehouseId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName", transaction.ProductId);
            return View(transaction);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Quantity,TransactionDate,ProductId,WarehouseId")] Transaction updatedTransaction)
        {
            if (id != updatedTransaction.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingTransaction = await _context.Transactions.FindAsync(id);

                    if (existingTransaction == null)
                    {
                        return NotFound();
                    }

                    // Check if Quantity is changed // if changed, change balance
                    if (existingTransaction.Quantity != updatedTransaction.Quantity)
                    {
                        int quantityChange = updatedTransaction.Quantity - existingTransaction.Quantity;
                        //await UpdateBalanceAsync(transaction, quantityChange);
                        // Update the balance based on the change in Quantity
                        await UpdateInventoryBalanceAsync(existingTransaction, quantityChange);
                    }
                    existingTransaction.Quantity = updatedTransaction.Quantity;
                    existingTransaction.TransactionDate = updatedTransaction.TransactionDate;
                    existingTransaction.ProductId = updatedTransaction.ProductId;
                    existingTransaction.WarehouseId = updatedTransaction.WarehouseId;
                    //await UpdateBalanceAsync(editedTransaction);
                    //_context.Update(transaction);
                    await _context.SaveChangesAsync();
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransactionExists(updatedTransaction.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "WarehouseId", "City", updatedTransaction.WarehouseId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName", updatedTransaction.ProductId);
            return View(updatedTransaction);
        }

        // GET: Transactions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Transactions == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .Include(t => t.Warehouse)
                .Include(t => t.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Transactions == null)
            {
                return Problem("Entity set 'AppDbContext.Transactions'  is null.");
            }
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction != null)
            {
                await UpdateInventoryBalanceOnDeleteAsync(transaction);
                _context.Transactions.Remove(transaction);
                
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransactionExists(int id)
        {
          return (_context.Transactions?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        public async Task UpdateInventoryBalanceAsync(Transaction transaction, int quantityChange)
        {
            try
            {
                InventoryBalance updateInventoryBalance = await FindInventoryBalanceAsync(transaction);

                if (updateInventoryBalance != null)
                {
                    // Update the TotalAmount
                    updateInventoryBalance.TotalAmount += quantityChange;

                    //_context.Update(updateInventoryBalance);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    HandleInventoryBalanceNotFound(transaction);
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
            }
        }

        public async Task UpdateInventoryBalanceOnDeleteAsync(Transaction transaction)
        {
            try
            {
                InventoryBalance updateInventoryBalance = await FindInventoryBalanceAsync(transaction);

                if (updateInventoryBalance != null)
                {
                    // Update the TotalAmount
                    updateInventoryBalance.TotalAmount -= transaction.Quantity;

                    //_context.Update(updateInventoryBalance);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    HandleInventoryBalanceNotFound(transaction);
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
            }
        }
        //Error message if InventoryBalance not found
        private void HandleInventoryBalanceNotFound(Transaction transaction)
        {
            ModelState.AddModelError(string.Empty, "Existing inventoryBalance not found for the given WarehouseId and ProductId. Create new balance section first");

            // Optionally, you can provide additional information or populate ViewData
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "WarehouseId", "City", transaction.WarehouseId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName", transaction.ProductId);
        }
        //Checks if IB is found on Warehouse && Product and adding Quantity to the balance
        public async Task<InventoryBalance> FindInventoryBalanceAsync(Transaction transaction)
        {
            return await _context.inventoryBalances
                .FirstOrDefaultAsync(ib => ib.WarehouseId == transaction.WarehouseId && ib.ProductId == transaction.ProductId);
        }

    }
}
