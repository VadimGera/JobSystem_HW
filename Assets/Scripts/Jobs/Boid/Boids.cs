#nullable enable
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;
using Random = UnityEngine.Random;


namespace Jobs.Boid
{
    public class Boids : MonoBehaviour
    {
        [Header("Entities")]
        [SerializeField] private int count;
        [SerializeField] private GameObject? prefab;


        
        [Header("Settings")]
        [SerializeField] private float velocityLimit;
        [SerializeField] private float destinationThreshold;
        [SerializeField] private float areaSize;
        private NativeArray<Vector3> positions;
        private NativeArray<Vector3> velocities;
        private NativeArray<Vector3> accelerations;
        private TransformAccessArray transformAccessArray;


        private void Start()
        {
            positions = new NativeArray<Vector3>(count, Allocator.Persistent);
            velocities = new NativeArray<Vector3>(count, Allocator.Persistent);
            accelerations = new NativeArray<Vector3>(count, Allocator.Persistent);
            transformAccessArray = new TransformAccessArray(Generate().ToArray());
        }


        private IReadOnlyList<Transform> Generate()
        {
            var transforms = new Transform[count];
            for (var i = 0; i < count; i++)
            {
                transforms[i] = Instantiate(prefab)!.transform;
                velocities[i] = Random.insideUnitSphere;
            }

            return transforms;
        }


        private void Update()
        {
            var limitJob = new LimitJob
            {
                positions = positions,
                accelerations = accelerations,
                areaSize = Vector3.one * areaSize
            };
            var directJob = new DirectJob
            {
                positions = positions,
                velocities = velocities,
                accelerations = accelerations,
                destinationThreshold = destinationThreshold,
            };
            var moveJob = new MoveJob
            {
                positions = positions,
                velocities = velocities,
                accelerations = accelerations,
                deltaTime = Time.deltaTime,
                speedLimit = velocityLimit
            };

            const int auto = 0;
            var boundsHandle = limitJob.Schedule(count, auto);
            var accelerationHandle = directJob.Schedule(count, auto, boundsHandle);
            var moveHandle = moveJob.Schedule(transformAccessArray, accelerationHandle);

            moveHandle.Complete();
        }


        private void OnDestroy()
        {
            positions.Dispose();
            velocities.Dispose();
            accelerations.Dispose();
            transformAccessArray.Dispose();
        }
    }
}