using DynamicFilter.Common;

namespace DynamicQuery.QueryBuilder.Models
{
    public class Query
    {
        public Query()
        {

        }

        public Query(string propertyId, string operation, Connector connector = DynamicFilter.Common.Connector.None) :
          this(propertyId, operation, null, null, connector)
        {
        }



        public Query(string propertyId, string operation, string value, Connector connector = DynamicFilter.Common.Connector.None) :
        this(propertyId, operation, value, null, connector)
        {

        }

        public Query(string propertyId, string operation, string value, string value2, Connector connector = DynamicFilter.Common.Connector.None)
        {
            PropertyId = propertyId;
            Value = value;
            Value2 = value2;
            Operation = operation;

            switch (connector)
            {
                case DynamicFilter.Common.Connector.And:
                    Connector = "AND";
                    break;

                case DynamicFilter.Common.Connector.Or:
                    Connector = "OR";
                    break;

            }
        }

        public string Connector { get; set; }
        public string PropertyId { get; set; }
        public string Operation { get; set; }
        public string Value { get; set; }
        public string Value2 { get; set; }
    }
}
