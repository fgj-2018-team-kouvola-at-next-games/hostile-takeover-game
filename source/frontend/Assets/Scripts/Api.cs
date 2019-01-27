using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using UnityEngine.Networking;
using System;

public class Api {
	private static SocketIOComponent _socket;
    private static WebApi _webApi;

    public static SocketIOComponent sInstance
    {
        get
        {
            if (Api._socket == null)
            {
                GameObject go = GameObject.Find("SocketIO");
                Api._socket = go.GetComponent<SocketIOComponent>();
            }

            return Api._socket;
        }
    }
    public static WebApi wInstance
    {
        get
        {
            if (Api._webApi == null)
            {
                GameObject go = GameObject.Find("WebApi");
                Api._webApi = go.GetComponent<WebApi>();
            }

            return Api._webApi;
        }
    }

    public static void SendMoveMessage(Vector2 delta) {
		JSONObject message = new JSONObject (JSONObject.Type.OBJECT);
		message.AddField ("x", delta.x);
		message.AddField ("y", delta.y);

		Api.Emit ("move", message);
	}

	public static void SendPickMessage(string blockId) {
		JSONObject message = new JSONObject (JSONObject.Type.OBJECT);
		message.AddField ("blockId", blockId);

		Api.Emit ("pick", message);
	}

	public static void SendPlaceMessage() {
		Api.Emit ("place");
	}

    public static void Emit(string eventName)
    {
        Api.Emit(eventName, new JSONObject("{}"));
    }

    public static void Emit(string eventName, JSONObject data)
    {
        if (Application.isEditor)
        {
            Api.sInstance.Emit(eventName, data);
        }
        else
        {
            Api.wInstance.Emit(eventName, data);
        }
    }

    public static void On(string eventName, Action<SocketIOEvent> action)
    {
        if (Application.isEditor)
        {
            Api.sInstance.On(eventName, action);
        }
        else
        {
            Api.wInstance.On(eventName, action);
        }
    }
}
