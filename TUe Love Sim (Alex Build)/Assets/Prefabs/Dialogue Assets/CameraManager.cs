using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance { get; private set; }

    [Header("Cameras")]
    [SerializeField] private Camera playerCam;

    private CameraMovement playerCamController;

    private void Awake()
    {
        Camera.main.enabled = false;
        playerCam.enabled = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null && instance != this)
        {
            Debug.Log("More than one instance of the singleton class DialogueManager found in the scene. " +
                      "The new instance has been terminated. ");
            Destroy(this);
            return;
        }
        instance = this;

        playerCamController = Camera.main.GetComponent<CameraMovement>();
        if(playerCamController == null)
        {
            Debug.Log("CameraManager tried to fetch the CameraMovement script component of the main camera, but failed.");
        }

    }

    public void SwitchToCamera(Camera camera)
    {
        foreach (Camera cam in Camera.allCameras)
        {
            cam.enabled = false;
        }
        camera.enabled = true;
    }
    public void ReturnToMainCamera(Camera current)
    {
        current.enabled = false;
        playerCam.enabled = true;
    }

    public void DisablePlayerCameraMovement()
    {
        playerCamController.enabled = false;
    }

    public void EnablePlayerCameraMovement()
    {
        playerCamController.enabled = true;
    }

}
