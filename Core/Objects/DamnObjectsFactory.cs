using System;
using System.Collections.Generic;
using Rietmon.Other;

namespace DamnEngine
{
    public static class DamnObjectsFactory
    {
#if ENABLE_WATCHING_DAMN_OBJECTS
        public static Dictionary<uint, DamnObject> DamnObjects { get; } = new();
#endif

        private static readonly Dictionary<PipelineTiming, List<DamnObject>> objectsToDestroy = new();

        private static readonly Dictionary<PipelineTiming, List<DamnObject>> objectsToRegister = new();

        public static void AddObjectToDestroy(DamnObject damnObject, PipelineTiming timing)
        {
            if (timing == PipelineTiming.Now)
            {
#if ENABLE_WATCHING_DAMN_OBJECTS
                DamnObjects.Add(damnObject.RuntimeId, damnObject);
#endif
                damnObject.Internal_OnRegister();
                return;
            }
            
            var objects = objectsToDestroy.GetOrAddDefault(timing);
            objects.Add(damnObject);
        }

        public static void AddObjectToRegister(DamnObject damnObject, PipelineTiming timing)
        {
            if (timing == PipelineTiming.Now)
            {
#if ENABLE_WATCHING_DAMN_OBJECTS
                DamnObjects.Remove(damnObject.RuntimeId);
#endif
                damnObject.Internal_OnRegister();
                return;
            }
            
            var objects = objectsToRegister.GetOrAddDefault(timing);
            objects.Add(damnObject);
        }

        public static void UpdateFactory(PipelineTiming timing)
        {
            if (objectsToDestroy.TryGetValue(timing, out var objectsToDestroyList))
            {
                for (var i = 0; i < objectsToDestroyList.Count; i++)
                {
                    var damnObject = objectsToDestroyList[i];
#if ENABLE_WATCHING_DAMN_OBJECTS
                    DamnObjects.Remove(damnObject.RuntimeId);
#endif
                    damnObject.Internal_OnDestroy();
                }
                objectsToDestroyList.Clear();
            }
            
            if (objectsToRegister.TryGetValue(timing, out var objectsToRegisterList))
            {
                for (var i = 0; i < objectsToRegisterList.Count; i++)
                {
                    var damnObject = objectsToRegisterList[i];
#if ENABLE_WATCHING_DAMN_OBJECTS
                    DamnObjects.Add(damnObject.RuntimeId, damnObject);
#endif
                    damnObject.Internal_OnRegister();
                }
                objectsToRegisterList.Clear();
            }
        }
    }
}