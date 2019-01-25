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

		Api.instance.Emit ("move", message);
	}

	public static void SendPickMessage(string blockId) {
		JSONObject message = new JSONObject (JSONObject.Type.OBJECT);
		message.AddField ("blockId", blockId);

		Api.instance.Emit ("pick", message);
	}

	public static void SendPlaceMessage() {
		Api.instance.Emit ("place");
	}


}
