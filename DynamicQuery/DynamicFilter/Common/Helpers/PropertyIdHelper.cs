using DynamicFilter;
using DynamicFilter.Common.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ExpressionBuilderCore.Helpers
{
    public class PropertyIdHelper : IPropertyIdHelper
    {
        private readonly Type[] ADDITIONAL_PRIMITIVE_TYPES = new Type[] { typeof(string), typeof(DateTime), typeof(decimal) };
        private readonly IOperationHelper _operationHelper;

        public PropertyIdHelper()
        {
            _operationHelper = new OperationHelper();
        }

        IEnumerable<string> GetPropertyIds<T>() where T : class => GetPropertyIds(typeof(T));

        IEnumerable<PropertyIdInfo> GetPropertyIdInfoss<T>() where T : class => GetPropertyIdInfos(typeof(T));

        public string GetInnerMostPropertyName(string propertyId)
        {
            string propertyName = propertyId;

            if (propertyId.Contains("[") && propertyId.Contains("]"))
            {
                int indexOfOpenBracket = propertyName.LastIndexOf("[");
                int indexOfCloseBracket = propertyName.IndexOf("]");
                propertyName = propertyName.Substring(indexOfOpenBracket + 1, indexOfCloseBracket - indexOfOpenBracket - 1);
            }

            if (propertyName.Contains("."))
            {
                propertyName.Substring(propertyName.LastIndexOf(".") + 1);
            }

            return propertyName;
        }



        private IEnumerable<PropertyIdInfo> GetPropertyIdInfos(Type type, string prefix = "", string suffix = "")
        {
            List<PropertyIdInfo> result = new List<PropertyIdInfo>();
            PropertyInfo[] propertyInfos = type.GetProperties();

            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                Type propertyType = propertyInfo.PropertyType;
                if (propertyType != typeof(string) && propertyType.GetInterfaces().Contains(typeof(IEnumerable)))
                {
                    string propertyId = string.Format("{0}{1}{2}", prefix, propertyInfo.Name, suffix);
                    List<string> supportedOperations = _operationHelper.SupportedOperations(propertyType)
                                                                        .Select(x => x.Name)
                                                                        .ToList();
                    PropertyIdInfo propertyIdInfo = new PropertyIdInfo(propertyId, supportedOperations);
                    result.Add(propertyIdInfo);
                }
                else
                {
                    result.AddRange(GetPropertyIdInfos(propertyType, $"{prefix}{propertyInfo.Name}.", $"{suffix}"));
                }
            }
            return result;
        }

        private IEnumerable<string> GetPropertyIds(Type type, string prefix = "", string suffix = "")
        {
            List<string> result = new List<string>();
            PropertyInfo[] propertyInfos = type.GetProperties();

            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                Type propertyType = propertyInfo.PropertyType;
                if (propertyType != typeof(string) && propertyType.GetInterfaces().Contains(typeof(IEnumerable)))
                {
                    Type genericArgument = propertyType.GetGenericArguments().Single();
                    result.AddRange(GetPropertyIds(genericArgument, $"{prefix}{propertyInfo.Name}[", $"]{suffix}"));
                }
                else
                {
                    if (propertyType.IsPrimitive || ADDITIONAL_PRIMITIVE_TYPES.Contains(propertyType) || Nullable.GetUnderlyingType(propertyType) != null)
                    {
                        result.Add(string.Format("{0}{1}{2}", prefix, propertyInfo.Name, suffix));
                    }
                    else
                    {
                        result.AddRange(GetPropertyIds(propertyType, $"{prefix}{propertyInfo.Name}.", $"{suffix}"));
                    }
                }

            }

            return result;
        }
    }

}
