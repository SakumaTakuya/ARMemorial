using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using GozaiNASU.AR.Picture;

namespace GozaiNASU.AR.Data
{
    public class MemorialCollection : MonoBehaviour
    {
        CancellationTokenSource _cancellationSource;
        [SerializeField] List<MemorialTrackeventHandler> _targets = default;
        [SerializeField] List<MemorialData> _dataSet = new List<MemorialData>();
        public List<MemorialData> DataSet => _dataSet;

        public MemorialData Create()
        {
            var data = ScriptableObject.CreateInstance("MemorialData") as MemorialData;

            _dataSet.Add(data);
            return data;
        }

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

