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
    public struct ColorJob : IJobParallelFor
    {
        public NativeArray<Vector3> positions;
        public NativeArray<Color> colors;

        public void Execute(int index)
        {
            Vector3 position = positions[index];

            // Нормализация значений позиции в диапазоне [0, 1]
            float normalizedX = Mathf.InverseLerp(-1f, 1f, position.x);
            float normalizedY = Mathf.InverseLerp(-1f, 1f, position.y);
            float normalizedZ = Mathf.InverseLerp(-1f, 1f, position.z);

            // Преобразование нормализованных значений в компоненты цвета RGB
            Color color = new Color(normalizedX, normalizedY, normalizedZ);
            colors[index] = color;
        }
    }

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
        private NativeArray<Color> colors;
        private TransformAccessArray transformAccessArray;


        private void Start()
        {
            positions = new NativeArray<Vector3>(count, Allocator.Persistent);
            velocities = new NativeArray<Vector3>(count, Allocator.Persistent);
            accelerations = new NativeArray<Vector3>(count, Allocator.Persistent);
            colors = new NativeArray<Color>(count, Allocator.Persistent);
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
            var colorJob = new ColorJob
            {
                positions = positions,
                colors = colors
            };

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
            var colorHandle = colorJob.Schedule(count, auto);
            var boundsHandle = limitJob.Schedule(count, auto, colorHandle);
            var accelerationHandle = directJob.Schedule(count, auto, boundsHandle);
            var moveHandle = moveJob.Schedule(transformAccessArray, accelerationHandle);

            moveHandle.Complete();

            for (int i = 0; i < count; i++)
            {
                MeshRenderer meshRenderer = transformAccessArray[i].GetComponentInChildren<MeshRenderer>();
                meshRenderer.material.color = colors[i];
            }
        }


        private void OnDestroy()
        {
            positions.Dispose();
            velocities.Dispose();
            accelerations.Dispose();
            colors.Dispose();
            transformAccessArray.Dispose();
        }
    }
}
