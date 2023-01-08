using UnityEngine;

public class GoombaDeadState: ModularState
{
    public GoombaEnemyEntity enemyEntity;
    private string entryAnimation;
    public Material deadMaterial;
    public override void enterState(State prev)
    {
        enemyEntity.setFaceMat(deadMaterial);
        //turn enemy into a pushable thing
        // enemyEntity.cc.enabled = false;
        
        enemyEntity.collider.isTrigger = false;
        // enemyEntity.rb.isKinematic = false;
    }

    public override void updateState()
    {
        
    }


    public override void exitState(State next)
    {
        
    }
}