using System;
using UnityEngine;
using UnityEngine.Events;

namespace Script.Manager
{
    public class MouseManager : MonoBehaviour
    {
        public static MouseManager instance;

        [SerializeField] private Texture2D point, doorway, attack, target, arrow;

        public event Action<Vector3> ONMouseClicked;

        private RaycastHit hitInfo;

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
            }

            instance = this;
        }

        private void Update()
        {
            SetCursorTexture();
            MouseControl();
        }

        private void SetCursorTexture()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hitInfo))
            {
                switch (hitInfo.collider.gameObject.tag)
                {
                    case "Ground":
                        Cursor.SetCursor(target, new Vector2(16, 16), CursorMode.Auto);
                        break;
                }
            }
        }

        private void MouseControl()
        {
            if (Input.GetMouseButtonDown(0) && hitInfo.collider != null)
            {
                if (hitInfo.collider.gameObject.CompareTag("Ground"))
                {
                    ONMouseClicked?.Invoke(hitInfo.point);
                }
            }
        }
    }
}