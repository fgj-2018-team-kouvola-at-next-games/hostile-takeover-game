using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using SocketIO;
using System;

public class WebApi : MonoBehaviour
{
    private Dictionary<string, List<Action<SocketIOEvent>>> handlers = new Dictionary<string, List<Action<SocketIOEvent>>>();

    // Start is called before the first frame update
    public void OnEvent(string e)
    {
        string[] parts = e.Split('—');
        string playerName = parts[0];
        string data = parts[1];
        JSONObject parsed = new JSONObject(data);
        SocketIOEvent evv = new SocketIOEvent(playerName, parsed);
        handlers[playerName].ForEach(action => action(evv));
    }

    public void On(string ev, Action<SocketIOEvent> callback)
    {
        if (!handlers.ContainsKey(ev))
        {
            handlers[ev] = new List<Action<SocketIOEvent>>();
        }
        handlers[ev].Add(callback);
        BindOn(ev);
    }

    public void Emit(string eventName, JSONObject data)
    {
        Emit(eventName, data.ToString());
    }

    [DllImport("__Internal")]
    private static extern int Emit(string eventName, string data);

    [DllImport("__Internal")]
    private static extern int BindOn(string eventName);
}
