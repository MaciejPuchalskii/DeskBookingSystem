using DeskBookingSystem.Data.Models;
using DeskBookingSystem.Dto;
using DeskBookingSystem.Repositories;

namespace DeskBookingSystem.Services
{
    public class DeskService : IDeskService
    {
        private readonly IDeskRepository _deskRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeskService(IDeskRepository deskRepo, IHttpContextAccessor httpContextAccessor)
        {
            _deskRepository = deskRepo;
            _httpContextAccessor = httpContextAccessor;
        }

        public AddDeskResponseDto Add(AddDeskCommandDto addDeskCommandDto)
        {
            var desk = new Desk
            {
                IsOperational = addDeskCommandDto.IsOperational,
                LocationId = addDeskCommandDto.LocationId,
            };
            _deskRepository.Add(desk);

            return new AddDeskResponseDto()
            {
                Id = desk.Id,
                LocationId = desk.LocationId,
                IsOperational = desk.IsOperational
            };
        }

        public RemoveDeskResponseDto Remove(RemoveDeskCommandDto removeDeskCommandDto)
        {
            var desk = _deskRepository.GetById(removeDeskCommandDto.Id);
            if (desk == null)
            {
                throw new Exception("Desk not found.");
            }

            if (desk.Reservations.Any())
            {
                throw new Exception("Cannot remove desk with existing reservations.");
            }

            if (!_deskRepository.Remove(desk))
            {
                throw new Exception("Failed to remove desk.");
            }

            return new RemoveDeskResponseDto()
            {
                Id = desk.Id
            };
        }

        public ChangeDeskAvailabiltyResponseDto ChangeDeskAvailability(ChangeDeskAvailabiltyCommandDto changeDeskAvailabiltyCommandDto)
        {
            var desk = _deskRepository.GetById(changeDeskAvailabiltyCommandDto.Id);
            if (desk == null)
            {
                throw new Exception("Desk not found.");
            }
            if (desk.Reservations.Any() && !changeDeskAvailabiltyCommandDto.IsOperational)
            {
                throw new Exception("Cannot disable a desk with existing reservations.");
            }
            _deskRepository.ChangeDeskAvailability(desk, changeDeskAvailabiltyCommandDto.IsOperational);

            return new ChangeDeskAvailabiltyResponseDto()
            {
                Id = desk.Id,
                Availability = desk.IsOperational
            };
        }

        public GetDeskDetailsResponseDto GetDeskDetails(GetDeskDetailsQueryDto getDeskDetailsQueryDto)
        {
            var desk = _deskRepository.GetDetails(getDeskDetailsQueryDto.Id);
            if (desk == null)
            {
                throw new Exception("Desk not found.");
            }

            var isAdmin = _httpContextAccessor?.HttpContext?.User?.IsInRole("Admin") ?? false;

            return new GetDeskDetailsResponseDto()
            {
                Id = desk.Id,
                IsOperational = desk.IsOperational,
                LocatioId = desk.LocationId,
                Reservations = isAdmin ? desk.Reservations.Select(r => new ReservationDto
                {
                    Id = r.Id,
                    BookingDate = r.BookingDate,
                    ReservationDate = r.ReservationDate,
                    HowManyDays = r.HowManyDays,
                    UserId = r.UserId
                }).ToList()
                : new List<ReservationDto>()
            };
        }
    }
}