using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelReymer.Models;

public partial class Room
{
    [Display(Name = "Код номера")]
    public int RoomId { get; set; }

    [Display(Name = "Категория")]
    public int CategoryId { get; set; }

    [Display(Name = "Номер комнаты")]
    [StringLength(20)]
    public string RoomNumber { get; set; } = null!;

    [Display(Name = "Этаж")]
    public int? Floor { get; set; }

    [Display(Name = "Статус")]
    [StringLength(30)]
    public string Status { get; set; } = null!;

    [Display(Name = "Примечание")]
    [StringLength(500)]
    public string? Note { get; set; }

    public virtual ICollection<BookingRoom> BookingRooms { get; set; } = new List<BookingRoom>();
    public virtual RoomCategory Category { get; set; } = null!;
    public virtual ICollection<HousekeepingTask> HousekeepingTasks { get; set; } = new List<HousekeepingTask>();
}
