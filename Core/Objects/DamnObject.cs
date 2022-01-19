using System;
using DamnEngine.Serialization;

namespace DamnEngine
{
    public abstract class DamnObject : LowLevelDamnObject
    {
        private static uint lastRuntimeId;

        public virtual string Name { get; set; } 
        
        public uint RuntimeId { get; }
        
        public bool IsDestroying { get; private set; }
        
        public bool IsRegistered { get; private set; }

        protected DamnObject(PipelineTiming timing)
        {
            RuntimeId = lastRuntimeId++;
            if (timing != PipelineTiming.Never)
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
            IsRegistered = false;
            OnDestroy();
        }

        public override void Destroy()
        {
            IsDestroying = true;
            DamnObjectsFactory.AddObjectToDestroy(this, PipelineTiming.OnEndFrame);
        }
    }
}