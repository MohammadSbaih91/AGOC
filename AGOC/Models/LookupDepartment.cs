using System;
using System.Collections.Generic;

namespace AGOC.Models;

public partial class LookupDepartment
{
    public int Id { get; set; }

    public string DepartmentName { get; set; } = null!;

    public DateTime? CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? ModifiedBy { get; set; }
}
