using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelReymer.Models;

public partial class Client
{
    [Display(Name = "Код клиента")]
    public int ClientId { get; set; }

    [Display(Name = "ФИО")]
    [StringLength(200)]
    public string Name { get; set; } = null!;

    [Display(Name = "Серия паспорта")]
    [StringLength(10)]
    public string PassportSeries { get; set; } = null!;

    [Display(Name = "Номер паспорта")]
    [StringLength(20)]
    public string PassportNumber { get; set; } = null!;

    [Display(Name = "Телефон")]
    [StringLength(30)]
    public string Phone { get; set; } = null!;

    [Display(Name = "Эл. почта")]
    [StringLength(255)]
    public string? Email { get; set; }

    [Display(Name = "Статус")]
    [StringLength(20)]
    public string Status { get; set; } = null!;

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    public virtual ICollection<Discount> Discounts { get; set; } = new List<Discount>();
}
