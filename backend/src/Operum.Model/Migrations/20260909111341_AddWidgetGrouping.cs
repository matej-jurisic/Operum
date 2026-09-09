using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Operum.Model.Migrations
{
    /// <inheritdoc />
    public partial class AddWidgetGrouping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Grouping",
                table: "Widgets",
                type: "text",
                nullable: true);

            // A Line/Bar chart's calculation used to be a single fused Code ("Daily",
            // "Count Bar Chart", ...). Split each retired code into the (Grouping, Code) pair
            // that now replaces it -- see LegacyLineBarCodes, which this mirrors.
            migrationBuilder.Sql("""
                UPDATE "Widgets" SET "Grouping" = CASE "Code"
                        WHEN 'Line Chart'        THEN 'None'
                        WHEN 'Aggregated Sum'    THEN 'Exact'
                        WHEN 'Cumulative Sum'    THEN 'Exact'
                        WHEN 'Daily'             THEN 'Daily'
                        WHEN 'Weekly'            THEN 'Weekly'
                        WHEN 'Monthly'           THEN 'Monthly'
                        WHEN 'Yearly'            THEN 'Yearly'
                        WHEN 'Count Bar Chart'   THEN 'Exact'
                        WHEN 'Sum Bar Chart'     THEN 'Exact'
                        WHEN 'Average Bar Chart' THEN 'Exact'
                        WHEN 'Daily Bar Chart'   THEN 'Daily'
                        WHEN 'Weekly Bar Chart'  THEN 'Weekly'
                        WHEN 'Monthly Bar Chart' THEN 'Monthly'
                        WHEN 'Yearly Bar Chart'  THEN 'Yearly'
                    END,
                    "Code" = CASE "Code"
                        WHEN 'Line Chart'        THEN 'Raw Values'
                        WHEN 'Aggregated Sum'    THEN 'Sum'
                        WHEN 'Cumulative Sum'    THEN 'Cumulative Sum'
                        WHEN 'Daily'             THEN 'Sum'
                        WHEN 'Weekly'            THEN 'Sum'
                        WHEN 'Monthly'           THEN 'Sum'
                        WHEN 'Yearly'            THEN 'Sum'
                        WHEN 'Count Bar Chart'   THEN 'Count'
                        WHEN 'Sum Bar Chart'     THEN 'Sum'
                        WHEN 'Average Bar Chart' THEN 'Average'
                        WHEN 'Daily Bar Chart'   THEN 'Sum'
                        WHEN 'Weekly Bar Chart'  THEN 'Sum'
                        WHEN 'Monthly Bar Chart' THEN 'Sum'
                        WHEN 'Yearly Bar Chart'  THEN 'Sum'
                    END
                WHERE "ResultType" IN ('Line Chart', 'Bar Chart');
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Fold (Grouping, Code) back into the single fused code. Lossy in the same way
            // the split was a widening: aggregations added since (Average/Count/Min/Max on a
            // date bucket, Min/Max per category, cumulative bar) have no legacy code, so
            // they collapse to the closest one that does.
            migrationBuilder.Sql("""
                UPDATE "Widgets" SET "Code" = CASE
                        WHEN "Grouping" = 'None'                                    THEN 'Line Chart'
                        WHEN "ResultType" = 'Line Chart' AND "Grouping" = 'Exact'
                             AND "Code" = 'Cumulative Sum'                          THEN 'Cumulative Sum'
                        WHEN "ResultType" = 'Line Chart' AND "Grouping" = 'Exact'   THEN 'Aggregated Sum'
                        WHEN "ResultType" = 'Line Chart'                            THEN "Grouping"
                        WHEN "ResultType" = 'Bar Chart'  AND "Grouping" = 'Exact'
                             AND "Code" = 'Count'                                   THEN 'Count Bar Chart'
                        WHEN "ResultType" = 'Bar Chart'  AND "Grouping" = 'Exact'
                             AND "Code" = 'Average'                                 THEN 'Average Bar Chart'
                        WHEN "ResultType" = 'Bar Chart'  AND "Grouping" = 'Exact'   THEN 'Sum Bar Chart'
                        WHEN "ResultType" = 'Bar Chart'                             THEN "Grouping" || ' Bar Chart'
                        ELSE "Code"
                    END
                WHERE "ResultType" IN ('Line Chart', 'Bar Chart');
                """);

            migrationBuilder.DropColumn(
                name: "Grouping",
                table: "Widgets");
        }
    }
}
