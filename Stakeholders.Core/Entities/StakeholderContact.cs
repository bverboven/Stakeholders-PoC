namespace Regira.Stakeholders.Core.Entities
{
    public class StakeholderContact
    {
        public int StakeholderId { get; set; }
        public int ContactId { get; set; }
        /// <summary>
        /// Role for contact towards parent
        /// </summary>
        public int RoleId { get; set; }

        public Stakeholder Stakeholder { get; set; }
        public Stakeholder Contact { get; set; }
        public ContactRole Role { get; set; }
    }
}
