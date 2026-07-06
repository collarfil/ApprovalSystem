using ApprovalSystem.Core.Entities;
using ApprovalSystem.Core.Enums;
using ApprovalSystem.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApprovalSystem.Application.DTOs.Workflows;

public class CreateWorkflowStepDto
{
    public int StepOrder { get; set; }

    public string StepName { get; set; } = string.Empty;

    public Guid RoleId { get; set; }
}