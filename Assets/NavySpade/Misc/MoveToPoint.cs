using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MoveToPoint : MonoBehaviour
{
    [SerializeField] private Vector3 initPosition;
    [SerializeField] private Vector3 movePoint;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float lookAtSpeed = 5f;
    [SerializeField] private float _tolerance = 0.01f;
    [SerializeField] private bool isFinished = true;
    [SerializeField] private bool isNeedUseLocalCoords = true;

    private Action pointReachedCallback;
    
    void Start()
    {
        initPosition = transform.localPosition;
        // if (movePoint != new Vector3(0, 0, 0))
        // {
        //     StartMoveToPoint(movePoint, speed, null);
        // }
    }

    void Update()
    {
        if (!isFinished)
        {
            float step = speed * Time.deltaTime; // calculate distance to move

            Vector3 checkPos;
            if (isNeedUseLocalCoords)
            {
                checkPos = transform.localPosition;
                transform.localPosition = Vector3.MoveTowards(checkPos, movePoint, step);
            }
            else
            {
                checkPos = transform.position;
                transform.position = Vector3.MoveTowards(checkPos, movePoint, step);
            }
            
            LookAtSmooth(movePoint - checkPos);
            if (Vector3.Distance(movePoint, transform.position) < _tolerance)
            {
                //transform.position = movePoint;
                isFinished = true;
                pointReachedCallback?.Invoke();
                pointReachedCallback = null;
            }
        }
    }

    public void StartMoveToPoint(Vector3 _movePoint, float _speed, Action _pointReachedCallback, bool _useLocalCoords = true)
    {
        movePoint = _movePoint;
        speed = _speed;
        isNeedUseLocalCoords = _useLocalCoords;
        pointReachedCallback = _pointReachedCallback;
        isFinished = false;
    }
    
    private void LookAtSmooth(Vector3 dir)
    {
        transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                Quaternion.LookRotation(dir), 
                lookAtSpeed * Time.deltaTime);
    }

    public void ReturnBack(float _speed)
    {
        StartMoveToPoint(initPosition, _speed, null);
    }
}