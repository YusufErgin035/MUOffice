using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebProgProjc.Models;

public class Reservation
{
    public int Id { get; set; }

    [Required]
    public int RoomId { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Başlangıç Saati")]
    public DateTime StartTime { get; set; }

    [Required]
    [Display(Name = "Bitiş Saati")]
    public DateTime EndTime { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public bool IsCancelled { get; set; } = false;

    [Display(Name = "İptal Nedeni")]
    public string? CancellationReason { get; set; }

    [ForeignKey(nameof(RoomId))]
    public Room Room { get; set; } = null!;

    [ForeignKey(nameof(UserId))]
    public ApplicationUser User { get; set; } = null!;
}
