using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float cameraSpeed;
    [SerializeField] private float zoomSpeed;

    void Update()
    {
        HandleObstaclesInteraction();
        HandleStartEndInteraction();
        HandleFreeCam();
        HandleCameraChange();
        HandleQuit();
    }

    //Handles player changes in obstacles placement
    private void HandleObstaclesInteraction()
    {
        //Left mouse click changes obstacle to normal field or normal field to obstacle
        if (Input.GetMouseButtonDown(0))
        {
            var pressedTile = GetTileIfPressed();
            if(pressedTile)
                pressedTile.InvertType();
        }
    }

    private void HandleQuit()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            #if !UNITY_EDITOR
                Application.Quit();
            #endif
        }
    }

    private void HandleStartEndInteraction()
    {
        //Right click and hold Q or E to set new start and end placements
        if (Input.GetMouseButton(1))
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                var pressedTile = GetTileIfPressed();
                if (pressedTile)
                    GameManager.Instance.mapDriver.SetStartTile(pressedTile.placement);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                var pressedTile = GetTileIfPressed();
                if (pressedTile)
                    GameManager.Instance.mapDriver.SetEndTile(pressedTile.placement);
            }
        }
    }

    //Changes camera mode on click
    private void HandleCameraChange()
    {
        if (Input.GetKeyDown(KeyCode.P))
            GameManager.Instance.cameraDriver.ChangeCameraMode();
    }

    private void HandleFreeCam()
    {
        var camTransform = GameManager.Instance.cameraDriver.gameObject.transform;
        
        //Handle movement with arrows
        var x = Input.GetAxis("Horizontal") * cameraSpeed * Time.deltaTime;
        var z = Input.GetAxis("Vertical") * cameraSpeed * Time.deltaTime;
        camTransform.position += new Vector3(x, 0, z);

        //Handle zoom
        var scroll = Input.GetAxis("Mouse ScrollWheel");
        
        if (scroll == 0.0f || GameManager.Instance.cameraDriver.GetComponent<Camera>().orthographic) 
            return;

        if (camTransform.position.y <= 2f && scroll > 0)
        {
            var pos = camTransform.position;
            pos.y = 2f;
            camTransform.position = pos;
            return;
        }

        var position = camTransform.position;
        position += camTransform.forward * (scroll * zoomSpeed);
        camTransform.position = position;
    }

    private TileObject GetTileIfPressed()
    {
        var cam = GameManager.Instance.cameraDriver.GetComponent<Camera>();
        var ray = cam.ScreenPointToRay(Input.mousePosition);
            
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            var target = hit.collider.gameObject;
            return target.GetComponent<TileObject>();
        }
        return null;
    }
}
