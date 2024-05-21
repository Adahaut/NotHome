using Mirror.Examples.Chat;
using UnityEngine;

public class BuildInterractable : MonoBehaviour
{
    public GameObject _uiGameObject;

    public void OpenUiGameObject(PC _player)
    {
        _uiGameObject.SetActive(true);
        QG_Manager.Instance._playerController.GetComponent<Rigidbody>().velocity = new Vector3(0,
            QG_Manager.Instance._playerController.GetComponent<Rigidbody>().velocity.y, 0);

        QG_Manager.Instance._playerController.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        QG_Manager.Instance._textUi.enabled = false;

        if (_uiGameObject.name == "CraftUi")
        {
            _uiGameObject.GetComponent<UseCrafter>().SetPlayerInventory(_player.GetInventory());
        }
    }
    public void CloseUiGameObject()
    {
        _uiGameObject.SetActive(false);
        QG_Manager.Instance._playerController.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        QG_Manager.Instance._textUi.enabled = true;
        QG_Manager.Instance._isOpen = false;
    }
}
