using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelReymer.Models;

public partial class MarketingCampaign : IValidatableObject
{
    [Display(Name = "Код акции")]
    public int CampaignId { get; set; }

    [Display(Name = "Категория")]
    public int RoomCategoryId { get; set; }

    [Display(Name = "Название")]
    [StringLength(150)]
    public string Name { get; set; } = null!;

    [Display(Name = "Описание")]
    [StringLength(1000)]
    public string? Description { get; set; }

    [Display(Name = "Значение скидки")]
    public decimal AdjustmentValue { get; set; }

    [Display(Name = "Дата начала")]
    public DateOnly StartDate { get; set; }

    [Display(Name = "Дата окончания")]
    public DateOnly EndDate { get; set; }

    [Display(Name = "Активна")]
    public bool IsActive { get; set; }

    public virtual RoomCategory RoomCategory { get; set; } = null!;

    /// <summary>CHECK (END_DATE &gt;= START_DATE) в MARKETING_CAMPAIGN.</summary>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (EndDate < StartDate)
            yield return new ValidationResult("Дата окончания не может быть раньше даты начала.", [nameof(EndDate), nameof(StartDate)]);
        if (AdjustmentValue < 0)
            yield return new ValidationResult("Значение корректировки не может быть отрицательным.", [nameof(AdjustmentValue)]);
    }
}
