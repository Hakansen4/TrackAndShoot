using System;
using EasyClap.Seneca.StateMachine;
using Eflatun.CodePatterns;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Assertions;

namespace EasyClap.Seneca.Common.MenuManagement
{
    public class MenuManager : MonoBehaviour
    { 
        [SerializeField] private Menu[] menus;
        [SerializeField] private bool hasInitialMenu;
        [SerializeField, ShowIf(nameof(hasInitialMenu))] private int initialMenuIndex;
        
        private static MenuManager _instance;

        public static MenuManager Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = FindObjectOfType<MenuManager>(true);
                }

                return _instance;
            }
        }
        
        private Menu _activeMenu;

        private void Awake()
        {
            if (_instance && _instance != this)
            {
                Destroy(this);
                return;
            }

            _instance = this;
            
            Initialize();
        }

        private void Initialize()
        {
            Assert.IsTrue(!hasInitialMenu || (initialMenuIndex >= 0 && initialMenuIndex < menus.Length));
            
            for (int i = 0; i < menus.Length; i++)
            {
                var menu = menus[i];
                menus[i].gameObject.SetActive(true);
                menus[i].gameObject.SetActive(false);

                if (hasInitialMenu && initialMenuIndex == i)
                {
                    OpenMenuInternal(menu);
                }
            }
        }

        public void OpenMenu(Menu menu)
        {
            Assert.IsNotNull(menu);
            if (_activeMenu != menu)
            {
                OpenMenuInternal(menu);
            }
        }

        public void CloseMenu(Menu menu)
        {
            Assert.IsTrue(_activeMenu == menu);
            CloseMenuInternal();
        }
        
        private void OpenMenuInternal(Menu menu)
        {
            if (_activeMenu)
            {
                CloseMenuInternal();
            }
            
            Assert.IsNotNull(menu);
            
            _activeMenu = menu;
            _activeMenu.gameObject.SetActive(true);
            _activeMenu.OnOpen();
        }

        private void CloseMenuInternal()
        {
            Assert.IsNotNull(_activeMenu);
            _activeMenu.gameObject.SetActive(false);
            _activeMenu.OnClose();
            _activeMenu = null;
        }
    }
}
