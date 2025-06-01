using UnityEngine;

namespace DC_Tool.Basic
{
    public abstract class State<T>
    {
        protected StateMachine<T> stateMachine;

        public State(StateMachine<T> stateMachine)
        {
            this.stateMachine = stateMachine;
        }

        public virtual void OnEnter()
        {
        }

        public virtual void OnUpdate()
        {

        }
        public virtual void OnFixedUpdate()
        {

        }
        public virtual void OnExit()
        {
        }

        public virtual bool CanChangeState(T state)
        {
            return true;
        }
    }
}
