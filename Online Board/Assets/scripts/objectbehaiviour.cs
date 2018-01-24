using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class objectbehaiviour : NetworkBehaviour {

    [SyncVar]
    public bool attached = false;

    [SyncVar]
    public string spriteName = "empty";

    [SyncVar]
    public float Scale = 1;

    public string spriteNamenetworked = "empty";

    private void Update()
    {
         
            if (isServer)
            {
                spriteName = spriteNamenetworked;
            }
            transform.localScale = new Vector3(Scale,Scale,0);
            GetComponent<SpriteRenderer>().sprite = Resources.Load(spriteName, typeof(Sprite)) as Sprite;
            Destroy(GetComponent<BoxCollider>());
            this.gameObject.AddComponent<BoxCollider>();
        
    }


}
