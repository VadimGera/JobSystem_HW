using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Jobs;


namespace Jobs.Boid
{
    public struct MoveJob : IJobParallelForTransform
    {
        public NativeArray<Vector3> positions;
        public NativeArray<Vector3> velocities;
        public NativeArray<Vector3> accelerations;
        public float deltaTime;
        public float speedLimit;
    
        public void Execute(int index, TransformAccess transform)
        {
            var velocity = velocities[index] + accelerations[index] * deltaTime;
            var direction = velocity.normalized;
            velocity = direction * math.clamp(velocity.magnitude, 1, speedLimit);
            transform.position += velocity * deltaTime;
            transform.rotation = Quaternion.LookRotation(direction);

            positions[index] = transform.position;
            velocities[index] = velocity;
            accelerations[index] = Vector3.zero;
        }
    }
}