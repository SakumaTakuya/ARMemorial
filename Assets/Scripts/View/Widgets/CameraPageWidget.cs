using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using Unity.UIWidgets.engine;
using Unity.UIWidgets.material;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Vuforia.UnityCompiled;
using Vuforia;

using GozaiNASU.AR.Data;

using Color = Unity.UIWidgets.ui.Color;
using Image = UnityEngine.UI.Image;
using ScrollView = GozaiNASU.AR.Picture.UI.ScrollView;


namespace GozaiNASU.AR.View
{
    public class CameraPageWidget : WidgetBehaviour
    {
        [SerializeField] VuforiaBehaviour _vuforia = default;
        [SerializeField] Image  _scrollerBackground = default;
        [SerializeField] MemorialCollection _memorialCollection = default;


        public override Widget Build(BuildContext context = null)
        {
            StartCoroutine(ChangeActivation(true));

            return new Scaffold(
                backgroundColor : Colors.white.withOpacity(0f),
                appBar : new AppBar(
                    backgroundColor : Colors.grey.withOpacity(0.125f),
                    elevation : 0f,
                    leading : new IconButton(
                        icon : new Icon(Icons.arrow_back),
                        onPressed : () => Exit(context)
                    )
                )
            );
        }

        void Exit(BuildContext context)
        {
            StartCoroutine(ChangeActivation(false));
            if (context == null) return;
            Navigator.of(context).pop();
        }

        IEnumerator ChangeActivation(bool IsActive)
        {
            const float delayTime = 0.3f;

            yield return new WaitForSeconds(delayTime);
            _vuforia.enabled = IsActive;

            yield return null;
            _scrollerBackground.raycastTarget = IsActive;

            yield return null;
            if (IsActive)
            {
                _memorialCollection.Activate();
            }
            else
            {
                _memorialCollection.Cancel();
            }
        }
    }
}