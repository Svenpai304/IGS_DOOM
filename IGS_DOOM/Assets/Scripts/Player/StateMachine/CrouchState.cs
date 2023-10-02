namespace FSM
{
    public class CrouchState : GroundedState
    {
        protected override void OnEnter()
        {
            cmc.Crouch();
        }

        protected override void OnUpdate()
        {
            
        }

        protected override void OnFixedUpdate()
        {
            
        }

        protected override void OnExit()
        {
            cmc.UnCrouch();
        }
    }
}