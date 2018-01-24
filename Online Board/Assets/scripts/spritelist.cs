using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spritelist : MonoBehaviour {


    public Object[] spriteList;
    
	// Use this for initialization
	void Start () {
        spriteList = Resources.LoadAll("", typeof(Sprite));
        foreach (var spr in spriteList)
        {
            Debug.Log(spr.name);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
