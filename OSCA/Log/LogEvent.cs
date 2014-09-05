using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OSCA.Log
{
    /// <summary>
    /// OSCA Event logger
    /// </summary>
    public class LogEvent
    {
        /// <summary>
        /// Event types
        /// </summary>
        public enum EventType
        {
            /// <summary>
            /// Create CA event
            /// </summary>
            CreateCA = 0,
            /// <summary>
            /// Start CA event
            /// </summary>
            StartCA = 1,
            /// <summary>
            /// Stop CA event
            /// </summary>
            StopCA = 2,
            /// <summary>
            /// Issue certificate event
            /// </summary>
            IssueCert = 3,
            /// <summary>
            /// Revoke certificate event
            /// </summary>
            RevokeCert = 4,
            /// <summary>
            /// Create CRL event
            /// </summary>
            CreateCRL = 5,
            /// <summary>
            /// Expire certificate event
            /// </summary>
            ExpireCert = 10,
            /// <summary>
            /// Backup CA key event
            /// </summary>
            BackupCAKey = 11,
            /// <summary>
            /// Add certificate to database event
            /// </summary>
            DBAddCert = 12,
            /// <summary>
            /// Generic error event
            /// </summary>
            Error = 20,
            /// <summary>
            /// Generic warning event
            /// </summary>
            Warning = 21
        }

        /// <summary>
        /// Write an event to the log
        /// </summary>
        /// <param name="loggerInstance">Instance of the event logger</param>
        /// <param name="id">Event type</param>
        /// <param name="message">Text message</param>
        internal static void WriteEvent(Logger loggerInstance, EventType id, string message)
        {
            loggerInstance.writeEvent(((int)id).ToString(), message);
        }

    }
}
