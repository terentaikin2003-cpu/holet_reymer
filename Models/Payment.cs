using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelReymer.Models;

public partial class Payment : IValidatableObject
{
    [Display(Name = "Код платежа")]
    public int PaymentId { get; set; }

    [Display(Name = "Проживание")]
    public int StayId { get; set; }

    [Display(Name = "Принял")]
    public int? UserId { get; set; }

    [Display(Name = "Дата/время")]
    public DateTime PaymentAt { get; set; }

    [Display(Name = "Сумма")]
    public decimal Amount { get; set; }

    [Display(Name = "Способ оплаты")]
    [StringLength(20)]
    public string PaymentMethod { get; set; } = null!;

    [Display(Name = "Статус")]
    [StringLength(20)]
    public string PaymentStatus { get; set; } = null!;

    [Display(Name = "Внешний ID")]
    [StringLength(100)]
    public string? ExternalId { get; set; }

    public virtual Stay Stay { get; set; } = null!;
    public virtual UserAccount? User { get; set; }

    /// <summary>CHECK (AMOUNT &gt; 0) в PAYMENT.</summary>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Amount <= 0)
            yield return new ValidationResult("Сумма должна быть больше нуля.", [nameof(Amount)]);
    }
}
