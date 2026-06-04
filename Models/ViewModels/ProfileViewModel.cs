using System.ComponentModel.DataAnnotations;

namespace WebProgProjc.Models.ViewModels;

public class ProfileViewModel
{
    [Display(Name = "E-posta")]
    public string Email { get; set; } = string.Empty;

    [StringLength(100)]
    [Display(Name = "Ad Soyad")]
    public string? FullName { get; set; }

    [StringLength(100)]
    [Display(Name = "Departman")]
    public string? Department { get; set; }

    [Phone]
    [Display(Name = "Telefon")]
    public string? PhoneNumber { get; set; }
}
