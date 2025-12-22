using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InputHandler : MonoBehaviour
{
    private Camera _mainCamera => Camera.main;


    public GameObject blackScreen;
    public GameObject shop;
    public GameObject inventory;
    public GameObject artifacts;
    public PlayerLoadout playerLoadout;
    public Info info;


    public void OnClick(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        var rayHit = Physics2D.GetRayIntersection(_mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()));
        if (!rayHit.collider) return;

        if (blackScreen.activeSelf) return;

        if (rayHit.collider.gameObject.name == "Shop")
        {
            OpenShop();
        }
        else if (rayHit.collider.gameObject.name == "Inventory")
        {
            OpenInventory();
        }
        else if (rayHit.collider.gameObject.name == "Play")
        {
            LoadGame();
        }
        else if (rayHit.collider.gameObject.name == "Artefacts")
        {
            OpenArtifacts();
        }
    }
    
    private void OpenShop()
    {
        shop.gameObject.SetActive(true);
        blackScreen.gameObject.SetActive(true);

        //EventManagerOld.SendShopOpened();
        EventManager.Trigger(GameEvents.ShopOpened);
    }
    private void OpenInventory()
    {
        inventory.gameObject.SetActive(true);
        blackScreen.gameObject.SetActive(true);

        //EventManagerOld.SendInventoryOpened();
        EventManager.Trigger(GameEvents.InventoryOpened);
    }
    private void LoadGame()
    {
        //EventManagerOld.SendGamePrepereToBeStarted();
            
        playerLoadout.FillPlayerLoadout(info);
        
        if (playerLoadout.activeItems[0] == StaticDatas.EMPTY_SLOT && playerLoadout.activeItems[1] == StaticDatas.EMPTY_SLOT )
        {
            Debug.Log("Нет оружия в руках!");
        }
        else
        {
            EventManager.Trigger(GameEvents.PrepareForGame);
            SceneManager.LoadScene("GameScene");
        }
    }
    private void OpenArtifacts()
    {
        artifacts.gameObject.SetActive(true);
        blackScreen.gameObject.SetActive(true);

        //EventManagerOld.SendArtifactsOpened();
        EventManager.Trigger(GameEvents.ArtifactsOpened);
    }
}
