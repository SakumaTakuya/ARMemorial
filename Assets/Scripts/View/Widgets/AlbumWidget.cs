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

using Color = Unity.UIWidgets.ui.Color;
using Image = Unity.UIWidgets.widgets.Image;


namespace GozaiNASU.AR.View
{
    public class AlbumWidget : WidgetBehaviour
    {
        [SerializeField] MemorialData[] _dataset = default;


        public override Widget Build(BuildContext context = null) =>
            new Scaffold(
                appBar : new AppBar(
                    leading : new IconButton(icon : new Icon(Icons.menu)),
                    title : new Text(Configuration.Instance.AppName),
                    actions : new List<Widget>{
                        new IconButton(icon : new Icon(Icons.add))
                    }
                ),
                body : new Container(
                    child : new CustomScrollView(
                        slivers : new List<Widget> {
                            new SliverAppBar(
                                floating : true,
                                snap : true,
                                title : new Text(
                                    "Memorial", 
                                    style : Theme.of(context).primaryTextTheme.headline
                                ),
                                backgroundColor : Theme.of(context).secondaryHeaderColor
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
                                                                                c => new TextEditWidget(_dataset[id])
                                                                            )),
                                                    padding : EdgeInsets.all(0f),
                                                    child : Image.asset(_dataset[id].Sample),
                                                    splashColor : Theme.of(cont).splashColor
                                                )
                                            )
                                            
                                        )
                                    ),
                                    childCount : _dataset.Length
                                )
                            )
                        }
                    )
                )
            );
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
