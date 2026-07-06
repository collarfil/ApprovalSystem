using System;
using System.Collections.Generic;

namespace ApprovalSystem.Core.Entities
{
    public class Role
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public ICollection<UserRole> UserRoles { get; set; }
            = new List<UserRole>();
    }
}