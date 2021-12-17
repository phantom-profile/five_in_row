using UnityEngine;
using System.Collections;
using static CsGlobals;



public class CameraControl : MonoBehaviour {
    Vector3 hit_position = Vector3.zero;
    Vector3 current_position = Vector3.zero;
    Vector3 camera_position = Vector3.zero;
    float z = 0.0f;

    private float left_limit;
    private float right_limit;
    private float bottom_limit;
    private float upper_limit;
    
    
    // Use this for initialization
    void Start () {
        
        left_limit   = (float) CsGlobals.left_limit;
        right_limit  = (float) CsGlobals.right_limit;
        bottom_limit = (float) CsGlobals.bottom_limit;
        upper_limit  = (float) CsGlobals.upper_limit;
    }
    
    void Update(){
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
            (Mathf.Clamp(transform.position.x, left_limit, right_limit),
             Mathf.Clamp(transform.position.y, bottom_limit, upper_limit),
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