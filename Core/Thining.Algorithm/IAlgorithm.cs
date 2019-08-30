namespace Thining.Algorithm
{
    interface IAlgorithm
    {
        byte[] Execute(byte[] pixels, int stride, int height, int width);
    }
}
