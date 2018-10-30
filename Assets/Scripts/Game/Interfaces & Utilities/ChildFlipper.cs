using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//CHILDFLIPPER
//Matches the flipX and flipY states of a child to the parent object. 
public class ChildFlipper : MonoBehaviour {

    //Declarations and Initializations
    #region
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer parentSpriteRenderer;
    [SerializeField] protected GameObject parent;
    #endregion

    //Runtime initializations
    //If no parent object is declared in Unity the code attempts to find a renderer itself.
    private void Start()
    {
        
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        if (parent == null)
        {
            parentSpriteRenderer = gameObject.GetComponentInParent<SpriteRenderer>();
        }
        parentSpriteRenderer = parent.GetComponent<SpriteRenderer>();

    }

    //Updates flip values each cycle to make sure the sprite matches the parent.
    void Update () {
        spriteRenderer.flipX = parentSpriteRenderer.flipX;
        spriteRenderer.flipY = parentSpriteRenderer.flipY;
    }
}
