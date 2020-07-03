using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;


namespace GozaiNASU.AR.Picture
{
    public class MemorialCollection : MonoBehaviour
    {
        CancellationTokenSource _cancellationSource;
        [SerializeField] List<MemorialTrackeventHandler> _targets = default;


        public void Activate()
        {
            _cancellationSource = new CancellationTokenSource();
            foreach(var target in _targets)
            {
                target.Initialize(_cancellationSource.Token);
                target.TrackingFoundEvent.AddListener(ActivateClosest);
            }
        }

        public void Cancel()
        {
            _cancellationSource.Cancel();    
            _cancellationSource.Dispose();
        }

        void ActivateClosest()
        {
            if (_targets.Count == 0) return;

            var ordered = _targets
                .FindAll(t => t.IsTracked)
                .OrderBy(t => t.Depth).GetEnumerator();

            if (!ordered.MoveNext()) return;
            
            ordered.Current.Activate();
            while(ordered.MoveNext())
            {
                ordered.Current.Deactivate();
            }
        }

    }
}

