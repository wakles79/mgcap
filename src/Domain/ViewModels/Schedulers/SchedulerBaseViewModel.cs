using MGCap.Domain.Enums;
using MGCap.Domain.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MGCap.Domain.ViewModels.Schedulers
{
    public class SchedulerBaseViewModel : EntityViewModel
    {
        public int scheduleID { get; set; }


        public string scheduleType { get; set; }

        public int scheduleDuration { get; set; }

        public string recurrencePattern { get; set; }

        public string recurrenceType { get; set; }

        public int recurrenceFrequency { get; set; }

        public string recurrenceDayValue { get; set; }

        public string recurrenceCustomValue { get; set; }

        public int dueDays { get; set; }

        public string rangeRecurrenceType { get; set; }

        public int rangeRecurrenceOccurrences { get; set; }

        public string rangeRecurrenceStartDate { get; set; }

        public string rangeRecurrenceEndDate { get; set; }

        public bool isApproved { get; set; }

        public int createdBy { get; set; }

        public DateTime createdDate { get; set; }

        public int updatedBy { get; set; }

        public DateTime updatedDate { get; set; }
        public int Submitted { get; set; }
    }
}
