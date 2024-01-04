using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class EnemyHPBar : MonoBehaviour
{
    [SerializeField] Transform hpPrefab;
    [SerializeField] Vector3 hpPositionOffset = new(0,1.6f,0);
    [SerializeField] Color hpColor;

    Transform hpInstance;
    Slider hpBar;
    EnemyHP enemyHP;
	
    void Awake()
    {
        enemyHP = GetComponent<EnemyHP>();

        enemyHP.TookDamage += DealWithBar;
        enemyHP.Healed += DealWithBar;
    }

    private void OnDestroy()
    {
        enemyHP.TookDamage -= DealWithBar;
        enemyHP.Healed -= DealWithBar;

        if(hpInstance != null)
        {
            Destroy(hpInstance.gameObject);
        }
    }

    void Update()
    {
        if (hpInstance != null)
            hpInstance.position = transform.position + hpPositionOffset;
    }

    private void DealWithBar()
    {
        //Debug.Log("DealWithBar chamado");

        if (enemyHP.CurrentHP >= enemyHP.MaxHP)
        {
            hpBar.gameObject.SetActive(false);
        }
        else
        {
            if (hpInstance == null)
            {
                hpPositionOffset.Scale(transform.lossyScale);

                hpInstance = Instantiate(hpPrefab, transform.position + hpPositionOffset, Quaternion.identity);
                hpBar = hpInstance.GetComponentInChildren<Slider>();
                hpBar.fillRect.gameObject.GetComponent<Image>().color = hpColor;
            }
            
            hpBar.gameObject.SetActive(true);
            hpBar.value = enemyHP.CurrentHP / enemyHP.MaxHP;

            //Debug.Log($"{enemyHP.CurrentHP}, {enemyHP.MaxHP}, {enemyHP.CurrentHP / enemyHP.MaxHP}");
        }
    }
}