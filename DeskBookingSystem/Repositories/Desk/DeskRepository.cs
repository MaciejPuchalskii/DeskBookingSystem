﻿using DeskBookingSystem.Data;
using DeskBookingSystem.Models;
using DeskBookingSystem.Services;
using Microsoft.EntityFrameworkCore;

namespace DeskBookingSystem.Repositories
{
    public class DeskRepository : IDeskRepository
    {
        private readonly BookingContext _context;

        public DeskRepository(BookingContext context)
        {
            _context = context;
        }

        public void Add(Desk desk)
        {
            _context.Desks.Add(desk);
            _context.SaveChanges();
        }

        public Desk GetById(int id)
        {
            return _context.Desks.Include(d => d.Reservations).FirstOrDefault(d => d.Id == id);
        }

        public bool Remove(Desk desk)
        {
            _context.Desks.Remove(desk);
            int changes = _context.SaveChanges();
            return changes > 0;
        }

        public void ChangeDeskAvailability(Desk desk, bool newStatus)
        {
            desk.IsAvailable = newStatus;
            _context.SaveChanges();
        }

        public Desk GetDetails(int id)
        {
            return _context.Desks.Include(d => d.Reservations).FirstOrDefault(d => d.Id == id);
        }
    }
}