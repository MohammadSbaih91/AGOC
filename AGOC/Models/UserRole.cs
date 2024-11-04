namespace AGOC.Models;

public partial class UserRole
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? RoleId { get; set; }

    public DateTime? CreatedOn { get; set; }

    public int? CreatedById { get; set; }

    public string? CreatedByName { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public int? ModifiedById { get; set; }

    public string? ModifiedByName { get; set; }

    public virtual Role? Role { get; set; }

    public virtual User? User { get; set; }
}
