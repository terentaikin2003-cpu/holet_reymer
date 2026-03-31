using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelReymer.Models;

public partial class Booking : IValidatableObject
{
    [Display(Name = "Код брони")]
    public int BookingId { get; set; }

    [Display(Name = "Клиент")]
    public int ClientId { get; set; }

    [Display(Name = "Пользователь")]
    public int UserId { get; set; }

    [Display(Name = "Дата создания")]
    public DateTime CreatedAt { get; set; }

    [Display(Name = "Дата заезда")]
    public DateOnly CheckinDatePlan { get; set; }

    [Display(Name = "Дата выезда")]
    public DateOnly CheckoutDatePlan { get; set; }

    [Display(Name = "Статус")]
    [StringLength(20)]
    public string Status { get; set; } = null!;

    [Display(Name = "Комментарий")]
    [StringLength(1000)]
    public string? Comment { get; set; }

    public virtual ICollection<BookingRoom> BookingRooms { get; set; } = new List<BookingRoom>();
    public virtual Client Client { get; set; } = null!;
    public virtual UserAccount User { get; set; } = null!;

    /// <summary>CHECK (CHECKOUT_DATE_PLAN &gt; CHECKIN_DATE_PLAN) в BOOKING.</summary>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (CheckoutDatePlan <= CheckinDatePlan)
        {
            yield return new ValidationResult(
                "Дата выезда должна быть позже даты заезда.",
                [nameof(CheckoutDatePlan), nameof(CheckinDatePlan)]);
        }
    }
}
