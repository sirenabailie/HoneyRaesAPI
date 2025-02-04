namespace HoneyRaesAPI.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Specialty { get; set; }

        // list of service tickets associated with employees
        public List<ServiceTicket> ServiceTickets { get; set; } = new List<ServiceTicket>();
    }
}
