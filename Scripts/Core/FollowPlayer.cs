using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FollowPlayerWithZoom : MonoBehaviour
{
    public Transform target;                 // Игрок
    public Vector3 offset = new Vector3(0f, 0f, -10f);  // Смещение камеры
    public float zoom = 5f;                  // Отдаление (чем больше — тем дальше)

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        cam.orthographic = true;
        cam.orthographicSize = zoom;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Позиционируем камеру по игроку с отступом
        transform.position = target.position + offset;

        // Обновляем размер камеры на случай динамического изменения
        cam.orthographicSize = zoom;
    }
}
