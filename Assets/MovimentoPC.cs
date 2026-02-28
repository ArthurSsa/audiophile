using UnityEngine;

public class MovimentoPC : MonoBehaviour
{
    public CharacterController controller;
    public float velocidade = 5f;
    public float sensibilidadeMouse = 2f;
    
    float rotacaoX = 0f;
    public Transform cameraPlayer;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Esconde o mouse
    }

    void Update()
    {
        // Andar (WASD)
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 movimento = transform.right * x + transform.forward * z;
        controller.Move(movimento * velocidade * Time.deltaTime);

        // Olhar (Mouse)
        float mouseX = Input.GetAxis("Mouse X") * sensibilidadeMouse;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidadeMouse;

        rotacaoX -= mouseY;
        rotacaoX = Mathf.Clamp(rotacaoX, -90f, 90f);

        cameraPlayer.localRotation = Quaternion.Euler(rotacaoX, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}