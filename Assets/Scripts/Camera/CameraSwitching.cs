using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera firstPersonCamera;
    public Camera thirdPersonCamera;
    public bool thirdPerspective = false;

    void Update()
    {
        if(!thirdPerspective){
            UseFirstCamera();
        }else{
            UseThirdCamera();
        }
    }

    void UseFirstCamera(){
        firstPersonCamera.gameObject.SetActive(true);
        thirdPersonCamera.gameObject.SetActive(false);

        if(Input.GetMouseButtonDown(1))
            thirdPerspective = true;
    }

    void UseThirdCamera(){
        firstPersonCamera.gameObject.SetActive(false);
        thirdPersonCamera.gameObject.SetActive(true);

        if(Input.GetMouseButtonDown(1))
            thirdPerspective = false;
    }
}
