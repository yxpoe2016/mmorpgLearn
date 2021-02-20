﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapManager : Singleton<MinimapManager> {

    public Transform PlayerTransform
    {
        get
        {
            if (User.Instance.CurrentCharacterObject == null)
                return null;
            return User.Instance.CurrentCharacterObject.transform;
        }
    }

    public Sprite LoadCurrentMinimap()
    {
        return Resloader.Load<Sprite>("UI/Minimap/"+User.Instance.CurrentMapData.MiniMap);
    }
}
