using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance { get; private set; }

    [Header("Cameras")]
    [SerializeField] private Camera playerCam;
    [SerializeField] private Camera dialogueCam;

    private CameraMovement playerCamController;

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

        Camera.main.enabled = true;
        playerCamController = Camera.main.GetComponent<CameraMovement>();

        if(playerCamController == null)
        {
            Debug.Log("CameraManager tried to fetch the CameraMovement script component of the main camera, but failed.");
        }

    }

    public void SwitchFromMainCamera(Camera camera)
    {
        Camera.main.enabled = false;
        Camera.main.gameObject.SetActive(false);
        camera.enabled = true;

    }
    public void DisablePlayerCameraMovement()
    {
        playerCamController.enabled = false;
    }

    public void EnablePlayerCameraMovement()
    {
        playerCamController.enabled = true;
    }
    public void ReturnToMainCamera()
    {
        Camera.current.enabled = false;
        Camera.main.enabled = true;
        Camera.main.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
