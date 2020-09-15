using System.Collections.Generic;

namespace Regira.Stakeholders.Core.Entities
{
    public class Organization : Stakeholder
    {
        public string Name { get; set; }
        public override string GetTitle() => Name;

    }
}