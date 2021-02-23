using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapManager : Singleton<MinimapManager>
{
    public UIMinimap minimap;
    private Collider minimapBoundingBox;
    public Collider MinimapBoundingBox
    {
        get { return minimapBoundingBox; }
    }
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

    public void UpdateMinimap(Collider minimaoBoundingBox)
    {
        this.minimapBoundingBox = minimaoBoundingBox;
        if(this.minimap!=null)
            this.minimap.UpdateMap();
    }
}
