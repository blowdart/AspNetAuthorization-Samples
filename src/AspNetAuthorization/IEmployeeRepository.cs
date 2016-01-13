namespace AspNetAuthorization
{
    public interface IEmployeeRepository
    {
        bool IsStillEmployed(string badgeNumber);
    }
}
