public class GoombaDeadState: ModularState
{
    public GoombaEnemyEntity enemyEntity;
    private string entryAnimation;
    public override void enterState(State prev)
    {
        enemyEntity.Animator.Play(entryAnimation);
        
        //turn enemy into a pushable thing
        enemyEntity.cc.enabled = false;
        enemyEntity.collider.isTrigger = false;
        enemyEntity.rb.isKinematic = false;
    }

    public override void updateState()
    {
        
    }


    public override void exitState(State next)
    {
        
    }
}