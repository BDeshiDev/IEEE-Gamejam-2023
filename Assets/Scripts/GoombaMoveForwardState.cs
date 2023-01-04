using UnityEngine;

public class GoombaMoveForwardState: ModularState
{
    public float moveSpeed = 5;
    public GoombaEnemyEntity enemyEntity;
    public Bumper bumper;
    public string entryAnimation;
    public override void enterState(State prev)
    {
        enemyEntity.Animator.Play(entryAnimation);
    }

    public override void updateState()
    {
        enemyEntity.cc.Move(moveSpeed * Time.deltaTime * enemyEntity.transform.forward);
        
        
        if (bumper.checkCollision())
        {
            enemyEntity.transform.Rotate(Vector3.up, 180 );
        }
        
    }



    public override void exitState(State next)
    {
        
    }
}