using System.Collections.Generic;

namespace DynamicFilter
{
    public class PropertyIdInfo
    {
        public PropertyIdInfo(string propertyId, List<string> supportedOperations)
        {
            PropertyId = propertyId;
            SupportedOperations = supportedOperations;
        }

        public string PropertyId { get; set; }

        public List<string> SupportedOperations { get; set; }
    }
}
