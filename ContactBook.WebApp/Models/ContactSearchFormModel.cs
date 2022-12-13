using System.ComponentModel.DataAnnotations;

using ContactBook.Data;

namespace ContactBook.WebApp.Models
{
    using static DataConstants;
    public class ContactSearchFormModel
    {
        [StringLength(MaxKeywordLength)]
        public string? Keyword { get; init; }

        public IEnumerable<ContactViewModel> Contacts { get; set; } = new List<ContactViewModel>();
    }
}
