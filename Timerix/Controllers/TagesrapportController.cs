﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Timerix.Models;
using Timerix.Models.ViewModels;

namespace Timerix.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagesrapportController : ControllerBase
    {
        private readonly TimerixContext _context;

        public TagesrapportController(TimerixContext context)
        {
            _context = context;
        }

        // GET: api/Tagesrapport
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tagesrapport>>> GetTagesrapport()
        {
            return await _context.Tagesrapport.ToListAsync();
        }

        // GET: api/Tagesrapport/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tagesrapport>> GetTagesrapport(int id)
        {
            var tagesrapport = await _context.Tagesrapport.FindAsync(id);

            if (tagesrapport == null)
            {
                return NotFound();
            }

            return tagesrapport;
        }

        // GET: alle Tagesrapporte vom tag
        //https://localhost:44339/api/tagesrapport/datum?datum=2021-10-11T00:00:00.000Z
        [HttpGet("datum")]
        public async Task<ActionResult<IEnumerable<TagesrapportViewModel>>> GetDatumTagesrapport(string datum)
        {
            var cultureInfo = new CultureInfo("de-DE");
            var date = DateTime.Parse(datum, cultureInfo);
            var tagesrapport =  _context.Tagesrapport
                .Where(tag => tag.Datum == date.Date)
                .Include(tag => tag.Auftrag)
                .Include(tag => tag.Produktionsstrasse)
                .ToList();

            if (tagesrapport == null)
            {
                return NotFound();
            }
            List<TagesrapportViewModel> tvml = new List<TagesrapportViewModel>();
            foreach(Tagesrapport t in tagesrapport)
            {
                TagesrapportViewModel tvm = new TagesrapportViewModel();
                tvm.Datum = t.Datum;
                tvm.Bemerkung = t.Bemerkung;
                tvm.Anzahl = t.Anzahl;
                tvm.AuftragId = t.AuftragId;
                tvm.ProduktionsstrasseId = t.ProduktionsstrasseId;
                AuftragViewModel avm = new AuftragViewModel();
                avm.AuftragId = t.Auftrag.AuftragId;
                avm.Beschreibung = t.Auftrag.Beschreibung;
                avm.ItemId = t.Auftrag.ItemId;
                avm.QtySched = t.Auftrag.QtySched;
                avm.Nummer = t.Auftrag.Nummer;
                tvm.Auftrag = avm;
                ProduktionsstrasseViewModel pvm = new ProduktionsstrasseViewModel();
                pvm.Aktiv = t.Produktionsstrasse.Aktiv;
                pvm.Beschreibung = t.Produktionsstrasse.Beschreibung;
                pvm.ProduktionsstrasseId = t.Produktionsstrasse.ProduktionsstrasseId;
                tvm.Produktionsstrasse = pvm;
                tvml.Add(tvm);
            }
            return tvml;
        }


        // PUT: api/Tagesrapport/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTagesrapport(int id, Tagesrapport tagesrapport)
        {
            if (id != tagesrapport.AuftragId)
            {
                return BadRequest();
            }

            _context.Entry(tagesrapport).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TagesrapportExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Tagesrapport
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Tagesrapport>> PostTagesrapport(Tagesrapport tagesrapport)
        {
            _context.Tagesrapport.Add(tagesrapport);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TagesrapportExists(tagesrapport.AuftragId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetTagesrapport", new { id = tagesrapport.AuftragId }, tagesrapport);
        }

        // DELETE: api/Tagesrapport/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTagesrapport(int id)
        {
            var tagesrapport = await _context.Tagesrapport.FindAsync(id);
            if (tagesrapport == null)
            {
                return NotFound();
            }

            _context.Tagesrapport.Remove(tagesrapport);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TagesrapportExists(int id)
        {
            return _context.Tagesrapport.Any(e => e.AuftragId == id);
        }
    }
}
