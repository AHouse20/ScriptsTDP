using UnityEngine;

public class VFXAutoDestroy : MonoBehaviour
{
    [SerializeField] private float maxLifeTime;
    private float lifeTime;

    private void OnEnable()
    {
        lifeTime = maxLifeTime;
    }
    // Update is called once per frame
    void Update()
    {
        if(lifeTime < 0)
        {
            DestroySelf();
        }
        else
        {
            lifeTime -= Time.deltaTime;
        }
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
