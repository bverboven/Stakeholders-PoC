using Regira.Entities.Abstractions;

namespace Regira.Stakeholders.Core.Entities
{
    public class ContactRole : IEntityWithSerial
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
}