﻿using FarmKeeper.Enums;

namespace FarmKeeper.Models.DTO
{
    public class AssignmentDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int NumberOfParticipants { get; set; }
        public Status Status { get; set; }
        public Priority Priority { get; set; }

    }
}