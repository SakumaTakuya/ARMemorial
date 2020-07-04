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
using GozaiNASU.AR.Utils;
using GozaiNASU.AR.Data;

using Color = Unity.UIWidgets.ui.Color;
using Image = Unity.UIWidgets.widgets.Image;


namespace GozaiNASU.AR.View
{
    public class AddMemorialWidget : WidgetBehaviour
    {
        [SerializeField] string _title = "Add memorial";
        [SerializeField] MemorialCollection _collection;

        public MemorialData Data { get; set; }

        public override Widget Build(BuildContext context = null)
        {
            if (!Data)
            {
                Data = _collection.Create();
            }

            return new Scaffold(
                appBar : new AppBar(
                    leading : new IconButton(
                        icon : new Icon(Icons.arrow_back),
                        onPressed : () => Navigator.of(context).pop()
                    ),
                    title : new Text(_title)
                ),
                body : new Container(
                    padding : EdgeInsets.all(24f),
                    child : new AddMemorial(Data)
                )
            );
        }
    }

    public class AddMemorial : StatefulWidget
    {
        MemorialData _data;
        public AddMemorial(MemorialData data) => _data = data;

        public override State createState() => new AddMemorialState(_data);
    }

    public class AddMemorialState : State<AddMemorial>
    {
        public class StateKey : GlobalKey<FormState> { }
        
        readonly StateKey _stateKey;

        string _thumbnailUri;
        string _dataUri;

        MemorialData _data;

        public AddMemorialState(MemorialData data)
        { 
            _stateKey = new StateKey();
            _data = data;
        }
        public override Widget build(BuildContext context) =>
            new ListView(
                shrinkWrap : true,
                children : new List<Widget> {
                    new Form(
                        key : _stateKey,
                        child : new Column(
                            mainAxisSize : MainAxisSize.max,
                            children : new List<Widget>{
                                new ImageFileFormField(
                                    title : "Thumbnail",
                                    lead : "Select thumbnail picture",
                                    onSaved : file => _data.Sample = file
                                ),
                                new FileFormFiled(
                                    "Data",
                                    "Select data file",
                                    "xml",
                                    onSaved : file => _data.ModelData = file
                                ),
                                new ImageFileFormSetField(
                                    title : "Pictures",
                                    lead : "Select associated picture",
                                    onSaved : files => _data.Pictures = files
                                )
                            }
                        )
                    ),
                    new Container(
                        width : float.MaxValue,
                        height : Theme.of(context).buttonTheme.height,
                        margin : EdgeInsets.symmetric(vertical : 16f),
                        child : new RaisedButton(
                            onPressed : () => {
                                if (!_stateKey.currentState.validate()) return;
                                _stateKey.currentState.save();
                                Navigator.of(context).pop();
                            },
                            child : new Text("Submit")
                        )
                    )
                }
            );
    }

    public class ImageFileFormField : FormField<string>
    {
        public ImageFileFormField(
            string title = null,
            string lead = "",
            FormFieldSetter<string> onSaved = null,
            string initialValue = "",
            bool autovalidate = false,
            bool enabled = true
        ) : base(
            onSaved : onSaved,
            initialValue : initialValue,
            autovalidate : autovalidate,
            enabled : enabled,
            builder : state => new Column(
                crossAxisAlignment : CrossAxisAlignment.start,
                mainAxisSize : MainAxisSize.max,
                children : new List<Widget>{
                    new Container(
                        margin : EdgeInsets.only(top : 8f),
                        height : string.IsNullOrEmpty(title) ? new float?(0f) : null,
                        child : string.IsNullOrEmpty(title) ? null : new Text(title)
                    ),
                    new Card(
                        child : new Container(
                            width : 128,
                            height : 128,
                            child : new ConstrainedBox(
                                constraints : BoxConstraints.expand(),
                                child : new FlatButton(
                                    onPressed : () => {
                                        var ret = DataUploader.OpenFile(
                                            lead,
                                            "",
                                            "png, jpg"
                                        );
                                        state.didChange(ret);
                                    },
                                    padding : EdgeInsets.all(0f),
                                    child : string.IsNullOrEmpty(state.value) ? 
                                                (Widget) (new Icon(Icons.add)) :
                                                (Widget) Image.file(state.value)
                                )
                            ) 
                        )
                    ),
                    new Container(
                        height : string.IsNullOrEmpty(state.errorText) ? new float?(0f) : null,
                        padding : EdgeInsets.only(top :4f),
                        child : new Text(
                            state.errorText ?? "",
                            style : new TextStyle(
                                color : Theme.of(state.context).errorColor
                            )
                        )
                    )
                }
            ),
            validator : value => {
                if (string.IsNullOrEmpty(value))
                {
                    return "Please select an image.";
                }
                return null;
            }
        ) {}
    }

    public class ImageFileFormSetField : FormField<List<string>>
    {
        public ImageFileFormSetField(
            string title = null,
            string lead = "",
            FormFieldSetter<List<string>> onSaved = null,
            List<string> initialValue = null,
            bool autovalidate = false,
            bool enabled = true
        ) : base(
            onSaved : onSaved,
            initialValue : initialValue,
            autovalidate : autovalidate,
            enabled : enabled,
            builder : state => 
            {
                var list = new List<Widget>{
                    new Card(
                        child : new Container(
                            width : 64f,
                            height : 64f,
                            child : new ConstrainedBox(
                                constraints : BoxConstraints.expand(),
                                child : new FlatButton(
                                    onPressed : () => {
                                        var ret = DataUploader.OpenFile(
                                            lead,
                                            "",
                                            "png, jpg"
                                        );

                                        if (state.value == null)
                                        {
                                            state.didChange(new List<string>{ret});
                                        }
                                        else
                                        {
                                            state.value.Add(ret);
                                            state.didChange(state.value);
                                        }
        
                                    },
                                    padding : EdgeInsets.all(0f),
                                    child : new Icon(Icons.add)
                                )
                            ) 
                        )
                    )
                };
                if (state.value != null)
                {
                    list.AddRange(
                        state
                            .value
                            .Select((path, i) => new Card(
                                child : new Container(
                                    width : 64f,
                                    height : 64f,
                                    child : new ConstrainedBox(
                                        constraints : BoxConstraints.expand(),
                                        child : new FlatButton(
                                            onPressed : () => {
                                                var ret = DataUploader.OpenFile(
                                                    lead,
                                                    "",
                                                    "png, jpg"
                                                );
                                                state.value[i] = ret;
                                                state.didChange(state.value);
                                            },
                                            padding : EdgeInsets.all(0f),
                                            child : Image.file(path)
                                        )
                                    )
                                )
                            )
                        )
                    );
                }

                return new Column(
                    crossAxisAlignment : CrossAxisAlignment.start,
                    mainAxisSize : MainAxisSize.max,
                    children : new List<Widget>{
                        new Container(
                            margin : EdgeInsets.only(top : 8f),
                            height : string.IsNullOrEmpty(title) ? new float?(0f) : null,
                            width : float.MaxValue,
                            child : string.IsNullOrEmpty(title) ? null : new Text(title)
                        ),
                        new Container(
                            height : 72f,
                            child : new ListView(
                                scrollDirection : Axis.horizontal,
                                children : list
                            )
                        )
                    }
                );
            },
            validator : value => {
                return null;
            }
        ) {}
    }

    public class FileFormFiled : FormField<string>
    {
        public FileFormFiled(
            string title,
            string lead,
            string ext = "",
            FormFieldSetter<string> onSaved = null,
            string initialValue = "",
            bool autovalidate = false,
            bool enabled = true
        ) : base(
            onSaved : onSaved,
            initialValue : initialValue,
            autovalidate : autovalidate,
            enabled : enabled,
            builder : state => new Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                mainAxisSize : MainAxisSize.max,
                children : new List<Widget>{
                    new Container(
                        margin : EdgeInsets.only(top : 8f),
                        child : new Text(
                            title
                        )
                    ),
                    new Row(
                        children : new List<Widget> {
                            new Container(
                                padding : EdgeInsets.all(8f),
                                child : new RaisedButton(
                                    child : new Text("Select"),
                                    onPressed : () => {
                                        var ret = DataUploader.OpenFile(
                                            lead,
                                            "",
                                            ext
                                        );
                                        state.didChange(ret);
                                    }
                                )
                            ),
                            new Flexible(
                                child : new Container(
                                    padding : EdgeInsets.all(4f),
                                    width : float.MaxValue,
                                    decoration : new BoxDecoration(
                                        border : new Border(
                                            bottom : new BorderSide(
                                                width : 1f,
                                                color : Theme
                                                            .of(state.context)
                                                            .primaryColorDark
                                            )
                                        )
                                    ),
                                    child : new Text(
                                        data : state.value,
                                        overflow : TextOverflow.ellipsis,
                                        maxLines : 1
                                    )
                                )
                            )
                        }
                    ),
                    new Container(
                        height : string.IsNullOrEmpty(state.errorText) ? new float?(0f) : null,
                        padding : EdgeInsets.only(top :4f),
                        child : new Text(
                            state.errorText ?? "",
                            style : new TextStyle(
                                color : Theme.of(state.context).errorColor
                            )
                        )
                    )
                }
            ),
            validator : value => {
                if (string.IsNullOrEmpty(value))
                {
                    return "Please select a file.";
                }
                return null;
            }
        ){}
    }


}