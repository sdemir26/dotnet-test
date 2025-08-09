using AutoMapper;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace serkan_test1.Models
{
    public class MusteriDto
    {
        [DisplayName("name"), MinLength(8,ErrorMessage ="en az 8 karakter olmalı.")]
        public string Adi { get; set; } = string.Empty;
    }
}
