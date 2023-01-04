public class GoombaDeadState: ModularState
{
    public GoombaEnemyEntity enemyEntity;
    private string entryAnimation;
    public override void enterState(State prev)
    {
        enemyEntity.Animator.Play(entryAnimation);
    }

    public override void updateState()
    {
        
    }


    public override void exitState(State next)
    {
        
    }
}