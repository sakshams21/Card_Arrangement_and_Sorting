using System;
using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
        [SerializeField] private Button MakeGroup_Btn;

        private void Start()
        {
                GameEventManager.OnToggleButton += ShowGroupBtn;
                MakeGroup_Btn.onClick.AddListener(GameEventManager.ButtonPressed);
        }

        private void OnDestroy()
        {
                GameEventManager.OnToggleButton -= ShowGroupBtn;
        }

        private void ShowGroupBtn(bool status)
        {
                MakeGroup_Btn.gameObject.SetActive(status);
        }
}