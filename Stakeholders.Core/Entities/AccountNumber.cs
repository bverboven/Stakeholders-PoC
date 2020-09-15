namespace Regira.Stakeholders.Core.Entities
{
    public enum AccountType
    {
        IBAN,
        BIC
    }
    public class AccountNumber
    {
        public string Number { get; set; }
        public AccountType AccountType { get; set; } = AccountType.IBAN;


        // help entity mapping
        public static implicit operator string(AccountNumber account) => account.Number;
        public static implicit operator AccountNumber(string nr) => new AccountNumber { Number = nr };
    }
}