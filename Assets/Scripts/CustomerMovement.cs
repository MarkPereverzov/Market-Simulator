using UnityEngine;
using UnityEngine.AI;

public class CustomerMovement : MonoBehaviour
{
    public float areaWidth = 10f; // ������ ������� (����)
    public float areaLength = 10f; // ����� ������� (����)

    private NavMeshAgent agent; // ����� ��� ������������ �� NavMesh

    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // �������� ��������� NavMeshAgent
        SetRandomDestination(); // ������ ��������� ����
    }

    void Update()
    {
        // ���� ������ ������ ����, ������ ����� ��������� �����
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            SetRandomDestination();
        }
    }

    private void SetRandomDestination()
    {
        // ��������� ���������� ����������� ������ ����
        Vector3 randomPosition = new Vector3(
            Random.Range(-areaWidth / 2, areaWidth / 2), // ��������� �������� �� ��� X
            1f, // ������ ��� ������� (��������� ����������)
            Random.Range(-areaLength / 2, areaLength / 2) // ��������� �������� �� ��� Z
        );

        // ���������, ���� �� ��������� ����� �� NavMesh � �������� �������� ���������
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPosition, out hit, 1f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position); // ������ ����� ���� ��� ������
        }
    }
}
