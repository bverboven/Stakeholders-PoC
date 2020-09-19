namespace Regira.Stakeholders.Library.Data
{
    public static class StoredProcedures
    {
        private const string ContactsOffspring = @"WITH RECURSIVE o (role_giver_id, role_bearer_id, role_id, level) as (
  SELECT        role_giver_id, role_bearer_id, role_id, 0 level
  FROM          stakeholder_contacts
  WHERE         ids IS NULL OR ids = '' OR Find_In_Set(role_giver_id, ids)
  UNION ALL
  SELECT        sc.role_giver_id, sc.role_bearer_id, sc.role_id, level + 1
  FROM          stakeholder_contacts sc
  INNER JOIN    o on o.role_bearer_id = sc.role_giver_id
  WHERE         ids <> '' AND (max_level IS NULL OR level < max_level)
)
SELECT * FROM o";
        private const string ContactsAncestors = @"WITH RECURSIVE a (role_giver_id, role_bearer_id, role_id, level) as (
  SELECT        role_giver_id, role_bearer_id, role_id, 0 level
  FROM          stakeholder_contacts
  WHERE         ids IS NULL OR ids = '' OR Find_In_Set(role_bearer_id, ids)
  UNION ALL
  SELECT        sc.role_giver_id, sc.role_bearer_id, sc.role_id, level + 1
  FROM          stakeholder_contacts sc
  INNER JOIN    a on a.role_giver_id = sc.role_bearer_id
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
SELECT role_giver_id, role_bearer_id, role_id, level FROM 
(
{ContactsOffspring}
) q1
UNION
SELECT role_giver_id, role_bearer_id, role_id, level*-1 FROM 
(
{ContactsAncestors}
) q2
) q
ORDER BY level;

END";

        public static string[] CREATE_ALL => new[] { CREATE_ContactsOffspring, CREATE_ContactsAncestors, CREATE_ContactsFamily };

        public const string DROP_ContactsOffspring = @"DROP PROCEDURE `contacts_offspring`;";
        public const string DROP_ContactsAncestors = @"DROP PROCEDURE `contacts_ancestors`;";
        public const string DROP_ContactsFamily = @"DROP PROCEDURE `contacts_family`;";
        public static string[] DROP_ALL => new[] { DROP_ContactsOffspring, DROP_ContactsAncestors, DROP_ContactsFamily };
    }
}
