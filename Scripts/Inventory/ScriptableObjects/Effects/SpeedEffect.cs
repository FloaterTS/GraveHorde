using System.Runtime.InteropServices;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;


[CreateAssetMenu(fileName = "New Speed Effect", menuName = "Item Effects/Consumable Effects/Speed Effect")]
public class SpeedEffect : ItemEffect
{
    public int speedModifier = 10;
    public float duration = 5f;
    
    private float initialSpeed;
    private IEnumerator coroutine;
    private static bool courStarted;
    public override void Use(GameObject player)
    {
        
        var character = player.GetComponent<CharacterControl>();
        initialSpeed = character.moveSpeed;
        Debug.Log(initialSpeed);
        character.CurrentSpeed = speedModifier;
        character.speedEffectOn = true;
        //character.StopAllCoroutines();

        if (courStarted == false)
        {
            coroutine = RemoveEffect(character, duration, initialSpeed);
            courStarted = true;
        }
        
        character.StopCoroutine(coroutine);
        coroutine = RemoveEffect(character, duration, initialSpeed);
        character.StartCoroutine(coroutine);
    }
    
    private static IEnumerator RemoveEffect(CharacterControl character, float duration, float initialSpeed)
    {
        yield return new WaitForSeconds(duration);
        character.CurrentSpeed = initialSpeed;
        character.speedEffectOn = false;
        Debug.Log("removed effect ");
    }
}
