using UnityEngine;


[CreateAssetMenu(fileName = "Armazenamento", menuName = "Level")]
public class Level : ScriptableObject
{
    public string levelName;
    public bool isCompleted;
    public bool isUnlocked;

}
