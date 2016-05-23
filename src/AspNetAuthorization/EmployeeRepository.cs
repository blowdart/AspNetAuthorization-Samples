namespace AspNetAuthorization
{
    public class EmployeeRepository : IEmployeeRepository
    {
        public bool IsStillEmployed(string badgeNumber)
        {
            // Go off to database to check

            return true;
        }
    }
}
