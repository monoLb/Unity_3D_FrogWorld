using UnityEngine;

public class UnitAttackState : StateMachineBehaviour
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
        if (FieldManager.Instance.currentField == FieldType.Battlefield)
        {
            animator.transform.LookAt(attackController.targetToAttack.transform);
            float distanceFromTarget=Vector3.Distance(attackController.targetToAttack.transform.position,self.transform.position);
             
            if (distanceFromTarget > self.attackDis)
            {
                animator.SetBool("isAttacking",false);
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        attackController.isAttacking = false;
        attackController.isFinding = true;
        attackController.FindNearestEnemy(self, BattleManager.Instance.GetOpposedGroupList(self.groupType));
        unitMovement.GetAgent().isStopped = false;
        animator.SetBool("isRunning",true);
    }
    
}
