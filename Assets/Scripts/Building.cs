using UnityEngine;
using TMPro;
using System.Collections; 

public class Building : MonoBehaviour
{
    public GameObject blockPrefab;
    public GameObject dustParticles;
    private AudioManager am;
    public int bricks = 0;
    public TextMeshProUGUI bricksText;
    bool canBuild;
    public float timeBetweenPlacement = .2f;
    private bool isHoldingMouse = false;

    private Vector2Int debugGridPosition;

    //Text Animation
    private Vector3 originalScale;
    public float animationDuration = 0.3f; // Duration of the animation
    public float scaleMultiplier = 1.2f;   // How much the text will grow

    private void Start() {
        am = FindObjectOfType<AudioManager>();
        bricksText.text = bricks.ToString();
        originalScale = bricksText.transform.localScale;
    }

    void Update()
    {
        if(canBuild) {

            if (Input.GetMouseButton(0))
            {
                if (!isHoldingMouse) 
                {
                    isHoldingMouse = true;
                    StartCoroutine(HoldToPlace());
                }
            }
            else
            {
                isHoldingMouse = false;
            }
        }


        bricksText.text = bricks.ToString();

        if(GameGlobals.Instance.globalMenuOpen) {
            canBuild = false;
        }else{
            canBuild = true;
        }
    }

    void PlaceBlock()
    {
        // Convert mouse position to world position
        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Convert world position to grid position
        // Vector2Int gridPosition = new Vector2Int(Mathf.FloorToInt(mouseWorldPosition.x), Mathf.FloorToInt(mouseWorldPosition.y));
        Vector2Int gridPosition = new Vector2Int(
            Mathf.RoundToInt(mouseWorldPosition.x - 0.5f),
            Mathf.RoundToInt(mouseWorldPosition.y - 0.5f)
        );
        
        // Check for existing block
        Collider2D existingBlock = Physics2D.OverlapBox(gridPosition + new Vector2(0.5f, 0.5f), Vector2.one, 0, LayerMask.GetMask("Block"));
        


        if (existingBlock != null)
        {
            Debug.Log("Already placed block here");
            return;
        }

        Instantiate(dustParticles, mouseWorldPosition, Quaternion.identity);
        am.Play("Construction");

        // Instantiate block at grid position
        Instantiate(blockPrefab, gridPosition + new Vector2(0.5f, 0.5f), Quaternion.identity);
        StartCoroutine(AnimateBricksText());
        bricks--;
    }




    IEnumerator AnimateBricksText()
    {
        float elapsed = 0f;
        Vector3 biggerScale = originalScale * scaleMultiplier;

        while (elapsed < animationDuration / 2)
        {
            elapsed += Time.deltaTime;
            bricksText.transform.localScale = Vector3.Lerp(originalScale, biggerScale, elapsed / (animationDuration / 2));
            yield return null;
        }

        elapsed = 0f;
        while (elapsed < animationDuration / 2)
        {
            elapsed += Time.deltaTime;
            bricksText.transform.localScale = Vector3.Lerp(biggerScale, originalScale, elapsed / (animationDuration / 2));
            yield return null;
        }

        bricksText.transform.localScale = originalScale; // Ensure it returns to its exact original scale
    }

    IEnumerator HoldToPlace() {
        while (isHoldingMouse)
        {
            if (bricks > 0)
            {
                PlaceBlock();
                yield return new WaitForSeconds(timeBetweenPlacement); // You can adjust the delay as needed
            }
            else
            {
                am.Play("Buzzer");
                yield return new WaitForSeconds(timeBetweenPlacement); // Prevent from spamming the buzzer sound
            }
        }
    }

    // void OnDrawGizmos()
    // {
    //     Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //     Vector2Int gridPosition = new Vector2Int(Mathf.FloorToInt(mouseWorldPosition.x), Mathf.FloorToInt(mouseWorldPosition.y));
    //     Vector2 exactPosition = gridPosition + new Vector2(0.5f, 0.5f);
        
    //     Gizmos.color = Color.green;
    //     Gizmos.DrawCube(exactPosition, Vector2.one);
    // }

    void OnDrawGizmos()
{
    // Draw the debug position
    Gizmos.color = Color.red;
    Gizmos.DrawWireCube(debugGridPosition + new Vector2(0.5f, 0.5f), Vector2.one * 0.95f);
}


}
