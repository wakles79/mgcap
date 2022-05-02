﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MGCap.Presentation.ViewModels.DataViewModels.Customer
{
    public class CustomerUpdateViewModel : CustomerBaseViewModel
    {
        public Guid Guid { get; set; }

        public List<int> GroupIds { get; set; }
    }
}
