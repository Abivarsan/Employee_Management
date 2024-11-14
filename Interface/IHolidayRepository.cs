namespace Employee_Management.Interface
{
    public interface IHolidayRepository
    {
        Task<List<DateTime>> GetPublicHolidaysAsync();
    }
}
