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
		Api.On ("removeUser", this.OnRemoveUser);
	}

	public void OnUpdate(SocketIOEvent e) {
		string id = "";
		e.data.GetField (ref id, "id");
		Item itemToChange = this._items.Find (item => item.id == id);

		this.UpdateItem (itemToChange, e, smooth: true);
	}

	public void OnSetCurrentUser(SocketIOEvent e) {
		Transform userTransform = GameObject.Instantiate (this.currentUserPrefab);
		string id = "";
		e.data.GetField (ref id, "id");

		Item item = new Item (id, "user", userTransform);
		this.UpdateItem (item, e, smooth: false);
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
		this.UpdateItem(item, e, smooth: false);
		this._items.Add (item);
	}

	public void OnRemoveUser(SocketIOEvent e) {
		string type = "";
		e.data.GetField(ref type, "type");
		if (type != "user") {
			return;
		}

		string id = "";
		e.data.GetField(ref id, "id");
		Item item = this._items.Find(i => i.id == id);

		Destroy(item.transform.gameObject);
		if (item != null) {
			this._items.Remove(item);
		}
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

	private void UpdateItem(Item item, SocketIOEvent e, bool smooth) {
		Vector3 position = Vector3.zero;
		e.data.GetField (ref position.x, "x");
		e.data.GetField (ref position.z, "y");

		Color color = new Color ();
		e.data.GetField (ref color.r, "r");
		e.data.GetField (ref color.g, "g");
		e.data.GetField (ref color.b, "b");

        if (item.transform.GetComponent<ColoringHelper>().objectToBeRotated) {
            Vector3 newDirection = new Vector3();
            e.data.GetField(ref newDirection.x, "directionX");
            e.data.GetField(ref newDirection.z, "directionY");

            GameObject objectToBeRotated = item.transform.GetComponent<ColoringHelper>().objectToBeRotated;


            if (newDirection.z > 0) {
                // UP
                objectToBeRotated.transform.localRotation = Quaternion.Euler(0, 90, 0);
            } else if (newDirection.z < 0) {
                // DOWN
                objectToBeRotated.transform.localRotation = Quaternion.Euler(0, -90, 0);
            } else if (newDirection.x < 0) {
                // LEFT
                objectToBeRotated.transform.localRotation = Quaternion.Euler(0, 0, 0);
            } else if (newDirection.x > 0) {
                // RIGHT
                objectToBeRotated.transform.localRotation = Quaternion.Euler(0, -180, 0);
            }
        }

        Jumper jumper = item.transform.gameObject.GetComponent<Jumper>();

        if (item.type == "block")
        {
            bool isCarried = false;
            e.data.GetField(ref isCarried, "isCarried");
            if (!isCarried)
            {
                jumper.addPosition = Vector3.zero;
            }
        }

        if (jumper != null && smooth)
        {
            jumper.MoveTo(position);
        }
        else
        {
            item.transform.position = position;
        }

		item.transform.gameObject.GetComponent<ColoringHelper>().modelRenderer.material.color = color;
        item.transform.gameObject.GetComponentInChildren<MinimapItem>().SetColor( color);

        if (item.type == "user") {
			string carries = null;
			e.data.GetField (ref carries, "carries");

			Player player = item.transform.gameObject.GetComponent<Player> ();

			if (carries != null) {
				Item carriesItem = this._items.Find (i => i.id == carries);
                Jumper j = carriesItem.transform.GetComponent<Jumper>();
                j.addPosition = Vector3.up;

                bool isFirstTime = player._isCarrying == null;
                if (isFirstTime)
                {
                    j.MoveTo(position);
                }
                else
                {
                    j.MoveTo(position, jumper.defaultJumpHeight);
                }

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