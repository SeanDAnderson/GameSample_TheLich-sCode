using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//PLAYER ARM CONTROLLER
//Used to manage the rotation and sprite of the Player Avatar's Attack Arm.
//The arm aims in an arch generally (but not exactly) toward the mouse cursor. This is to avoid the visual weirdness of a 'turret arm'
//and match the other limited animation sprites.
public class ControllerPlayerArm : MonoBehaviour {

    //Declarations & Initializations
    #region
    [Header ("Sprites")]
    [SerializeField] protected Sprite armExtendedSprite;
    [SerializeField] protected Sprite armBentSprite;
    private Vector3 rotation;
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer parentSpriteRenderer;

    [Header("Rotation")]
    [SerializeField][Range(.01f, 1f)] protected float rotationSpeed = .3f;
    private Vector2 inputMouseLocation = new Vector2(4, 4);
    public Vector2 location = new Vector2 (404, 404);
    private Vector2 relativeMousePosition;
    #endregion

    //Runtime Initialization
    //Provides default values if valid sprites are not assigned
    void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        parentSpriteRenderer = transform.parent.GetComponentInParent<SpriteRenderer>();

        if (armExtendedSprite == null)
        {
            armExtendedSprite = spriteRenderer.sprite;
        }
        if (armBentSprite == null)
        {
            armBentSprite = spriteRenderer.sprite;
        }

    }
	
    //Unity Update
    //Update functionality broken into individual methods for readability
    //See individual methods for details
	void Update () {

        //Only updates if the game is not current paused
        if (ControllerGame.IsPaused == false) {
            UpdateLocation();
            UpdateRotation();
        }
    }

    //UpdateLocation
    //Updates teh location for both the arm and the mouse cursor
    private void UpdateLocation()
    {
        //Getting Mouse Location (mouseLoc) and converting screen position to world position
        inputMouseLocation = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));

        //Getting arm location (armLoc) and converting from relative position to world position
        location = new Vector2(transform.TransformPoint(0, 0, 0).x, transform.TransformPoint(0, 0, 0).y);
    }

    //Update Rotation
    //Updates the sprite selection and rotation based on the mouse position relative to the player and the players flipX status.
    private void UpdateRotation()
    {
        //ANGLE SELECTOR
        //Selects an angle for the arm based on mouse position relative to the arm.
        relativeMousePosition = inputMouseLocation - location;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(Mathf.Atan2(relativeMousePosition.y, relativeMousePosition.x) * Mathf.Rad2Deg + 90, Vector3.forward), rotationSpeed);

        //SPRITE STATE UPDATES
        #region
        //Enabling/Disabling arm sprite to match parent sprite
        if (parentSpriteRenderer.enabled == false)
        {
            spriteRenderer.enabled = false;
        }
        else
        {
            spriteRenderer.enabled = true;
        }

        //Flipping arm sprite to match parent sprite
        if (parentSpriteRenderer.flipX == true)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
        #endregion

        //SPRITE SELECTOR 
        //Uses descrete angles to avoid the strange appearence of a 'turret arm'. 
        //Selects the sprite and angle based upon the flipX status and current angle.
        #region

        float armAngle = 0;

        //IF the mouse curser is within one space of the shoulder on the X-Axis (in the same Column as the PC)
        //We will aim the arm straing up or down
        if ((inputMouseLocation.x < transform.position.x + 1) && (inputMouseLocation.x > transform.position.x - 1))
        {
            //Down
            if (inputMouseLocation.y < location.y)
            {
                armAngle = 0;
            }
            //Up
            else
            {
                armAngle = 180;
            }

        }
        //ELSE IF the mouse curers is further than 1 unit to the right of the character
        //We will aim the arm to the right (angle determined by Y and magnitude)
        else if (inputMouseLocation.x > location.x)
        {
            //Up Right
            if ((Mathf.Pow((inputMouseLocation.x - location.x), 2) < Mathf.Pow((inputMouseLocation.y - location.y), 2)) && ((inputMouseLocation.y - location.y) > 0))
            {
                armAngle = 135;
            }
            //Down Right
            else if ((Mathf.Pow((inputMouseLocation.x - location.x), 2) < Mathf.Pow((inputMouseLocation.y - location.y), 2)) && ((inputMouseLocation.y - location.y) < 0))
            {
                armAngle = 45;
            }
            //Middle Right
            else
            {
                armAngle = 90;
            }

        }
        //ELSE IF the mouse curser is further than 1 unit to the left of the character
        //We will aim the arm to the left (angle determined by y & magnitude)
        else if (inputMouseLocation.x < transform.position.x)
        {

            //Up Left
            if ((Mathf.Pow((inputMouseLocation.x - location.x), 2) < Mathf.Pow((inputMouseLocation.y - location.y), 2)) && ((inputMouseLocation.y - location.y) > 0))
            {
                armAngle = 136;
            }
            //Down Left
            else if ((Mathf.Pow((inputMouseLocation.x - location.x), 2) < Mathf.Pow((inputMouseLocation.y - location.y), 2)) && ((inputMouseLocation.y - location.y) < 0))
            {
                armAngle = 315;
            }
            //Middle Left
            else
            {
                armAngle = 181;
            }
        }
        //Error Signaling, arm turns Magenta
        else
        {
            spriteRenderer.color = Color.magenta;
        }
        
        #endregion


        //Selects a sprite based on the angle of the arm and parent facing
        #region
        
        if (parentSpriteRenderer.flipX == false)
        {
            switch ((int)armAngle)
            {
                case 0:
                case 45:
                case 90:
                case 135:
                case 180:
                case 315:
                    spriteRenderer.sprite = armExtendedSprite;
                    spriteRenderer.flipX = false;
                    spriteRenderer.flipY = false;
                    break;
                case 136:
                case 181:
                    spriteRenderer.sprite = armBentSprite;
                    spriteRenderer.flipX = false;
                    spriteRenderer.flipY = false;
                    break;
            }
        }
        else
        {
            switch ((int)armAngle)
            {
                case 0:
                case 45:
                case 180:
                case 315:
                    spriteRenderer.sprite = armExtendedSprite;
                    spriteRenderer.flipX = true;
                    spriteRenderer.flipY = false;
                    break;
                case 136:
                    armAngle = 225;
                    spriteRenderer.sprite = armExtendedSprite;
                    spriteRenderer.flipX = true;
                    spriteRenderer.flipY = false;
                    break;
                case 181:
                    armAngle = 270;
                    spriteRenderer.sprite = armExtendedSprite;
                    spriteRenderer.flipX = true;
                    spriteRenderer.flipY = false;
                    break;
                case 90:
                    armAngle = 180;
                    spriteRenderer.sprite = armBentSprite;
                    spriteRenderer.flipX = true;
                    spriteRenderer.flipY = false;
                    break;

                case 135:
                    armAngle = 225;
                    spriteRenderer.sprite = armBentSprite;
                    spriteRenderer.flipX = true;
                    spriteRenderer.flipY = false;
                    break;
            }
        }

        transform.rotation = Quaternion.Euler(0, 0, armAngle);

        #endregion


        //Color Matching
        //Makes sure the arm is the same color as the parent
        spriteRenderer.color = parentSpriteRenderer.color;


    }
}
