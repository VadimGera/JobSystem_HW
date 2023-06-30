using Jobs.Miscs;
using Unity.Collections;
using Unity.Jobs;


namespace IJobs
{
    public struct NativeArrayFactorialJob : IJob
    {
        private readonly int value;
        private NativeArray<int> output;


        public int Output => output[0];


        public NativeArrayFactorialJob(int value, NativeArray<int> output)
        {
            this.value = value;
            this.output = output;
        }


        public void Execute()
        {
            output[0] = Factorial.For(value);
        }
    }
}