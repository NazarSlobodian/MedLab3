using System;
using System.Collections.Generic;

namespace MedLab.Model.DbModels;

public partial class TestPerformer
{
    public int LaboratoryId { get; set; }

    public int TestTypeId { get; set; }

    public virtual Laboratory Laboratory { get; set; } = null!;

    public virtual TestType TestType { get; set; } = null!;
}
