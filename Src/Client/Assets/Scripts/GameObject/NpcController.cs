using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using Assets.Scripts.Models;
using Common.Data;
using UnityEngine;
using Random = UnityEngine.Random;

public class NpcController : MonoBehaviour
{
    public int npcId;
    private SkinnedMeshRenderer renderer;
    private Animator anim;
    private bool inInteractive = false;
    public NpcDefine npc;

    private Color orignColor;

    private NpcQuestStatus questStatus;
	// Use this for initialization
	void Start ()
    {
        renderer = this.gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
        anim = this.gameObject.GetComponent<Animator>();
        npc = NpcManager.Instance.GetNpcDefine(npcId);
        orignColor = renderer.sharedMaterial.color;
        this.StartCoroutine(Actions());

        RefreshNpcStatus();
        QuestManager.Instance.onQuestStatusChanged += OnQuestStatusChanged;
    }

    void OnQuestStatusChanged(Quest quest)
    {
        this.RefreshNpcStatus();
    }

    void RefreshNpcStatus()
    {
        questStatus = QuestManager.Instance.GetQuestStatusByNpc(this.npcId);
        UIWorldElementManager.Instance.AddNpcQuestStatus(transform, questStatus);
    }

    void OnDestroy()
    {
        QuestManager.Instance.onQuestStatusChanged -= OnQuestStatusChanged;
        if(UIWorldElementManager.Instance!=null)
            UIWorldElementManager.Instance.RemoveNpcQuestStatus(this.transform);
    }

    IEnumerator Actions()
    {
        while (true)
        {
            if (inInteractive)
                yield return new WaitForSeconds(2f);
            else
                yield return new WaitForSeconds(Random.Range(5f,10f));
            this.Relax();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Relax()
    {
        anim.SetTrigger("Relax");
    }

 

    void Interactive()
    {
        if (!inInteractive)
        {
            inInteractive = true;
            StartCoroutine(DoInteractive());
        }
    }

    IEnumerator DoInteractive()
    {
       
        yield return FacePlayer();
        if (NpcManager.Instance.Interactive(this.npcId))
        {
            anim.SetTrigger("Talk");
        }
        yield return new WaitForSeconds(3f);
        inInteractive = false;
    }

    IEnumerator FacePlayer()
    {
        Vector3 faceTo = (User.Instance.CurrentCharacterObject.transform.position - this.transform.position).normalized;
        while (Mathf.Abs(Vector3.Angle(this.gameObject.transform.forward,faceTo))>5)
        {
            this.gameObject.transform.forward =
                Vector3.Lerp(this.gameObject.transform.forward, faceTo, Time.deltaTime * 5f);
            yield return null;
        }
    }

    void OnMouseDown()
    {
        Debug.Log(npc.Name);
        Interactive();
    }

    private void OnMouseOver()
    {
        Highlight(true);
    }

    private void OnMouseEnter()
    {
        Highlight(true);
    }

    private void OnMouseExit()
    {
        Highlight(false);
    }

    private void Highlight(bool b)
    {
        if (b)
        {
            if(renderer.sharedMaterial.color!=Color.white)
                renderer.sharedMaterial.color = Color.white;
        }
        else
        {
            if (renderer.sharedMaterial.color != orignColor)
                renderer.sharedMaterial.color = orignColor;
        }
    }
}
