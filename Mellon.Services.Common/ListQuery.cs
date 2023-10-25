namespace Mellon.Common.Services
{
    public class QueryPaging
    {
        /// <summary>
        /// An offset into the collection of the first resource to be returned.
        /// </summary>
        public int? Start { get; set; }
        /// <summary>
        /// The maximum number of resources to include in a single response.
        /// </summary>
        public int? PageSize { get; set; }
    }

    public class ListPaging
    {
        private const int DEFAULT_START = 0;
        private const int DEFAULT_LENGTH = 10;
        private const int MIN_LENGTH = 2;
        private const int MAX_LENGTH = 1000;

        public ListPaging() : this(DEFAULT_START, DEFAULT_LENGTH)
        {
        }
        public ListPaging(QueryPaging source)
            : this(source?.Start ?? DEFAULT_START, source?.PageSize ?? DEFAULT_LENGTH)
        {
        }
        public ListPaging(int start, int length)
        {
            Start = Guards.NumberNonNegative(start, nameof(start));
            Length = Guards.NumberInRange(length, MIN_LENGTH, MAX_LENGTH, nameof(length));
        }

        /// <summary>
        /// An offset into the collection of the first resource to be returned.
        /// </summary>
        public int Start { get; set; }
        /// <summary>
        /// The maximum number of resources to include in a single response.
        /// </summary>
        public int Length { get; set; }
    }

    public class QueryOrder
    {
        /// <summary>
        /// Ordering parameters. Every value MAY include the suffix "asc" for ascending or "desc" for descending, separated from the property name by one space. Default is asc.
        /// </summary>
        public string[] OrderBy { get; set; }
    }

    public class ListOrder
    {
        public ListOrder() : this((IEnumerable<Property>)null)
        {
        }
        public ListOrder(IEnumerable<Property> properties)
        {
            Properties = (properties ?? new List<Property>())
                .Select(p => new Property(p.Name, p.Ascending)).ToList();
        }
        public ListOrder(QueryOrder order)
        {
            Properties = order?.OrderBy?
                .Select(o => new Property(o)).ToList()
                ?? new List<Property>();
        }

        /// <summary>
        /// A list of expressions that specify the order of the returned resources.
        /// </summary>
        public IList<Property> Properties { get; set; }

        public class Property
        {
            public Property(string name, bool ascending = true)
            {
                Name = name;
                Ascending = ascending;
            }
            public Property(string value)
            {
                if (value == null)
                    throw new BadRequestException("Invalid order clause.");
                var parts = value.Split(' ');
                Name = parts[0];
                Ascending = parts.Length <= 1 || ResolveAscending(parts[1]);
            }

            private static bool ResolveAscending(string v)
            {
                if (v.Equals("asc", StringComparison.InvariantCultureIgnoreCase))
                    return true;
                else if (v.Equals("desc", StringComparison.InvariantCultureIgnoreCase))
                    return false;
                else throw new BadRequestException("Invalid order clause.");
            }

            public string Name { get; set; }
            public bool Ascending { get; }
        }
    }

    public class QuerySearch
    {
        /// <summary>
        /// A value used to select resources to be returned.
        /// </summary>
        public string Search { get; set; }
    }

    public class ListSearch
    {
        public ListSearch() { }
        public ListSearch(QuerySearch search)
        {
            Value = search.Search;
        }

        /// <summary>
        /// A value used to select resources to be returned.
        /// </summary>
        public string Value { get; set; }
        public bool HasValue => !string.IsNullOrWhiteSpace(Value);
    }

    public class QueryFilter
    {
        /// <summary>
        /// An expression on the resource type that selects the resources to be returned.
        /// </summary>
        public string Filter { get; set; }
    }

    public class QueryTop
    {
        /// <summary>
        /// The maximum number of resources to return from the collection.
        /// </summary>
        public int Top { get; set; }
    }

    public class QuerySelect
    {
        /// <summary>
        /// A list of field names to be returned for each resource.
        /// </summary>
        public string[] Select { get; set; }
    }

    public class QueryExpand
    {
        /// <summary>
        /// A list of the related resources to be included in line with each resource.
        /// </summary>
        public string[] Expand { get; set; }
    }
}

