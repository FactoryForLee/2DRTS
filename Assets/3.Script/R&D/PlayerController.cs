using UnityEngine;

/// <summary>
/// Debugging용 스크립트
/// </summary>
public class PlayerController : MonoBehaviour
{
    [SerializeField] private NavMesh2D agent;
    [SerializeField] private Transform target;

    private bool canTargetMove = true;

    private void Awake()
    {
        agent = GetComponent<NavMesh2D>();
    }

    private void Update()
    {
        agent.SetDestination(target.position);

        if (Input.GetKeyDown(KeyCode.Escape))
            canTargetMove = !canTargetMove;

        if (canTargetMove)
        {
            Vector3Int newPos = Vector3Int.RoundToInt(InputManager.Instance.GetMousePos());
            newPos.z = 0;
            target.position = newPos;
        }
    }
}
