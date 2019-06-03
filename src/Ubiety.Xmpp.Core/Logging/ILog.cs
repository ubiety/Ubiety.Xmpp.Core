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

namespace Ubiety.Xmpp.Core.Logging
{
    /// <summary>
    ///     Defines an interface for logging.
    /// </summary>
    public interface ILog
    {
        /// <summary>
        ///     Log a generic message.
        /// </summary>
        /// <param name="level">Severity level of the message.</param>
        /// <param name="message">Message to log.</param>
        void Log(LogLevel level, object message);

        /// <summary>
        ///     Log a message with an exception.
        /// </summary>
        /// <param name="level">Severity level of the message.</param>
        /// <param name="exception">Exception to log.</param>
        /// <param name="message">Message to log.</param>
        void Log(LogLevel level, Exception exception, object message);
    }
}
