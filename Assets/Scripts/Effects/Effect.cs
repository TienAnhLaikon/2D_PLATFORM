using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Effect : MonoBehaviour
{
    // Start is called before the first frame update
    public UnityEvent animationComplete;
    [SerializeField] private Animator animator;
    public bool effectComplete;
    protected void Start()
    {
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    protected void Update()
    {
        if (effectComplete) Destroy(gameObject);

    }
    private void OnEffectBegin()
    {
        effectComplete = false;
    }
    private void OnEffectEnd()
    {
        effectComplete = true;
        animationComplete.Invoke();
    }
}
