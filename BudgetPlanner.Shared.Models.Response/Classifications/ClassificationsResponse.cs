﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetPlanner.Shared.Models.Response.Classifications
{
    public class ClassificationsResponse
    {
        public string? Tag { get; set; }
        public Guid ClassificationId { get; set; }
    }
}
