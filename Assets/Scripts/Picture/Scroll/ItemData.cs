using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;


namespace GozaiNASU.AR.Picture.UI
{
    class ItemDataNetwork : IPictureItemData
    {
        public string Url { get; }
        public CancellationToken Token { get; set; }

        public ItemDataNetwork(string url, CancellationToken token)
        {
            Url = url;
            Token = token;
        }

        public async UniTask<Texture> LoadTextureAsync()
        {
            const int timeOut = 3;

            var (isTimeout, (isCanceled, result)) = await UnityWebRequestTexture
                .GetTexture(Url)
                .SendWebRequest()
                .WithCancellation(Token)
                .SuppressCancellationThrow()
                .TimeoutWithoutException(System.TimeSpan.FromSeconds(timeOut));

            if (isTimeout || isCanceled || result == null)
            {
                Debug.LogWarningFormat(
                    "[Error] is timeout: {0}, is canceled:{1}", 
                    isTimeout, isCanceled);
                return null;
            }

            if (result.isNetworkError)
            {
                Debug.LogErrorFormat(
                    "[Error] network error: {0}", 
                    result.error);
                return null;
            }

            return ((DownloadHandlerTexture) result.downloadHandler).texture;
        }
    }

    public class ItemDataResource : IPictureItemData
    {
        public string Path { get; }
        public CancellationToken Token { get; set; }

        public ItemDataResource(string path, CancellationToken token)
        {
            Path = path;
            Token = token;
        }

        public async UniTask<Texture> LoadTextureAsync()
        {
            const int timeOut = 3;

            var (isTimeout, (isCanceled, result)) = await Resources
                .LoadAsync<Texture>(Path)
                .WithCancellation(Token)
                .SuppressCancellationThrow()
                .TimeoutWithoutException(System.TimeSpan.FromSeconds(timeOut));

            if (isTimeout || isCanceled || result == null)
            {
                Debug.LogWarningFormat(
                    "[Error] is timeout: {0}, is canceled:{1}", 
                    isTimeout, isCanceled);
                return null;
            }

            return result as Texture;
        }
    }

    public interface IPictureItemData
    {
        CancellationToken Token { get; set; }
        UniTask<Texture> LoadTextureAsync();
    }
}