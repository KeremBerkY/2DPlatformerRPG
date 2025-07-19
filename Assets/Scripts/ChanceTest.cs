using UnityEngine;

public class ChanceTest : MonoBehaviour
{
    [SerializeField] private float chance = 25;
    [SerializeField] private float rollResult;
    [SerializeField] private string result;
    
    [ContextMenu("Try")]
    public void Try()
    {
        rollResult = Random.Range(0, 100);

        if (rollResult < chance)
            result = "Successful";
        else
            result = "Failed";
    }
}
