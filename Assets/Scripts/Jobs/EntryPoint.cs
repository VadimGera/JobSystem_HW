using IJobs;
using Jobs.IJobParallels;
using Jobs.IJobs;
using Jobs.Miscs;
using System;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;


public class EntryPoint : MonoBehaviour
{
    [ContextMenu(nameof(Simple))]
    private void Simple()
    {
        var job = new SimpleFactorialJob();
        print("After");
        var handle = job.Schedule();
        print("Before");
        handle.Complete();
    }


    [ContextMenu(nameof(WithParams))]
    private void WithParams()
    {
        var value = 12;
        var job = new BrokenParametricFactorialJob(value);
        job.Schedule().Complete();
        Debug.Log($"Result for {value} is {job.Output}");
    }


    [ContextMenu(nameof(ValueBoxedParams))]
    private void ValueBoxedParams()
    {
        var value = 12;
        var job = new ValueBoxedFactorialJob(value);
        job.Schedule().Complete();
        Debug.Log($"Result for {value} is {job.Output}");
    }


    [ContextMenu(nameof(NativeArrayParams))]
    private void NativeArrayParams()
    {
        var value = 12;
        var array = new NativeArray<int>(1, Allocator.TempJob);
        var job = new NativeArrayFactorialJob(value, array);
        job.Schedule().Complete();
        Debug.Log($"Result for {value} is {job.Output}");
        //array.Dispose();
    }


    [ContextMenu(nameof(FoolSafetyArrayParams))]
    private void FoolSafetyArrayParams()
    {
        var value = 12;
        var array = new NativeArray<int>(1, Allocator.TempJob);
        var job = new NativeArrayFactorialJob(value, array);
        var anotherJob = new NativeArrayFactorialJob(value, array);
        job.Schedule();
        anotherJob.Schedule();
        Debug.Log($"Result for {value} is {job.Output}");
        //array.Dispose();
    }


    [ContextMenu(nameof(ParallelTest))]
    private void ParallelTest()
    {
        var stopwatch = new StopWatch();
        var size = 100_000_000;

        TimeSpan NotParallel() =>
            stopwatch.Launch(() =>
                {
                    new PowJob(
                            new NativeArray<float>(size, Allocator.TempJob),
                            new NativeArray<float>(size, Allocator.TempJob)
                        )
                        .Schedule()
                        .Complete();
                }
            );

        TimeSpan Parallel() =>
            stopwatch.Launch(() =>
                {
                    var workers = 4;

                    new PowParallelJob(
                            new NativeArray<float>(size, Allocator.TempJob),
                            new NativeArray<float>(size, Allocator.TempJob)
                        )
                        .Schedule(size, workers)
                        .Complete();
                }
            );

        Debug.Log($"Not Parallel {NotParallel().TotalMilliseconds}");
        Debug.Log($"Parallel {Parallel().TotalMilliseconds}");
    }
}