using UnityEngine;
using UnityEngine.AI;

public class CustomerMovement : MonoBehaviour
{
    public float areaWidth = 10f; // Ширина области (пола)
    public float areaLength = 10f; // Длина области (пола)

    private NavMeshAgent agent; // Агент для передвижения по NavMesh

    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // Получаем компонент NavMeshAgent
        SetRandomDestination(); // Задаем случайную цель
    }

    void Update()
    {
        // Если клиент достиг цели, задаем новую случайную точку
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            SetRandomDestination();
        }
    }

    private void SetRandomDestination()
    {
        // Генерация случайного направления внутри пола
        Vector3 randomPosition = new Vector3(
            Random.Range(-areaWidth / 2, areaWidth / 2), // Случайное значение по оси X
            1f, // Высота для капсулы (оставляем постоянной)
            Random.Range(-areaLength / 2, areaLength / 2) // Случайное значение по оси Z
        );

        // Проверяем, есть ли доступная точка на NavMesh в пределах заданных координат
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPosition, out hit, 1f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position); // Задаем новую цель для агента
        }
    }
}
