namespace Regira.Stakeholders.Library.Data
{
    public static class StoredProcedures
    {
        private const string ContactsOffspring = @"WITH RECURSIVE o (stakeholder_id, contact_id, role_id, level) as (
  SELECT        stakeholder_id, contact_id, role_id, 0 level
  FROM          stakeholder_contacts
  WHERE         ids IS NULL OR ids = '' OR Find_In_Set(stakeholder_id, ids)
  UNION ALL
  SELECT        sc.stakeholder_id, sc.contact_id, sc.role_id, level + 1
  FROM          stakeholder_contacts sc
  INNER JOIN    o on o.contact_id = sc.stakeholder_id
  WHERE         ids <> '' AND (max_level IS NULL OR level < max_level)
)
SELECT * FROM o";

        private const string ContactsAncestors = @"WITH RECURSIVE a (stakeholder_id, contact_id, role_id, level) as (
  SELECT        stakeholder_id, contact_id, role_id, 0 level
  FROM          stakeholder_contacts
  WHERE         ids IS NULL OR ids = '' OR Find_In_Set(contact_id, ids)
  UNION ALL
  SELECT        sc.stakeholder_id, sc.contact_id, sc.role_id, level + 1
  FROM          stakeholder_contacts sc
  INNER JOIN    a on a.stakeholder_id = sc.contact_id
  WHERE         ids <> '' AND (max_level IS NULL OR level < max_level)
)
SELECT * FROM a";

        public static readonly string CREATE_ContactsOffspring = $@"CREATE PROCEDURE `contacts_offspring`(IN ids varchar(1024), IN max_level int)
BEGIN

{ContactsOffspring};

END";
        public static readonly string CREATE_ContactsAncestors = $@"CREATE PROCEDURE `contacts_ancestors`(IN ids varchar(1024), IN max_level int)
BEGIN

{ContactsAncestors};

END";
        public static readonly string CREATE_ContactsFamily = $@"CREATE PROCEDURE `contacts_family` (IN ids varchar(1024), IN max_level int)
BEGIN

SELECT * FROM (
SELECT stakeholder_id, contact_id, role_id, level FROM 
(
{ContactsOffspring}
) q1
UNION
SELECT stakeholder_id, contact_id, role_id, level*-1 FROM 
(
{ContactsAncestors}
) q2
) q
ORDER BY level;

END";
        public const string DROP_ContactsOffspring = @"DROP PROCEDURE `contacts_offspring`;";
        public const string DROP_ContactsAncestors = @"DROP PROCEDURE `contacts_ancestors`;";
        public const string DROP_ContactsFamily = @"DROP PROCEDURE `contacts_family`;";
    }
}
