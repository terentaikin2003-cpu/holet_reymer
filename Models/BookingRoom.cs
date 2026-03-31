using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelReymer.Models;

public partial class BookingRoom : IValidatableObject
{
    [Display(Name = "Код")]
    public int BookingRoomId { get; set; }

    [Display(Name = "Бронирование")]
    public int BookingId { get; set; }

    [Display(Name = "Номер")]
    public int RoomId { get; set; }

    [Display(Name = "Цена/сутки")]
    public decimal PricePerDay { get; set; }

    [Display(Name = "Вместимость")]
    public int Capacity { get; set; }

    public virtual Booking Booking { get; set; } = null!;
    public virtual Room Room { get; set; } = null!;
    public virtual ICollection<Stay> Stays { get; set; } = new List<Stay>();

    /// <summary>CHECK (PRICE_PER_DAY &gt;= 0), (CAPACITY &gt; 0) в BOOKING_ROOM.</summary>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (PricePerDay < 0)
            yield return new ValidationResult("Цена за сутки не может быть отрицательной.", [nameof(PricePerDay)]);
        if (Capacity < 1)
            yield return new ValidationResult("Вместимость должна быть не меньше 1.", [nameof(Capacity)]);
    }
}
