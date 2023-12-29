using System;
using System.Collections.Generic;

namespace Task1.Models;

public partial class DossierStructure
{
    public int Id { get; set; }

    public int? ParentId { get; set; }

    public int? OrderNumber { get; set; }

    public string? SectionCode { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<DossierStructure> InverseParent { get; set; } = new List<DossierStructure>();

    public virtual DossierStructure? Parent { get; set; }
}
