using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mellon.Common.Services
{
    public enum ErrorCodes
    {
        GENERIC_UNKNOWN = 1001,
        GENERIC_BAD_REQUEST = 1002,
        GENERIC_SERVICE_UNAVAILABLE = 1003,
        GENERIC_NOT_FOUND = 1004,
        GENERIC_GUARD = 1005,
        GENERIC_INVALID_NUMERIC_RESOURCE_ID = 1006,
        GENERIC_FORBIDDEN = 1007,
        APPROVAL_WRONG_STATUS=1008,
        ERP_ERROR= 1009
    }
}
