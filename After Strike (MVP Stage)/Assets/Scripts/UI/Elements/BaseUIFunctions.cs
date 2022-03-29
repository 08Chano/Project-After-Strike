using System;
using UnityEngine;

namespace AfterStrike.UI
{
    /// <summary>
    /// Base function calls for UI elements.
    /// </summary>
    public class BaseUIFunctions : MonoBehaviour
    {
        protected virtual void Awake()
        {
            Sync();
        }

        protected virtual void Enable()
        {
            Sync();
        }

        /// <summary>
        /// Called whenever the UI element is enabled and is enabled awake.
        /// </summary>
        protected virtual void Sync() { }

        protected virtual void LogWarning(Type origin, string errorText) 
        {
            Debug.LogWarning($"[{origin.Name}] {errorText}");
        }

        protected virtual void LogError(Type origin, string errorText) 
        {
            Debug.LogError($"[{origin.Name}] {errorText}");
        }
    }
}