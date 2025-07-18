using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace BudgetPlanner.Shared.Models.Request.Classifications
{
    public class AddClassificationsRequest
    {
        [Description("Tag or label for the new classification")]
        public required string Tag { get; set; }
    }
}
