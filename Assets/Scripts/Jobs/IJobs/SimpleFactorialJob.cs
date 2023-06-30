using Jobs.Miscs;
using Unity.Jobs;
using UnityEngine;


namespace Jobs.IJobs
{
    public struct SimpleFactorialJob : IJob
    {
        public void Execute()
        {
            var value = 12;
            Debug.Log($"Factorial for {value} is {Factorial.For(value)}");
        }
    }
}