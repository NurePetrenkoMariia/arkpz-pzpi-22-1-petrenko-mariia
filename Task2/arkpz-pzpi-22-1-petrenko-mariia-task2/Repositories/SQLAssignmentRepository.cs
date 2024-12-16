﻿using FarmKeeper.Data;
using FarmKeeper.Models;
using Microsoft.EntityFrameworkCore;

namespace FarmKeeper.Repositories
{
    public class SQLAssignmentRepository : IAssignmentRepository
    {
        private readonly FarmKeeperDbContext dbContext;
        public SQLAssignmentRepository(FarmKeeperDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Assignment> CreateAsync(Assignment assignment)
        {
            await dbContext.Assignments.AddAsync(assignment);
            await dbContext.SaveChangesAsync();
            return assignment;
        }

        public async Task<Assignment?> DeleteAsync(Guid id)
        {
            var existingAssignment = await dbContext.Assignments.FirstOrDefaultAsync(x => x.Id == id);

            if (existingAssignment == null)
            {
                return null;
            }

            dbContext.Assignments.Remove(existingAssignment);
            await dbContext.SaveChangesAsync();
            return existingAssignment;
        }

        public async Task<List<Assignment>> GetAllAsync()
        {
            return await dbContext.Assignments.ToListAsync();
        }

        public async Task<Assignment?> GetByIdAsync(Guid id)
        {
            return await dbContext.Assignments.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Assignment?> UpdateAsync(Guid id, Assignment assignment)
        {
            var existingAssignment = await dbContext.Assignments.FirstOrDefaultAsync(x => x.Id == id);

            if (existingAssignment == null)
            {
                return null;
            }

            existingAssignment.Name = assignment.Name;
            existingAssignment.Description = assignment.Description;
            existingAssignment.Status = assignment.Status;
            existingAssignment.Priority = assignment.Priority;

            await dbContext.SaveChangesAsync();
            return existingAssignment;
        }
    }
}
