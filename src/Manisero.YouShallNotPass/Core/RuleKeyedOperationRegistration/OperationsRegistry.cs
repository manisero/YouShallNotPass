using System;
using System.Collections.Generic;

namespace Manisero.YouShallNotPass.Core.RuleKeyedOperationRegistration
{
    public class OperationsRegistry<TOperation>
    {
        public struct GenericOperationRegistration
        {
            public Type OperationOpenGenericType { get; set; }
            public Func<Type, TOperation> Factory { get; set; }
        }

        /// <summary>rule type -> operation instance</summary>
        public IDictionary<Type, TOperation> OperationInstances { get; set; }

        /// <summary>rule type -> operation factory</summary>
        public IDictionary<Type, Func<TOperation>> OperationFactories { get; set; }

        /// <summary>rule generic type definition -> registration</summary>
        public IDictionary<Type, GenericOperationRegistration> GenericOperationFactories { get; set; }
    }
}
