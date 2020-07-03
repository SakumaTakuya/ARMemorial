using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

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
            .Select(url => new ItemData(url, _cancellationSource.Token))
            .ToList();
        _scrollView.UpdateData(items);
        _scrollView.UpdateData(items);
    }
}
