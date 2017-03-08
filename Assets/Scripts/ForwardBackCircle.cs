﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardBackCircle : MonoBehaviour
{
    public float moveSpeed = 6;
    public float forwardPosition = -16.25f;
    public float backPosition = -8.75f;
    public float pauseAtForward = 0;
    public float pauseAtBack = 0;
    private Vector3 _moveVec;
    private bool _ismoving = true;
    private bool _atForward = false;
    private Transform _targetParent;
    private Vector3 _targetSourceScale;
    private bool _isPause = false;
    // Use this for initialization
    void Start()
    {
        if(forwardPosition > backPosition)
        {
            var temp = forwardPosition;
            forwardPosition = backPosition;
            backPosition = temp;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_isPause) return;
        
        if (_atForward)
        {
            moveToBack();
        }
        else
        {
            moveToForward();
        }
    }

    void moveToBack()
    {
        GetComponent<Rigidbody>().velocity = new Vector3(moveSpeed, 0, 0);
        transform.Translate(new Vector3(moveSpeed, 0, 0) * Time.deltaTime);
        if (transform.position.x >= backPosition)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            transform.position = new Vector3(backPosition, transform.position.y, transform.position.z);
            _ismoving = false;
            _atForward = false;
            doPauseAtBack();
        }
    }

    void moveToForward()
    {
        GetComponent<Rigidbody>().velocity = new Vector3(-moveSpeed, 0, 0);
        transform.Translate(new Vector3(-moveSpeed, 0, 0) * Time.deltaTime);
        if (transform.position.x <= forwardPosition)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            transform.position = new Vector3(forwardPosition, transform.position.y, transform.position.z);
            _ismoving = false;
            _atForward = true;
            doPauseAtForward();
        }
    }


    void doPauseAtForward()
    {
        StartCoroutine(movePause(pauseAtForward));
    }

    void doPauseAtBack()
    {
        StartCoroutine(movePause(pauseAtBack));
    }

    IEnumerator movePause(float seconds)
    {
        _isPause = true;
        yield return new WaitForSeconds(seconds);
        _isPause = false;
    }

    void DoActivateTrigger()
    {
        _ismoving = true;
    }
}
