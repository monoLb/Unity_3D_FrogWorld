using UnityEngine;

public class UnitRunningState : StateMachineBehaviour
{
    private AttackController attackController;
    private UnitBase self;
    
    private UnitMovement unitMovement;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        unitMovement=animator.GetComponentInParent<UnitMovement>();
        if (FieldManager.Instance.currentField == FieldType.Battlefield)
        {
            self=animator.GetComponentInParent<UnitBase>();
            attackController = animator.GetComponentInParent<AttackController>();
        }
        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (FieldManager.Instance.currentField == FieldType.Land)
        {
            if (unitMovement.GetAgent().velocity.magnitude<0.01f)
                animator.SetBool("isRunning", false);
        }

        else if (FieldManager.Instance.currentField == FieldType.Battlefield)
        {
            unitMovement.GetAgent().SetDestination(attackController.targetToAttack.transform.position);
            animator.transform.LookAt(attackController.targetToAttack.transform);
            float distanceFromTarget=Vector3.Distance(attackController.targetToAttack.transform.position,self.transform.position);
           
            if (distanceFromTarget < self.attackDis)
            {
                // 停止移动
                unitMovement.GetAgent().isStopped = true;
                unitMovement.GetAgent().velocity = Vector3.zero;
                // 进入攻击状态
                animator.SetBool("isAttacking", true);
            }
        }
        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }


}
