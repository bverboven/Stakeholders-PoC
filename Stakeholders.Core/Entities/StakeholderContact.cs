namespace Regira.Stakeholders.Core.Entities
{
    public class StakeholderContact
    {
        public int RoleGiverId { get; set; }
        public int RoleBearerId { get; set; }
        /// <summary>
        /// Role for contact towards parent
        /// </summary>
        public int RoleId { get; set; }

        public Stakeholder RoleGiver { get; set; }
        public Stakeholder RoleBearer { get; set; }
        public ContactRole Role { get; set; }
    }
}
