using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public FloatingJoystick moveJoystick;
    public FloatingJoystick lookJoystick;
   
    [SerializeField] private float Speed;

    

    public Animator playerAnimator;
    // Update is called once per frame
   private void Awake() {
    playerAnimator = GetComponentInChildren<Animator>();
    
  }
    private void Start()
    {
       // playerAnimator = GetComponentInChildren<Animator>();
       // if (SceneManager.GetActiveScene().buildIndex == 0)
       // {
         //  animator.SetInteger("WeaponType_int",0);
      //  }

    }

    private void FixedUpdate()
    {

        if (moveJoystick != null && lookJoystick != null)
        {
            UpdateMoveJoystick();
            UpdateLookJoystick();
        }
        else playerAnimator.SetInteger("WeaponType_int", 0);
    }

    void UpdateMoveJoystick()
    {
        float hoz = moveJoystick.Horizontal;
        float ver = moveJoystick.Vertical;
        Vector2 convertedXY = ConvertWithCamera(Camera.main.transform.position, hoz, ver);
        Vector3 direction = new Vector3(convertedXY.x * Speed, 0, convertedXY.y * Speed) * Time.deltaTime;
        transform.Translate(direction * 0.02f, Space.World);
        Vector3 lookAtPosition = transform.position + direction;
        transform.LookAt(lookAtPosition);


        if (hoz != 0 || ver != 0)
        {
            playerAnimator.SetFloat("Speed_f", 0.5f);

        }
        else playerAnimator.SetFloat("Speed_f", 0);

    }

    void UpdateLookJoystick()
    {
        float hoz = lookJoystick.Horizontal;
        float ver = lookJoystick.Vertical;
        Vector2 convertedXY = ConvertWithCamera(Camera.main.transform.position, hoz, ver);
        Vector3 direction = new Vector3(convertedXY.x, 0, convertedXY.y).normalized;
        Vector3 lookAtPosition = transform.position + direction;
        transform.LookAt(lookAtPosition);
    }


    private Vector2 ConvertWithCamera(Vector3 cameraPos, float hor, float ver)
    {
        Vector2 joyDirection = new Vector2(hor, ver).normalized;
        Vector2 camera2DPos = new Vector2(cameraPos.x, cameraPos.z);
        Vector2 playerPos = new Vector2(transform.position.x, transform.position.z);
        Vector2 cameraToPlayerDirection = (Vector2.zero - camera2DPos).normalized;
        float angle = Vector2.SignedAngle(cameraToPlayerDirection, new Vector2(0, 1));
        Vector2 finalDirection = RotateVector(joyDirection, -angle);
        return finalDirection;
    }

    public Vector2 RotateVector(Vector2 v, float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        float _x = v.x * Mathf.Cos(radian) - v.y * Mathf.Sin(radian);
        float _y = v.x * Mathf.Sin(radian) + v.y * Mathf.Cos(radian);
        return new Vector2(_x, _y);
    }

}
