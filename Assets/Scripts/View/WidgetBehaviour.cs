using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.engine;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.material;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

using Color = Unity.UIWidgets.ui.Color;

namespace GozaiNASU.AR.View
{
    [ExecuteInEditMode]
    public abstract class WidgetBehaviour : MonoBehaviour
    {
        [SerializeField] private string _path = default;
        public string Path => _path;
        public abstract Widget Build(BuildContext context=null);
    }
}

