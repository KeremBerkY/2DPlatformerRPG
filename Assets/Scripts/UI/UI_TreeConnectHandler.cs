using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class UI_TreeConnectDetails
{
    public UI_TreeConnectHandler childNode;
    public NodeDirectionType direction;
    [Range(100f, 250f)] public float length;
}

public class UI_TreeConnectHandler : MonoBehaviour
{
    private RectTransform _rect => GetComponent<RectTransform>();
    [SerializeField] private UI_TreeConnectDetails[] connectionDetails;
    [SerializeField] private UI_TreeConnection[] connections;

    private void OnValidate()
    {
        if (connectionDetails.Length <= 0)
            return;
        
        if (connectionDetails.Length != connections.Length)
            Debug.Log("Amount of details should be same as amount of connections. - " + gameObject.name);
        
        UpdateConnection();
    }

    private void UpdateConnection()
    {
        for (int i = 0; i < connectionDetails.Length; i++)
        {
            var detail = connectionDetails[i];
            var connection = connections[i];
            Vector2 targetPosition = connection.GetConnectionPoint(_rect);
            
            connection.DirectConnection(detail.direction, detail.length);
            detail.childNode.SetPosition(targetPosition);
        }
    }

    public void SetPosition(Vector2 position) => _rect.anchoredPosition = position;
}
