using System;
using DamnEngine.Serialization;

namespace DamnEngine
{
    public abstract class DamnObject : LowLevelDamnObject
    {
        private static uint lastRuntimeId;

        public string Name { get; set; } 
        
        public uint RuntimeId { get; }
        
        public bool IsDestroying { get; internal set; }
        
        public bool IsRegistered { get; private set; }

        protected DamnObject(PipelineTiming timing)
        {
            RuntimeId = lastRuntimeId++;
            ForceRegister(timing);
        }

        public void ForceRegister(PipelineTiming timing)
        {
            DamnObjectsFactory.AddObjectToRegister(this, timing);
        }

        internal void Internal_OnRegister()
        {
            IsRegistered = true;
            OnRegister();
        }
        
        protected virtual void OnRegister() { }

        internal void Internal_OnDestroy()
        {
#if ENABLE_WATCHING_DAMN_OBJECTS
            Debug.Log($"[{nameof(DamnObject)}] ({nameof(Internal_OnDestroy)}) Destroying object {Name}");
#endif
            IsRegistered = false;
            OnDestroy();
        }

        public override void Destroy()
        {
            DamnObjectsFactory.AddObjectToDestroy(this, PipelineTiming.OnEndFrame);
        }
    }
}