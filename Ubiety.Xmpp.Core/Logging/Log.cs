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
using System.Linq;

namespace Ubiety.Xmpp.Core.Logging
{
    /// <summary>
    ///     Central log class
    /// </summary>
    public static class Log
    {
        private static ILogManager _manager = new DefaultManager();

        /// <summary>
        ///     Gets a logger for the type
        /// </summary>
        /// <typeparam name="T">Type to get a logger for</typeparam>
        /// <returns>Logger for the type</returns>
        public static ILog Get<T>()
        {
            return _manager.GetLogger(NameFor<T>());
        }

        /// <summary>
        ///     Gets a logger for the type
        /// </summary>
        /// <param name="type">Type to get a logger for</param>
        /// <returns>Logger for the type</returns>
        public static ILog Get(Type type)
        {
            return _manager.GetLogger(NameFor(type));
        }

        /// <summary>
        ///     Gets a logger for the type name
        /// </summary>
        /// <param name="name">Name of the type</param>
        /// <returns>Logger for the type name</returns>
        public static ILog Get(string name)
        {
            return _manager.GetLogger(name);
        }

        /// <summary>
        ///     Gets the name of the type
        /// </summary>
        /// <typeparam name="T">Type to get the name for</typeparam>
        /// <returns>Name of the type</returns>
        public static string NameFor<T>()
        {
            return NameFor(typeof(T));
        }

        /// <summary>
        ///     Gets the name of the type
        /// </summary>
        /// <param name="type">Type to get the name for</param>
        /// <returns>Name of the type</returns>
        public static string NameFor(Type type)
        {
            if (!type.IsGenericType)
            {
                return type.FullName;
            }

            var name = type.GetGenericTypeDefinition().FullName;

            return name.Substring(0, name.IndexOf('`')) + "<" + string.Join(
                       ",",
                       type.GetGenericArguments().Select(NameFor).ToArray()) + ">";
        }

        /// <summary>
        ///     Initializes the logger
        /// </summary>
        /// <param name="manager">Log manager</param>
        internal static void Initialize(ILogManager manager)
        {
            _manager = manager;
        }

        private class DefaultManager : ILogManager
        {
            /// <inheritdoc />
            public ILog GetLogger(string name)
            {
                return new DefaultLogger(name);
            }

            private class DefaultLogger : ILog
            {
                private readonly string _name;

                public DefaultLogger(string name)
                {
                    _name = name;
                }

                public void Log(LogLevel level, object message)
                {
                    Log(level, message.ToString());
                }

                public void Log(LogLevel level, Exception exception, object message)
                {
                    Log(level, $"{message}{Environment.NewLine}{exception}");
                }

                private void Log(LogLevel level, string message)
                {
                    Console.WriteLine($"[{_name}::{level}] {message}");
                }
            }
        }
    }
}