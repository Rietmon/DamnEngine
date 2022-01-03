using OpenTK.Mathematics;
using Rietmon.Extensions;

namespace DamnEngine
{
    public class TestPhysics : Component
    {
        protected internal override void OnUpdate()
        {
            if (Input.IsKeyDown(KeyCode.B))
            {
                var rigidBody = GetComponent<RigidBody>();
                var x = RandomUtilities.Range(-0.5f, 0.5f);
                var y = RandomUtilities.Range(1, 10);
                var z = RandomUtilities.Range(-0.5f, 0.5f);
                var x2 = RandomUtilities.Range(-0.5f, 0.5f);
                var y2 = RandomUtilities.Range(-0.5f, 0.5f);
                var z2 = RandomUtilities.Range(-0.5f, 0.5f);
                rigidBody.ApplyImpulse(new Vector3(x,y,z), new Vector3(x2, y2, z2));
            }

            if (Input.IsKeyDown(KeyCode.N))
            {
                var rigidBody = GetComponent<RigidBody>();
                rigidBody.ApplyImpulse(new Vector3(-2, 5, 0), new Vector3(0.5f, 0.5f, 0));
            }

            if (Input.IsKeyDown(KeyCode.M))
            {
                var rigidBody = GetComponent<RigidBody>();
                rigidBody.ApplyImpulse(new Vector3(0, 5, 2), new Vector3(0, 0.5f, 0.5f));
            }

            if (Input.IsKeyDown(KeyCode.L))
            {
                var rigidBody = GetComponent<RigidBody>();
                rigidBody.ApplyImpulse(new Vector3(2, 5, 2), new Vector3(0, 0.5f, 0));
            }

            if (Input.IsKeyDown(KeyCode.U))
            {
                var rigidBody = GetComponent<RigidBody>();
                rigidBody.ApplyImpulse(new Vector3(0, 5, 0));
            }
            
            if (Input.IsKeyDown(KeyCode.D1))
                Physics.Gravity += Vector3.UnitY.FromToBepuPosition();
            if (Input.IsKeyDown(KeyCode.D2))
                Physics.Gravity -= Vector3.UnitY.FromToBepuPosition();
        }
    }
}