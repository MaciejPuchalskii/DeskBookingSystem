﻿using DeskBookingSystem.Data.Models;
using DeskBookingSystem.Dto;
using DeskBookingSystem.Repositories;

namespace DeskBookingSystem.Services
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _locationRepository;

        public LocationService(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        public AddLocationResponseDto Add(AddLocationCommandDto addLocationCommandDto)
        {
            if (string.IsNullOrEmpty(addLocationCommandDto.Name))
            {
                throw new Exception("Location name cannot be empty.");
            }

            if (_locationRepository.DoesLocationExist(addLocationCommandDto.Name))
            {
                throw new Exception("Location with this name already exists.");
            }

            var newLocation = new Location
            {
                Name = addLocationCommandDto.Name
            };

            _locationRepository.Add(newLocation);

            return new AddLocationResponseDto()
            {
                Id = newLocation.Id,
                Name = newLocation.Name
            };
        }

        public void ExistLocation(int locationId)
        {
            if (!_locationRepository.DoesLocationExist(locationId))
            {
                throw new Exception("Location not found.");
            }
        }

        public void DoesLocationExist(string name)
        {
            if (!_locationRepository.DoesLocationExist(name))
            {
                throw new Exception("Location not found.");
            }
        }

        public GetDesksFromLocationResponseDto GetDesks(GetDesksFromLocationQueryDto getDesksFromLocationQueryDto)
        {
            var location = _locationRepository.GetById(getDesksFromLocationQueryDto.LocationId);
            if (location == null)
            {
                throw new Exception("Location not found.");
            }

            var allDesksInLocation = location.Desks;

            if (!allDesksInLocation.Any())
            {
                throw new Exception("No desks found in this location.");
            }

            if (getDesksFromLocationQueryDto.areOperational.HasValue)
            {
                allDesksInLocation = allDesksInLocation.Where(d => d.IsOperational == getDesksFromLocationQueryDto.areOperational).ToList();
            }

            var desksDto = new GetDesksFromLocationResponseDto()
            {
                LocationId = location.Id,
                Desks = allDesksInLocation.Select(d => new DeskDetailsDto()
                {
                    Id = d.Id,
                    IsOperational = d.IsOperational
                }).ToList()
            };

            return desksDto;
        }

        public GetDesksByAvailabilityResponseDto GetDesksByAvailability(GetDesksByAvailabilityQueryDto getDesksByAvailabilityQueryDto)
        {
            var location = _locationRepository.GetById(getDesksByAvailabilityQueryDto.LocationId);
            if (location == null)
            {
                throw new Exception("Location not found.");
            }
            if (!location.Desks.Any())
            {
                throw new Exception("No desks found in this location.");
            }
            var allAvailableDesksInLocation = location.Desks.Where(d => !d.Reservations.Any(r => (r.ReservationDate < getDesksByAvailabilityQueryDto.EndDate && r.ReservationDate.AddDays(r.DaysCount) > getDesksByAvailabilityQueryDto.StartDate)) && d.IsOperational == true).ToList();

            if (!allAvailableDesksInLocation.Any())
            {
                throw new Exception("No desks available in this location during the specified time period.");
            }

            var desksDto = new GetDesksByAvailabilityResponseDto()
            {
                Desks = allAvailableDesksInLocation.Select(d => new GetDeskByAvailabilityResponseDto()
                {
                    Id = d.Id,
                    LocationName = location.Name,
                }).ToList()
            };

            return desksDto;
        }

        public RemoveLocationResponseDto Remove(RemoveLocationCommandDto removeLocationCommandDto)
        {
            var location = _locationRepository.GetById(removeLocationCommandDto.Id);
            if (location == null)
            {
                throw new Exception("Location not found.");
            }

            if (location.Desks.Any())
            {
                throw new Exception("Cannot remove location with assigned desks.");
            }

            if (!_locationRepository.Remove(location))
            {
                throw new Exception("Failed to remove location.");
            }

            return new RemoveLocationResponseDto()
            {
                Id = location.Id,
                Name = location.Name
            };
        }
    }
}