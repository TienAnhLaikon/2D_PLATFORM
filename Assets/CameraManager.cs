using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Vector3 currentPosition;
    Coroutine C_ShakeCoroutine;
    [SerializeField]
    private CinemachineBrain cinemachineCam; // Reference đến Cinemachine Virtual Camera

    // Start is called before the first frame update
    public void Awake()
    {
        currentPosition = transform.position;
        // Kiểm tra xem đã gán Cinemachine trong Inspector chưa
        if (cinemachineCam == null)
        {
            cinemachineCam = FindObjectOfType<CinemachineBrain>();
        }
    }
    public void StartShake(Vector3 shakeDirection, float duration, float shakeIntensity)
    {
        // Nếu đã có một Coroutine rung nào đang chạy thì dừng nó trước
        if (C_ShakeCoroutine != null)
        {
            StopCoroutine(C_ShakeCoroutine);
        }
        // Lấy vị trí hiện tại của camera ngay khi bắt đầu rung
        currentPosition = transform.position;
        // Disable Cinemachine khi rung bắt đầu
        if (cinemachineCam != null)
        {
            cinemachineCam.enabled = false;
        }

        // Bắt đầu Coroutine rung mới với tham số điều chỉnh cường độ rung
        C_ShakeCoroutine = StartCoroutine(ShakeSequence(shakeDirection, duration, shakeIntensity));
    }

    private IEnumerator ShakeSequence(Vector3 shakeDirection, float duration, float shakeIntensity)
    {
        float durationPassed = 0f;  // Thời gian đã trôi qua
        float shakeDistance = 0.02f * shakeIntensity;  // Tính khoảng cách rung dựa trên cường độ rung

        // Đặt vị trí camera bằng vị trí ban đầu cộng với hướng rung và khoảng cách rung
        transform.position = currentPosition + shakeDirection * shakeDistance;

        // Vòng lặp cho đến khi hết thời gian rung
        while (durationPassed < duration)
        {
            durationPassed += Time.deltaTime;  // Tăng thời gian đã trôi qua
                                               // Giảm khoảng cách rung dần dần cho tới khi hết rung
            shakeDistance -= (durationPassed / duration) * 0.02f * shakeIntensity;
            // Đặt lại vị trí camera dựa trên khoảng cách rung đã giảm
            transform.position = currentPosition + shakeDirection * shakeDistance;

            yield return null;  // Chờ đến frame tiếp theo
        }

        // Khi hoàn thành, đưa camera trở về vị trí ban đầu
        transform.localPosition = currentPosition;

        // Re-enable Cinemachine khi rung kết thúc
        if (cinemachineCam != null)
        {
            cinemachineCam.enabled = true;
        }
    }

    private void DoShake(float maxOffset)
    {
        float xOffSet = Random.Range(-maxOffset, maxOffset);
        float yOffSet = Random.Range(-maxOffset, maxOffset);

    }
}
