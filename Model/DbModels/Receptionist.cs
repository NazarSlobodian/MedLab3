using System;
using System.Collections.Generic;

namespace MedLab.Model.DbModels;

public partial class Receptionist
{
    public int ReceptionistId { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string ContactNumber { get; set; } = null!;

    public int CollectionPointId { get; set; }

    public virtual CollectionPoint CollectionPoint { get; set; } = null!;

    public virtual ICollection<TestBatch> TestBatches { get; set; } = new List<TestBatch>();
}
