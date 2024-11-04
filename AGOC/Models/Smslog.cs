using System;
using System.Collections.Generic;

namespace AGOC.Models;

public partial class Smslog
{
    public int Id { get; set; }

    public int? EmpolyeeCode { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Message { get; set; }

    public string? Status { get; set; }

    public DateTime? SentOn { get; set; }

    public string? ErrorMessage { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? ModifiedBy { get; set; }
}
