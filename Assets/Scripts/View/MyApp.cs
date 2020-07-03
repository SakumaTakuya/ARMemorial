using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Unity.UIWidgets.engine;
using Unity.UIWidgets.material;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.service;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;


namespace GozaiNASU.AR.View
{
    public class MyApp : UIWidgetsPanel
    {
        protected override Widget createWidget() =>
            new MaterialApp(
                theme : new ThemeData(
                    brightness : Brightness.light,
                    primarySwatch : Colors.pink,
                    canvasColor : Colors.white,
                    primaryTextTheme : new TextTheme(
                        title : new TextStyle(fontSize : 20, fontWeight : FontWeight.w900),
                        headline : new TextStyle(fontSize : 20, fontWeight : FontWeight.w400, color : Colors.pink)
                    )
                ),
                home : Configuration.Instance.Route
                            .First()
                            .Value.Invoke(null),
                routes : Configuration.Instance.Route
            );

        protected override void OnEnable()
        {
            FontManager.instance.addFont(Resources.Load<Font>(path: "MaterialIcons-Regular"), "Material Icons");
            Screen.fullScreen = false; // ナビゲーションバー表示用
            base.OnEnable();
        }
    }
}

