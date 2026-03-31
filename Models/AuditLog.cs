using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelReymer.Models;

public partial class AuditLog
{
    [Display(Name = "Код записи")]
    public int LogId { get; set; }

    [Display(Name = "Пользователь")]
    public int UserId { get; set; }

    [Display(Name = "Дата/время")]
    public DateTime Date { get; set; }

    [Display(Name = "Действие")]
    [StringLength(200)]
    public string Action { get; set; } = null!;

    [Display(Name = "Сущность")]
    [StringLength(100)]
    public string Entity { get; set; } = null!;

    [Display(Name = "ID сущности")]
    [StringLength(50)]
    public string? EntityId { get; set; }

    [Display(Name = "Результат")]
    [StringLength(20)]
    public string Status { get; set; } = null!;

    public virtual UserAccount User { get; set; } = null!;
}
