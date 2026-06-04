using System.ComponentModel.DataAnnotations;

namespace WebProgProjc.Models.ViewModels;

public class ReservationViewModel
{
    [Required(ErrorMessage = "Oda seçimi zorunludur.")]
    [Display(Name = "Oda")]
    public int RoomId { get; set; }

    [Required(ErrorMessage = "Başlangıç saati zorunludur.")]
    [Display(Name = "Başlangıç Saati")]
    public DateTime StartTime { get; set; } = DateTime.Now.AddHours(1);

    [Required(ErrorMessage = "Bitiş saati zorunludur.")]
    [Display(Name = "Bitiş Saati")]
    public DateTime EndTime { get; set; } = DateTime.Now.AddHours(2);
}
