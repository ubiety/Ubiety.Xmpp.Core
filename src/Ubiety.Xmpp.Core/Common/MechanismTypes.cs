// Copyright 2018 Dieter Lunn
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

namespace Ubiety.Xmpp.Core.Common
{
    /// <summary>
    /// Represents the available authentication mechanisms in the XMPP protocol.
    /// </summary>
    [Flags]
    public enum MechanismTypes
    {
        /// <summary>
        /// No authentication mechanism selected.
        /// </summary>
        None,

        /// <summary>
        /// Represents the PLAIN authentication mechanism, which transmits the password in plain text.
        /// </summary>
        Plain = 1 << 0,

        /// <summary>
        /// Digest-MD5 authentication mechanism.
        /// </summary>
        DigestMd5 = 1 << 1,

        /// <summary>
        /// External authentication mechanism for delegating authentication
        /// to an external provider or system.
        /// </summary>
        External = 1 << 2,

        /// <summary>
        /// Represents the SCRAM (Salted Challenge Response Authentication Mechanism) authentication mechanism.
        /// SCRAM provides a secure authentication protocol using salted hashing and iterative processes,
        /// ensuring resistance to passive and active attacks.
        /// </summary>
        Scram = 1 << 3,

        /// <summary>
        /// Enhanced SCRAM (Salted Challenge Response Authentication Mechanism) authentication mechanism
        /// with channel binding support for increased security.
        /// </summary>
        ScramPlus = 1 << 4,

        /// <summary>
        /// Represents the default combination of authentication mechanisms: Scram and ScramPlus.
        /// </summary>
        Default = Scram | ScramPlus,
    }
}
