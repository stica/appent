using System;

namespace Start.Common.Interfaces
{
    public interface IAuditInfo
    {
        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
