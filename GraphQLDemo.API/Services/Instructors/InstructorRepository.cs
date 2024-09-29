using GraphQLDemo.API.DTOs;
using Microsoft.EntityFrameworkCore;

namespace GraphQLDemo.API.Services.Instructors;

public class InstructorRepository
{
    private readonly IDbContextFactory<SchoolDbContext> _contextFactory;
    public InstructorRepository(IDbContextFactory<SchoolDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }
   
    public async Task<InstructorDTO> GetById(Guid instructorId)
    {
        using (SchoolDbContext context = _contextFactory.CreateDbContext())
        {
            return await context.Instructors.FirstOrDefaultAsync(c => c.Id == instructorId);
        }
    }

    internal async Task<IEnumerable<InstructorDTO>> GetManyByIds(IReadOnlyList<Guid> instructorIds)
    {
        using (SchoolDbContext context = _contextFactory.CreateDbContext())
        {
            return await context.Instructors
                .Where(i => instructorIds.Contains(i.Id))
                .ToListAsync();
        }
    }
}
