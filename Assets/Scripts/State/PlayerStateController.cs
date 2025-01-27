using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerState { NeedWater, NeedMedicine, NeedRope }

public class PlayerStateController : MonoBehaviour
{
    public PlayerState currentState; // ��ǰ״̬
    public float ropeStateDelay = 3f; // ��Ҫ���ӵ�״̬�ӳ�ʱ��

    public int currentFloor; // ��ɫ���ڵ�¥��
    public int[] ropesNeededPerFloor; // ÿ����Ҫ������������ͨ�� Inspector ���壩

    public bool enableWaterLogic = false; // �Ƿ�����ˮ���߼�
    public bool enableMedicineLogic = false; // �Ƿ�����ҩ����߼�

    public AudioClip receiveItemSound; // ��ɫ�������ʺ����Ч
    public AudioClip slideSound; // ������ʱ����Ч
    private AudioSource audioSource;   // ��Ч������

    private int ropeNeeded;         // ��ǰ¥����Ҫ����������
    private int ropeCollected = 0;  // ��ǰ¥�����ռ�����������

    private Transform wantRope;     // Rope������
    private Transform wantWater;    // Water������
    private Transform wantMedicine; // Medicine������
    public Text ropeText;          // WantRope �������е� Text ���

    public GameObject ropeObject; // ���Ӷ������� Inspector ��ָ����
    public SlideDownRope slideDownRope; // �����ӽű�����

    private bool firstRoundComplete = false; // �Ƿ�����˵�һ������

    void Start()
    {
        if (ropesNeededPerFloor == null || currentFloor <= 0 || currentFloor > ropesNeededPerFloor.Length)
        {
            Debug.LogError("Invalid currentFloor or ropesNeededPerFloor is not set correctly!");
            return;
        }

        // ��ȡ������
        wantRope = transform.Find("WantRope");
        wantWater = transform.Find("WantWater");
        wantMedicine = transform.Find("WantMedicine");

        if (wantRope != null)
        {
            ropeText = wantRope.GetComponentInChildren<Text>();
        }

        // ��ʼ����Ч������
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // ��ʼ���������Ӷ���
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
                currentState = (Random.value > 0.5f) ? PlayerState.NeedWater : PlayerState.NeedMedicine; // �������Ϊ��Ҫˮ��ҩ��
            }
            else if (enableWaterLogic)
            {
                currentState = PlayerState.NeedWater; // ֻ��Ҫˮ
            }
            else if (enableMedicineLogic)
            {
                currentState = PlayerState.NeedMedicine; // ֻ��Ҫҩ��
            }
        }
        else
        {
            currentState = PlayerState.NeedRope; // �ڶ���Ϊ��Ҫ����
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

                // ���Ž���������Ч
                PlayReceiveItemSound();

                if (currentState == PlayerState.NeedRope)
                {
                    HandleRopeLogic();
                }
                else
                {
                    firstRoundComplete = true; // ��ǵ�һ�����
                    SetActiveState(false);
                    StartCoroutine(SwitchToNeedRopeState());
                }

                // ��������
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
            // ��ʾ���Ӳ����������߼�
            if (ropeObject != null)
            {
                ropeObject.SetActive(true);
            }

            if (slideDownRope != null)
            {
                slideDownRope.StartSliding();
            }

            // ���ؽ�ɫ
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
        gameObject.SetActive(false); // ���ؽ�ɫ����
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bubble"))
        {
            ReceiveItem(other.gameObject);
        }
    }
}
