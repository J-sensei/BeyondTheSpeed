using UnityEngine;

public class Coin : MonoBehaviour
{
    public float RotateSpeed = 2f;
    GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find(GameObjectNames.GAME_MANAGER).GetComponent<GameManager>(); ;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * (RotateSpeed * Time.deltaTime));
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            AudioManager.Instance.PlayClip(AudioManager.Instance.CollectCoinClip);
            //gameManager.CollectCoin(1);
            Destroy(gameObject);
        }
    }
}
