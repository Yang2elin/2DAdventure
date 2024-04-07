using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatBar : MonoBehaviour
{
    public Image healthImage;
    public Image healthDelayImage;
    public Image powerImage;

    private void Update()
    {
        if (healthDelayImage.fillAmount > healthImage.fillAmount)
        {
            healthDelayImage.fillAmount -= Time.deltaTime;
        }
        if (healthDelayImage.fillAmount < healthImage.fillAmount)
        {
            healthDelayImage.fillAmount = healthImage.fillAmount;
        }
    }

    /// <summary>
    /// 接收health的变更百分比
    /// </summary>
    /// <param name="persetage">百分比：current/max</param>
    public void OnHealthChange(float persetage)
    {
        healthImage.fillAmount = persetage;
    }
}
