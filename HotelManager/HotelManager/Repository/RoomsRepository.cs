using HotelManager.Data;
using HotelManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManager.Repository
{
    public class RoomsRepository
    {
        private readonly ApplicationDbContext _context;

        public RoomsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        async public Task<bool> Create(Room room)
        {
            try
            {
                if(!await RoomExists(room.Number))
                {
                    _context.Rooms.Add(room);
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    //Room with that room number already exists
                    return false;
                }
                
            }
            catch
            {
                return false;
            }
        }

        async public Task<bool> Remove(Room room)
        {
            try
            {

                _context.Rooms.Remove(room);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        async public Task<bool> Remove(int roomId)
        {
            try
            {
                Room room = _context.Rooms.Where(room => room.Id == roomId).First();
                _context.Rooms.Remove(room);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        async public Task<bool> Update(Room room)
        {
            try
            {
                _context.Rooms.Update(room);
                return true;
            }
            catch
            {
                return false;
            }
        }

        async public Task<bool> RoomExists(string roomNumber)
        {
            Room room = _context.Rooms.Where(r => r.Number == roomNumber).FirstOrDefault();
            if(room == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
