using Unity.Collections;
using Unity.Jobs;
using UnityEngine;


namespace Jobs.Boid
{
    public struct DirectJob : IJobParallelFor
    {
        [ReadOnly] public NativeArray<Vector3> positions;
        [ReadOnly] public NativeArray<Vector3> velocities;
        public NativeArray<Vector3> accelerations;
        public float destinationThreshold;
        

        public void Execute(int index)
        {
            var averageSpread = Vector3.zero;
            var averageVelocity = Vector3.zero;
            var averagePosition = Vector3.zero;
            
            var count = positions.Length - 1;

            for (var i = 0; i < count; i++)
            {
                if (i == index)
                {
                    continue;
                }

                var targetPosition = positions[i];
                var delta = positions[index] - targetPosition;
                if (delta.magnitude > destinationThreshold)
                {
                    continue;
                }

                averageSpread += delta.normalized;
                averageVelocity += velocities[i];
                averagePosition += targetPosition;
            }

            var myPosition = positions[index];
            accelerations[index] += averageSpread / count +
                                    averageVelocity / count +
                                    (averagePosition / count - myPosition);
        }
    }
}