using UnityEngine;

public class LevelData : MonoBehaviour
{
    [SerializeField] private int levelNo = 1;
    [SerializeField] private string nextLevelNameOverride;
    [SerializeField] private float killHeight = -50;
    public float KillHeight => killHeight;
    public static string generateLevelSceneNameFromLevelNo(int levelNo)
    {
        return "level " + levelNo;
    }

    public string getNextSceneAfterThisLevel()
    {
        if (nextLevelNameOverride.Length > 0)
            return nextLevelNameOverride;
        return generateLevelSceneNameFromLevelNo(levelNo + 1);
    }
}