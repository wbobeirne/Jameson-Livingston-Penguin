using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace LudumDareGame {
    class CommonResources {

        Texture2D collidableSky00Mask;
        AnimatedSprite collidableSky00Animated;
        SoundEffect collidableSky00Sound;

        Texture2D collidableEvening00Mask;
        AnimatedSprite collidableEvening00Animated;
        SoundEffect collidableEvening00Sound;

        Texture2D collidableSpace00Mask;
        AnimatedSprite collidableSpace00Animated;
        SoundEffect collidableSpace00Sound;

        Texture2D collectibleHealthMask;
        AnimatedSprite collectibleHealthAnimated;
        SoundEffect collectibleHealthSound;

        Texture2D collectibleSpeedMask;
        AnimatedSprite collectibleSpeedAnimated;
        SoundEffect collectibleSpeedSound;

        public Texture2D bigBlank { get; protected set; }

        public CommonResources() {

        }

        public void LoadContent(SpriteBatch sb, ContentManager content) {

            collidableSky00Mask = content.Load<Texture2D>("Sprites/Collidables/plane1mask");
            Texture2D sky00Anim = content.Load<Texture2D>("Sprites/Collidables/plane1animated");
            collidableSky00Animated = new AnimatedSprite(sky00Anim, new Point(196, 84), Point.Zero, new Point(4, 1), 25);
            collidableSky00Sound = content.Load<SoundEffect>("Audio/Collidables/plane1");

            collidableEvening00Mask = content.Load<Texture2D>("Sprites/Collidables/jetmask");
            Texture2D evening00Anim = content.Load<Texture2D>("Sprites/Collidables/jetanimated");
            collidableEvening00Animated = new AnimatedSprite(evening00Anim, new Point(196, 84), Point.Zero, new Point(4, 1), 25);
            collidableEvening00Sound = content.Load<SoundEffect>("Audio/Collidables/jet");

            collidableSpace00Mask = content.Load<Texture2D>("Sprites/Collidables/ufo");
            Texture2D Space00Anim = content.Load<Texture2D>("Sprites/Collidables/ufoanimation");
            collidableSpace00Animated = new AnimatedSprite(Space00Anim, new Point(196, 84), Point.Zero, new Point(4, 1), 25);
            collidableSpace00Sound = content.Load<SoundEffect>("Audio/Collidables/ufo");

            collectibleHealthMask = content.Load<Texture2D>("Sprites/Collectibles/healthPickup");
            collectibleHealthAnimated = new AnimatedSprite(collectibleHealthMask, new Point(24, 48), Point.Zero, new Point(1, 1), 1000);
            collectibleHealthSound = content.Load<SoundEffect>("Audio/Collectibles/health");

            collectibleSpeedMask = content.Load<Texture2D>("Sprites/Collectibles/speedPickup");
            collectibleSpeedAnimated = new AnimatedSprite(collectibleSpeedMask, new Point(48, 48), Point.Zero, new Point(1, 1), 1000);
            collectibleSpeedSound = content.Load<SoundEffect>("Audio/Collectibles/speed");

            bigBlank = content.Load<Texture2D>("Sprites/UI/bigblank");

        }

        public Texture2D ReturnRandomCollidableMask(string theme) {

            Texture2D retTex = null;

            Random r = new Random();
            int rNum = r.Next(100);

            if (theme == "sky") {
                if (rNum >= 0) {
                    retTex = collidableSky00Mask;
                }
            }
            else if (theme == "evening") {
                retTex = collidableEvening00Mask;
            }
            else if (theme == "space") {
                if (rNum >= 0) {
                    retTex = collidableSpace00Mask;
                }
            }

            return retTex;
        }

        public AnimatedSprite ReturnAnimatedSpriteFromMask(Texture2D mask) {
            AnimatedSprite retSprite = null;

            if (mask == collidableSky00Mask) {
                retSprite = collidableSky00Animated;
            }
            else if (mask == collidableEvening00Mask) {
                retSprite = collidableEvening00Animated;
            }
            else if (mask == collidableSpace00Mask) {
                retSprite = collidableSpace00Animated;
            }
            else if (mask == collectibleHealthMask) {
                retSprite = collectibleHealthAnimated;
            }
            else if (mask == collectibleSpeedMask) {
                retSprite = collectibleSpeedAnimated;
            }

            return retSprite;
        }

        public SoundEffect ReturnSoundEffectFromMask(Texture2D mask) {
            SoundEffect retSound = null;

            if (mask == collidableSky00Mask) {
                retSound = collidableSky00Sound;
            }
            else if (mask == collidableEvening00Mask) {
                retSound = collidableEvening00Sound;
            }
            else if (mask == collidableSpace00Mask) {
                retSound = collidableSpace00Sound;
            }
            else if (mask == collectibleHealthMask) {
                retSound = collectibleHealthSound;
            }
            else if (mask == collectibleSpeedMask) {
                retSound = collectibleSpeedSound;
            }

            return retSound;
        }

        public Texture2D ReturnCollectibleMaskFromType(int type) {
            //0 is health
            //1 is speed
            //2 is sick fish
            //3 is invinc
            Texture2D retMask = null;

            if (type == 0) {
                retMask = collectibleHealthMask;
            }
            if (type == 1) {
                retMask = collectibleSpeedMask;
            }

            return retMask;
        }

        public Collectible ReturnCollectibleFromType(int type, int xPos) {
            Collectible retCol;
            Texture2D mask;

            mask = ReturnCollectibleMaskFromType(type);
            retCol = new Collectible(mask, ReturnAnimatedSpriteFromMask(mask),
                ReturnSoundEffectFromMask(mask), type, xPos);

            return retCol;
        }
    }
}
