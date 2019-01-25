using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;


public class Api {
	private static SocketIOComponent _socket;

	public static SocketIOComponent instance {
		get {
			if (Api._socket == null) {
				GameObject go = GameObject.Find("SocketIO");
				Api._socket = go.GetComponent<SocketIOComponent>();
			}

			return Api._socket;
		}
	}

	public static void SendMoveMessage(Vector2 delta) {
		JSONObject message = new JSONObject (JSONObject.Type.OBJECT);
		message.AddField ("x", delta.x);
		message.AddField ("y", delta.y);
		message.AddField ("player", "p1");

		Api.instance.Emit ("move", message);
	}
}
