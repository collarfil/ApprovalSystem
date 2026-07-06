using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApprovalSystem.Core.Entities;
using ApprovalSystem.Core.Enums;
using ApprovalSystem.Core.Interfaces;

namespace ApprovalSystem.Application.DTOs.Approvals;

public class ApproveRequestDto
{
    public Guid RequestId { get; set; }
    public Guid ApproverId {get; set;}
    
    public string Comment { get; set; } = string.Empty;
}