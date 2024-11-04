using System;
using System.Collections.Generic;

namespace AGOC.Models;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public DateTime? CreatedOn { get; set; }

    public int? CreatedById { get; set; }

    public string? CreatedByName { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public int? ModifiedById { get; set; }

    public string? ModifiedByName { get; set; }

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
