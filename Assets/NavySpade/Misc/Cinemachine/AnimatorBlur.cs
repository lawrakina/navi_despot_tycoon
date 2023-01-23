using System;
using UnityEngine;

public class AnimatorBlur : MonoBehaviour
{
    private Animator _animator;

    private bool _isBlur;
    private static readonly int Start1 = Animator.StringToHash("Start");
    private static readonly int IsBlur = Animator.StringToHash("IsBlur");

    // Update is called once per frame
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _animator.SetBool(Start1, true);
    }

    void Update()
    {
        _animator.SetBool(IsBlur, Math.Abs(Time.timeScale - 1) > .1f);
    }
}
