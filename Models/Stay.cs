using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelReymer.Models;

public partial class Stay : IValidatableObject
{
    [Display(Name = "Код проживания")]
    public int StayId { get; set; }

    [Display(Name = "Бронь-Номер")]
    public int BookingRoomId { get; set; }

    [Display(Name = "Заезд факт")]
    public DateTime CheckinAt { get; set; }

    [Display(Name = "Выезд план")]
    public DateOnly CheckoutDatePlan { get; set; }

    [Display(Name = "Выезд факт")]
    public DateTime? CheckoutAt { get; set; }

    [Display(Name = "Статус")]
    [StringLength(20)]
    public string StayStatus { get; set; } = null!;

    [Display(Name = "Сумма без скидки")]
    public decimal AmountBeforeDiscount { get; set; }

    [Display(Name = "Скидка")]
    public decimal DiscountAmount { get; set; }

    [Display(Name = "Итого")]
    public decimal TotalAmount { get; set; }

    public virtual BookingRoom BookingRoom { get; set; } = null!;
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (AmountBeforeDiscount < 0 || DiscountAmount < 0 || TotalAmount < 0)
            yield return new ValidationResult("Суммы не могут быть отрицательными.", [nameof(AmountBeforeDiscount), nameof(DiscountAmount), nameof(TotalAmount)]);
    }
}
