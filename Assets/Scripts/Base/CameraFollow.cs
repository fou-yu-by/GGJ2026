using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
   private Transform player;
   [SerializeField] protected GameObject background;
   [SerializeField] private Camera targetCamera;
   [Header("Optional Custom Bounds")]
   [SerializeField] private bool useCustomBounds = true;
   [SerializeField] private Vector2 customBoundsCenterOffset;
   [SerializeField] private Vector2 customBoundsSize = new Vector2(110f, 110f);
   [Header("Follow Settings")]
   [SerializeField] private float followSpeed = 5f;
   [Header("Debug")]
   [SerializeField] private bool showDebugInfo;

   private void Awake()
   {
      if (targetCamera == null)
      {
         targetCamera = GetComponent<Camera>();
      }
   }

   private void Start()
   {
      GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
      if (playerObj != null)
      {
         player = playerObj.transform;
      }

      if (background == null)
      {
         Debug.LogWarning("CameraFollow: background is not assigned.", this);
      }
      if (background != null && background.transform.IsChildOf(transform))
      {
         Debug.LogWarning("CameraFollow: background is a child of the camera, bounds will move with the camera and clamping won't work.", this);
      }
   }

   private void LateUpdate()
   {
      if (player == null)
      {
         return;
      }

      Vector3 targetPos = new Vector3(player.position.x, player.position.y, transform.position.z);

      if (TryGetBackgroundBounds(out Bounds bounds) && targetCamera != null)
      {
         GetCameraExtentsAtBounds(bounds, out float horizExtent, out float vertExtent);

         float minX = bounds.min.x + horizExtent;
         float maxX = bounds.max.x - horizExtent;
         float minY = bounds.min.y + vertExtent;
         float maxY = bounds.max.y - vertExtent;

         if (minX > maxX)
         {
            targetPos.x = bounds.center.x;
         }
         else
         {
            targetPos.x = Mathf.Clamp(targetPos.x, minX, maxX);
         }

         if (minY > maxY)
         {
            targetPos.y = bounds.center.y;
         }
         else
         {
            targetPos.y = Mathf.Clamp(targetPos.y, minY, maxY);
         }
      }

      // 平滑跟随玩家
      if (followSpeed > 0)
      {
         transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);
      }
      else
      {
         transform.position = targetPos;
      }
   }

   private void OnDrawGizmos()
   {
      if (!showDebugInfo || targetCamera == null) return;
      if (!TryGetBackgroundBounds(out Bounds bounds)) return;

      GetCameraExtentsAtBounds(bounds, out float horizExtent, out float vertExtent);

      // 画背景边界（绿色）
      Gizmos.color = Color.green;
      Gizmos.DrawWireCube(bounds.center, bounds.size);

      // 画相机可移动范围（黄色）
      float minX = bounds.min.x + horizExtent;
      float maxX = bounds.max.x - horizExtent;
      float minY = bounds.min.y + vertExtent;
      float maxY = bounds.max.y - vertExtent;

      if (minX <= maxX && minY <= maxY)
      {
         Gizmos.color = Color.yellow;
         Vector3 moveCenter = new Vector3((minX + maxX) / 2f, (minY + maxY) / 2f, bounds.center.z);
         Vector3 moveSize = new Vector3(maxX - minX, maxY - minY, 0f);
         Gizmos.DrawWireCube(moveCenter, moveSize);
      }

      // 画相机可视范围（红色）
      Gizmos.color = Color.red;
      Gizmos.DrawWireCube(transform.position, new Vector3(horizExtent * 2f, vertExtent * 2f, 0f));

      Debug.Log($"Background bounds: {bounds}, CamExtent: {horizExtent}x{vertExtent}, MoveRange X:[{minX},{maxX}] Y:[{minY},{maxY}]");
   }

   private void GetCameraExtentsAtBounds(Bounds bounds, out float horizExtent, out float vertExtent)
   {
      Vector3 camPos = targetCamera.transform.position;
      Vector3 camForward = targetCamera.transform.forward;
      float planeDistance = Vector3.Dot(bounds.center - camPos, camForward);
      if (planeDistance <= 0.01f)
      {
         planeDistance = 0.01f;
      }

      Vector3 bottomLeft = targetCamera.ViewportToWorldPoint(new Vector3(0f, 0f, planeDistance));
      Vector3 topRight = targetCamera.ViewportToWorldPoint(new Vector3(1f, 1f, planeDistance));

      horizExtent = Mathf.Abs(topRight.x - bottomLeft.x) * 0.5f;
      vertExtent = Mathf.Abs(topRight.y - bottomLeft.y) * 0.5f;
   }

   private bool TryGetBackgroundBounds(out Bounds bounds)
   {
      bounds = new Bounds();
      if (background == null)
      {
         return false;
      }

      if (useCustomBounds)
      {
         Vector3 center = background.transform.position + new Vector3(customBoundsCenterOffset.x, customBoundsCenterOffset.y, 0f);
         bounds = new Bounds(center, new Vector3(customBoundsSize.x, customBoundsSize.y, 0f));
         return true;
      }

      Collider2D collider2D = background.GetComponent<Collider2D>();
      if (collider2D != null)
      {
         bounds = collider2D.bounds;
         return true;
      }

      Collider collider3D = background.GetComponent<Collider>();
      if (collider3D != null)
      {
         bounds = collider3D.bounds;
         return true;
      }

      SpriteRenderer spriteRenderer = background.GetComponent<SpriteRenderer>();
      if (spriteRenderer != null)
      {
         bounds = spriteRenderer.bounds;
         return true;
      }

      Renderer renderer = background.GetComponent<Renderer>();
      if (renderer != null)
      {
         bounds = renderer.bounds;
         return true;
      }

      RectTransform rectTransform = background.GetComponent<RectTransform>();
      if (rectTransform != null)
      {
         Vector3[] corners = new Vector3[4];
         rectTransform.GetWorldCorners(corners);
         Canvas canvas = rectTransform.GetComponentInParent<Canvas>();
         if (canvas != null && canvas.renderMode != RenderMode.WorldSpace)
         {
            if (targetCamera == null)
            {
               Debug.LogWarning("CameraFollow: Screen-space UI needs a targetCamera for bounds conversion.", this);
               return false;
            }

            float zDistance = Mathf.Abs(targetCamera.transform.position.z - rectTransform.position.z);
            for (int i = 0; i < corners.Length; i++)
            {
               corners[i] = targetCamera.ScreenToWorldPoint(new Vector3(corners[i].x, corners[i].y, zDistance));
            }
         }

         bounds = new Bounds(corners[0], Vector3.zero);
         for (int i = 1; i < corners.Length; i++)
         {
            bounds.Encapsulate(corners[i]);
         }
         return true;
      }

      return false;
   }
}
