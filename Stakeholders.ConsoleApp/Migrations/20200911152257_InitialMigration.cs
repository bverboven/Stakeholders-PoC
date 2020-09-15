using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Regira.Stakeholders.ConsoleApp.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "contact_roles",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_contact_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "stakeholders",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    stakeholder_type = table.Column<int>(nullable: false),
                    phone = table.Column<string>(nullable: true),
                    email = table.Column<string>(nullable: true),
                    kbo = table.Column<string>(nullable: true),
                    address_street = table.Column<string>(nullable: true),
                    address_number = table.Column<string>(nullable: true),
                    address_box = table.Column<string>(nullable: true),
                    address_post_box = table.Column<string>(nullable: true),
                    address_postal_code = table.Column<string>(nullable: true),
                    address_city = table.Column<string>(nullable: true),
                    address_country_code = table.Column<string>(nullable: true),
                    note = table.Column<string>(nullable: true),
                    name = table.Column<string>(nullable: true),
                    salutation = table.Column<string>(nullable: true),
                    given_name = table.Column<string>(nullable: true),
                    family_name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_stakeholders", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "AccountNumber",
                columns: table => new
                {
                    stakeholder_id = table.Column<int>(nullable: false),
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    account_number = table.Column<string>(nullable: true),
                    account_type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_account_number", x => new { x.stakeholder_id, x.id });
                    table.ForeignKey(
                        name: "fk_account_number_stakeholders_stakeholder_id",
                        column: x => x.stakeholder_id,
                        principalTable: "stakeholders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "stakeholder_contacts",
                columns: table => new
                {
                    stakeholder_id = table.Column<int>(nullable: false),
                    contact_id = table.Column<int>(nullable: false),
                    role_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_stakeholder_contacts", x => new { x.stakeholder_id, x.contact_id, x.role_id });
                    table.ForeignKey(
                        name: "fk_stakeholder_contacts_stakeholders_contact_id",
                        column: x => x.contact_id,
                        principalTable: "stakeholders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_stakeholder_contacts_contact_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "contact_roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_stakeholder_contacts_stakeholders_stakeholder_id",
                        column: x => x.stakeholder_id,
                        principalTable: "stakeholders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_stakeholder_contacts_contact_id",
                table: "stakeholder_contacts",
                column: "contact_id");

            migrationBuilder.CreateIndex(
                name: "ix_stakeholder_contacts_role_id",
                table: "stakeholder_contacts",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "ix_stakeholders_kbo",
                table: "stakeholders",
                column: "kbo");

            migrationBuilder.CreateIndex(
                name: "ix_stakeholders_kbo1",
                table: "stakeholders",
                column: "kbo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountNumber");

            migrationBuilder.DropTable(
                name: "stakeholder_contacts");

            migrationBuilder.DropTable(
                name: "stakeholders");

            migrationBuilder.DropTable(
                name: "contact_roles");
        }
    }
}
