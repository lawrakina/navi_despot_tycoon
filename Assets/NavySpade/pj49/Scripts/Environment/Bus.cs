using System.Collections;
using UnityEngine;

public class Bus : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private AnimationCurve _movePattern;

    public IEnumerator MoveToCoroutine(Vector3 targetPos)
    {
        Vector3 startPos = transform.position;
        
        float progress = 0;
        while (progress < 1)
        {
            progress += Time.deltaTime * _speed;
            transform.position = Vector3.Lerp(startPos, targetPos, _movePattern.Evaluate(progress));
            yield return null;
        }
    }
}
