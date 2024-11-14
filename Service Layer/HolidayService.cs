using Employee_Management.Interface;
using Microsoft.Extensions.Caching.Memory;

namespace Employee_Management.Service_Layer
{
    public class HolidayService : IHolidayService
    {
        private readonly IHolidayRepository _holidayRepository;
        private readonly IMemoryCache _cache;

        public HolidayService(IHolidayRepository holidayRepository, IMemoryCache cache)
        {
            _holidayRepository = holidayRepository;
            _cache = cache;
        }

        public async Task<List<DateTime>> GetPublicHolidaysAsync()
        {
            if (!_cache.TryGetValue("PublicHolidays", out List<DateTime> publicHolidays))
            {
                publicHolidays = await _holidayRepository.GetPublicHolidaysAsync();
                _cache.Set("PublicHolidays", publicHolidays, TimeSpan.FromMinutes(5));
            }
            return publicHolidays;
        }
    }
}
