#nullable enable
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;


namespace Jobs.Boid
{
    public struct ColorJob : IJobParallelFor
    {
        public NativeArray<Vector3> positions;
        public NativeArray<Color> colors;

        public void Execute(int index)
        {
            Vector3 position = positions[index];

            float normalizedX = Mathf.InverseLerp(-1f, 1f, position.x);
            float normalizedY = Mathf.InverseLerp(-1f, 1f, position.y);
            float normalizedZ = Mathf.InverseLerp(-1f, 1f, position.z);

            Color color = new Color(normalizedX, normalizedY, normalizedZ);
            colors[index] = color;
        }
    }
}
