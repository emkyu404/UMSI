using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * author : Bui Minh-Quân
 * last update : 03/02/2020
 * Script gérant les déplacements de caméra (zqsd + souris) ainsi que le zoom (roulette souris) et enfin le reset de la cam (spacebar)
 */ 
public class CameraManager : MonoBehaviour
{
    public float panSpeed = 10f;
    public int panBorderThickness = 20;
    public float scrollSpeed = 10f;
    private float SPEED_FACTOR;

    private readonly float MIN_ZOOM = 5f;
    private readonly float MAX_ZOOM = 20f;


    Vector3 basePosition;
    float baseCameraSize;
    // Start is called before the first frame update
    private void Awake()
    {
        transform.rotation.SetEulerAngles(new Vector3(-31.657f, -37.43f, 56.413f));
        basePosition = transform.position;
        baseCameraSize = this.GetComponent<Camera>().orthographicSize;
        SPEED_FACTOR = (Time.deltaTime / Time.timeScale);
    }

    private void Update()
    {
        if (Time.timeScale != 0)
        {
            SPEED_FACTOR = (Time.deltaTime / Time.timeScale);
            Vector3 pos = transform.position;
            Debug.Log(pos);
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey("q") || Input.mousePosition.x <= panBorderThickness)
            {
                pos.x -= panSpeed * SPEED_FACTOR;
                pos.y -= panSpeed * SPEED_FACTOR;
            }
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBorderThickness)
            {
                pos.x += panSpeed * SPEED_FACTOR;
                pos.y += panSpeed * SPEED_FACTOR;
            }
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey("z") || Input.mousePosition.y >= Screen.height - panBorderThickness)
            {
                pos.x -= panSpeed * SPEED_FACTOR;
                pos.y += panSpeed * SPEED_FACTOR;
            }
            if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey("s") || Input.mousePosition.y <= panBorderThickness)
            {
                pos.x += panSpeed * SPEED_FACTOR;
                pos.y -= panSpeed * SPEED_FACTOR;
            }
            float scroll = Input.GetAxis("Mouse ScrollWheel");


            // camera scroll
            float newOrthographicSize = this.GetComponent<Camera>().orthographicSize - scroll * scrollSpeed * 50f * SPEED_FACTOR;
            if (newOrthographicSize >= MIN_ZOOM && newOrthographicSize <= MAX_ZOOM)
            {
                this.GetComponent<Camera>().orthographicSize = newOrthographicSize;
            }           
            else if(newOrthographicSize < MIN_ZOOM)
            {
                this.GetComponent<Camera>().orthographicSize = MIN_ZOOM;
            }
            else if (newOrthographicSize > MAX_ZOOM)
            {
                this.GetComponent<Camera>().orthographicSize = MAX_ZOOM;
            }

            transform.position = pos;

            if (Input.GetKey(KeyCode.Space))
            {
                ResetCamera();
            }
        }

    }

    private void ResetCamera()
    {
        this.GetComponent<Camera>().orthographicSize = baseCameraSize;
        transform.position = basePosition;
    }

}   
