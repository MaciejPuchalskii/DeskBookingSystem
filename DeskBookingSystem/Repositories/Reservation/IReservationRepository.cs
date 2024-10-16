﻿using DeskBookingSystem.Models;

namespace DeskBookingSystem.Repositories
{
    public interface IReservationRepository
    {
        List<Reservation> GetAllReservationsByDeskId(int deskId);

        Reservation GetById(int id);

        void Update(Reservation reservation);

        void UpdateReservation(Reservation reservation, int howManyDays, DateTime newDate);

        public void Add(Reservation reservation);
    }
}