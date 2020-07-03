using System.Threading;


namespace GozaiNASU.AR.Picture.UI
{
    class ItemData
    {
        public string Url { get; }
        public CancellationToken Token { get; }

        public ItemData(string url, CancellationToken token)
        {
            Url = url;
            Token = token;
        }
    }
}