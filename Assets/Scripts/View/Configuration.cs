using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Unity.UIWidgets.widgets;


namespace GozaiNASU.AR.View
{
    [ExecuteInEditMode]
    public sealed class Configuration : MonoBehaviour
    {
        [SerializeField] string _appName = "AR";
        [SerializeField] List<WidgetBehaviour> _route = default;

        public string AppName => _appName;
        public readonly Dictionary<string, WidgetBuilder> Route = new Dictionary<string, WidgetBuilder>();

        static Configuration _instance;
        public static Configuration Instance => _instance ?? InitIncetance();


        static Configuration InitIncetance()
        {
            _instance = FindObjectOfType<Configuration>() ?? new GameObject(typeof(Configuration).Name).AddComponent<Configuration>();
            foreach(var widget in _instance._route)
            {
                _instance.Route.Add(widget.Path, c => widget.Build(c));
            }
            return _instance;
        }

        void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }
    }
}