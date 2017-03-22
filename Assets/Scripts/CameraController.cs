﻿using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class CameraController : MonoBehaviour {

    const float MAX_DISTANCE = 10;
    const float MIN_DISTANCE = 1;

    public GameObject target;
    public float cameraRotateSpeed = 30;
    public float cameraMoveSpeed = 3;
    private float _distance = 2;
    private float _theta;
    private float _phi;
    private Vector3 _pos;
    private Vector3 _direction;
	// Use this for initialization
	void Start () {
        resetAngel();
    }
	
	// Update is called once per frame
	void Update () {

        if(Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            _distance -= 0.1f * cameraMoveSpeed;
            if (_distance < MIN_DISTANCE) _distance = MIN_DISTANCE;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            _distance += 0.1f * cameraMoveSpeed;
            if (_distance > MAX_DISTANCE) _distance = MAX_DISTANCE;
        }

        float x;
        float y;
        if(Input.GetMouseButton(0))
        {
            x = CrossPlatformInputManager.GetAxis("MouseRightHorizontal");
            y = CrossPlatformInputManager.GetAxis("MouseRightVertical");
            x *= cameraRotateSpeed / 3;
            y *= cameraRotateSpeed / 3;
        }
        else
        {
            

            x = CrossPlatformInputManager.GetAxis("RightHorizontal");
            y = CrossPlatformInputManager.GetAxis("RightVertical");
            x *= cameraRotateSpeed;
            y *= -cameraRotateSpeed;
        }
        _theta -= x;
        _phi -= y;
        _phi = Mathf.Clamp(_phi, -89, 89);
        if(Input.GetMouseButton(1))
        {
            resetAngel();
        }

        transform.position = getCameraPosition();
        transform.LookAt(target.transform);
    }

    Vector3 caclCameraPosition()
    {                                          
        float x = _distance * Mathf.Cos(Mathf.PI / 180 * _phi) * Mathf.Cos(Mathf.PI / 180 * _theta);
        float y = _distance * Mathf.Sin(Mathf.PI / 180 * _phi);
        float z = _distance * Mathf.Cos(Mathf.PI / 180 * _phi) * Mathf.Sin(Mathf.PI / 180 * _theta);
        var pos = target.transform.position;
        return pos + new Vector3(x, y, z);
        //return target.transform.TransformPoint(new Vector3(x, y, z));
    }

    //防止穿墙
    Vector3 fixedCameraPosition(Vector3 pos)
    {
        RaycastHit hitInfo;
        if(Physics.Raycast(target.transform.position, pos - target.transform.position ,out hitInfo,_distance))
        {
            return hitInfo.point + hitInfo.normal * 0.2f;
        }

        return pos;
    }

    Vector3 getCameraPosition()
    {
        var pos = caclCameraPosition();
        pos = fixedCameraPosition(pos);
        return pos;
    }

    public void resetAngel()
    {
        var targetForward = target.transform.parent.transform.forward;
        _theta = Vector3.Angle(Vector3.right, targetForward);
        if(Vector3.Dot(Vector3.forward, targetForward) > 0.001f)
        {
            _theta = 360 - _theta;
        }
        _theta = 180 - _theta;       
        _phi = 10;
    }

    void OnDrawGizmos()
    {

    }
}
