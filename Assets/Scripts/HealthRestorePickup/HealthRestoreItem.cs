using System;
using UnityEngine;

public class HealthRestoreItem : MonoBehaviour
{
    private Transform _moveTargetTransform;

    public static event Action OnHealthPickUp;
    private void OnEnable()
    {
        _moveTargetTransform = GameObject.Find("HealthIcon").gameObject.transform;
        transform.localScale = Vector3.zero;
        transform.LeanScale(Vector3.one, 0.1f).setOnComplete(AnimationPhaseTwo);     
    }

    private void OnDisable()
    {
        transform.localScale = Vector3.zero;
    }

    private void OnAnimationComplete()
    {
        gameObject.SetActive(false);
        OnHealthPickUp?.Invoke();
    }

    private void AnimationPhaseTwo()
    {
        transform.LeanMoveY(transform.position.y + 80f, 0.2f).setEaseOutQuad().setOnComplete(AnimationPhaseThree);
    }

    private void AnimationPhaseThree()
    {
        transform.LeanMove(_moveTargetTransform.position, 0.3f).setOnComplete(OnAnimationComplete);
    }
}
