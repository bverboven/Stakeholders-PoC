using Bogus;
using Regira.Stakeholders.Core.Entities;
using Regira.Stakeholders.Library.Data;
using Regira.Stakeholders.Library.Extensions;
using Regira.Web.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Bogus.Extensions;
using Address = Regira.Stakeholders.Core.Entities.Address;
using Person = Regira.Stakeholders.Core.Entities.Person;

namespace Regira.Stakeholders.ConsoleApp
{
    public class Seeder
    {
        private const int MAX_COUNT = 5000;
        private readonly StakeholdersContext _db;
        public Seeder(StakeholdersContext dbContext)
        {
            _db = dbContext;
        }

        public void Seed(int n = MAX_COUNT)
        {
            var randomizer = new Randomizer();

            var i = 0;
            do
            {
                var count = Math.Min(n, MAX_COUNT);

                var addressesRule = new Faker<Address>()
                    .RuleFor(a => a.Street, (f, u) => f.Address.StreetName())
                    .RuleFor(a => a.Number, (f, u) => f.Address.BuildingNumber())
                    .RuleFor(a => a.PostalCode, (f, u) => f.Address.ZipCode())
                    .RuleFor(a => a.City, (f, u) => f.Address.City())
                    .RuleFor(a => a.CountryCode, (f, u) => f.Address.CountryCode());

                var accountRule = new Faker<AccountNumber>()
                    .RuleFor(a => a.Number, (f, u) =>
                    {
                        var nr = long.Parse($"{randomizer.Number(10, 50)}32{randomizer.Number(99999).ToString().PadLeft(6, '0')}");
                        return $"BE{nr}{(nr % 97).ToString().PadLeft(2, '0')}";
                    });

                var personRules = new Faker<Person>()
                    .RuleFor(x => x.GivenName, (f, x) => f.Name.FirstName())
                    .RuleFor(x => x.FamilyName, (f, x) => f.Name.LastName())
                    .RuleFor(x => x.Salutation, (f, x) => f.Name.Prefix().OrNull(f, 0.80f))
                    .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.GivenName, u.FamilyName).ToLower());

                var organizationRules = new Faker<Organization>()
                    .RuleFor(u => u.Name, (f, u) => f.Company.CompanyName())
                    .RuleFor(u => u.Email, (f, u) => $"info@{Normalize(u.Name)}.com".ToLower());

                var persons = personRules
                    .RuleFor(u => u.Kbo, (f, u) => $"BE{randomizer.Number(int.MaxValue).ToString().PadLeft(10, '0')}")
                    .RuleFor(u => u.Phone, (f, u) => f.Phone.PhoneNumberFormat())
                    .RuleFor(u => u.Address, (f, u) => addressesRule.Generate().OrNull(f, .25f))
                    .RuleFor(x => x.BankAccounts, (f, u) => accountRule.Generate(randomizer.Number(0, 2)))
                    .RuleFor(x => x.Note, (f, u) => f.Lorem.Paragraphs(0, 2))
                    .Generate(count);

                var organizations = organizationRules
                    .RuleFor(u => u.Kbo, (f, u) => $"BE{randomizer.Number(int.MaxValue).ToString().PadLeft(10, '0')}")
                    .RuleFor(u => u.Phone, (f, u) => f.Phone.PhoneNumberFormat())
                    .RuleFor(u => u.Address, (f, u) => addressesRule.Generate().OrNull(f, .10f))
                    .RuleFor(x => x.BankAccounts, (f, u) => accountRule.Generate(randomizer.Number(0, 3)))
                    .RuleFor(x => x.Note, (f, u) => f.Lorem.Paragraphs(0, 3))
                    .Generate(count);

                var stakeholders = organizations.Cast<Stakeholder>().Concat(persons).ToList();

                _db.Stakeholders.AddRange(stakeholders);
                _db.SaveChanges();

                var roles = new Faker<ContactRole>()
                    .RuleFor(x => x.Title, (f, x) => f.Name.JobType());
                var contacts = new Faker<StakeholderContact>()
                    .RuleFor(x => x.Stakeholder, (f, x) => f.PickRandom(stakeholders))
                    .RuleFor(x => x.Contact, (f, x) => f.PickRandom(stakeholders))
                    .RuleFor(x => x.Role, (f, x) => f.PickRandom(roles))
                    .Generate(count)
                    .FindAll(c => c.Stakeholder != c.Contact);
                
                _db.StakeholderContacts.AddRange(contacts);
                _db.SaveChanges();

                // clean up invalid contacts
                var tree = stakeholders.ToStakeholdersTree();
                var treeContacts = tree.SelectMany(node => node.Value.Contacts ?? new List<StakeholderContact>())
                    .ToList();
                var invalidContacts = contacts
                    .Except(treeContacts)
                    .ToList();

                _db.StakeholderContacts.RemoveRange(invalidContacts);
                _db.SaveChanges();
            }
            while (++i * MAX_COUNT < n);
        }

        string Normalize(string s)
        {
            s = UriUtility.Slugify(s);
            s = Regex.Replace(s, "-+", "-").Trim('-');
            return s;
        }
    }
}
