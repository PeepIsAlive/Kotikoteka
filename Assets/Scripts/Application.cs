using Modules.Managers;
using UI.Controllers;
using UnityEngine;
using Controllers;
using Core;

namespace DWS
{
    public static class Application
    {
        public static PopupViewManager PopupViewManager { get; private set; }
        public static RectTransform MainCanvasRect
        {
            get
            {
                return GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<RectTransform>();
            }
        }
        public static NpcType CurrentNpcType
        {
            get
            {
                var npcController = Object.FindObjectOfType<NpcController>();
                return npcController != null ? npcController.NpcType : NpcType.Cat;
            }
        }
        public static DropdownController DropdownController
        {
            get => Object.FindObjectOfType<DropdownController>();
        }

        static Application()
        {
            PopupViewManager = new PopupViewManager();
        }
    }
}
