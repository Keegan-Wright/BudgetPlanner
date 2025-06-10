using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace BudgetPlanner.Shared.Models.Request.OpenBanking
{
    public class GetProviderSetupUrlRequestModel
    {
        [Description("Collection of banking provider identifiers to set up")]
        public IEnumerable<string> ProviderIds { get; set; }

        [Description("Collection of access scopes required for the banking integration")]
        public IEnumerable<string> Scopes { get; set; }
    }
}
