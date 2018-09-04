using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Controller: Input
//Handles all input managment
//Primarily supports the ControllerPlayer and ControllerGame classes
//Standard Unity Input is not used directly because of the number of values that have to be converted or can be altered.
//I.e. The need for world mouse position instead of screen position and Y axis being used for jumping.
//
//JUMPING
//The Player can jump with either the Jump key (defaults to SPACEBAR) or 'UP' on the Y Axis (W, UPARROW, or Gamepad UP)
//Checks for both clicking and holding the button.
public static class ControllerInput {

    //Declarations & Initializations
    #region
    //Keyboard & Gamepad Values
    public static float X { get; set; }
    public static float Y { get; set; }
    public static bool JumpHold { get; set; }
    public static bool JumpClick { get; set; }

    //MouseValues
    public static Vector2 MousePostion { get; set; }
    public static bool MouseLeftClick { get; set; }
    public static bool MouseRightClick { get; set; }
    public static bool MouseLeftHold { get; set; }
    public static bool MouseRightHold { get; set; }
    public static bool MouseLeftLift { get; set; }
    public static bool MouseRightLift { get; set; }
    #endregion

    //UpdateKeys
    //Updates inputs form both Keyboard and GamePad inputs.+
    //TODO: Finish GamePad support.
    private static void UpdateKeys()
    {
        X = Input.GetAxisRaw("Horizontal");
        Y = Input.GetAxisRaw("Vertical");

        //Checks the old jump value against current Y to see if Y has been 'clicked' this update.
        //MUST remain before JumpHold is updated to use the previous value of JumpHold.
        if (((JumpHold == false) && (Y > 0)) || (Input.GetButtonDown("Jump")))
        {
            JumpClick = true;
        }
        else
        {
            JumpClick = false;
        }

        //Checking is a Jump input is being held
        if ((Y > 0) || (Input.GetButton("Jump"))){
            JumpHold = true;
        }
        else
        {
            JumpHold = false;
        }

        if (Input.GetKeyDown(KeyCode.SysReq))
        {
            Debug.Log("Attempting screenshot.");
            ScreenCapture.CaptureScreenshot("screenshots/" + System.DateTime.Now.ToString("yyyy_MM_dd_HHMMss") + ".png");
        }
    }


    //UpdateMouse
    //Updates the mouse cursor position in the game world and checks the button status of the mouse and (TODO) the gamepad triggers.
    private static void UpdateMouse()
    {
        MousePostion = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));

        //MouseButton & Controller Triggers
        //TODO: Controller Logic
        MouseLeftHold = Input.GetMouseButton(0);
        MouseLeftClick = Input.GetMouseButtonDown(0);
        MouseLeftLift = Input.GetMouseButtonUp(0);
        MouseRightHold = Input.GetMouseButton(1);
        MouseRightClick = Input.GetMouseButtonDown(1);
        MouseRightLift = Input.GetMouseButtonUp(1);
    }

    //Updates the User Inputs
    //Called by ControllerGame
    public static void UpdateInput()
    {
        UpdateKeys();
        UpdateMouse();
    }

}
