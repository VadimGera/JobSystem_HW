using Jobs.Miscs;
using Unity.Jobs;


namespace Jobs.IJobs
{
    public struct ValueBoxedFactorialJob : IJob
    {
        private readonly int value;
        private readonly Box output;


        public int Output => output!.value;


        public ValueBoxedFactorialJob(int value)
        {
            this.value = value;
            output = new Box();
        }


        private class Box
        {
            public int value;
        }


        public void Execute()
        {
            output!.value = Factorial.For(value);
        }
    }
}