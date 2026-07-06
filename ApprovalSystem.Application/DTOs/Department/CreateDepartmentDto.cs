using ApprovalSystem.Core.Entities;
using ApprovalSystem.Core.Enums;
using ApprovalSystem.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApprovalSystem.Application.DTOs.Department;
public class CreateDepartmentDto
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
}