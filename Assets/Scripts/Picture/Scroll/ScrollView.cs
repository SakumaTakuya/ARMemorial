using UnityEngine;
using System.Collections.Generic;
using FancyScrollView;

namespace GozaiNASU.AR.Picture.UI
{
    class ScrollView : FancyScrollView<IPictureItemData>
    {
        [SerializeField] Scroller _scroller = default;
        [SerializeField] GameObject _cellPrefab = default;

        protected override GameObject CellPrefab => _cellPrefab;

        protected override void Initialize()
        {
            base.Initialize();
            _scroller.OnValueChanged(UpdatePosition);
        }

        public void UpdateData(IList<IPictureItemData> items)
        {
            UpdateContents(items);
            _scroller.SetTotalCount(items.Count);
        }
    }
}