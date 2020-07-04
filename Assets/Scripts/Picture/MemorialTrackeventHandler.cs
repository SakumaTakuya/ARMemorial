using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using GozaiNASU.AR.Picture.UI;

namespace GozaiNASU.AR.Picture
{
    public class MemorialTrackeventHandler : DefaultTrackableEventHandler
    {
        [SerializeField] List<string> _urls = default;

        [SerializeField] ScrollView _scrollView = default;
        [SerializeField] Canvas _canvas = default;
        [SerializeField] UnityEvent _trackingFoundEvent = new UnityEvent();
        [SerializeField] UnityEvent _trackingLostEvent = new UnityEvent();

        Transform _transform;

        public UnityEvent TrackingFoundEvent => _trackingFoundEvent;
        public UnityEvent TrackingLostEvent => _trackingLostEvent;

        public bool IsTracked { get; private set; }
        public float Depth => _transform.position.z;

        void Awake()
        {
            _transform = transform;
        }

        public void Initialize(CancellationToken token)
        {
            var items = _urls
                .Select(url => new ItemDataResource(url, token))
                .Cast<IPictureItemData>()
                .ToList();
            _scrollView.UpdateData(items);
        }

        // 最も近いやつがActivateされる
        public void Activate()
        {
            _canvas.enabled = true;
        }

        public void Deactivate()
        {
            _canvas.enabled = false;
        }

        protected override void OnTrackingFound()
        {
            IsTracked = true;
            TrackingFoundEvent.Invoke();
        }

        protected override void OnTrackingLost()
        {
            IsTracked = false;
            TrackingLostEvent.Invoke();
            Deactivate();
        }
    }
}


