using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelReymer.Models;

public partial class HousekeepingTask : IValidatableObject
{
    [Display(Name = "Код задачи")]
    public int HousekeepingId { get; set; }

    [Display(Name = "Номер")]
    public int RoomId { get; set; }

    [Display(Name = "Назначена")]
    public int? UserId { get; set; }

    [Display(Name = "Дата создания")]
    public DateTime CreatedAt { get; set; }

    [Display(Name = "Срок")]
    public DateTime? DueAt { get; set; }

    [Display(Name = "Выполнена")]
    public DateTime? CompletedAt { get; set; }

    [Display(Name = "Статус")]
    [StringLength(20)]
    public string TaskStatus { get; set; } = null!;

    [Display(Name = "Приоритет")]
    public int PriorityNo { get; set; }

    [Display(Name = "Комментарий")]
    [StringLength(1000)]
    public string? Comment { get; set; }

    public virtual Room Room { get; set; } = null!;
    public virtual UserAccount? User { get; set; }

    /// <summary>CHECK (PRIORITY_NO BETWEEN 1 AND 10). Значение 0 — «не задано», контроллер подставит 5 при создании.</summary>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (PriorityNo != 0 && (PriorityNo < 1 || PriorityNo > 10))
            yield return new ValidationResult("Приоритет должен быть от 1 до 10.", [nameof(PriorityNo)]);
    }
}
