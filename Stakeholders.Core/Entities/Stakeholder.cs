using System.Collections.Generic;
using Regira.Entities.Abstractions;
using Regira.Stakeholders.Core.Abstractions;

namespace Regira.Stakeholders.Core.Entities
{
    public enum StakeholderType
    {
        Organization,
        Person
    }

    public abstract class Stakeholder : IEntityWithSerial, IStakeholder
    {
        public int Id { get; set; }
        public StakeholderType StakeholderType { get; set; }

        public string Phone { get; set; }
        public string Email { get; set; }
        public string Kbo { get; set; }
        public Address Address { get; set; } = new Address();
        public ICollection<AccountNumber> BankAccounts { get; set; }
        public ICollection<StakeholderContact> Contacts { get; set; }
        public string Note { get; set; }

        public abstract string GetTitle();
    }
}
