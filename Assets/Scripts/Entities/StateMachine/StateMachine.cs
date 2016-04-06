using UnityEngine;
using System.Collections;

public class StateMachine<T> {
    private T owner;
    private State<T> currentState = null;
    private State<T> previousState = null;
    private State<T> globalState = null;

    public StateMachine(T owner, State<T> initialState) {
        this.owner = owner;

        ChangeState(initialState);
    }

    public void Update() {
        if (globalState != null) { 
            globalState.Execute(owner); 
        }

        if (currentState != null) {
            currentState.Execute(owner);
        }
    }

    public State<T> GetCurrentState() {
        return currentState;
    }

    public void ChangeState(State<T> newState) {
        previousState = currentState;
        if (currentState != null) {
            currentState.Exit(owner);
        }

        currentState = newState;
        if (currentState != null) {
            currentState.Enter(owner);
        }
    }

    public void RevertToPreviousState() {
        if (previousState != null) {
            ChangeState(previousState);
        }
    }
}
