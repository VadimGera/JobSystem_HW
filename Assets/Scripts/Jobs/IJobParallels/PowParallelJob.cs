using Unity.Collections;
using Unity.Jobs;


namespace Jobs.IJobParallels
{
    public struct PowParallelJob : IJobParallelFor
    {
        [ReadOnly] private NativeArray<float> input;
        [WriteOnly] private NativeArray<float> output;


        public PowParallelJob(NativeArray<float> input, NativeArray<float> output)
        {
            this.input = input;
            this.output = output;
        }


        public void Execute(int i)
        {
            output[i] = input[i] * input[i];
        }
    }
}