namespace AGOC.Models;

public partial class Role
{
    public int Id { get; set; }

    public string RoleDescription { get; set; } = null!;

    public DateTime? CreatedOn { get; set; }

    public int? CreatedById { get; set; }

    public string? CreatedByName { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public int? ModifiedById { get; set; }

    public string? ModifiedByName { get; set; }

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
