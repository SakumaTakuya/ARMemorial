using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Events;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.engine;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.material;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

using Color = Unity.UIWidgets.ui.Color;


namespace GozaiNASU.AR.View
{
    public class MainPageWidget : WidgetBehaviour
    {
        [SerializeField] WidgetBehaviour _cameraWidget = default;
        [SerializeField] WidgetBehaviour _homeWidget = default;
        [SerializeField] WidgetBehaviour _settingsWidget = default;

        public override Widget Build(BuildContext context) =>
            new MainPage(
                _cameraWidget.Build, 
                _homeWidget.Build, 
                _settingsWidget.Build);
    }

    public class MainPage : StatefulWidget
    {
        WidgetBuilder _camera;
        WidgetBuilder _home;
        WidgetBuilder _settings;

        public MainPage( 
            WidgetBuilder camera,
            WidgetBuilder home,
            WidgetBuilder settings,
            Key key=null) : base(key)
        {
            _camera = camera;
            _home = home;
            _settings = settings;
        }

        public override State createState() => 
            new MainPageState(
                _camera,
                _home,
                _settings);  
    }

    public class MainPageState : State<MainPage>
    {
        int _currentIndex;
        PageController _pageController;
        WidgetBuilder _camera;

        List<WidgetBuilder> _pages;

        public MainPageState(
            WidgetBuilder camera,
            WidgetBuilder home,
            WidgetBuilder settings)
        {
            _camera = camera;
            _pages = new List<WidgetBuilder>{
                home, settings
            };
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
            new Scaffold(
                body: new PageView(
                    controller : _pageController,
                    onPageChanged : id => setState(() => _currentIndex = id),
                    children : _pages
                                    .Select(b => b(context))
                                    .ToList()
                ),
                bottomNavigationBar : new Theme(
                    data : Theme.of(context),
                    child : new BottomNavigationBar(
                        items: new List<BottomNavigationBarItem> {
                            new BottomNavigationBarItem(
                                icon : new Icon(Icons.home), 
                                title : new Text("Home")
                            ),
                            new BottomNavigationBarItem(
                                icon : new Icon(Icons.settings), 
                                title : new Text("Settings")
                            ),
                        },
                        showUnselectedLabels : true,
                        currentIndex : _currentIndex,
                        onTap : id => _pageController.animateToPage(
                            id,
                            duration : System.TimeSpan.FromMilliseconds(300),
                            curve : Curves.ease
                        ),
                        type : BottomNavigationBarType.fix
                    )
                ),
                floatingActionButtonLocation: FloatingActionButtonLocation.centerDocked,
                floatingActionButton : new FloatingActionButton(
                    child : new Icon(Icons.camera_alt),
                    onPressed : () => Navigator
                                        .of(context)
                                        .push(
                                            new PageRouteBuilder(
                                                pageBuilder : (cont, _, __) => _camera(cont),
                                                transitionsBuilder : (_, animation, __, child) => {
                                                    var begin = new Offset(0f, 1f);
                                                    var end = Offset.zero;
                                                    var curve = Curves.ease;
                                                    var tween = new OffsetTween(begin: begin, end : end)
                                                                        .chain(new CurveTween(curve));
                                                    return new SlideTransition(
                                                        position: animation.drive(tween),
                                                        child : child);
                                                }
                                            )
                                        )
                )
            );
    }
}