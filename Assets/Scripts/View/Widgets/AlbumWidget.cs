using com.unity.uiwidgets.Runtime.rendering;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.engine;
using Unity.UIWidgets.material;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using GozaiNASU.AR.Data;

using Color = Unity.UIWidgets.ui.Color;
using Image = Unity.UIWidgets.widgets.Image;


namespace GozaiNASU.AR.View
{
    public class AlbumWidget : WidgetBehaviour
    {
        [SerializeField] AddMemorialWidget _addMemorial = default;
        [SerializeField] AddMemorialWidget _editMemorial = default;
        [SerializeField] MemorialCollection _collection = default;



        public override Widget Build(BuildContext context = null)
        {
            var dataset = _collection.DataSet;
            return new Scaffold(
                appBar : new AppBar(
                    // leading : new IconButton(icon : new Icon(Icons.menu)),
                    title : new Text(Configuration.Instance.AppName),
                    actions : new List<Widget>{
                        new IconButton(
                            icon : new Icon(Icons.add),
                            onPressed : () => Navigator
                                                .of(context)
                                                .push(
                                                    new MaterialPageRoute(
                                                        c => {
                                                            _addMemorial.Data = null;
                                                            return _addMemorial.Build(c);
                                                        }
                                                        
                                                    )
                                                )

                        )
                    }
                ),
                body : new Container(
                    child : new CustomScrollView(
                        slivers : new List<Widget> {
                            new SliverAppBar(
                                floating : true,
                                snap : true,
                                title : new Text(
                                    "Album", 
                                    style : Theme
                                                .of(context)
                                                .primaryTextTheme
                                                .headline
                                ),
                                backgroundColor : Theme
                                                    .of(context)
                                                    .secondaryHeaderColor
                            ),
                            new SliverGrid(
                                gridDelegate : new SliverGridDelegateWithFixedCrossAxisCount(
                                    crossAxisCount : 3
                                ),
                                layoutDelegate : new SliverChildBuilderDelegate(
                                    (cont, id) => new Card(
                                        child : new Container(
                                            child : new ConstrainedBox(
                                                constraints : BoxConstraints.expand(),
                                                child : new FlatButton(
                                                    onPressed : () => Navigator
                                                                        .of(context)
                                                                        .push(
                                                                        new  MaterialPageRoute(
                                                                                c => {
                                                                                    _editMemorial.Data = dataset[id];
                                                                                    return _editMemorial.Build(c);
                                                                                }
                                                                            )),
                                                    padding : EdgeInsets.all(0f),
                                                    child : string.IsNullOrEmpty(dataset[id].Sample) ?
                                                                null :
                                                                dataset[id].DataType == DataType.Asset ? 
                                                                    Image.asset(dataset[id].Sample) :
                                                                    Image.file(dataset[id].Sample),
                                                    splashColor : Theme.of(cont).splashColor
                                                )
                                            ) 
                                        )
                                    ),
                                    childCount : dataset.Count
                                )
                            )
                        }
                    )
                )
            );
        }
    }

    public class TextEditWidget : StatelessWidget
    {
        MemorialData _data;

        public TextEditWidget(MemorialData data)
        {
            _data = data;
        }

        public override Widget build(BuildContext context) =>
            new Scaffold(
                appBar : new AppBar(
                    leading : new IconButton(
                        icon : new Icon(Icons.arrow_back),
                        onPressed : () => Navigator.of(context).pop()
                    ),
                    title : new Text(Configuration.Instance.AppName)
                ),
                body : new Text(_data.name)
            );
    }
}
