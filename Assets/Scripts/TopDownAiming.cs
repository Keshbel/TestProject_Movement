using UnityEngine;

 public class TopDownAiming : MonoBehaviour
 {
     [Header("Aim")] 
     [SerializeField] private bool aim;
     [SerializeField] private LayerMask groundMask;
     [SerializeField] private bool ignoreHeight;
     [SerializeField] private Transform aimedTransform;

     [Header("Laser")] 
     [SerializeField] private LineRenderer laserRenderer;
     [SerializeField] private LayerMask laserMask;
     [SerializeField] private float laserLength;

     [Header("Gizmos")] 
     private bool _gizmoCameraRay = false;
     private bool _gizmoGround = false;
     private bool _gizmoTarget = false;
     private bool _gizmoIgnoredHeightTarget = false;

     private Camera _mainCamera;

     private void Start()
     {
         _mainCamera = Camera.main;

         if (laserRenderer != null)
         {
             laserRenderer.SetPositions(new Vector3[]
             {
                 Vector3.zero,
                 Vector3.zero
             });
         }
     }

     private void Update()
     {
         Aim();
         RefreshLaser();
         ChangeTargetMode();
         GizmoSettings();
     }

     #region Gizmos
     private void OnDrawGizmos()
     {
         if (Application.isPlaying == false)
         {
             return;
         }

         var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
         if (Physics.Raycast(ray, out var hitInfo, float.MaxValue, groundMask))
         {
             if (_gizmoCameraRay)
             {
                 Gizmos.color = Color.magenta;
                 Gizmos.DrawLine(ray.origin, hitInfo.point);
                 Gizmos.DrawWireSphere(ray.origin, 0.5f);
             }

             var hitPosition = hitInfo.point;
             var hitGroundHeight = Vector3.Scale(hitInfo.point, new Vector3(1, 0, 1));
             ;
             var hitPositionIngoredHeight = new Vector3(hitInfo.point.x, aimedTransform.position.y, hitInfo.point.z);

             if (_gizmoGround)
             {
                 Gizmos.color = Color.gray;
                 Gizmos.DrawWireSphere(hitGroundHeight, 0.5f);
                 Gizmos.DrawLine(hitGroundHeight, hitPosition);
             }

             if (_gizmoTarget)
             {
                 Gizmos.color = Color.yellow;
                 Gizmos.DrawWireSphere(hitInfo.point, 0.5f);
                 Gizmos.DrawLine(aimedTransform.position, hitPosition);
             }

             if (_gizmoIgnoredHeightTarget)
             {
                 Gizmos.color = Color.cyan;
                 Gizmos.DrawWireSphere(hitPositionIngoredHeight, 0.5f);
                 Gizmos.DrawLine(aimedTransform.position, hitPositionIngoredHeight);
             }
         }
     }
     #endregion

     private void Aim()
     {
         if (aim == false)
         {
             return;
         }

         var (success, position) = GetMousePosition();
         if (success)
         {
             // Direction is usually normalized, 
             // but it does not matter in this case.
             var direction = position - aimedTransform.position;

             if (ignoreHeight)
             {
                 // Ignore the height difference.
                 direction.y = 0;
             }

             // Make the transform look at the mouse position.
             aimedTransform.forward = direction;
             //laserRenderer.SetPosition(1, prefabSpawn.position + aimedTransform.forward * laserLength);
         }
     }

     private (bool success, Vector3 position) GetMousePosition()
     {
         var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

         if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, groundMask))
         {
             return (success: true, position: hitInfo.point);
         }
         else
         {
             return (success: false, position: Vector3.zero);
         }
     }

     private void RefreshLaser()
     {
         if (laserRenderer == null)
         {
             return;
         }

         Vector3 lineEnd;

         if (Physics.Raycast(aimedTransform.position, aimedTransform.forward, out var hitinfo, laserLength, laserMask))
         {
             lineEnd = hitinfo.point;
         }
         else
         {
             lineEnd = aimedTransform.position + aimedTransform.forward * laserLength;
         }

         laserRenderer.SetPosition(1, aimedTransform.InverseTransformPoint(lineEnd));
     }

     private void ChangeTargetMode()
     {
         if (Input.GetKeyDown(KeyCode.Space))
         {
             ignoreHeight = !ignoreHeight;
         }
     }

     private void GizmoSettings()
     {
         if (Input.GetKeyDown(KeyCode.Alpha1))
         {
             _gizmoCameraRay = !_gizmoCameraRay;
         }
         else if (Input.GetKeyDown(KeyCode.Alpha2))
         {
             _gizmoGround = !_gizmoGround;
         }
         else if (Input.GetKeyDown(KeyCode.Alpha3))
         {
             _gizmoTarget = !_gizmoTarget;
         }
         else if (Input.GetKeyDown(KeyCode.Alpha4))
         {
             _gizmoIgnoredHeightTarget = !_gizmoIgnoredHeightTarget;
         }
     }
 }
