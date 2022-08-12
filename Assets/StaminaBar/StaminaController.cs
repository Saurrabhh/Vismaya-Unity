using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StaminaController : MonoBehaviour
{
    [Header("Stamina Main Parameters")]
    public float playerStamina = 100.0f;
    [SerializeField] public float maxStamina = 100.0f;
    [SerializeField] private float jumpCost = 5;
    [SerializeField] public float runCost = 10;
    [HideInInspector] public bool hasRegenerated = true;
    [HideInInspector] public bool weAreSprinting = false;

    [Header("Stamina Regen Parameters")]
    [Range(0, 50)][SerializeField] private float staminaDrain = 0.5f;
    [Range(0, 50)][SerializeField] private float staminaRegen = 0.75f;

    [Header("Stamina UI Elements")]
    [SerializeField] private Image staminaProgressUI = null;
    [SerializeField] private CanvasGroup sliderCanvasGroup = null;

    private StarterAssets.ThirdPersonController playerController;
    private StarterAssets.StarterAssetsInputs assetsInputs;

    private void Start()
    {
        playerController = GetComponent<StarterAssets.ThirdPersonController>();
    }

    private void Update()
    {
        StaminaRegen();
        
    }

    public void StaminaRegen()
    {
        if (!weAreSprinting)
        {
            if (playerStamina <= maxStamina - 0.01)
            {
                playerStamina += staminaRegen * Time.deltaTime;
                //update stamina
                UpdateStamina(1);

                if (playerStamina >= maxStamina)
                {
                    sliderCanvasGroup.alpha = 0;
                    hasRegenerated = true;
                }
            }
        }
    }
    public void Sprinting()
    {
       
            if (hasRegenerated)
            {
                weAreSprinting = false;
                playerStamina -= staminaDrain * Time.deltaTime;
                UpdateStamina(1);
                
                if (playerStamina <= 0)
                {
                    hasRegenerated = false;
                    //slow the player
                    
                    sliderCanvasGroup.alpha = 0;
                }
            }

        
        
    }
    public void StaminaJump()
    {
        if(playerStamina>= (maxStamina* jumpCost / maxStamina))
        {
            playerStamina -= jumpCost;
            //allow the player to jump
            playerController.PlayerJump();
            UpdateStamina(1);
        }
    }

    void UpdateStamina(int value)
    {
        staminaProgressUI.fillAmount = playerStamina / maxStamina;
        if (value == 0)
        {
            sliderCanvasGroup.alpha = 0;
        }
        else
        {
            sliderCanvasGroup.alpha = 1;
        }
    }
}
