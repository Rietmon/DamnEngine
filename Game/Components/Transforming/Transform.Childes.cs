using System.Collections.Generic;
using OpenTK.Mathematics;

namespace DamnEngine
{
    public partial class Transform
    {
        public Transform Parent
        {
            get => parent;
            set
            {
                if (parent == this || parent == value)
                    return;

                var oldParent = parent;
                parent = value;
                OnChangeParent(oldParent, value);
            }
        }

        public int ChildesCount => childes.Count;

        private readonly List<Transform> childes = new();

        private Transform parent;

        public Transform GetChild(int index) => childes[index];
        
        public Transform[] GetChildes() => childes.ToArray();

        private void OnChangeParent(Transform oldParent, Transform newParent)
        {
            if (oldParent)
            {
                position += oldParent.Position;
                oldParent.childes.Remove(this);
                oldParent.CallOnTransformChanged();
            }

            if (newParent)
            {
                position -= newParent.Position;
                newParent.childes.Add(this);
                newParent.CallOnTransformChanged();
            }
            
            CallOnTransformChanged();
        }
    }
}