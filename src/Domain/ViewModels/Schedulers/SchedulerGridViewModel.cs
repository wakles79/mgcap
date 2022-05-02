using MGCap.Domain.Utils;
using MGCap.Domain.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MGCap.Domain.ViewModels.Schedulers
{
    public class SchedulerGridViewModel : SchedulerBaseViewModel, IGridViewModel
    {
        public Guid Guid { get; set; }

       // public string ScheduleCategoryTypeName => this.ScheduleCategoryType.ToString().SplitCamelCase();

       // public int Subcategories { get; set; }

        [IgnoreDataMember]
        public int Count { get; set; }
    }
}
