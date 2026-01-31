using UnityEngine;

public class MonsterCollider : MonoBehaviour
{
    [SerializeField] private Transform targetObject;

    private void Update()
    {
        if (targetObject != null)
        {
            transform.position = targetObject.position;
        }
    }
}
