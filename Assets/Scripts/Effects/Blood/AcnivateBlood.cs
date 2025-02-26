using UnityEngine;

public class AcnivateBlood : MonoBehaviour
{
   
   public ParticleSystem bsa;

    void Update()
    {
        if(Input.GetKey(KeyCode.P))
        bsa.Play();
    }
   
}
