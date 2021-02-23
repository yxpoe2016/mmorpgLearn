using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMinimap : MonoBehaviour
{
    private Collider MinimapBoundingBox;
    public Image minimap;

    public Image arrow;

    public Text mapName;

    private Transform playerTransform;

    // Use this for initialization
    void Start()
    {
        MinimapManager.Instance.minimap = this;
        UpdateMap();
    }

    public void UpdateMap()
    {
        this.mapName.text = User.Instance.CurrentMapData.Name;
        this.minimap.overrideSprite = MinimapManager.Instance.LoadCurrentMinimap();
        this.minimap.SetNativeSize();
        this.minimap.transform.localPosition = Vector3.zero;
        this.MinimapBoundingBox = MinimapManager.Instance.MinimapBoundingBox;
        this.playerTransform = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.playerTransform == null)
        {
            this.playerTransform = MinimapManager.Instance.PlayerTransform;
        }
        if (playerTransform == null || MinimapBoundingBox == null)
            return;
        float realWidth = MinimapBoundingBox.bounds.size.x;
        float realHeight = MinimapBoundingBox.bounds.size.z;

        //角色在地图的相对位置
        float relaX = playerTransform.position.x - MinimapBoundingBox.bounds.min.x;
        float relaY = playerTransform.position.z - MinimapBoundingBox.bounds.min.z;

        float pivotX = relaX / realWidth;
        float pivotY = relaY / realHeight;

        this.minimap.rectTransform.pivot = new Vector2(pivotX, pivotY);
        this.minimap.rectTransform.localPosition = Vector3.zero;
        this.arrow.transform.eulerAngles = new Vector3(0, 0, -playerTransform.eulerAngles.y);

    }
}
