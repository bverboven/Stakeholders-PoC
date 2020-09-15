using System.Collections.Generic;
using Regira.Stakeholders.Core.Entities;

namespace Regira.Stakeholders.Core.Abstractions
{
    public interface IStakeholder
    {
        StakeholderType StakeholderType { get; set; }
        string Kbo { get; set; }
        string Phone { get; set; }
        string Email { get; set; }
        Address Address { get; set; }
        ICollection<AccountNumber> BankAccounts { get; set; }

        string GetTitle();
    }
}