using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerState { NeedWater, NeedMedicine, NeedRope }

public class PlayerStateController : MonoBehaviour
{
    public PlayerState currentState; // 当前状态
    public float ropeStateDelay = 3f; // 需要绳子的状态延迟时间

    public int currentFloor; // 角色所在的楼层
    public int[] ropesNeededPerFloor; // 每层需要的绳子数量（通过 Inspector 定义）

    public bool enableWaterLogic = false; // 是否启用水的逻辑
    public bool enableMedicineLogic = false; // 是否启用药物的逻辑

    public AudioClip receiveItemSound; // 角色接收物资后的音效
    public AudioClip slideSound; // 滑绳子时的音效
    private AudioSource audioSource;   // 音效播放器

    private int ropeNeeded;         // 当前楼层需要的绳子数量
    private int ropeCollected = 0;  // 当前楼层已收集的绳子数量

    private Transform wantRope;     // Rope子物体
    private Transform wantWater;    // Water子物体
    private Transform wantMedicine; // Medicine子物体
    public Text ropeText;          // WantRope 子物体中的 Text 组件

    public GameObject ropeObject; // 绳子对象（需在 Inspector 中指定）
    public SlideDownRope slideDownRope; // 滑绳子脚本引用

    private bool firstRoundComplete = false; // 是否完成了第一轮需求

    void Start()
    {
        if (ropesNeededPerFloor == null || currentFloor <= 0 || currentFloor > ropesNeededPerFloor.Length)
        {
            Debug.LogError("Invalid currentFloor or ropesNeededPerFloor is not set correctly!");
            return;
        }

        // 获取子物体
        wantRope = transform.Find("WantRope");
        wantWater = transform.Find("WantWater");
        wantMedicine = transform.Find("WantMedicine");

        if (wantRope != null)
        {
            ropeText = wantRope.GetComponentInChildren<Text>();
        }

        // 初始化音效播放器
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // 初始化隐藏绳子对象
        if (ropeObject != null)
        {
            ropeObject.SetActive(false);
        }

        InitializeFloorState();
        RandomizeInitialState();
    }

    void InitializeFloorState()
    {
        ropeNeeded = ropesNeededPerFloor[currentFloor - 1];
        ropeCollected = 0;

        if (currentState == PlayerState.NeedRope && ropeText != null)
        {
            UpdateRopeText();
        }

        Debug.Log($"Floor {currentFloor} initialized with {ropeNeeded} ropes needed.");
    }

    void RandomizeInitialState()
    {
        if (!firstRoundComplete && (enableWaterLogic || enableMedicineLogic))
        {
            if (enableWaterLogic && enableMedicineLogic)
            {
                currentState = (Random.value > 0.5f) ? PlayerState.NeedWater : PlayerState.NeedMedicine; // 随机设置为需要水或药物
            }
            else if (enableWaterLogic)
            {
                currentState = PlayerState.NeedWater; // 只需要水
            }
            else if (enableMedicineLogic)
            {
                currentState = PlayerState.NeedMedicine; // 只需要药物
            }
        }
        else
        {
            currentState = PlayerState.NeedRope; // 第二轮为需要绳子
        }

        UpdateStateUI();
    }

    public void ReceiveItem(GameObject bubble)
    {
        foreach (Transform child in bubble.transform)
        {
            if ((currentState == PlayerState.NeedWater && child.CompareTag("Water")) ||
                (currentState == PlayerState.NeedMedicine && child.CompareTag("Medicine")) ||
                (currentState == PlayerState.NeedRope && child.CompareTag("Rope")))
            {
                Debug.Log($"State satisfied: {currentState}");

                // 播放接收物资音效
                PlayReceiveItemSound();

                if (currentState == PlayerState.NeedRope)
                {
                    HandleRopeLogic();
                }
                else
                {
                    firstRoundComplete = true; // 标记第一轮完成
                    SetActiveState(false);
                    StartCoroutine(SwitchToNeedRopeState());
                }

                // 销毁泡泡
                DestroyBubble(bubble);

                return;
            }
        }

        Debug.Log("Item in bubble does not satisfy the current state.");
    }

    void HandleRopeLogic()
    {
        ropeCollected++;

        UpdateRopeText();

        if (ropeCollected >= ropeNeeded)
        {
            // 显示绳子并触发滑动逻辑
            if (ropeObject != null)
            {
                ropeObject.SetActive(true);
            }

            if (slideDownRope != null)
            {
                slideDownRope.StartSliding();
            }

            // 隐藏角色
            HideCharacter();

            SetActiveState(false);
        }
    }

    IEnumerator SwitchToNeedRopeState()
    {
        yield return new WaitForSeconds(ropeStateDelay);
        RandomizeInitialState();
    }

    void UpdateStateUI()
    {
        if (wantRope != null) wantRope.gameObject.SetActive(currentState == PlayerState.NeedRope);
        if (wantWater != null) wantWater.gameObject.SetActive(currentState == PlayerState.NeedWater && enableWaterLogic);
        if (wantMedicine != null) wantMedicine.gameObject.SetActive(currentState == PlayerState.NeedMedicine && enableMedicineLogic);

        if (currentState == PlayerState.NeedRope && ropeText != null)
        {
            UpdateRopeText();
        }
    }

    private void SetActiveState(bool isActive)
    {
        if (currentState == PlayerState.NeedWater && wantWater != null && enableWaterLogic)
        {
            wantWater.gameObject.SetActive(isActive);
        }
        else if (currentState == PlayerState.NeedMedicine && wantMedicine != null && enableMedicineLogic)
        {
            wantMedicine.gameObject.SetActive(isActive);
        }
        else if (currentState == PlayerState.NeedRope && wantRope != null)
        {
            wantRope.gameObject.SetActive(isActive);
        }
    }

    private void UpdateRopeText()
    {
        if (ropeText != null)
        {
            int displayedCollected = ropeCollected / 2;
            int displayedNeeded = ropeNeeded / 2;
            ropeText.text = $"{displayedCollected}/{displayedNeeded}";
        }
    }

    private void DestroyBubble(GameObject bubble)
    {
        foreach (Transform child in bubble.transform)
        {
            Destroy(child.gameObject);
        }

        Destroy(bubble);
    }

    private void PlayReceiveItemSound()
    {
        if (receiveItemSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(receiveItemSound);
        }
    }

    private void HideCharacter()
    {
        gameObject.SetActive(false); // 隐藏角色本体
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bubble"))
        {
            ReceiveItem(other.gameObject);
        }
    }
}
