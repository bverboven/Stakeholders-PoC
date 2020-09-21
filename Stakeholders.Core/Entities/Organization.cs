namespace Regira.Stakeholders.Core.Entities
{
    public class Organization : Stakeholder
    {
        public string Name { get; set; }
        public override string Title => Name;

    }
}