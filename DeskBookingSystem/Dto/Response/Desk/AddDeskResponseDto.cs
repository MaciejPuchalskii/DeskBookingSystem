﻿namespace DeskBookingSystem.Dto
{
    public class AddDeskResponseDto
    {
        public int Id { get; set; }
        public int LocationId { get; set; }
        public bool IsAvailable { get; set; }
    }
}