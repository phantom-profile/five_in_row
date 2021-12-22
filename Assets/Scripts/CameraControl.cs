using UnityEngine;
using System.Collections;
using static CsGlobals;



public class CameraControl : MonoBehaviour {
    Vector3 hit_position = Vector3.zero;
    Vector3 current_position = Vector3.zero;
    Vector3 camera_position = Vector3.zero;
    float z = 0.0f;

    private Vector3 leftBottomTilemapLimit;
    private Vector3 rigthUpperTilemapLimit;
    
    // Use this for initialization
    void Start () {
        leftBottomTilemapLimit = new Vector3((float) CsGlobals.leftLimit, (float) CsGlobals.bottomLimit, 0);
        rigthUpperTilemapLimit = new Vector3((float) CsGlobals.rightLimit, (float) CsGlobals.upperLimit, 0);
    }
    
    void Update(){
        
		Vector3 direction = transform.position;
		if (Input.GetKeyDown(KeyCode.W))
        {
            direction.y = direction.y + 1;
        }
		if (Input.GetKeyDown(KeyCode.A))
        {
            direction.y = direction.x - 1;
        }
		if (Input.GetKeyDown(KeyCode.S))
        {
            direction.y = direction.y - 1;
        }
		if (Input.GetKeyDown(KeyCode.D))
        {
            direction.y = direction.x + 1;
        }




		if(Input.GetMouseButtonDown(1)){
            hit_position = Input.mousePosition;
            camera_position = transform.position;
            
        }

        if (Input.GetMouseButton(1))
        {
            current_position = Input.mousePosition;
            LeftMouseDrag();
        }
        
        transform.position = new Vector3
            (Mathf.Clamp(transform.position.x, leftBottomTilemapLimit.x, rigthUpperTilemapLimit.x),
             Mathf.Clamp(transform.position.y, leftBottomTilemapLimit.y, rigthUpperTilemapLimit.y),
             transform.position.z);
    }
    
    void LeftMouseDrag(){
        // From the Unity3D docs: "The z position is in world units from the camera."  In my case I'm using the y-axis as height
        // with my camera facing back down the y-axis.  You can ignore this when the camera is orthograhic.
        current_position.z = hit_position.z = camera_position.y;
        
        // Get direction of movement.  (Note: Don't normalize, the magnitude of change is going to be Vector3.Distance(current_position-hit_position)
        // anyways.  
        Vector3 direction = Camera.main.ScreenToWorldPoint(current_position) - Camera.main.ScreenToWorldPoint(hit_position);
        
        // Invert direction to that terrain appears to move with the mouse.
        direction = direction * -1;
        
        Vector3 position = camera_position + direction;
        
        transform.position = position;
    }
}