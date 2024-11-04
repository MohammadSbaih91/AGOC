using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AGOC.Models;

namespace AGOC.ViewModels
{
    [Index("Username", Name = "UQ__Users__536C85E4C8DE036C", IsUnique = true)]
    [Index("Email", Name = "UQ__Users__A9D1053470EEB861", IsUnique = true)]
    public partial class UsersViewModel
    {
        [Key]
        public int Id { get; set; }

        [StringLength(500)]
        public string Username { get; set; } = null!;

        [StringLength(500)]
        public string? FirstName { get; set; }

        [StringLength(500)]
        public string? LastName { get; set; }

        [StringLength(500)]
        public string Email { get; set; } = null!;

        [StringLength(500)]
        public string Password { get; set; } = null!;

        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }

        public int? CreatedById { get; set; }

        [StringLength(500)]
        public string? CreatedByName { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? ModifiedOn { get; set; }

        public int? ModifiedById { get; set; }

        [StringLength(500)]
        public string? ModifiedByName { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
