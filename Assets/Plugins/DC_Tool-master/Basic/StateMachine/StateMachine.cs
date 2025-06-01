using System.Collections.Generic;

namespace DC_Tool.Basic
{
    public class StateMachine<T> 
    {
        private Dictionary<T, State<T>> states = new Dictionary<T, State<T>>();
        private T currentStateKey;
        private State<T> currentState;

        public T CurrentState => currentStateKey;

        public void AddState(T stateKey, State<T> state)
        {
            states[stateKey] = state;
        }

        public bool TryChangeState(T state)
        {
            if (currentState != null && !currentState.CanChangeState(state))
            {

                DebugTool.Error($"Invalid state transition: {currentStateKey} -> {state}");
                return false;
            }
            return ChangeState(state);
        }

        private bool ChangeState(T state)
        {
            if (!states.ContainsKey(state))
            {
                DebugTool.Error($"State {state} is not defined");
                return false;
            }

            /*if (state.Equals(currentStateKey))
            {
                DebugTool.Info($"Already in {state} state, skipping");
                return false;
            }*/

            DebugTool.Info($"Change state: {currentStateKey} -> {state}");
            currentState?.OnExit();
            currentState = states[state];
            currentStateKey = state;
            currentState.OnEnter();
            return true;
        }

        public void OnUpdate()
        {
            currentState?.OnUpdate();
        }

        public void OnFixedUpdate()
        {
            currentState?.OnFixedUpdate();
        }
    }
}
