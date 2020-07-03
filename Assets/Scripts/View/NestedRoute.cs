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
using RSG;

using Color = Unity.UIWidgets.ui.Color;
using Image = Unity.UIWidgets.widgets.Image;


namespace GozaiNASU.AR.View
{
    // public delegate Widget NestedRouteBuilder (Widget child);

    // public class NestedRoute
    // {
    //     readonly string _name;
    //     readonly List<NestedRoute> _subRoute;
    //     readonly NestedRouteBuilder _builder;

    //     public NestedRoute(
    //         string name,
    //         NestedRouteBuilder builder,
    //         List<NestedRoute> subRoute = null
    //     )
    //     {
    //         _name = name;
    //         _builder = builder;
    //         _subRoute = subRoute;
    //     }

    //     Route BuildRoute(string[] paths, int index) => 
    //         new PageRouteBuilder(
    //             pageBuilder: (_, __, ___) => Build(paths, index)
    //         );

    //     Widget Build(string[] paths, int index)
    //     {
    //         if (index > paths.Length) {
    //             return _builder(null);
    //         }
    //         var route = _subRoute?.FirstOrDefault(r => r._name == paths[index]);
    //         return _builder(route?.Build(paths, index+1));
    //     }

    //     public RouteFactory BuildNestedRoutes(List<NestedRoute> routes) =>
    //         settings => {
    //             var paths = settings.name.Split('/');
    //             if (paths.Length <= 1)
    //             {
    //                 return null;
    //             }
    //             var rooteRoute = routes.FirstOrDefault(r => r._name == paths[1]);
    //             return rooteRoute.BuildRoute(paths, 2);

    //         };
    // }

    public class NestedNavigator : StatelessWidget
    {
        public delegate void RouteCallback(RouteSettings settings);
        public delegate void PopCallback();

        GlobalKey<NavigatorState> _navigatorKey;
        string _initialRoute;
        Dictionary<string, WidgetBuilder> _routes;

        RouteCallback _onGenerateRoute;
        PopCallback _onWillPop;


        public NestedNavigator(
            GlobalKey<NavigatorState> navigatorKey,
            string initialRoute,
            Dictionary<string, WidgetBuilder> routes,
            RouteCallback onGenerateRoute = null,
            PopCallback onWillPop = null
        )
        {
            _navigatorKey = navigatorKey;
            _initialRoute = initialRoute;
            _routes = routes;
            _onGenerateRoute = onGenerateRoute;
            _onWillPop = onWillPop;
        }

        public override Widget build(BuildContext context) =>
            new WillPopScope(
                child : new Navigator(
                    key : _navigatorKey,
                    initialRoute : _initialRoute,
                    onGenerateRoute : routeSettings => {
                        _onGenerateRoute?.Invoke(routeSettings);

                        var builder = _routes[routeSettings.name];
                        if (routeSettings.isInitialRoute) 
                        {
                            return new PageRouteBuilder(
                                pageBuilder : (cont, _, __) => builder(cont),
                                settings : routeSettings
                            );
                        }
                        return new MaterialPageRoute(
                            builder : builder,
                            settings : routeSettings
                        );
                    }
                )
            );
    }
}