using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetPlanner.Shared.Models.Request.OpenBanking
{
    public class GetProviderSetupUrlRequestModel
    {
        public IEnumerable<string> ProviderIds { get; set; }
        public IEnumerable<string> Scopes { get; set; }
    }
}
