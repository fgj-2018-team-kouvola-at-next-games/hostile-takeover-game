using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class Game : MonoBehaviour {

	public Transform currentUserPrefab;
	public Transform otherUserPrefab;
	public Transform blockPrefab;
   
	private List<Item> _items = new List<Item>();

    // Use this for initialization
    void Start () {
		Api.On ("setCurrentUser", this.OnSetCurrentUser);
		Api.On ("update", this.OnUpdate);
		Api.On ("initItem", this.OnInitItem);
	}

	public void OnUpdate(SocketIOEvent e) {
		string id = "";
		e.data.GetField (ref id, "id");
		Item itemToChange = this._items.Find (item => item.id == id);

		this.UpdateItem (itemToChange, e);
	}

	public void OnSetCurrentUser(SocketIOEvent e) {
		Transform userTransform = GameObject.Instantiate (this.currentUserPrefab);
		string id = "";
		e.data.GetField (ref id, "id");

		Item item = new Item (id, "user", userTransform);
		this.UpdateItem (item, e);
		this._items.Add (item);
	}

	public void OnInitItem(SocketIOEvent e) {
		string id = "";
		e.data.GetField (ref id, "id");

		bool alreadyHasItem = this._items.Find (i => i.id == id) != null;
		if (alreadyHasItem) {
			return;
		}

		string type = "";
		e.data.GetField (ref type, "type");
		Transform itemToInstantiate = this.ItemTypeToTransform (type);
		Transform transform = GameObject.Instantiate (itemToInstantiate);

		Item item = new Item (id, type, transform);
		this.UpdateItem(item, e);
		this._items.Add (item);
	}

	private Transform ItemTypeToTransform(string type) {
		switch (type) {
		case "user":
			return this.otherUserPrefab;
		case "block":
			return this.blockPrefab;
		}

		return null;
	}

	private void UpdateItem(Item item, SocketIOEvent e) {
		Vector3 position = Vector3.zero;
		e.data.GetField (ref position.x, "x");
		e.data.GetField (ref position.z, "y");

		Color color = new Color ();
		e.data.GetField (ref color.r, "r");
		e.data.GetField (ref color.g, "g");
		e.data.GetField (ref color.b, "b");

		item.transform.position = position;
		item.transform.gameObject.GetComponent<PlayerHelper>().modelRenderer.material.color = color;

		if (item.type == "user") {
			string carries = null;
			e.data.GetField (ref carries, "carries");

			Player player = item.transform.gameObject.GetComponent<Player> ();

			if (carries != null) {
				Item carriesItem = this._items.Find (i => i.id == carries);
				carriesItem.transform.position = item.transform.position + Vector3.up;
				if (player) {
					player.SetCarrying (carriesItem.transform.GetComponent<Block> ());
				}
			} else {
				if (player) {
					player.SetCarrying (null);
				}
			}
		}
	}
}

public class Item {
	public string id;
	public string type;
	public Transform transform;

	public Item(string newId, string newType, Transform newTransform) {
		this.id = newId;
		this.type = newType;
		this.transform = newTransform;
		newTransform.gameObject.name = id;
	}
}