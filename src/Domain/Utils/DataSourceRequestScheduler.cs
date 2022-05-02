using System;
using System.Collections.Generic;
using System.Text;

namespace MGCap.Domain.Utils
{
    public class DataSourceRequestScheduler: DataSourceRequest
    {
        public string? RangeRecurrenceStartDate { get; set; }
    }
}
