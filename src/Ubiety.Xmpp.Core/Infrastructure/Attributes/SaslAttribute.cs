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
using Ubiety.Xmpp.Core.Common;

namespace Ubiety.Xmpp.Core.Infrastructure.Attributes;

/// <summary>
///     SASL authentication attribute.
/// </summary>
[AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
public sealed class SaslAttribute : Attribute
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="SaslAttribute" /> class.
    /// </summary>
    /// <param name="mechanismName">Name of the SASL mechanism.</param>
    /// <param name="processorType">Type of SASL processor.</param>
    /// <param name="weight">Weight of the mechanism.</param>
    /// <param name="channelBinding">Whether to use channel binding.</param>
    /// <param name="type">Type of the mechanism.</param>
    public SaslAttribute(string mechanismName, Type processorType, int weight, bool channelBinding, MechanismTypes type)
    {
        MechanismName = mechanismName;
        ProcessorType = processorType;
        Weight = weight;
        ChannelBinding = channelBinding;
        Type = type;
    }

    /// <summary>
    ///     Gets the SASL mechanism name.
    /// </summary>
    public string MechanismName { get; }

    /// <summary>
    ///     Gets the SASL processor.
    /// </summary>
    public Type ProcessorType { get; }

    /// <summary>
    ///     Gets the mechanism weight.
    /// </summary>
    public int Weight { get; }

    /// <summary>
    ///     Gets a value indicating whether to use channel binding.
    /// </summary>
    public bool ChannelBinding { get; }

    /// <summary>
    ///     Gets the mechanism type.
    /// </summary>
    public MechanismTypes Type { get; }
}
