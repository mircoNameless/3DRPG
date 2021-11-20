using System;
using Tools;
using UnityEngine;
using UnityEngine.Events;

namespace Script.Manager
{
    public class MouseManager : Singleton<MouseManager>
    {
        [SerializeField] private Texture2D point, doorway, attack, target, arrow;

        public event Action<Vector3> ONMouseClicked;
        public event Action<GameObject> OnEnemyClicked; 

        private RaycastHit hitInfo;

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(this);
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
                    case "Enemy":
                        Cursor.SetCursor(attack, new Vector2(16, 16), CursorMode.Auto);
                        break;
                    case "Portal":
                        Cursor.SetCursor(doorway, new Vector2(16, 16), CursorMode.Auto);
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

                if (hitInfo.collider.gameObject.CompareTag("Enemy"))
                {
                    OnEnemyClicked?.Invoke(hitInfo.collider.gameObject);
                }
                if (hitInfo.collider.gameObject.CompareTag("Attackable"))
                {
                    OnEnemyClicked?.Invoke(hitInfo.collider.gameObject);
                }
                
                if (hitInfo.collider.gameObject.CompareTag("Portal"))
                {
                    ONMouseClicked?.Invoke(hitInfo.point);
                }
                
            }
        }
    }
}