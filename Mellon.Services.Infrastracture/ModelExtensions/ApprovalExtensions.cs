using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mellon.Services.Infrastracture.ModelExtensions
{
    internal class ApprovalExtensions
    {
    }

    public enum ApprovalStatusEnum
    {
        Requested,
        Open,
        Created,
        Canceled,
        Rejected,
        Approved,
    }
}
