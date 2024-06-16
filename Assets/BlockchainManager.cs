using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Thirdweb;
using TMPro;
using UnityEngine.UI;
using System.Threading.Tasks;

public class BlockchainManager : MonoBehaviour
{
    public string Address { get; private set; }

    public GameObject tokenGatePanel;
    public GameObject mainMenuPanel;
    public GameObject shopPanel;

    public Button loginButton;
    public Button tokenGateBtn;
    public Text tokenGateBtnText;

    public Button playButton;
    public Button shopButton;

    public Button hpBtn;
    public Button pistolBtn;
    public Button machinegunBtn;
    public Button shotgunBtn;
    public Button uziBtn;

    public Text claimStatusText;

    private bool isPanelVisible = false;

    public GameObject player;
    public GameObject gameManager;
    public GameObject canvasManager;

    public Button reviveButton;
    public Text reviveButtonText;
    public Button rePlayButton;
    public Button claimTokenButton;
    public Text claimTokenButtonText;
    public Button rankingButton;
    public Text rankingButtonText;
    public Text tokenBalanceText;
    public Text rankText;
    public Text deadZombie;


    private void Start()
    {
        tokenGatePanel.SetActive(false);
        mainMenuPanel.SetActive(false);
        shopPanel.SetActive(false);
        loginButton.gameObject.SetActive(true);
    }

    private void Update()
    {
        Game_Manger gameManagerScript = gameManager.GetComponent<Game_Manger>();
        if (gameManagerScript != null)
        {
            deadZombie.text = gameManagerScript.Dead_Zombie.ToString();
        }
        else
        {
            Debug.LogError("deadZombie error");
        }
    }

    public async void GetTokenBalance()
    {
        var contract = ThirdwebManager.Instance.SDK.GetContract("0x25667D0b5a8Bc42C79748058736F5D0eF705018E");
        var balance = await contract.ERC20.BalanceOf(Address);
        tokenBalanceText.text = "Token owned: " + balance.displayValue;
    }

    public async void Login()
    {
        hpBtn.gameObject.SetActive(true);
        hpBtn.interactable = true;
        pistolBtn.gameObject.SetActive(true);
        pistolBtn.interactable = true;
        machinegunBtn.gameObject.SetActive(true);
        machinegunBtn.interactable = true;
        shotgunBtn.gameObject.SetActive(true);
        shotgunBtn.interactable = true;
        uziBtn.gameObject.SetActive(true);
        uziBtn.interactable = true;

        rankingButtonText.text = "Ranking Zombie Killing";
        reviveButtonText.text = "Revive";
        claimTokenButtonText.text = "Token";
        reviveButton.interactable = true;
        rePlayButton.interactable = true;
        claimTokenButton.interactable = true;
        rankingButton.interactable = false;
        reviveButton.gameObject.SetActive(true);
        rePlayButton.gameObject.SetActive(true);
        claimTokenButton.gameObject.SetActive(true);
        rankingButton.gameObject.SetActive(false);

        Address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
        Debug.Log(Address);
        Contract contract = ThirdwebManager.Instance.SDK.GetContract("0xC7D48242DA46B64872CD3F94Ced9663086Ff7424");
        List<NFT> nftList = await contract.ERC721.GetOwned(Address);
        if (nftList.Count == 0)
        {
            tokenGatePanel.SetActive(true);
            tokenGateBtn.gameObject.SetActive(true);
            tokenGateBtn.interactable = true;
        }
        else
        {
            mainMenuPanel.SetActive(true);
            playButton.gameObject.SetActive(true);
            shopButton.gameObject.SetActive(true);
            GetTokenBalance();
            GetRank();
        }
    }

    public async void ClaimNFTPass()
    {
        tokenGateBtnText.text = "Claiming...";
        tokenGateBtn.interactable = false;
        var contract = ThirdwebManager.Instance.SDK.GetContract("0xC7D48242DA46B64872CD3F94Ced9663086Ff7424");
        var result = await contract.ERC721.ClaimTo(Address, 1);
        tokenGateBtnText.text = "Claimed NFT Pass!";
        tokenGatePanel.SetActive(false);
        mainMenuPanel.SetActive(true);
        playButton.gameObject.SetActive(true);
        shopButton.gameObject.SetActive(true);
    }

    public void TogglePanelVisibility()
    {
        // Toggle the panel visibility state
        isPanelVisible = !isPanelVisible;

        // Set the panel active or inactive based on the new state
        shopPanel.SetActive(isPanelVisible);
    }

    private bool hpBtnOriginalState;
    private bool pistolBtnOriginalState;
    private bool machinegunBtnOriginalState;
    private bool shotgunBtnOriginalState;
    private bool uziBtnOriginalState;

    public async void ClaimHP()
    {
        claimStatusText.gameObject.SetActive(true);
        claimStatusText.text = "Claiming!";
        hpBtn.gameObject.SetActive(false);
        pistolBtn.interactable = false;
        machinegunBtn.interactable = false;
        shotgunBtn.interactable = false;
        uziBtn.interactable = false;
        playButton.interactable = false;
        shopButton.interactable = false;
        var contract = ThirdwebManager.Instance.SDK.GetContract("0x7685817E7D46AD0AdFba0b4f83fedd48c23f2cA4");
        var result = await contract.ERC20.Claim("1");
        //HP Added here
        // Get the Player script component attached to the Player GameObject
        Game_Manger gameManagerScript = gameManager.GetComponent<Game_Manger>();
        // Check if the Player script is found
        if (gameManagerScript != null)
        {
            // Change the value of Pistol_bullet to 20
            gameManagerScript.harts += 2;
            Debug.Log("harts + 2");
        }
        else
        {
            Debug.LogError("harts error");
        }

        Debug.Log("Token claimed");
        playButton.interactable = true;
        shopButton.interactable = true;
        hpBtn.interactable = true;
        pistolBtn.interactable = true;
        machinegunBtn.interactable = true;
        shotgunBtn.interactable = true;
        uziBtn.interactable = true;
        claimStatusText.text = "+2 Hearts";
        //+2
    }

    public async void ClaimPistol()
    {
        claimStatusText.gameObject.SetActive(true);
        claimStatusText.text = "Claiming!";
        hpBtn.interactable = false;
        pistolBtn.gameObject.SetActive(false);
        pistolBtn.interactable = false;
        machinegunBtn.interactable = false;
        shotgunBtn.interactable = false;
        uziBtn.interactable = false;
        playButton.interactable = false;
        shopButton.interactable = false;
        var contract = ThirdwebManager.Instance.SDK.GetContract("0x7685817E7D46AD0AdFba0b4f83fedd48c23f2cA4");
        var result = await contract.ERC20.Claim("1");

        // Get the Player script component attached to the Player GameObject
        Player playerScript = player.GetComponent<Player>();
        // Check if the Player script is found
        if (playerScript != null)
        {
            // Change the value of Pistol_bullet to 20
            playerScript.Pistol_bullet = 20;
            Debug.Log("Pistol_bullet value changed to 20");
        }
        else
        {
            Debug.LogError("Player script not found on the Player GameObject");
        }

        Debug.Log("Token claimed");
        playButton.interactable = true;
        shopButton.interactable = true;
        hpBtn.interactable = true;
        pistolBtn.interactable = true;
        machinegunBtn.interactable = true;
        shotgunBtn.interactable = true;
        uziBtn.interactable = true;
        claimStatusText.text = "Pistol Equipped!";
        //+20
    }

    public async void ClaimMachinegun()
    {
        claimStatusText.gameObject.SetActive(true);
        claimStatusText.text = "Claiming!";
        hpBtn.interactable = false;
        pistolBtn.interactable = false;
        machinegunBtn.gameObject.SetActive(false);
        machinegunBtn.interactable = false;
        shotgunBtn.interactable = false;
        uziBtn.interactable = false;
        playButton.interactable = false;
        shopButton.interactable = false;
        var contract = ThirdwebManager.Instance.SDK.GetContract("0x7685817E7D46AD0AdFba0b4f83fedd48c23f2cA4");
        var result = await contract.ERC20.Claim("1");

        // Get the Player script component attached to the Player GameObject
        Player playerScript = player.GetComponent<Player>();
        // Check if the Player script is found
        if (playerScript != null)
        {
            // Change the value of Pistol_bullet to 20
            playerScript.Machinegun_bullet = 20;
            Debug.Log("Pistol_bullet value changed to 20");
        }
        else
        {
            Debug.LogError("Player script not found on the Player GameObject");
        }

        Debug.Log("Token claimed");
        playButton.interactable = true;
        shopButton.interactable = true;
        hpBtn.interactable = true;
        pistolBtn.interactable = true;
        machinegunBtn.interactable = true;
        shotgunBtn.interactable = true;
        uziBtn.interactable = true;
        claimStatusText.text = "Machinegun Equipped!";
        //+20
    }

    public async void ClaimShotgun()
    {
        claimStatusText.gameObject.SetActive(true);
        claimStatusText.text = "Claiming!";
        hpBtn.interactable = false;
        pistolBtn.interactable = false;
        machinegunBtn.interactable = false;
        shotgunBtn.gameObject.SetActive(false);
        shotgunBtn.interactable = false;
        uziBtn.interactable = false;
        playButton.interactable = false;
        shopButton.interactable = false;
        var contract = ThirdwebManager.Instance.SDK.GetContract("0x7685817E7D46AD0AdFba0b4f83fedd48c23f2cA4");
        var result = await contract.ERC20.Claim("1");

        // Get the Player script component attached to the Player GameObject
        Player playerScript = player.GetComponent<Player>();
        // Check if the Player script is found
        if (playerScript != null)
        {
            // Change the value of Pistol_bullet to 20
            playerScript.Shotgun_bullet = 20;
            Debug.Log("Pistol_bullet value changed to 20");
        }
        else
        {
            Debug.LogError("Player script not found on the Player GameObject");
        }

        Debug.Log("Token claimed");
        playButton.interactable = true;
        shopButton.interactable = true;
        hpBtn.interactable = true;
        pistolBtn.interactable = true;
        machinegunBtn.interactable = true;
        shotgunBtn.interactable = true;
        uziBtn.interactable = true;
        claimStatusText.text = "Shotgun Equipped!";
        //+20
    }

    public async void ClaimUzi()
    {
        claimStatusText.gameObject.SetActive(true);
        claimStatusText.text = "Claiming!";
        hpBtn.interactable = false;
        pistolBtn.interactable = false;
        machinegunBtn.interactable = false;
        shotgunBtn.interactable = false;
        uziBtn.gameObject.SetActive(false);
        uziBtn.interactable = false;
        playButton.interactable = false;
        shopButton.interactable = false;
        var contract = ThirdwebManager.Instance.SDK.GetContract("0x7685817E7D46AD0AdFba0b4f83fedd48c23f2cA4");
        var result = await contract.ERC20.Claim("1");

        // Get the Player script component attached to the Player GameObject
        Player playerScript = player.GetComponent<Player>();
        // Check if the Player script is found
        if (playerScript != null)
        {
            // Change the value of Pistol_bullet to 20
            playerScript.Uzi_bullet = 20;
            Debug.Log("Pistol_bullet value changed to 20");
        }
        else
        {
            Debug.LogError("Player script not found on the Player GameObject");
        }

        Debug.Log("Token claimed");
        playButton.interactable = true;
        shopButton.interactable = true;
        hpBtn.interactable = true;
        pistolBtn.interactable = true;
        machinegunBtn.interactable = true;
        shotgunBtn.interactable = true;
        uziBtn.interactable = true;
        claimStatusText.text = "Uzi Equipped!";
        //+20
    }

    public async void RevivePlayer()
    {
        reviveButtonText.text = "Reviving!";
        reviveButton.interactable = false;
        rePlayButton.interactable = false;
        claimTokenButton.interactable = false;
        rankingButton.interactable = false;
        var contract = ThirdwebManager.Instance.SDK.GetContract("0x7685817E7D46AD0AdFba0b4f83fedd48c23f2cA4");
        var result = await contract.ERC20.Claim("1");

        Canvas_Manger canvasManagerScript = canvasManager.GetComponent<Canvas_Manger>();
        if (canvasManagerScript != null)
        {
            Game_Manger gameManagerScript = gameManager.GetComponent<Game_Manger>();
            if (gameManagerScript != null)
            {
                gameManagerScript.Dead_Zombie = 0;
            }
            else
            {
                Debug.LogError("Token error");
            }
            canvasManagerScript.Revive();
            Debug.Log("Revive");
        }
        else
        {
            Debug.LogError("Revive failed");
        }

        Debug.Log("Token claimed");
        rankingButtonText.text = "Ranking Zombie Killing";
        reviveButtonText.text = "Revive";
        claimTokenButtonText.text = "Token";
        reviveButton.interactable = true;
        rePlayButton.interactable = true;
        claimTokenButton.interactable = true;
        rankingButton.interactable = false;
        reviveButton.gameObject.SetActive(true);
        rePlayButton.gameObject.SetActive(true);
        claimTokenButton.gameObject.SetActive(true);
        rankingButton.gameObject.SetActive(false);
    }

    public async void ClaimingToken()
    {
        Game_Manger gameManagerScript = gameManager.GetComponent<Game_Manger>();
        if (gameManagerScript.Dead_Zombie == 0) {
            claimTokenButton.interactable = false;
            rankingButton.interactable = false;
            return;
        }
        claimTokenButtonText.text = "Claiming!";
        reviveButton.interactable = false;
        rePlayButton.interactable = false;
        claimTokenButton.interactable = false;
        rankingButton.interactable = false;
        var contract = ThirdwebManager.Instance.SDK.GetContract("0x25667D0b5a8Bc42C79748058736F5D0eF705018E");

        if (gameManagerScript != null)
        {
            var result = await contract.ERC20.Claim(gameManagerScript.Dead_Zombie.ToString());
            Debug.Log("Token Claimed");
            claimTokenButton.gameObject.SetActive(false);
            reviveButton.interactable = true;
            rePlayButton.interactable = true;
            rankingButton.interactable = true;
            rankingButton.gameObject.SetActive(true);
            claimTokenButtonText.text = "Token";
            GetTokenBalance();
        }
        else
        {
            Debug.LogError("Token error");
        }
    }

    public async void SubmitScore()
    {
        rankingButtonText.text = "Ranking!";
        reviveButton.interactable = false;
        rePlayButton.interactable = false;
        claimTokenButton.interactable = false;
        rankingButton.interactable = false;

        Game_Manger gameManagerScript = gameManager.GetComponent<Game_Manger>();
        if (gameManagerScript != null)
        {
            var contract = ThirdwebManager.Instance.SDK.GetContract(
                "0x9019e4a6eABc4B2a6919d09c7A74A1ee02560671",
                "[{\"type\":\"event\",\"name\":\"ScoreAddedd\",\"inputs\":[{\"type\":\"address\",\"name\":\"player\",\"indexed\":true,\"internalType\":\"address\"},{\"type\":\"uint256\",\"name\":\"score\",\"indexed\":false,\"internalType\":\"uint256\"}],\"outputs\":[],\"anonymous\":false},{\"type\":\"function\",\"name\":\"_scores\",\"inputs\":[{\"type\":\"address\",\"name\":\"\",\"internalType\":\"address\"}],\"outputs\":[{\"type\":\"uint256\",\"name\":\"\",\"internalType\":\"uint256\"}],\"stateMutability\":\"view\"},{\"type\":\"function\",\"name\":\"getRank\",\"inputs\":[{\"type\":\"address\",\"name\":\"player\",\"internalType\":\"address\"}],\"outputs\":[{\"type\":\"uint256\",\"name\":\"rank\",\"internalType\":\"uint256\"}],\"stateMutability\":\"view\"},{\"type\":\"function\",\"name\":\"submitScore\",\"inputs\":[{\"type\":\"uint256\",\"name\":\"score\",\"internalType\":\"uint256\"}],\"outputs\":[],\"stateMutability\":\"nonpayable\"}]"
            );
            await contract.Write("submitScore", (int)gameManagerScript.Dead_Zombie);
            reviveButton.interactable = true;
            rePlayButton.interactable = true;
            GetRank();
        }
        else
        {
            Debug.LogError("Token error");
        }
    }

    internal async void GetRank()
    {
        var contract = ThirdwebManager.Instance.SDK.GetContract(
            "0x9019e4a6eABc4B2a6919d09c7A74A1ee02560671",
            "[{\"type\":\"event\",\"name\":\"ScoreAddedd\",\"inputs\":[{\"type\":\"address\",\"name\":\"player\",\"indexed\":true,\"internalType\":\"address\"},{\"type\":\"uint256\",\"name\":\"score\",\"indexed\":false,\"internalType\":\"uint256\"}],\"outputs\":[],\"anonymous\":false},{\"type\":\"function\",\"name\":\"_scores\",\"inputs\":[{\"type\":\"address\",\"name\":\"\",\"internalType\":\"address\"}],\"outputs\":[{\"type\":\"uint256\",\"name\":\"\",\"internalType\":\"uint256\"}],\"stateMutability\":\"view\"},{\"type\":\"function\",\"name\":\"getRank\",\"inputs\":[{\"type\":\"address\",\"name\":\"player\",\"internalType\":\"address\"}],\"outputs\":[{\"type\":\"uint256\",\"name\":\"rank\",\"internalType\":\"uint256\"}],\"stateMutability\":\"view\"},{\"type\":\"function\",\"name\":\"submitScore\",\"inputs\":[{\"type\":\"uint256\",\"name\":\"score\",\"internalType\":\"uint256\"}],\"outputs\":[],\"stateMutability\":\"nonpayable\"}]"
            );
        var rank = await contract.Read<int>("getRank", Address);
        Debug.Log($"Rank for address {Address} is {rank}");
        rankText.text = rank.ToString();
    }

    public void ReplayGame() {
        reviveButton.interactable = false;
        claimTokenButton.interactable = false;
        rankingButton.interactable = false;
        Canvas_Manger canvasManagerScript = canvasManager.GetComponent<Canvas_Manger>();
        if (canvasManagerScript != null)
        {
            canvasManagerScript.REviv_Replay();

            rankingButtonText.text = "Ranking Zombie Killing";
            reviveButtonText.text = "Revive";
            claimTokenButtonText.text = "Token";
            reviveButton.interactable = true;
            rePlayButton.interactable = true;
            claimTokenButton.interactable = true;
            rankingButton.interactable = false;
            reviveButton.gameObject.SetActive(true);
            rePlayButton.gameObject.SetActive(true);
            claimTokenButton.gameObject.SetActive(true);
            rankingButton.gameObject.SetActive(false);

        }
        else
        {
            Debug.LogError("Replay failed");
        }
    }
}