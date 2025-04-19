using System;
using System.Collections.Generic;

namespace MedLab.Model.DbModels;

public partial class RegistrationCode
{
    public string Login { get; set; } = null!;

    public string Code { get; set; } = null!;
}
