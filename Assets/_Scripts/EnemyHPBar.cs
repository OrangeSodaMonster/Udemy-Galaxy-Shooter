using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPBar : MonoBehaviour
{
    [SerializeField] Transform hpPrefab;
    [SerializeField] Vector3 hpPositionOffset = new(0,1.6f,0);
    public Vector3 PositionOffset => hpPositionOffset;
    [SerializeField] Color hpColor;
    public Color Color => hpColor;

    Transform hpInstance;
    Slider hpBar;
    EnemyHP enemyHP;
    Vector3 defaultPosOffset;
	
    void Awake()
    {
        enemyHP = GetComponent<EnemyHP>();

        defaultPosOffset = hpPositionOffset;
        enemyHP.TookDamage += DealWithBar;
        enemyHP.Healed += DealWithBar;        
    }

    private void OnDestroy()
    {
        enemyHP.TookDamage -= DealWithBar;
        enemyHP.Healed -= DealWithBar;        
    }

    private void OnEnable()
    {
        hpInstance = null;
        hpBar = null;

        hpPositionOffset = defaultPosOffset;
        Vector3 positiveLossy = new Vector3(Mathf.Abs(transform.lossyScale.x), Mathf.Abs(transform.lossyScale.y), Mathf.Abs(transform.lossyScale.z));
        hpPositionOffset.Scale(positiveLossy);
    }

    private void OnDisable()
    {
        if (hpInstance != null)
            hpInstance.gameObject.SetActive(false);
    }

    void Update()
    {
        if (hpInstance != null)
            hpInstance.position = transform.position + hpPositionOffset;
    }

    private void DealWithBar()
    {
        int MaxHP;
        if (GameManager.IsSurvival)
        {
            MaxHP = enemyHP.SurvivalMaxHP;
            //Debug.Log($"{gameObject.name}: {MaxHP}");
        }
        else
            MaxHP = enemyHP.MaxHP;

            if (enemyHP.CurrentHP >= MaxHP)
        {
            if (hpInstance != null)
                hpInstance.gameObject.SetActive(false);
        }
        else
        {
            if (hpInstance == null)
            {         
                hpInstance = PoolRefs.s_hpBarPool.GetPooledGameObject().transform;
                hpBar = hpInstance.GetComponentInChildren<Slider>();
                hpBar.fillRect.gameObject.GetComponent<Image>().color = hpColor;
                hpInstance.position = transform.position + hpPositionOffset;
                hpInstance.gameObject.SetActive(true);
            }
            
            hpBar.gameObject.SetActive(true);
            hpBar.value = enemyHP.CurrentHP / MaxHP;
        }
    }
}