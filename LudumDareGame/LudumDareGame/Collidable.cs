using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace LudumDareGame {
    class Collidable {

        public Texture2D spriteMask { get; protected set; }
        public AnimatedSprite animatedSprite { get; protected set; }
        public float speed { get; protected set; }
        public Vector2 position { get; protected set; }
        public int direction { get; protected set; }
        public Rectangle boundingBox { get; protected set; }
        
        SoundEffect soundEffect;

        private int oscillateHeight;
        private int oscillateDirection;

        public Collidable(Texture2D msk, AnimatedSprite aSprt, float spd, Vector2 pos, int dir, SoundEffect se) {
            spriteMask = msk;
            animatedSprite = aSprt;
            speed = spd;
            position = pos;
            direction = dir;
            soundEffect = se;

            if (soundEffect != null) {
                soundEffect.Play();
            }

            //Flips sprite if the direction is right to left. Need to do this here
            //instead of at draw time for per-pixel collision
            if (dir == -1) {
                spriteMask = Utility.Flip(spriteMask, false, true);
            }

            boundingBox = new Rectangle((int)position.X, (int)position.Y, spriteMask.Width, spriteMask.Height);

            oscillateHeight = 0;
            oscillateDirection = 0;
        }

        public void Update(GameTime gt, float verticalSpeed) {

            animatedSprite.Update(gt);

            position = new Vector2(position.X + speed * direction, position.Y + verticalSpeed);

            boundingBox = new Rectangle((int)position.X, (int)position.Y, spriteMask.Width, spriteMask.Height);
        }

        public void UpdateVertical(GameTime gt, float verticalSpeed) {

            if (oscillateDirection == 0) {
                Random random = new Random();
                int rNum = random.Next(10);
                if(rNum > 5){
                    oscillateDirection = -1;
                    position = new Vector2(position.X, position.Y + 900);
                }
                else{
                    oscillateDirection = 1;
                }
            }

            animatedSprite.Update(gt);

            position = new Vector2(position.X + speed * direction, position.Y + (verticalSpeed*oscillateDirection));

            boundingBox = new Rectangle((int)position.X, (int)position.Y, spriteMask.Width, spriteMask.Height);

        }

        public void UpdateOscillate(GameTime gt, float verticalSpeed) {

            animatedSprite.Update(gt);

            if (oscillateDirection == 0) {
                oscillateDirection = 1;
            }

            oscillateHeight += 1 * oscillateDirection;
            if (oscillateHeight > 10 || oscillateHeight < -10) {
                oscillateDirection *= -1;
            }
            position = new Vector2(position.X + speed * direction, position.Y + verticalSpeed + oscillateHeight);

            boundingBox = new Rectangle((int)position.X, (int)position.Y, spriteMask.Width, spriteMask.Height);
        }

        public void Draw(SpriteBatch sb, GameTime gt) {

            animatedSprite.Draw(gt, sb, position, direction);
        }
    }
}
