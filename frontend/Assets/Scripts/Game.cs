using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class Game : MonoBehaviour {

	public Transform currentUserPrefab;
	public Transform otherUserPrefab;

	private List<Item> _items = new List<Item>();

	// Use this for initialization
	void Start () {
		Debug.Log ("jee");
		Api.instance.On ("setCurrentUser", this.OnSetCurrentUser);
		Api.instance.On ("updateUser", this.OnUpdateUser);
		Api.instance.On ("initItem", this.OnInitItem);
	}

	public void OnUpdateUser(SocketIOEvent e) {
		string id = "";
		e.data.GetField (ref id, "id");
		Item itemToChange = this._items.Find (item => item.id == id);

		Vector3 newPosition = Vector3.zero;
		e.data.GetField (ref newPosition.x, "x");
		e.data.GetField (ref newPosition.z, "y");

		itemToChange.transform.position = newPosition;
	}

	public void OnSetCurrentUser(SocketIOEvent e) {
		Transform userTransform = GameObject.Instantiate (this.currentUserPrefab);
		string id = "";
		e.data.GetField (ref id, "id");
		this._items.Add (new Item (id, userTransform));
	}

	public void OnInitItem(SocketIOEvent e) {
		
	}
}

public class Item {
	public string id;
	public Transform transform;

	public Item(string newId, Transform newTransform) {
		this.id = newId;
		this.transform = newTransform;
	}
}