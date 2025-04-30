using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable
namespace LibraryManagement.Persistence.Migrations
{
    public partial class ChangeStatusEnumToString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1️⃣  change column type  (int -> nvarchar)
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "BookBorrowingRequests",
                type: "nvarchar(24)",          // enough for enum names
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            // 2️⃣  translate existing numeric values to enum names
            migrationBuilder.Sql("""
                UPDATE BookBorrowingRequests
                SET Status = CASE Status
                    WHEN '0' THEN 'Approved'  -- adjust to your enum
                    WHEN '1' THEN 'Waiting'
                    WHEN '2' THEN 'Rejected'
                    ELSE Status END;
            """);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // reverse the text->number map so a rollback works
            migrationBuilder.Sql("""
                UPDATE BookBorrowingRequests
                SET Status = CASE Status
                    WHEN 'Approved' THEN '0'
                    WHEN 'Waiting'  THEN '1'
                    WHEN 'Rejected' THEN '2'
                    ELSE Status END;
            """);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "BookBorrowingRequests",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(24)");
        }
    }
}
