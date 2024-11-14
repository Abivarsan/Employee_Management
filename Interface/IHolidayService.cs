namespace Employee_Management.Interface
{
    public interface IHolidayService
    {
        Task<List<DateTime>> GetPublicHolidaysAsync();
    }
}
