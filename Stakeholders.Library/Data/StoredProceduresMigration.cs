using Microsoft.EntityFrameworkCore.Migrations;

namespace Regira.Stakeholders.Library.Data
{
    public class StoredProceduresMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(StoredProcedures.CREATE_ContactsOffspring);
            migrationBuilder.Sql(StoredProcedures.CREATE_ContactsAncestors);
            migrationBuilder.Sql(StoredProcedures.CREATE_ContactsFamily);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(StoredProcedures.DROP_ContactsOffspring);
            migrationBuilder.Sql(StoredProcedures.DROP_ContactsAncestors);
            migrationBuilder.Sql(StoredProcedures.DROP_ContactsFamily);
        }
    }
}
