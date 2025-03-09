// Copyright 2020 Dieter Lunn
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.

using System;
using System.Collections.Generic;
using System.Reflection;
using Ubiety.Xmpp.Core.Infrastructure.Attributes;
using Ubiety.Xmpp.Core.Infrastructure.Extensions;
using Ubiety.Xmpp.Core.Logging;

namespace Ubiety.Xmpp.Core.Registries
{
    /// <summary>
    /// Represents a registry for SASL mechanisms and their associated processors.
    /// </summary>
    public class SaslRegistry
    {
        private static readonly ILog Logger = Log.Get<SaslRegistry>();
        private readonly Dictionary<string, (Type processor, int weight)> _mechanisms = new ();

        /// <summary>
        /// Adds all SASL mechanisms defined in the specified assembly to the SASL registry.
        /// </summary>
        /// <param name="assembly">The assembly containing SASL mechanisms to register.</param>
        public void AddAssembly(Assembly assembly)
        {
            Logger.Log(LogLevel.Information, $"Adding assembly {assembly.FullName} to SASL registry.");

            var attributes = assembly.GetAttributes<SaslAttribute>();
            foreach (var attribute in attributes)
            {
                _mechanisms.Add(attribute.MechanismName, (attribute.ProcessorType, attribute.Weight));
            }
        }
    }
}
