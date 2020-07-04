using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using EasingCore;
using FancyScrollView;
using Cysharp.Threading.Tasks;

namespace GozaiNASU.AR.Picture.UI
{
    class Cell : FancyCell<IPictureItemData>
    {
        readonly EasingFunction alphaEasing = Easing.Get(Ease.OutQuint);

        [SerializeField] RawImage _image = default;
        [SerializeField] Texture _defaultTexture = default;
        [SerializeField] Image background = default;
        [SerializeField] CanvasGroup canvasGroup = default;

        public override sealed bool IsVisible => base.IsVisible;
        public override sealed void SetVisible(bool visible) => base.SetVisible(visible);

        public override void UpdateContent(IPictureItemData itemData)
        {
            _image.texture = null;            
            LoadTextureAsync(itemData).Forget();
            UpdateSibling();
        }

        async UniTask LoadTextureAsync(IPictureItemData data)
        {
            var ret = await data.LoadTextureAsync();
            _image.texture = ret ?? _defaultTexture;
        }

        ///<summary>
        ///写真は全て重なっているので表示中のものが最後に描画されるように順番を管理(Canvas内にオーダーが存在しないため必要)
        ///</summary>
        void UpdateSibling()
        {
            var cells = transform.parent.GetComponentsInChildren<Cell>(false);

            if (Index == cells.Min(x => x.Index))
            {
                transform.SetAsLastSibling();
            }

            if (Index == cells.Max(x => x.Index))
            {
                transform.SetAsFirstSibling();
            }
        }

        public override void UpdatePosition(float t)
        {
            const float popAngle = -15;
            const float slideAngle = 25;

            const float popSpan = 0.75f;
            const float slideSpan = 0.25f;

            t = 1f - t;

            var pop = Mathf.Min(popSpan, t) / popSpan;
            var slide = Mathf.Max(0, t - popSpan) / slideSpan;

            transform.localRotation = t < popSpan
                ? Quaternion.Euler(0, 0, popAngle * (1f - pop))
                : Quaternion.Euler(0, 0, slideAngle * slide);

            transform.localPosition = Vector3.left * 500f * slide;

            canvasGroup.alpha = alphaEasing(1f - slide);

            background.color = Color.Lerp(Color.gray, Color.white, pop);
        }
    }
}