using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using Vuforia;

using GozaiNASU.AR.Picture.UI;

public class Test : MonoBehaviour
{
    [SerializeField] List<string> _urls = default;
    [SerializeField] ScrollView _scrollView = default;

    CancellationTokenSource _cancellationSource;

    void Start()
    {
        _cancellationSource = new CancellationTokenSource();

        var items = _urls
            .Select(url => new ItemDataNetwork(url, _cancellationSource.Token))
            .Cast<IPictureItemData>()
            .ToList();
        _scrollView.UpdateData(items);


        var a = gameObject.AddComponent<ImageTargetBehaviour>();
        var tracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
        var dataset = tracker.CreateDataSet();
        print(dataset.Load(
            "C:/Users/Lab/Documents/sakuma/test/AR.xml", 
            VuforiaUnity.StorageType.STORAGE_ABSOLUTE));
        // tracker.ActivateDataSet(dataset);
        // print(string.Join(", ",tracker.GetActiveDataSets()));
    }
}
