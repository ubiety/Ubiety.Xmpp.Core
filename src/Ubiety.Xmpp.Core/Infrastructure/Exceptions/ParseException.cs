// Copyright 2019 Dieter Lunn
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
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Ubiety.Xmpp.Core.Infrastructure.Exceptions
{
    /// <summary>
    ///     Parse Exception.
    /// </summary>
    [Serializable]
    public class ParseException : Exception
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ParseException"/> class.
        /// </summary>
        public ParseException()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ParseException"/> class.
        /// </summary>
        /// <param name="message">Message to use for the exception.</param>
        public ParseException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ParseException"/> class.
        /// </summary>
        /// <param name="message">Message to use for the exception.</param>
        /// <param name="innerException">Inner exception.</param>
        public ParseException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ParseException"/> class.
        /// </summary>
        /// <param name="info">Serialization info.</param>
        /// <param name="context">Streaming context.</param>
        protected ParseException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <inheritdoc/>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}
