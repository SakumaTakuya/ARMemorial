using com.unity.uiwidgets.Runtime.rendering;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
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
    public class HomeWidget : WidgetBehaviour
    {
        [SerializeField] AlbumWidget _album = default;


        public override Widget Build(BuildContext context = null) =>
            new Home(_album);
    }

    public class Home : StatefulWidget
    {
        AlbumWidget _album;
        public Home(AlbumWidget album) => _album = album;

        public override State createState() => new HomeState(_album);
    }

    public class HomeState : State<Home>
    {
        public class NavigatorKey : GlobalKey<NavigatorState> {}

        PageController _pageController;
        AlbumWidget _album;

        GlobalKey<NavigatorState> _navigationKey = new NavigatorKey();

        public PageController Controller => _pageController;


        public HomeState(AlbumWidget album) 
        {  
            _album = album;
        }

        public override void initState() 
        {
            base.initState();
            _pageController = new PageController();
        }

        public override void dispose()
        {
            base.dispose();
            _pageController.dispose();
        }

        public override Widget build(BuildContext context) =>
            new Navigator(
                initialRoute : "/",
                onGenerateRoute : settings => new MaterialPageRoute(
                    builder : _album.Build,
                    settings : settings
                )
            );
    }
}