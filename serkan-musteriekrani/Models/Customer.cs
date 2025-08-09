using System.ComponentModel.DataAnnotations;

namespace serkan_test1.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        public string Adi { get; set; } = string.Empty;
    }
}
