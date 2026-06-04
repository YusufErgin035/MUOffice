using System.ComponentModel.DataAnnotations;

namespace WebProgProjc.Models;

public class Room
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Oda adı zorunludur.")]
    [StringLength(100)]
    [Display(Name = "Oda Adı")]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    [Display(Name = "Açıklama")]
    public string? Description { get; set; }

    [Range(1, 500)]
    [Display(Name = "Kapasite")]
    public int Capacity { get; set; } = 10;

    [Display(Name = "Aktif")]
    public bool IsActive { get; set; } = true;

    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
