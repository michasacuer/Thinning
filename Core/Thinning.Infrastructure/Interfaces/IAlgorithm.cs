namespace Thinning.Infrastructure.Interfaces
{
    public interface IAlgorithm
    {
        byte[] Execute(byte[] pixels, int stride, int height, int width);
    }
}
