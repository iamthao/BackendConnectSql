namespace Framework.Utility
{
    public interface IResizeImage
    {
        byte[] ResizeImageByHeight(string filePath, int height);

        byte[] ResizeImageByWidth(string filePath, int width);

        byte[] ResizeImageByHeightAndWidth(string filePath, int height, int width);
    }
}