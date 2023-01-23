using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MoveToDestroy : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _delayBetweenDestroy;
    [SerializeField] private UnityEvent _actionBeforeDestroy;
    
    private Vector3 _endPos;

    public event Action ReachedEnd;
    
    public IEnumerator MoveCoroutine(Vector3 startPos, Vector3 endPos)
    {
        transform.position = startPos;
        _endPos = endPos;
        
        float progress = 0;
        while (progress < 1)
        {
            progress += Time.deltaTime * _speed;
            transform.position = Vector3.Lerp(startPos, _endPos, progress);
            yield return null;
        }
        
        _actionBeforeDestroy.Invoke();
        //ReachedEnd?.Invoke();
        Destroy(gameObject, _delayBetweenDestroy);
    }
}
