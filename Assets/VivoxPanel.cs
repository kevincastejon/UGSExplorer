using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace KevinCastejon.MultiplayerAPIExplorer
{
    public class VivoxPanel : UIPanel
    {
        private static VivoxPanel _instance;
        public static VivoxPanel Instance { get => _instance; }

        protected override void Awake()
        {
            _instance = this;
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
