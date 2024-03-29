﻿using System;
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
    public class InventoryBalanceController : Controller
    {
        private readonly AppDbContext _context;

        public InventoryBalanceController(AppDbContext context)
        {
            _context = context;
        }

        // GET: InventoryBalance
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.inventoryBalances.Include(i => i.Product).Include(i => i.Warehouse);
            return View(await appDbContext.ToListAsync());
        }

        // GET: InventoryBalance/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.inventoryBalances == null)
            {
                return NotFound();
            }

            var inventoryBalance = await _context.inventoryBalances
                .Include(i => i.Product)
                .Include(i => i.Warehouse)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inventoryBalance == null)
            {
                return NotFound();
            }

            return View(inventoryBalance);
        }

        // GET: InventoryBalance/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName");
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "WarehouseId", "City");
            return View();
        }

        // POST: InventoryBalance/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TotalAmount,ProductId,WarehouseId")] InventoryBalance inventoryBalance)
        {
            if (ModelState.IsValid)
            {
                _context.Add(inventoryBalance);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName", inventoryBalance.ProductId);
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "WarehouseId", "City", inventoryBalance.WarehouseId);
            return View(inventoryBalance);
        }

        // GET: InventoryBalance/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.inventoryBalances == null)
            {
                return NotFound();
            }

            var inventoryBalance = await _context.inventoryBalances.FindAsync(id);
            if (inventoryBalance == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName", inventoryBalance.ProductId);
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "WarehouseId", "City", inventoryBalance.WarehouseId);
            return View(inventoryBalance);
        }

        // POST: InventoryBalance/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TotalAmount,ProductId,WarehouseId")] InventoryBalance inventoryBalance)
        {
            if (id != inventoryBalance.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inventoryBalance);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InventoryBalanceExists(inventoryBalance.Id))
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
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName", inventoryBalance.ProductId);
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "WarehouseId", "City", inventoryBalance.WarehouseId);
            return View(inventoryBalance);
        }

        // GET: InventoryBalance/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.inventoryBalances == null)
            {
                return NotFound();
            }

            var inventoryBalance = await _context.inventoryBalances
                .Include(i => i.Product)
                .Include(i => i.Warehouse)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inventoryBalance == null)
            {
                return NotFound();
            }

            return View(inventoryBalance);
        }

        // POST: InventoryBalance/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.inventoryBalances == null)
            {
                return Problem("Entity set 'AppDbContext.inventoryBalances'  is null.");
            }
            var inventoryBalance = await _context.inventoryBalances.FindAsync(id);
            if (inventoryBalance != null)
            {
                _context.inventoryBalances.Remove(inventoryBalance);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InventoryBalanceExists(int id)
        {
          return (_context.inventoryBalances?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
