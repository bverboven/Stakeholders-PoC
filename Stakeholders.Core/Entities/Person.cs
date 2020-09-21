﻿namespace Regira.Stakeholders.Core.Entities
{
    public class Person : Stakeholder
    {
        public string Salutation { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }

        public override string Title => $"{Salutation ?? string.Empty} {GivenName ?? string.Empty} {FamilyName ?? string.Empty}".Trim();
    }
}
