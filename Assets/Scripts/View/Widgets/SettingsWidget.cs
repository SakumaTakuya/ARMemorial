using System.Collections.Generic;

using UnityEngine;
using Unity.UIWidgets.engine;
using Unity.UIWidgets.material;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

using Color = Unity.UIWidgets.ui.Color;


namespace GozaiNASU.AR.View
{
    public delegate bool SwitchCallback(bool isActive);
    public class SettingsWidget : WidgetBehaviour
    {
        public bool CanNotificate { get; private set; }

        public override Widget Build(BuildContext context = null)
        {
            var lst = new Widget[]{
                new StateTile("Notification", e => CanNotificate = e, CanNotificate),
                new ListTile(
                    title : new Text("Terms of Service"),
                    isThreeLine : false,
                    onTap : () => {}
                ),
                new ListTile(
                    title : new Text("Help"),
                    isThreeLine : false,
                    onTap : () => {}
                ),
            };

            return new Scaffold(
                appBar : new AppBar(
                    title : new Text("Settings")
                ),
                body : ListView.seperated(
                    itemCount : lst.Length,
                    separatorBuilder : (cont, id) => new Divider(),
                    itemBuilder : (cont, id) => lst[id],
                    padding : EdgeInsets.all(0)
                )
            );
        }
    }

    class StateTile : StatefulWidget
    {
        string _title;
        SwitchCallback _onChanged;

        bool _initialState;

        public StateTile(string title, SwitchCallback onChanged = null, bool initialState = false)
        {
            _title = title;
            _onChanged = onChanged;
            _initialState = initialState;
        }

        public override State createState() => new SwitchState(_title, _onChanged, _initialState);
    }

    class SwitchState : State<StateTile>
    {
        string _title;
        private bool _isActive;

        private SwitchCallback _onChanged;

        public SwitchState(string title, SwitchCallback onChanged, bool initialState) 
        {
            _title = title;
            _onChanged = onChanged;
            _isActive = initialState;
        }

        void OnChange(bool? isActive)
        {
            var e = isActive ?? false;
            setState(() => _isActive = e);
            _onChanged?.Invoke(e);
        }

        public override Widget build(BuildContext context) =>
            new ListTile(
                title : new Text(_title),
                trailing : new Switch(
                    value : _isActive,
                    onChanged : OnChange
                ),
                isThreeLine : false
            );
    }
}