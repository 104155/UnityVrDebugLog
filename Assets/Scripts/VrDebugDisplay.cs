using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class VrDebugDisplay : MonoBehaviour
{
    private Dictionary<string, string> _debugLogs = new Dictionary<string, string>();
    [SerializeField]
    private Text _display;

    ////TESTING////////////////////////////////////////////////
    //[SerializeField]
    //public int _counter = 0;
    //[SerializeField]
    //public OVRInput.Controller _activeController;
    //[SerializeField]
    //public OVRInput.Button _mapButtonInput;

    //private void Update() {
    //    Debug.Log("time:" + Time.time);
    //    Debug.Log(gameObject.name);
    //    if (OVRInput.GetDown(_mapButtonInput, _activeController)) {
    //        _counter++;
    //        Debug.Log("Hello World" + _counter);
    //    }
    //}

    private void OnEnable() {
        Application.logMessageReceived += HandleLog;
    }

    private void OnDisable() {
        Application.logMessageReceived -= HandleLog;
    }

    public void HandleLog(string _logString, string _stackTrace, LogType _type) {
        if(_type == LogType.Log) {
            string[] _splitString = _logString.Split(char.Parse(":"));
            string _debugKey = _splitString[0];
            string _debugValue = _splitString.Length > 1 ? _splitString[1] : "";

            if(_debugLogs.ContainsKey(_debugKey)) {
                _debugLogs[_debugKey] = _debugValue;
            } else {
                _debugLogs.Add(_debugKey, _debugValue);
            }
        }

        string _displayText = "";
        foreach(KeyValuePair<string, string> _log in _debugLogs) {
            if(_log.Value == "") {
                _displayText += _log.Key + "\n";
            } else {
                _displayText += _log.Key + ": " + _log.Value + "\n";
            }

            //keep only what is visible on display
            string _finalDisplayText = ""; 
            foreach (string _line in TakeLastLines(_displayText, 12)) {
                _finalDisplayText += _line + "\n";
            }

            _display.text = _finalDisplayText;
        }
    }

    private static List<string> TakeLastLines(string text, int count) {
        List<string> lines = new List<string>();
        Match match = Regex.Match(text, "^.*$", RegexOptions.Multiline | RegexOptions.RightToLeft);

        while (match.Success && lines.Count < count) {
            lines.Insert(0, match.Value);
            match = match.NextMatch();
        }

        return lines;
    }
}
