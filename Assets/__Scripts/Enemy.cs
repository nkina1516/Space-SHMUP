using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof(BoundsCheck) )] 
public class Enemy : MonoBehaviour
{

     [Header("Inscribed")]
     public float speed    = 10f;   // The movement speed is 10m/s
     public float fireRate = 0.3f;  // Seconds/shot (Unused)
     public float health   = 10;    // Damage needed to destroy this enemy
     public int   score    = 100;   // Points earned for destroying this

     protected BoundsCheck bndCheck;

     void Awake() { 
       bndCheck = GetComponent<BoundsCheck>();
     }

     // This is a Property: A method that acts like a field
     public Vector3 pos {                                                       // a
         get {
             return this.transform.position;
         }
         set {
             this.transform.position = value;
         }
     }
 
     void Update() {
         Move();                                                                // b
     

    if ( bndCheck.LocIs( BoundsCheck.eScreenLocs.offDown ) ) {             // a
             Destroy( gameObject );
         }
         
    }
     public virtual void Move() { // c
         Vector3 tempPos = pos;
         tempPos.y -= speed * Time.deltaTime;
         pos = tempPos;
     }

     void OnCollisionEnter( Collision coll ) {
         GameObject otherGO = coll.gameObject;                                  // a
         if ( otherGO.GetComponent<ProjectileHero>() != null ) {                // b
             Destroy( otherGO );      // Destroy the Projectile
             Destroy( gameObject );   // Destroy this Enemy GameObject 
         } else {
             Debug.Log( "Enemy hit by non-ProjectileHero: " + otherGO.name );  // c
         }
     }

}
