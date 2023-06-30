using Unity.Collections;
using Unity.Jobs;


namespace Jobs.IJobParallels
{
    public struct PowJob : IJob
    {
        [ReadOnly] private NativeArray<float> input;
        [WriteOnly] private NativeArray<float> output;


        public PowJob(NativeArray<float> input, NativeArray<float> output)
        {
            this.input = input;
            this.output = output;
        }


        public void Execute()
        {
            for (int i = 0; i < input.Length; i++)
            {
                output[i] = input[i] * input[i];
            }
        }
    }
}