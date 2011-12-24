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
    class Collectible {

        public Rectangle boundingBox { get; protected set; }
        public SoundEffect pickupSound { get; protected set; }
        public int type { get; protected set; }     //0 = health
                                                    //1 = speed increase
                                                    //2 = poison fish (reverse controls)
                                                    //3 = invulnerability
        public Vector2 position { get; protected set; }
        AnimatedSprite sprite;
        Texture2D spriteMask;

        public Collectible(Texture2D mask, AnimatedSprite s, SoundEffect se, int t, int xPos){

            spriteMask = mask;
            sprite = s;
            pickupSound = se;
            type = t;
            position = new Vector2(xPos, -100);

            boundingBox = new Rectangle((int)position.X, (int)position.Y, spriteMask.Width, spriteMask.Height);

        }

        public void Update(GameTime gt, float verticalSpeed) {

            verticalSpeed -= 2;

            sprite.Update(gt);

            position = new Vector2(position.X, position.Y + verticalSpeed);

            boundingBox = new Rectangle((int)position.X, (int)position.Y, spriteMask.Width, spriteMask.Height);
        }

        public void Draw(SpriteBatch sb, GameTime gt) {

            sprite.Draw(gt, sb, position, 1);

        }
    }
}
