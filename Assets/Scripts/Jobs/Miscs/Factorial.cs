namespace Jobs.Miscs
{
    public class Factorial
    {
        public static int For(int value)
        {
            if (value == 0)
            {
                return 1;
            }

            return value * For(value - 1);
        }
    }
}