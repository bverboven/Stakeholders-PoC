using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
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
        public abstract string Title { get; }

        public string Phone { get; set; }
        public string Email { get; set; }
        public string Kbo { get; set; }
        public Address Address { get; set; } = new Address();
        public string Note { get; set; }

        public ICollection<AccountNumber> BankAccounts { get; set; }

        // RoleGivers
        public ICollection<StakeholderContact> Superiors { get; set; }
        // RoleReceivers
        public ICollection<StakeholderContact> Subordinates { get; set; }
        [NotMapped]
        public ICollection<StakeholderContact> Contacts
        {
            get
            {
                if (Superiors == null || Subordinates == null)
                {
                    return null;
                }

                return new ReadOnlyCollection<StakeholderContact>(Superiors.Concat(Subordinates).ToList());
            }
        }
    }
}
