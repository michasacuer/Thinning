namespace Thinning.Algorithm.Interfaces
{
    public interface IAlgorithm
    {
        byte[] Execute(byte[] pixels, int stride, int height, int width, out double executionTime);
    }
}
