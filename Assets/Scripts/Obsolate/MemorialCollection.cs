using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using Vuforia;

using GozaiNASU.AR.Picture;
using GozaiNASU.AR.Picture.UI;

namespace GozaiNASU.AR.Data.Obsolate
{
    public class MemorialCollection : MonoBehaviour
    {
        CancellationTokenSource _cancellationSource;
        ObjectTracker _tracker;

        [SerializeField] List<MemorialData> _dataSet = new List<MemorialData>();
        [SerializeField] ScrollView _scrollView = default;
        [SerializeField] Canvas _canvas = default;

        public List<MemorialData> DataSet => _dataSet;

        Dictionary<MemorialData, bool> _loadDataMap = new Dictionary<MemorialData, bool>();
        Dictionary<TrackableBehaviour, MemorialTrackeventHandler> _trackableEventMap = new Dictionary<TrackableBehaviour, MemorialTrackeventHandler>();

        List<Trackable> _trackables;

        public MemorialData Create()
        {
            var data = new MemorialData();

            _dataSet.Add(data);
            _loadDataMap.Add(data, false);
            return data;
        }

        public void Activate()
        {
            _cancellationSource = new CancellationTokenSource();
        }

        async UniTask ActivateAsync()
        {
            _cancellationSource = new CancellationTokenSource();

            foreach(var data in _dataSet)
            {
                CreateDataset(data);
                await UniTask.WaitForEndOfFrame();
                
            }

            _tracker.Start();

            var targets = TrackerManager
                            .Instance
                            .GetStateManager()
                            .GetTrackableBehaviours();
            foreach(var target in targets)
            {
                if (_trackableEventMap.ContainsKey(target))
                {
                    var handler = _trackableEventMap[target];
                }
                else
                {
                    var handler = target
                                .gameObject
                                .AddComponent<MemorialTrackeventHandler>();

                    handler.TrackingFoundEvent.AddListener(ActivateClosest);

                    _trackableEventMap[target] = handler;
                }

                await UniTask.WaitForEndOfFrame();
            }
        }

        public void Cancel()
        {
            _cancellationSource.Cancel();    
            _cancellationSource.Dispose();
            _tracker.Stop();
        }

        void ActivateClosest()
        {
            if (_trackableEventMap.Count == 0) return;

            var ordered = _trackableEventMap
                            .Values
                            .Where(t => t.IsTracked)
                            .OrderBy(t => t.Depth)
                            .GetEnumerator();

            if (!ordered.MoveNext()) return;
            
            
            while(ordered.MoveNext())
            {
                ordered.Current.Deactivate();
            }
        }

        void CreateDataset(MemorialData data)
        {
            if (_loadDataMap[data])
            {
                return;
            }

            var dataset = _tracker.CreateDataSet();
            var type = data.DataType == DataType.Asset ? 
                            VuforiaUnity.StorageType.STORAGE_APP :
                            VuforiaUnity.StorageType.STORAGE_APPRESOURCE;

            if (dataset.Load(data.ModelData, type))
            {
                _tracker.ActivateDataSet(dataset);
            }

            _trackables.AddRange(dataset.GetTrackables());

            _loadDataMap[data] = true;
        }

        void Awake()
        {
            _loadDataMap = _dataSet.ToDictionary(data => data, data => false);

            _tracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
            _tracker.Stop();
        }
    }
}

