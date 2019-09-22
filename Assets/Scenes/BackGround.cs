using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour {

    public float bgSpeed = 1f;
    float move;

    Renderer renderer;

	// Use this for initialization
	void Start () {

        renderer = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
        move += Time.deltaTime * bgSpeed;
        renderer.material.mainTextureOffset = new Vector2(move, 0);
	}
}
