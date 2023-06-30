using Jobs.Miscs;
using Unity.Jobs;


namespace Jobs.IJobs
{
    public struct BrokenParametricFactorialJob : IJob
    {
        private readonly int value;
        private int output;


        public int Output => output;


        public BrokenParametricFactorialJob(int value)
        {
            this.value = value;
            output = 0;
        }


        public void Execute()
        {
            output = Factorial.For(value);
        }
    }
}