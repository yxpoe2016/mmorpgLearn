using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapManager : Singleton<MinimapManager> {

    public Sprite LoadCurrentMinimap()
    {
        return Resloader.Load<Sprite>("UI/Minimap/"+User.Instance.CurrentMapData.MiniMap);
    }
}
