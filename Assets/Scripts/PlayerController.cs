using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private static readonly int Blend = Animator.StringToHash("Blend");
    
    [Header("Components")] 
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private CharacterController characterController;
    
    [Header("Options")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float blendSpeed = 0.02f;
    private Vector2 _move;
    private float _blend;

    private void Update()
    {
        BlendingAnimateTree();
        MovePlayer();
    }

    //Input action move (input manager)
    public void OnMove(InputValue value)
    {
        _move = value.Get<Vector2>();
    }

    //Movement
    private void MovePlayer()
    {
        Vector3 direction = new Vector3(_move.x, 0f, _move.y);
        if (direction.sqrMagnitude > 1) direction.Normalize();
        
        characterController.Move(direction * Time.deltaTime * speed);
    }

    //Fade moving animation
    private void BlendingAnimateTree()
    {
        if (_move.magnitude == 0 && _blend > 0) _blend-=blendSpeed;
        else if (_move.magnitude !=0 && _blend <= 1) _blend+=blendSpeed;
        
        playerAnimator.SetFloat(Blend, _blend);
    }
}
