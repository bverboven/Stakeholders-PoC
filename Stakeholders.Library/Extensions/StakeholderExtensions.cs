using Regira.Extensions;
using Regira.Stakeholders.Core.Entities;
using Regira.TreeList;
using System.Collections.Generic;
using System.Linq;

namespace Regira.Stakeholders.Library.Extensions
{
    public static class StakeholderExtensions
    {
        public static TreeList<Stakeholder> ToStakeholdersTree(this IEnumerable<Stakeholder> items)
        {
            var stakeholders = items.AsList();
            var contacts = stakeholders.SelectMany(x => x.Contacts ?? new StakeholderContact[0])
                .ToList();

            return ToTreeList(stakeholders, contacts);
        }

        static TreeList<Stakeholder> ToTreeList(IList<Stakeholder> stakeholders, IList<StakeholderContact> contacts)
        {
            var tree = new TreeList<Stakeholder>();
            var rootValues = stakeholders
                .Where(s => !contacts.Any(c => c.RoleBearer == s || c.RoleBearerId == s.Id))
                .ToList();
            foreach (var stakeholder in rootValues)
            {
                var node = tree.AddValue(stakeholder);
                AddChildren(contacts, node);
            }

            return tree;
        }
        static void AddChildren(IList<StakeholderContact> contacts, TreeNode<Stakeholder> node)
        {
            //var nodeAncestors = node.GetAncestors().ToArray();
            var children = contacts
                .Where(c => c.RoleGiver == node.Value)
                // avoid infinite recursion
                //.Where(c => !nodeAncestors.Any(a => a.Value == c.Stakeholder || a.Value.Id == c.StakeholderId))
                .ToList();
            foreach (var child in children)
            {
                var childNode = node.AddChild(child.RoleBearer);
                AddChildren(contacts, childNode);
            }
        }
    }
}
