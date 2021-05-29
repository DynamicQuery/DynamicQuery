using DynamicFilter.Common;
using DynamicFilter.Common.Exceptions;
using DynamicFilter.Common.Interfaces;
using DynamicFilter.Operations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExpressionBuilderCore.Helpers
{
    /// <summary>
    /// Useful methods regarding <seealso cref="IOperation"></seealso>.
    /// </summary>
    public class OperationHelper : IOperationHelper
    {
        private static HashSet<IOperation> _operations;

        private readonly Dictionary<TypeGroup, HashSet<Type>> _typeGroups;

        static OperationHelper()
        {
            LoadDefaultOperations();
        }

        /// <summary>
        /// Loads the default operations overwriting any previous changes to the <see cref="Operations"></see> list.
        /// </summary>
        public static void LoadDefaultOperations()
        {
            List<IOperation> listOfOperations = new List<IOperation>()
            {
                new Between(), new Contains(), new DoesNotContain(),
                new EndsWith(), new EqualTo(), new GreaterThan(),
                new GreaterThanOrEqualTo(), new In(), new IsEmpty(),
                new IsNotEmpty(), new IsNotNull(), new IsNotNullNorWhiteSpace(),
                new IsNull(), new IsNullOrWhiteSpace(), new LessThan(),
                new LessThanOrEqualTo(), new NotEqualTo(), new NotIn(),
                new StartsWith()
            };
            _operations = new HashSet<IOperation>(listOfOperations, new OperationEqualityComparer());
        }

        /// <inheritdoc/>
        public IEnumerable<IOperation> Operations { get { return _operations.ToArray(); } }

        public OperationHelper()
        {
            _typeGroups = new Dictionary<TypeGroup, HashSet<Type>>
            {
                { TypeGroup.Text, new HashSet<Type> { typeof(string), typeof(char) } },
                { TypeGroup.Number, new HashSet<Type> { typeof(int), typeof(uint), typeof(byte), typeof(sbyte), typeof(short), typeof(ushort), typeof(long), typeof(ulong), typeof(Single), typeof(double), typeof(decimal) } },
                { TypeGroup.Boolean, new HashSet<Type> { typeof(bool) } },
                { TypeGroup.Date, new HashSet<Type> { typeof(DateTime) } },
                { TypeGroup.Nullable, new HashSet<Type> { typeof(Nullable<>), typeof(string) } }
            };
        }


        /// <inheritdoc/>
        public HashSet<IOperation> SupportedOperations(Type type)
        {
            return GetSupportedOperations(type);
        }


        private HashSet<IOperation> GetSupportedOperations(Type type)
        {
            var underlyingNullableType = Nullable.GetUnderlyingType(type);
            var typeName = (underlyingNullableType ?? type).Name;

            var supportedOperations = new List<IOperation>();
            if (type.IsArray)
            {
                typeName = type.GetElementType().Name;
                supportedOperations.AddRange(Operations.Where(o => o.SupportsLists && o.Active));
            }

            var typeGroup = TypeGroup.Default;
            if (_typeGroups.Any(i => i.Value.Any(v => v.Name == typeName)))
            {
                typeGroup = _typeGroups.FirstOrDefault(i => i.Value.Any(v => v.Name == typeName)).Key;
            }

            supportedOperations.AddRange(Operations.Where(o => o.TypeGroup.HasFlag(typeGroup) && !o.SupportsLists && o.Active));

            if (underlyingNullableType != null)
            {
                supportedOperations.AddRange(Operations.Where(o => o.TypeGroup.HasFlag(TypeGroup.Nullable) && !o.SupportsLists && o.Active));
            }

            return new HashSet<IOperation>(supportedOperations);
        }

        /// <inheritdoc/>
        public IOperation GetOperationByName(string operationName)
        {
            var operation = Operations.SingleOrDefault(o => o.Name == operationName && o.Active);

            if (operation == null)
            {
                throw new OperationNotFoundException(operationName);
            }

            return operation;
        }

        /// <summary>
        /// Loads a list of custom operations into the <see cref="Operations"></see> list.
        /// </summary>
        /// <param name="operations">List of operations to load.</param>
        public void LoadOperations(List<IOperation> operations)
        {
            LoadOperations(operations, false);
        }

        /// <inheritdoc/>
        public void LoadOperations(List<IOperation> operations, bool overloadExisting)
        {
            foreach (var operation in operations)
            {
                DeactivateOperation(operation.Name, overloadExisting);
                _operations.Add(operation);
            }
        }

        private void DeactivateOperation(string operationName, bool overloadExisting)
        {
            if (!overloadExisting)
            {
                return;
            }

            var op = _operations.FirstOrDefault(o => string.Compare(o.Name, operationName, StringComparison.InvariantCultureIgnoreCase) == 0);
            if (op != null)
            {
                op.Active = false;
            }
        }
    }
}
