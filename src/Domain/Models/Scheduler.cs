using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MGCap.Domain.Models
{
   public class Scheduler  : AuditableCompanyEntity
    {
        /// <summary>
        ///     Gets or sets the current cleaning report Number
        /// </summary>
        //public int Number { get; set; }
        [Key]
        public int scheduleID { get; set; }

        public string scheduleType { get; set; }

        public DateTime scheduleStartTime { get; set; }

        public DateTime scheduleEndTime { get; set; }

        public int scheduleDuration { get; set; }

        public string recurrencePattern { get; set; }

        public string recurrenceType { get; set; }

        public int recurrenceFrequency { get; set; }

        public string recurrenceDayValue { get; set; }

        public string recurrenceCustomValue { get; set; }

        public Boolean applyDueDays { get; set; }
        public int dueDays { get; set; }

        public string rangeRecurrenceType { get; set; }

        public DateTime rangeRecurrenceStartDate { get; set; }

        public DateTime? rangeRecurrenceEndDate { get; set; }

        public int rangeRecurrenceOccurrences { get; set; }

        public bool isApproved { get; set; }

        //public Scheduler()
        //{
        //    this.SchedulerItems = new HashSet<Scheduler>();
        //}

    }
}
