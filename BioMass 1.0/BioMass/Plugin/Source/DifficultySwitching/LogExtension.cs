using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Text.RegularExpressions;

namespace KSPBioMass
{
    public static class LogExtension
    {
        //Log Unity
        public static void Log(this UnityEngine.Object obj, string message)
        {
            Debug.Log(obj.GetType().FullName + "[" + Time.time.ToString("0.00") + "]: " + message);
        }

        public static void LogWarning(this UnityEngine.Object obj, string message)
        {
            Debug.LogWarning(obj.GetType().FullName + "[" + Time.time.ToString("0.00") + "]: " + message);
        }

        public static void LogError(this UnityEngine.Object obj, string message)
        {
            Debug.LogError(obj.GetType().FullName + "[" + Time.time.ToString("0.00") + "]: " + message);
        }

        //Log .net
        public static void Log(this System.Object obj, string message)
        {
            Debug.Log(obj.GetType().FullName + "[" + Time.time.ToString("0.00") + "]: " + message);
        }

        public static void LogWarning(this System.Object obj, string message)
        {
            Debug.LogWarning(obj.GetType().FullName + "[" + Time.time.ToString("0.00") + "]: " + message);
        }

        public static void LogError(this System.Object obj, string message)
        {
            Debug.LogError(obj.GetType().FullName + "[" + Time.time.ToString("0.00") + "]: " + message);
        }

        //Other Log / String
        public static void Log(string context, string message)
        {
            Debug.Log(context + "[" + Time.time.ToString("0.00") + "]: " + message);
        }

        public static void LogWarning(string context, string message)
        {
            Debug.LogWarning(context + "[" + Time.time.ToString("0.00") + "]: " + message);
        }

        public static void LogError(string context, string message)
        {
            Debug.LogError(context + "[" + Time.time.ToString("0.00") + "]: " + message);
        }


        //Log Only in Debug
        [System.Diagnostics.Conditional("DEBUG")]
        internal static void Log_DebugOnly(this UnityEngine.Object obj, string message)
        {
            Log(obj, message);
        }

        [System.Diagnostics.Conditional("DEBUG")]
        internal static void Log_DebugOnly(this System.Object obj, string message)
        {
            Log(obj, message);
        }

        [System.Diagnostics.Conditional("DEBUG")]
        internal static void Log_DebugOnly(string context, string message)
        {
            Log(context, message);
        }
    }
}
