using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace BudgetPlanner.Shared.Models.Response.Classifications
{
    public class ClassificationsResponse
    {
        [Description("Tag or label associated with the classification")]
        public string? Tag { get; set; }

        [Description("Unique identifier for the classification")]
        public Guid ClassificationId { get; set; }
    }
}
