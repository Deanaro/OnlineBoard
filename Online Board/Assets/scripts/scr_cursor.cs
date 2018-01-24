using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class scr_cursor : NetworkBehaviour {

    [SyncVar]
    public string pname = "player";

    [SyncVar]
    public Color pcolor = Color.green;
    bool clicked = false;
    bool rclicked = false;
    bool rclicked2 = false;
    GameObject attachedObject;
    Vector2 mouseStart;
    Vector2 objectStart;
    public GameObject spriteSelector;
    public GameObject Spriteobject;
    public UnityEngine.Object[] spriteList;
    GameObject[] selection = new GameObject[40];
    float sprSize = 2;
    public LayerMask Add;
    public LayerMask Move;
    int numOfImages;

    void Start()
    {
        if (isLocalPlayer)
        {
            spriteList = Resources.LoadAll("", typeof(Sprite));
            numOfImages = spriteList.Length;
            Cursor.visible = false;
        }
        GetComponent<SpriteRenderer>().color = pcolor;

    }
    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer && Application.isFocused)
        {
            Vector3 pos = Input.mousePosition;
            pos.z = 20;
            pos = Camera.main.ScreenToWorldPoint(pos);
            transform.position = pos;

            if (Input.GetMouseButtonDown(1))
            {


                if (rclicked)
                {
                    rclickunfocus();
                }


                int i = 0;
                rclicked = true;
                foreach (var spr in spriteList)
                {
                    selection[i] = Instantiate(spriteSelector, new Vector3(sprSize * i + Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0), Quaternion.identity);
                    selection[i].GetComponent<SpriteRenderer>().sprite = spr as Sprite;
                    i++;
                }
            }

            

            if (rclicked)
            {

                for (int ii = 0; ii < numOfImages; ii++)
                {

                    selection[ii].transform.Translate(new Vector3(Input.GetAxis("Mouse ScrollWheel")*5, 0, 0));
                }
            }


            if (Input.GetMouseButtonDown(0))
            {
                    if (rclicked)
                    {
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        RaycastHit hit;
                        if (Physics.Raycast(ray, out hit, 100, Add))
                        {
                            string SpriteToUse = hit.transform.gameObject.GetComponent<SpriteRenderer>().sprite.name;
                            CmdSpawn(hit.transform.position, SpriteToUse);
                        }
                        rclickunfocus();
                    }
                    else
                    {
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        RaycastHit hit;
                        if (Physics.Raycast(ray, out hit, 100, Move))
                        {
                            attachedObject = hit.transform.gameObject;

                            if (attachedObject.GetComponent<objectbehaiviour>().attached == false)
                            {
                                clicked = true;
                                attachedObject.GetComponent<objectbehaiviour>().attached = true;
                                mouseStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                                objectStart = attachedObject.transform.position;
                                Debug.Log(hit.transform.gameObject.name);
                            }
                        }
                    }
                    rclicked = false;
                
            }



            if (Input.GetMouseButtonUp(0) && clicked)
            {
                attachedObject.GetComponent<objectbehaiviour>().attached = false;
                //Cmdmoveobject(attachedObject.transform.position, attachedObject);
                clicked = false;
            }

            if (clicked)

            {

                Vector2 mouseChange = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x - mouseStart.x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y - mouseStart.y);
                Cmdmoveobject(objectStart + mouseChange, attachedObject);

                CmdScale(Input.GetAxis("Mouse ScrollWheel"), attachedObject);
                

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    CmdDestroy(attachedObject);
                    clicked = false;
                }
            }

        }
    }
    

    private void OnDestroy()
    {
        if (isLocalPlayer)
        { Cursor.visible = true; }
    }
    void OnGUI()
    {
        
        Vector3 pos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y,0));
        GUI.color = Color.black;
        GUI.Label(new Rect(pos.x - 10, -pos.y + Camera.main.pixelHeight - 20, 100, 20), pname);
    }

    [Command]
    void Cmdmoveobject(Vector3 pos, GameObject obj)
    {
        obj.transform.position = pos;
    }

    [Command]
    void CmdSpawn(Vector3 pos, string SpriteToUse)
    {
        Debug.Log(SpriteToUse);
        Debug.Log("hit?");
        GameObject go = Instantiate(Spriteobject, pos, Quaternion.identity);
        go.GetComponent<objectbehaiviour>().spriteNamenetworked = SpriteToUse;
        NetworkServer.Spawn(go);
    }

    [Command]
    void CmdScale(float scale, GameObject ob)
    {
        ob.GetComponent<objectbehaiviour>().Scale += scale;
        ob.GetComponent<objectbehaiviour>().Scale = Mathf.Clamp((ob.GetComponent<objectbehaiviour>().Scale),0.1f,float.MaxValue);
    }

    [Command]
    void CmdDestroy(GameObject ob)
    {
        NetworkServer.Destroy(ob);
    }

    void rclickunfocus()
    {
        foreach (GameObject o in selection)
        {
            Destroy(o);
        }
        Array.Clear(selection, 0, selection.Length);
    }
}
