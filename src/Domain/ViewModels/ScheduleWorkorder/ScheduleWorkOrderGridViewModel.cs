using MGCap.Domain.Enums;
using MGCap.Domain.Utils;
using MGCap.Domain.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MGCap.Domain.ViewModels.ScheduleWorkorder
{
    public class ScheduleWorkOrderGridViewModel : EntityViewModel
    {
		public int ScheduleID { get; set; }

		public int WorkOrderID { get; set; }

		public int WOType { get; set; }

		public string Description { get; set; }

		public string Location { get; set; }

		public string BuildingName { get; set; }

		public int IsExpired { get; set; }

		public int StatusId { get; set; }

		public bool IsApproved { get; set; }

		public string RecurrencePattern { get; set; }

		public string RangeRecurrenceType { get; set; }

		public int RecurrenceFrequency { get; set; }

		public int RangeRecurrenceOccurrences { get; set; }

		public DateTime RangeRecurrenceStartDate { get; set; }

		public DateTime RangeRecurrenceEndDate { get; set; }

		public DateTime NextOccurrence { get; set; }

		public int CompanyID { get; set; }

		public int DueDateStatus { get; set; }

		public bool ScheduleDateConfirmed { get; set; }

		public int BuildingId { get; set; }

		public string RecurrenceDayValue { get; set; }
		public string RecurrenceCustomValue { get; set; }

		public Guid Guid { get; set; }
	}
}
