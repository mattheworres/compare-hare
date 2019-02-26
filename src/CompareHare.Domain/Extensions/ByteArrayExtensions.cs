namespace CompareHare.Domain.Extensions
{
    public static class ByteArrayExtensions
    {
        public static string ToUrlSafeBase64String(this byte[] bytes)
        {
            return System.Convert.ToBase64String(bytes)
                .Replace('+', '-')
                .Replace('=', '_')
                .Replace('/', '~');
        }
    }
}
