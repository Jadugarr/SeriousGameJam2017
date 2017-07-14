using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour {


    

    public int horizontalRayCount = 10;
    public int verticalRayCount = 10;

    //Private Stuff
    const float skinWidth = 0.2f;

    private float horizontalSpacing;
    private float verticalSpacing;

    private BoxCollider2D boxCollider2D;
    private RaycastOrigins raycastOrigins;


    // Update is called once per frame
    void Update () {
		
	}

    private void UpdateCastOrigins()
    {
        Bounds bounds = boxCollider2D.bounds;
        bounds.Expand(skinWidth * -2f);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    private void CalculateRaySpacing()
    {
        Bounds bounds = boxCollider2D.bounds;
        bounds.Expand(skinWidth * -2f);

        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);
        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);

        horizontalSpacing = bounds.size.x / horizontalRayCount;
        verticalSpacing = bounds.size.y / verticalRayCount;
    }



















    struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }

}
