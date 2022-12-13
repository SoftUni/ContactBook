namespace ContactBook.WebApp.Models
{
    public class ContactViewModel
    {
        public int Id { get; init; }

        public string FirstName { get; init; }

        public string LastName { get; init; }

        public string Email { get; init; }

        public string PhoneNumber { get; init; }

        public string? Comments { get; init; }
    }
}
