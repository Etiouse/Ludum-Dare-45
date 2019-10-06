using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class MovementController : MonoBehaviour
{
    private CharacterController controller = null;

    private Vector2 currentMove;

    // Start is called before the first frame update
    void Start()
    {
       controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckInputs();
        UpdateCharacterOrientation();
    }

    private void UpdateCharacterOrientation()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        Vector2 direction = new Vector2(
            mousePos.x - transform.position.x,
            mousePos.y - transform.position.y
        );

        controller.LookAt(direction);
    }

    private void FixedUpdate()
    {
        controller.Move(currentMove);
    }

    private void CheckInputs()
    {
        currentMove = Vector2.zero;

        float moveY = Input.GetAxisRaw("Vertical");
        float moveX = Input.GetAxisRaw("Horizontal");

        if (Input.GetMouseButton(0))
        {
            controller.Attack();
        }

        if (moveY > 0)
        {
            currentMove.y = 1;
        }
        else if (moveY < 0)
        {
            currentMove.y = -1;
        }

        if (moveX > 0)
        {
            currentMove.x = 1;
        }
        else if (moveX < 0)
        {
            currentMove.x = -1;
        }

        if (currentMove.y != 0 && currentMove.x != 0)
        {
            currentMove.y /= Mathf.Sqrt(2);
            currentMove.x /= Mathf.Sqrt(2);
        }
    }
}
