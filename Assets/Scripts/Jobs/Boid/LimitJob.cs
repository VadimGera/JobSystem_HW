using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;


namespace Jobs.Boid
{
    public struct LimitJob : IJobParallelFor
    {
        [ReadOnly] public NativeArray<Vector3> positions;
        public NativeArray<Vector3> accelerations;
        public Vector3 areaSize;


        public void Execute(int index)
        {
            var pos = positions[index];
            var size = areaSize / 2;
            accelerations[index] += Align(-size.x - pos.x, Vector3.right)
                                    + Align(size.x - pos.x, Vector3.left)
                                    + Align(-size.y - pos.y, Vector3.up)
                                    + Align(size.y - pos.y, Vector3.down)
                                    + Align(-size.z - pos.z, Vector3.forward)
                                    + Align(size.z - pos.z, Vector3.back);
        }


        private static Vector3 Align(float delta, Vector3 direction)
        {
            const float threshold = 3f;
            const float multiplier = 100f;
            delta = math.abs(delta);
            if (delta > threshold)
                return Vector3.zero;
            return direction * (1 - delta / threshold) * multiplier;
        }
    }
}