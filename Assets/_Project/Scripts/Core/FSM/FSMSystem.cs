using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMSystem : MonoBehaviour {

    private List<FSMState> states = new List<FSMState>();
    public FSMState currentState;
    protected bool hasPredefineState = false;

	// Use this for initialization
	void Start () {
		
	}


    protected virtual void PredefineState()
    {
        //Debug.LogError("Has Predefine :" + gameObject.name);
        hasPredefineState = true;
    }


    public void AddState(FSMState state_)
    {
        states.Add(state_);

        if (states.Count == 1)
        {
            currentState = state_;
            currentState.OnEnter();
        }
    }

    public void GoToState(FSMState state_)
    {
        if (hasPredefineState == false)
            PredefineState();
        if (currentState != null)
        {
            currentState.OnExit();
        }

        currentState = state_;
        currentState.OnEnter();
    }

    public void GoToState (FSMState state_, object data)
    {
        if (currentState != null)
        {
            currentState.OnExit();
        }

        currentState = state_;
        currentState.OnEnter(data);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState != null)
        {
            currentState.OnUpdate();
        }
        OnSystemUpdate();
    }

    private void FixedUpdate()
    {
        if (currentState != null)
        {
            currentState.OnFixedUpdate();
        }
        OnSystemFixedUpdate();
    }

    private void LateUpdate()
    {
        if (currentState != null)
        {
            currentState.OnLateUpdate();
        }
        OnSystemLateUpdate();
    }

    public virtual void OnSystemUpdate()
    {

    }

    public virtual void OnSystemFixedUpdate()
    {

    }

    public virtual void OnSystemLateUpdate()
    {

    }
}
