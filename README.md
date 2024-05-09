# blue-archive-fan-game-project

------------------------------------------
**Why fan game?**
------------------------------------------
I initially was impressed by a horror-game concept art of a derivative work created by a blue archive user. That art became the first and biggest motivation for me to start creating a game. This game I am creating has quite different game design than the concept art that I saw, but blue archive is still being one of the motivations for me to create games. Hence, I decided to start working on making a fan game.

------------------------------------------
**game design?**
------------------------------------------
#about original design
Blue Archive game itself has game design of character gacha. There are stages, for each stage, the players can form a party of 4 strikers and 2 supporter characters - where strikers show up on screen, battle automatically, and supporter character does not show up nor battle, but supports striker characters with their stat + skills. The types are not conversible, strikers cannot be supporter and supporters cannot be striker.
Battles are automatic, only thing that player can interact during battle is when to use each character's ex skill on where, while each ex skill has their own cost. cost resource recovers by 1 every few seconds, up to maximum number 10.

#my game design plan
Just as the original game, I will be creating stages, forming different goals for each stage unlike original game.
The goal can be breaking an specific game object, taking a key and returning to a specific point, securing point(or wave defense), and reaching to target location, etc.
As original game, player will be forming a party, but the format will be a little different.
In my game, there will be max. 4 striker characters and 4 supporters, but supporters will be treated more like an equipment in other games.
each striker can take one supporter character, and that striker with supporter will be get increased stat + supporter ex skill depends on the supporter they took.
Combination of each striker & supporter can cause synergy, expecting it to be one of the parts where player will feel fun. To maximize the joy, there will be some specific stages where some special combinations are required to be cleared. (it can be little similar style to total assault content in original game.)

The stage will not be proceeded automatically, but instead, player will be playing in hyper TPS style.
There will be no cost resource, but instead each character will be having own cooldown for each skill they have.
The strikers in a party will not show up at the same time like original game, but instead, there will be one character and player can change to other party member whenever they want(except under specific condition). You can think of Genshin's player party system.

------------------------------------------
**programing architecture**
------------------------------------------
There is a parent class named 'Character' and 'Supporter'.
Striker characters' classes are the children classes of 'Character' class, and supporter characters' classes are the children classes of 'Supporter' class.
Each Character class has a public Supporter variable, and when this section is filled, the character will be affected by the Supporter variable.
ex) if Supporter A has:
```
[SerializeField]
[Range(0.0f, 1.0f)]
float MovingSpeedIncrease = 0.1f;
[SerializeField]
[Range(0.0f, 1.0f)]
float bulletIncrease = 0.1f;
[SerializeField]
[Range(0.0f, 1.0f)]
float DamageIncrease = 0.1f;
[SerializeField]
[Range(0.0f, 1.0f)]
float HpIncrease = 0.2f;
```

and if Striker B equips Supporter A, then B will has following kind of calculation:
```
void StatCalculation(){//will be called on void Start() since these are the basic stats
                       //of the striker character during the battle scene.
  MovingSpeed = appliedMovingSpeed*Time.deltaTime;
  //MaxBullet will be defined manually for each character
  //same for Damage
  //Also HP value
  if(SupporterA){
    MovingSpeed *= (1+SupporterA.MovingSpeedIncrease);
    MaxBullet *= (1+SupporterA.bulletIncrease);
    dmg *= (1+SupporterA.DamageIncrease);
    MaxHp *= (1+SupporterA.HpIncrease);
  }
  curBullet = MaxBUllet;

  curHp = MaxHp;
}
```

There is a PlayerMove script, and the PlayerMove script is going to manage all moving related features. Each Character GameObject has their own scripts, modeling, boolean value for each state and such, but they do not actually move in the Battle Scene. Instead, they will be a children component of the Player GameObject with PlayerMove script, and PlayerMove script will be used to move and jump, etc.

For each Character's modeling, there are unique BulletPos value. Whenever the player pushes Mouse0 button, the Bullet will be initiated from the BulletPos.position, and moves.
Since it is TPS, it does raycasthit from the Camera.main.ScreenToViewPortPoint(0.5f,0.5f), where is the centre of the game window. Max distance is mostly 500.0f, so if the raycasthit does not return any value, the bullet will be moving to the 500.0f location far away from the camera's centre. However, if raycasthit returns that if it collided with wall, floor, enemy or etc., the bullet will be moving to the position where raycasthit occurred.

For my game, player needs to aim in order to fire. When player is pushing Mouse1 button, the camera will get closer to the curCharacter and the curCharacter will be always facing front unless the player releases the Mouse1 button.

In this game, some modeling such as halos and cola can are from other assets, but other things are all created by myself.
I know there is an asset called Third person game starter asset, but I did not apply that asset to this game since I wanted to make sure I can create everything by myself so I have full understanding about all these before I start using such assets.
