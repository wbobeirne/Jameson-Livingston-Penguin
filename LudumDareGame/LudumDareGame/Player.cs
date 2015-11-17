using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace LudumDareGame
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Player : Microsoft.Xna.Framework.GameComponent
    {

        public Vector2 position { get; protected set; }
        public float speed { get; set; }
        public Rectangle boundingBox { get; protected set; }
        public int health { get; set; }
        public int playerState { get; protected set; } //0 = normal
        //1 = damage taken, flashing and invincible
        public Texture2D spriteMask { get; protected set; }
        public AnimatedSprite spriteAnimated { get; protected set; }

        private Level level;

        private float movementSpeed;

        private int stateTimer;
        private const int damageRecoveryTime = 100;

        private SoundEffect jetpackSound;
        private SoundEffect damageSound;
        private SoundEffect deathSound;
        public SoundEffectInstance jetpackSoundEffect { get; protected set; } // Quick fix to turn the bastard off

        private Texture2D hat;
        private AnimatedSprite trailEffect;
        private Texture2D spaceHelmet;
        private Texture2D aviatorHat;

        public Player(Game game, Level l)
            : base(game){

            position = new Vector2(240, 700);
            level = l;
            speed = 5f;
            health = 3;
            stateTimer = 0;
            movementSpeed = 8;
            
        }

        public void LoadContent(SpriteBatch sb, ContentManager content){

            spriteMask = content.Load<Texture2D>("Sprites/Player/penguin");

            Texture2D sprAnim = content.Load<Texture2D>("Sprites/Player/penguinAnimated");
            spriteAnimated = new AnimatedSprite(sprAnim, new Point(32, 64), Point.Zero, new Point(4, 1), 20);

            jetpackSound = content.Load<SoundEffect>("Audio/Player/jetpack");
            jetpackSoundEffect = jetpackSound.CreateInstance();
            jetpackSoundEffect.IsLooped = true;
            jetpackSoundEffect.Volume = 0.1f;

            damageSound = content.Load<SoundEffect>("Audio/Player/damage");
            deathSound = content.Load<SoundEffect>("Audio/Player/death");

            aviatorHat = content.Load<Texture2D>("Sprites/Player/aviatorhat");
            spaceHelmet = content.Load<Texture2D>("Sprites/Player/spacehelmet");

            boundingBox = new Rectangle((int)position.X, (int)position.Y, spriteMask.Width, spriteMask.Height);
        }


        public void Update(GameTime gt, KeyboardState kb){

            boundingBox = new Rectangle((int)position.X, (int)position.Y, spriteMask.Width, spriteMask.Height);

            jetpackSoundEffect.Play();

            if (kb.IsKeyDown(Keys.Left) || kb.IsKeyDown(Keys.A)) {
                Move("left");
            }

            if (kb.IsKeyDown(Keys.Right) || kb.IsKeyDown(Keys.D)) {
                Move("right");
            }

            if (kb.IsKeyDown(Keys.Up) || kb.IsKeyDown(Keys.W)) {
                Move("up");
            }

            if (kb.IsKeyDown(Keys.Down) || kb.IsKeyDown(Keys.S)) {
                Move("down");
            }

            if (playerState == 1) {
                stateTimer++;
                if(stateTimer > damageRecoveryTime){
                    stateTimer = 0;
                    playerState = 0;
                }
            }

            spriteAnimated.Update(gt);

            base.Update(gt);
        }

        private void Move(string dir) {
            if (dir == "left") {
                if (position.X > 0) {
                    position = new Vector2(position.X - (movementSpeed * level.gameSpeed), position.Y);
                }
            }
            else if (dir == "right") {
                if (position.X < 480 - spriteMask.Width) {
                    position = new Vector2(position.X + (movementSpeed * level.gameSpeed), position.Y);
                }
            }
            else if (dir == "up") {
                if (position.Y > 0) {
                    position = new Vector2(position.X, position.Y - (movementSpeed * level.gameSpeed));
                }
            }
            else {
                if (position.Y < 800 - spriteMask.Height) {
                    position = new Vector2(position.X, position.Y + (movementSpeed * level.gameSpeed));
                }
            }
        }

        public void Draw(GameTime gt, SpriteBatch sb) {

            if (playerState == 0) {
                spriteAnimated.Draw(gt, sb, position, 1);
                if (hat != null) {
                    sb.Draw(hat, position, Color.White);
                }
            }
            else if (playerState == 1) {
                if (stateTimer % 2 == 1) {
                    spriteAnimated.Draw(gt, sb, position, 1);
                    if (hat != null) {
                        sb.Draw(hat, position, Color.White);
                    }
                }
            }

        }

        public void Damage() {
            if (playerState == 0) {

                health -= 1;
                playerState = 1;

                if (health >= 1) {
                    damageSound.Play();
                }

                if (health < 1) {
                    jetpackSoundEffect.Stop();
                    deathSound.Play();
                }

            }
        }

        public void ResetPlayer() {
            position = new Vector2(240, 700);
            speed = 5f;
            health = 3;
            stateTimer = 0;
            movementSpeed = 8;
            hat = null;
        }

        public void PickupCollectible(int type) {
            if (type == 0) {
                health += 1;
                if (health > 3)
                    health = 3;
            }
            if (type == 1) {
                level.gameSpeed += 0.05f;
                if (level.gameSpeed > 1.2f) {
                    level.gameSpeed = 1.2f;
                }
            }
        }

        public void EveningTransition() {
            if (speed < 50) {
                speed = speed * 10;
                hat = aviatorHat;
            }
        }

        public void SpaceTransition() {
            if (speed < 1000) {
                speed = speed * 100;
                hat = spaceHelmet;
            }
        }
    }
}
