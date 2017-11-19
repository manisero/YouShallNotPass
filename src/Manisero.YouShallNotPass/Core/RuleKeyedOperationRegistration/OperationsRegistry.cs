using System;
using System.Collections.Generic;

namespace Manisero.YouShallNotPass.Core.RuleKeyedOperationRegistration
{
    public class OperationsRegistry<TOperation>
    {
        public struct GenericOperationRegistration
        {
            public Type OperationOpenGenericType { get; set; }
            public Func<Type, TOperation> Getter { get; set; }
        }

        /// <summary>rule type -> operation instance</summary>
        public IDictionary<Type, TOperation> Operations { get; set; }
            = new Dictionary<Type, TOperation>();

        /// <summary>rule generic type definition -> registration</summary>
        public IDictionary<Type, GenericOperationRegistration> GenericOperations { get; set; }
            = new Dictionary<Type, GenericOperationRegistration>();
    }
}
