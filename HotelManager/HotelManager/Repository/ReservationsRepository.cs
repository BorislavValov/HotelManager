using HotelManager.Data;
using HotelManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace HotelManager.Repository
{
    public class ReservationsRepository
    {
        private readonly ApplicationDbContext _context;

        public ReservationsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        async public Task<bool> Create(Reservation reservation, string userId, int childrenCount, int adultsCount)
        {
            try
            {
                reservation.ClientId = userId;
                Room room = GetAvailableRoom(childrenCount + adultsCount);
                if(room == null)
                {
                    return false;
                }
                reservation.RoomId = room.Id;
                await BookARoom(room.Id);
                reservation.TotalPrice = adultsCount * room.AdultsPrice + childrenCount * room.ChildrenPrice;

                _context.Add(reservation);
                await _context.SaveChangesAsync();

                return true;
            }
            catch 
            {
                return false;
            }
            
            
        }

        async public Task<bool> Delete(int id)
        {
            try
            {
                Reservation reservation = _context.Reservations.Where(r => r.Id == id).First();
                if (reservation == null)
                {
                    return false;
                }
                await FreeUpRoom(reservation.RoomId);
                _context.Reservations.Remove(reservation);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private Room GetAvailableRoom(int requestedCapacity)
        {
            try
            {
                return _context.Rooms.Where(room => room.IsAvailable == true && room.Capacity >= requestedCapacity).First();
            }
            catch
            {
                return null;
            }

        }

        async private Task FreeUpRoom(int roomId)
        {
            Room room = _context.Rooms.Where(room => room.Id == roomId).First();
            room.IsAvailable = true;
            _context.Update(room);
            await _context.SaveChangesAsync();
        }

        async private Task<bool> BookARoom(int roomId)
        {
            try
            {
                Room room = _context.Rooms.Where(room => room.Id == roomId).First();
                room.IsAvailable = false;
                _context.Update(room);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
            
        }

    }
}
