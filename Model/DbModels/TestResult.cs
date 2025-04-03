using System;
using System.Collections.Generic;

namespace MedLab.Model.DbModels;

public partial class TestResult
{
    public int TestOrderId { get; set; }

    public decimal Result { get; set; }

    public DateOnly DateOfTest { get; set; }

    public virtual TestOrder TestOrder { get; set; } = null!;
}
