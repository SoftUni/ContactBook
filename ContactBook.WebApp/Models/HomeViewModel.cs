namespace ContactBook.WebApp.Models
{
    public class HomeViewModel
    {
        public int AllContactsCount { get; set; }

        public int UserContactsCount { get; set; }

        public string? UserFullName { get; set; }

        public ContactViewModel? UserContact { get; set; }
    }
}
