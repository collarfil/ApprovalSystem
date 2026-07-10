using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApprovalSystem.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCurrentApprovalStepId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CurrentApprovalStepId",
                table: "Requests",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowRules_TargetRoleId",
                table: "WorkflowRules",
                column: "TargetRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowRules_WorkflowDefinitionId",
                table: "WorkflowRules",
                column: "WorkflowDefinitionId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowRules_Roles_TargetRoleId",
                table: "WorkflowRules",
                column: "TargetRoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowRules_WorkflowDefinitions_WorkflowDefinitionId",
                table: "WorkflowRules",
                column: "WorkflowDefinitionId",
                principalTable: "WorkflowDefinitions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowRules_Roles_TargetRoleId",
                table: "WorkflowRules");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowRules_WorkflowDefinitions_WorkflowDefinitionId",
                table: "WorkflowRules");

            migrationBuilder.DropIndex(
                name: "IX_WorkflowRules_TargetRoleId",
                table: "WorkflowRules");

            migrationBuilder.DropIndex(
                name: "IX_WorkflowRules_WorkflowDefinitionId",
                table: "WorkflowRules");

            migrationBuilder.DropColumn(
                name: "CurrentApprovalStepId",
                table: "Requests");
        }
    }
}
