using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KevinCastejon.MultiplayerAPIExplorer
{
    public class VerticalLayoutFixer : MonoBehaviour
    {
        private void Start()
        {
            StartCoroutine(DebugCollapsing());

        }
        private IEnumerator DebugCollapsing()
        {
            yield return new WaitForSeconds(0.1f);
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)transform);
            yield return new WaitForSeconds(0.1f);
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)transform);
            yield return new WaitForSeconds(0.1f);
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)transform);
        }
    }
}