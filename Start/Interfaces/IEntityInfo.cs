using System;

namespace Start.Common.Interfaces
{
    public interface IEntityInfo
    {
        public int CreatedBy { get; set; }

        public DateTime DateCreated { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
