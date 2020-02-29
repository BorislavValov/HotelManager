using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HotelManager.Data;
using HotelManager.Models;
using System.Security.Claims;
using HotelManager.Repository;

namespace HotelManager.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ReservationsRepository _reservationsRepository;

        public ReservationsController(ApplicationDbContext context, ReservationsRepository reservationsRepository)
        {
            _context = context;
            _reservationsRepository = reservationsRepository;
        }

        // GET: Reservations
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Reservations.Include(r => r.Client).Include(r => r.Room);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Reservations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.Client)
                .Include(r => r.Room)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // GET: Reservations/Create
        public IActionResult Create()
        {
            //ViewData["ClientId"] = new SelectList(_context.UsersTable, "Id", "Id");
            //ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Number");
            return View(new CreateReservationViewModel());
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateReservationViewModel createReservationViewModel)  //([Bind("ArrivalDate,DepartureDate,BreakfastIncluded,IsAllInclusive")] Reservation reservation)
        {
            Reservation reservation = new Reservation();
            
            reservation.ArrivalDate = createReservationViewModel.ArrivalDate;
            reservation.DepartureDate = createReservationViewModel.DepartureDate;
            reservation.BreakfastIncluded = createReservationViewModel.BreakfastIncluded;
            reservation.IsAllInclusive = createReservationViewModel.IsAllInclusive;

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            reservation.ClientId = userId;
            //ReservationsRepository reservationsRepository = new ReservationsRepository(_context);
            bool success = await _reservationsRepository.Create(reservation, userId, createReservationViewModel.ChildrenCount, createReservationViewModel.AdultsCount);
            return RedirectToAction(nameof(Index));
           
        }

        // GET: Reservations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RoomId,ClientId,ArrivalDate,DepartureDate,BreakfastIncluded,IsAllInclusive,TotalPrice")] Reservation reservation)
        {
            if (id != reservation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.Id))
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
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.Client)
                .Include(r => r.Room)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _reservationsRepository.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservations.Any(e => e.Id == id);
        }
    }
}
