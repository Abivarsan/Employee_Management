using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore; // Keep this for the Index attribute

namespace Employee_Management.Models;

[Table("Employee")]
[Microsoft.EntityFrameworkCore.Index("Email", Name = "UQ__Employee__A9D10534B178EAEC", IsUnique = true)]  // This is from EF Core
public partial class Employee
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(100)]
    public string Email { get; set; } = null!;

    [StringLength(100)]
    public string JobPosition { get; set; } = null!;
}
