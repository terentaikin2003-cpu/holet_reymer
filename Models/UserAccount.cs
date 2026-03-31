using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelReymer.Models;

public partial class UserAccount
{
    [Display(Name = "Код пользователя")]
    public int UserId { get; set; }

    [Display(Name = "Роль")]
    public int RoleId { get; set; }

    [Display(Name = "Логин")]
    [StringLength(100)]
    public string Login { get; set; } = null!;

    [Display(Name = "Хэш пароля")]
    [StringLength(64)]
    public string Hash { get; set; } = null!;

    [Display(Name = "ФИО")]
    [StringLength(200)]
    public string FullName { get; set; } = null!;

    [Display(Name = "Телефон")]
    [StringLength(30)]
    public string? Phone { get; set; }

    [Display(Name = "Эл. почта")]
    [StringLength(255)]
    public string? Email { get; set; }

    public virtual ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    public virtual ICollection<HousekeepingTask> HousekeepingTasks { get; set; } = new List<HousekeepingTask>();
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
    public virtual Role Role { get; set; } = null!;
}
