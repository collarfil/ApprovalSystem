using System;
using System.Collections.Generic;
using System.Linq;

namespace ApprovalSystem.Core.Enums
{
public enum RequestStatus
{
    Draft = 1,
    Pending = 2,
    InProgress = 3,
    Approved = 4,
    Rejected = 5,
    Escalated = 6,
    Cancelled = 7
}
}
