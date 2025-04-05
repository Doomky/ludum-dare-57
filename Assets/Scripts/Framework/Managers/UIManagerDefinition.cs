using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Canvas = Framework.UI.Canvas;
using Framework.Core;

namespace Framework.Managers
{
    public class UIManagerDefinition : ManagerDefinition
    {
        [SerializeField, HideInPlayMode]
        private List<PrefabReference<Canvas>> _canvasPrefabs = null;

        public List<PrefabReference<Canvas>> CanvasPrefabs => this._canvasPrefabs;
    }
}