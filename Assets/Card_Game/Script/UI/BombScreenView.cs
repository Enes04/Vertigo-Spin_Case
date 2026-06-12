using System;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.UI
{
    public class BombScreenView : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject popupRoot; 
        [SerializeField] private Button giveUpButton; 
        [SerializeField] private Button reviveButton; 

        private Action _onGiveUpCallback;
        private Action _onReviveCallback;

        private void Start()
        {
            popupRoot.SetActive(false); 
            
            giveUpButton.onClick.AddListener(OnGiveUpClicked);
            reviveButton.onClick.AddListener(OnReviveClicked);
        }

        public void ShowBombScreen(Action onGiveUp, Action onRevive)
        {
            _onGiveUpCallback = onGiveUp;
            _onReviveCallback = onRevive;
            
            popupRoot.SetActive(true);
        }

        private void OnGiveUpClicked()
        {
            popupRoot.SetActive(false);
            _onGiveUpCallback?.Invoke(); 
        }

        private void OnReviveClicked()
        {
            popupRoot.SetActive(false);
            _onReviveCallback?.Invoke(); 
        }
    }
}