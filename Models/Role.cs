using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelReymer.Models;

public partial class Role
{
    [Display(Name = "Код роли")]
    public int RoleId { get; set; }

    [Display(Name = "Название")]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Display(Name = "Описание")]
    [StringLength(500)]
    public string? Description { get; set; }

    public virtual ICollection<UserAccount> UserAccounts { get; set; } = new List<UserAccount>();
}
