using Employee_Management.Interface;
using Employee_Management.Models;
using Microsoft.EntityFrameworkCore;

namespace Employee_Management.Repository_Layer
{
    public class HolidayRepository : IHolidayRepository
    
    {
        private readonly AppDbContext _context;

        public HolidayRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<DateTime>> GetPublicHolidaysAsync()
        {
            // Assuming the database stores public holidays as DateOnly, we convert it to DateTime
            return await _context.PublicHolidays
                                 .Select(h => h.HolidayDate.ToDateTime(TimeOnly.MinValue))  // Convert DateOnly to DateTime
                                 .ToListAsync();
        }
    }
}
