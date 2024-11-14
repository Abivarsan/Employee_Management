using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Employee_Management.Models;

[Table("PublicHoliday")]
public partial class PublicHoliday
{
    [Key]
    public int Id { get; set; }

    public DateOnly HolidayDate { get; set; }

    [StringLength(255)]
    public string? Description { get; set; }
}
