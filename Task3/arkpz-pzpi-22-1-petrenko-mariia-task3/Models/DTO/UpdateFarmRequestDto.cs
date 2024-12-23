﻿namespace FarmKeeper.Models.DTO
{
    public class UpdateFarmRequestDto
    {
        public string Name { get; set; }
        public string Country { get; set; }
        public string City { get; set; } 
        public string Street { get; set; }

        public Guid OwnerId { get; set; }
    }
}