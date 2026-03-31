using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelReymer.Models;

public partial class Discount : IValidatableObject
{
    [Display(Name = "Код скидки")]
    public int DiscountId { get; set; }

    [Display(Name = "Клиент")]
    public int ClientId { get; set; }

    [Display(Name = "Название")]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Display(Name = "Значение")]
    public decimal DiscountValue { get; set; }

    [Display(Name = "Дата начала")]
    public DateOnly StartDate { get; set; }

    [Display(Name = "Дата окончания")]
    public DateOnly? EndDate { get; set; }

    [Display(Name = "Основание")]
    [StringLength(300)]
    public string? ReasonText { get; set; }

    [Display(Name = "Активна")]
    public bool IsActive { get; set; }

    public virtual Client Client { get; set; } = null!;

    /// <summary>CHECK (DISCOUNT_VALUE &gt;= 0) в DISCOUNT.</summary>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (DiscountValue < 0)
            yield return new ValidationResult("Значение скидки не может быть отрицательным.", [nameof(DiscountValue)]);
    }
}
