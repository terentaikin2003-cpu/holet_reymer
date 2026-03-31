using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelReymer.Models;

public partial class RoomCategory : IValidatableObject
{
    [Display(Name = "Код категории")]
    public int RoomCategoryId { get; set; }

    [Display(Name = "Название")]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Display(Name = "Вместимость")]
    public int Capacity { get; set; }

    [Display(Name = "Уровень комфорта")]
    [StringLength(30)]
    public string ComfortLevel { get; set; } = null!;

    [Display(Name = "Базовая цена/сутки")]
    public decimal BasePricePerDay { get; set; }

    public virtual ICollection<MarketingCampaign> MarketingCampaigns { get; set; } = new List<MarketingCampaign>();
    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();

    /// <summary>CHECK (CAPACITY &gt; 0), (BASE_PRICE_PER_DAY &gt;= 0) в ROOM_CATEGORY.</summary>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Capacity < 1)
            yield return new ValidationResult("Вместимость должна быть не меньше 1.", [nameof(Capacity)]);
        if (BasePricePerDay < 0)
            yield return new ValidationResult("Базовая цена не может быть отрицательной.", [nameof(BasePricePerDay)]);
    }
}
