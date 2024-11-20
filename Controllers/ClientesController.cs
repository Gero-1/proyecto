using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MiradorB.Models;

namespace MiradorB.Controllers
{
    public class ClientesController : Controller
    {
        private readonly BdMiradorContext _context;

        public ClientesController(BdMiradorContext context)
        {
            _context = context;
        }

        // GET: Clientes
        public async Task<IActionResult> Index()
        {
            var bdMiradorContext = _context.Clientes.Include(c => c.IdRolNavigation).Include(c => c.IdTipoDocumentoNavigation);
            return View(await bdMiradorContext.ToListAsync());
        }

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .Include(c => c.IdRolNavigation)
                .Include(c => c.IdTipoDocumentoNavigation)
                .FirstOrDefaultAsync(m => m.NroDocumento == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {
            ViewData["IdRol"] = new SelectList(_context.Roles, "IdRol", "IdRol");
            ViewData["IdTipoDocumento"] = new SelectList(_context.TipoDocumentos, "IdTipoDocumento", "NomTipoDcumento");
            return View();
        }

        // POST: Clientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NroDocumento,IdTipoDocumento,Nombres,Apellidos,Celular,Correo,Contrasena,Estado,IdRol")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cliente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdRol"] = new SelectList(_context.Roles, "IdRol", "IdRol", cliente.IdRol);
            ViewData["IdTipoDocumento"] = new SelectList(_context.TipoDocumentos, "IdTipoDocumento", "IdTipoDocumento", cliente.IdTipoDocumento);
            return View(cliente);
        }

        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            ViewData["IdRol"] = new SelectList(_context.Roles, "IdRol", "IdRol", cliente.IdRol);
            ViewData["IdTipoDocumento"] = new SelectList(_context.TipoDocumentos, "IdTipoDocumento", "IdTipoDocumento", cliente.IdTipoDocumento);
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NroDocumento,IdTipoDocumento,Nombres,Apellidos,Celular,Correo,Contrasena,Estado,IdRol")] Cliente cliente)
        {
            if (id != cliente.NroDocumento)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cliente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.NroDocumento))
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
            ViewData["IdRol"] = new SelectList(_context.Roles, "IdRol", "IdRol", cliente.IdRol);
            ViewData["IdTipoDocumento"] = new SelectList(_context.TipoDocumentos, "IdTipoDocumento", "IdTipoDocumento", cliente.IdTipoDocumento);
            return View(cliente);
        }

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .Include(c => c.IdRolNavigation)
                .Include(c => c.IdTipoDocumentoNavigation)
                .FirstOrDefaultAsync(m => m.NroDocumento == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente != null)
            {
                _context.Clientes.Remove(cliente);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClienteExists(int id)
        {
            return _context.Clientes.Any(e => e.NroDocumento == id);
        }
    }
}
