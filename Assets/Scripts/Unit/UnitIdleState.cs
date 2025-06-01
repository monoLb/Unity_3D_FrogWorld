using UnityEngine;

public class UnitIdleState : StateMachineBehaviour
{
    
    private AttackController attackController;
    private UnitBase unitBase;
    
    private UnitMovement unitMovement;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        unitMovement=animator.GetComponentInParent<UnitMovement>();
        
        
        if (FieldManager.Instance.currentField== FieldType.Battlefield)
        {
            attackController = animator.GetComponentInParent<AttackController>();
        }
        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (FieldManager.Instance.currentField == FieldType.Land)
        {
            if (unitMovement.GetAgent().velocity.magnitude > 0)
            {
                animator.SetBool("isRunning", true);
            }
        }
        else if(FieldManager.Instance.currentField==FieldType.Battlefield)
        {
            if (attackController.targetToAttack != null)
            {
                animator.SetBool("isRunning", true);
            }
        }
        
        
            

    }

    
}
