using System.Text.Json.Serialization;

namespace ContactBook.WebAPI.Models.Contact
{
    public class ContactsCountModel
    {
        public int AllContactsCount { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? UserContactsCount { get; set; }
    }
}
