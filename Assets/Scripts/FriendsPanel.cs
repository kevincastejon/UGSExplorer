using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace KevinCastejon.MultiplayerAPIExplorer
{
    public class FriendsPanel : UIPanel
    {
        private static FriendsPanel _instance;
        public static FriendsPanel Instance { get => _instance; }

        protected override void Awake()
        {
            base.Awake();
            _instance = this;
        }
    }
}