using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mellon.Services.Infrastracture.Base
{
    [Serializable]
    public class RepositoryException : Exception
    {
        public RepositoryException(int errorNumber) : this(errorNumber, null) { }
        public RepositoryException(int errorNumber, Exception inner)
            : base("A repository exception occurred.", inner)
        {
            ErrorNumber = errorNumber;
            Error = Enum.IsDefined(typeof(RepositoryErrors), errorNumber)
                ? (RepositoryErrors)errorNumber
                : RepositoryErrors.GENERIC;
        }

        public int ErrorNumber { get; }
        public RepositoryErrors Error { get; }
    }
}
