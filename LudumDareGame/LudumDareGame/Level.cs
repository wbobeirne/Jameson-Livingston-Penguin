using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace LudumDareGame
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Level : Microsoft.Xna.Framework.GameComponent
    {

        public float height { get; protected set; }
        public Vector2 size { get; protected set; }
        public Player player { get; protected set; }
        public bool playerAlive { get; protected set; }
        public float gameSpeed { get; set; }

        private CommonResources resources;

        private Texture2D skyBg0;
        private Texture2D skyBg1;
        private Texture2D skyBg2;
        private Texture2D skyBg3;
        private Texture2D eveningBg0;
        private Texture2D eveningBg1;
        private Texture2D eveningBg2;
        private Texture2D eveningBg3;
        private Texture2D eveningTransitionBg0;
        private Texture2D eveningTransitionBg1;
        private Texture2D eveningTransitionBg2;
        private Texture2D eveningTransitionBg3;
        private Texture2D eveningTransitionBg4;
        private Texture2D eveningTransitionBg5;
        private Texture2D eveningTransitionBg6;
        private Texture2D eveningTransitionBg7;
        private Texture2D spaceBg0;
        private Texture2D spaceBg1;
        private Texture2D spaceBg2;
        private Texture2D spaceBg3;
        private Texture2D spaceTransitionBg0;
        private Texture2D spaceTransitionBg1;
        private Texture2D spaceTransitionBg2;
        private Texture2D spaceTransitionBg3;
        private Texture2D spaceTransitionBg4;
        private Texture2D spaceTransitionBg5;
        private Texture2D spaceTransitionBg6;
        private Texture2D spaceTransitionBg7;
        private Texture2D moon;
        //This is used to tell when to add a new background image
        private float oldHeight;
        private int transitionNum;

        private HUD hud;
        private Texture2D[] bgList = new Texture2D[9];
        private int[] bgOffsetList = new int[9];

        private List<Collidable> collidablesList = new List<Collidable>();
        private List<Collectible> collectiblesList = new List<Collectible>();

        public Level(Game game, HUD h)
            : base(game){

            height = 0;
            oldHeight = 0;
            player = new Player(game, this);
            hud = h;
            playerAlive = true;
            gameSpeed = 0.8f;
            transitionNum = 0;
            
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize(){

            base.Initialize();
        }

        public void LoadContent(SpriteBatch sb, ContentManager content) {

            resources = new CommonResources();

            skyBg0 = content.Load<Texture2D>("Sprites/Background/skybg0");
            skyBg1 = content.Load<Texture2D>("Sprites/Background/skybg1");
            skyBg2 = content.Load<Texture2D>("Sprites/Background/skybg2");
            skyBg3 = content.Load<Texture2D>("Sprites/Background/skybg3");
            eveningBg0 = content.Load<Texture2D>("Sprites/Background/eveningbg0");
            eveningBg1 = content.Load<Texture2D>("Sprites/Background/eveningbg1");
            eveningBg2 = content.Load<Texture2D>("Sprites/Background/eveningbg2");
            eveningBg3 = content.Load<Texture2D>("Sprites/Background/eveningbg3");
            eveningTransitionBg0 = content.Load<Texture2D>("Sprites/Background/eveningtransitionbg0");
            eveningTransitionBg1 = content.Load<Texture2D>("Sprites/Background/eveningtransitionbg1");
            eveningTransitionBg2 = content.Load<Texture2D>("Sprites/Background/eveningtransitionbg2");
            eveningTransitionBg3 = content.Load<Texture2D>("Sprites/Background/eveningtransitionbg3");
            eveningTransitionBg4 = content.Load<Texture2D>("Sprites/Background/eveningtransitionbg4");
            eveningTransitionBg5 = content.Load<Texture2D>("Sprites/Background/eveningtransitionbg5");
            eveningTransitionBg6 = content.Load<Texture2D>("Sprites/Background/eveningtransitionbg6");
            eveningTransitionBg7 = content.Load<Texture2D>("Sprites/Background/eveningtransitionbg7");
            spaceBg0 = content.Load<Texture2D>("Sprites/Background/spacebg0");
            spaceBg1 = content.Load<Texture2D>("Sprites/Background/spacebg1");
            spaceBg2 = content.Load<Texture2D>("Sprites/Background/spacebg2");
            spaceBg3 = content.Load<Texture2D>("Sprites/Background/spacebg3");
            spaceTransitionBg0 = content.Load<Texture2D>("Sprites/Background/spacetransitionbg0");
            spaceTransitionBg1 = content.Load<Texture2D>("Sprites/Background/spacetransitionbg1");
            spaceTransitionBg2 = content.Load<Texture2D>("Sprites/Background/spacetransitionbg2");
            spaceTransitionBg3 = content.Load<Texture2D>("Sprites/Background/spacetransitionbg3");
            spaceTransitionBg4 = content.Load<Texture2D>("Sprites/Background/spacetransitionbg4");
            spaceTransitionBg5 = content.Load<Texture2D>("Sprites/Background/spacetransitionbg5");
            spaceTransitionBg6 = content.Load<Texture2D>("Sprites/Background/spacetransitionbg6");
            spaceTransitionBg7 = content.Load<Texture2D>("Sprites/Background/spacetransitionbg7");
            moon = content.Load<Texture2D>("Sprites/Background/moon");

            InitializeBgArray();

            player.LoadContent(sb, content);
            hud.LoadContent(sb, content);
            resources.LoadContent(sb, content);

        }

        
        public void Update(GameTime gt, KeyboardState kb, int gs){

            //Title menu infinite scrolling
            if (gs == 0 || gs == 2) {
                height += 1f;

                if (oldHeight % 100 > height % 100) {
                    AddRandomBg("sky");
                }

                oldHeight = height;
            }

            //Actual gameplay
            if (gs == 1) {
                if (playerAlive) {
                    
                    height += player.speed * gameSpeed;

                    if (oldHeight % 100 > height % 100 && height < 12000) {
                        AddRandomBg("sky");
                    }
                    else if (oldHeight % 100 > height % 100 && height >= 12000 && height < 15000 && transitionNum < 8) {
                        TransitionBg("evening");
                    }
                    else if (oldHeight % 500 > height % 500 && height >= 12800 && height < 100000) {
                        if(height > 16000)
                            transitionNum = 0;
                        //The new transition stuff is REALLY hack, thanks LD48 with 4 hours left!
                        AddRandomBg("evening");
                    }
                    else if (oldHeight % 100000 > height % 100000 && height >= 100000 && height < 800000 && transitionNum < 8) {
                        TransitionBg("space");
                    }
                    else if (oldHeight % 100000 > height % 100000 && height >= 100800) {
                        transitionNum = 0;
                        AddRandomBg("space");
                    }

                    if (height < 17000000) {
                        if (oldHeight % (gameSpeed * player.speed * 80) > height % (gameSpeed * player.speed * 80)) {
                            Random r = new Random();
                            int rNum = r.Next(2);
                            if (rNum % 2 == 1)
                                collidablesList.Add(CreateRandomCollidable());
                        }
                        if (oldHeight % (gameSpeed * player.speed * 800) > height % (gameSpeed * player.speed * 800)) {
                            Random r = new Random();
                            int rNum = r.Next(100);
                            if (rNum > 75) {
                                collectiblesList.Add(resources.ReturnCollectibleFromType(0, r.Next(20, 440)));
                            }
                            else {
                                collectiblesList.Add(resources.ReturnCollectibleFromType(1, r.Next(20, 440)));
                            }
                        }
                    }

                    player.Update(gt, kb);

                    for (int i = 0; i < collidablesList.Count; i++) {
                        try {
                            if (height < 12000) {
                                collidablesList[i].Update(gt, 8 * gameSpeed);
                            }
                            else if (height < 100000) {
                                collidablesList[i].UpdateVertical(gt, 8 * gameSpeed);
                            }
                            else {
                                collidablesList[i].UpdateOscillate(gt, 6 * gameSpeed);
                            }

                            if (collidablesList[i].position.X > 1800 || collidablesList[i].position.X < -1300)
                                collidablesList.Remove(collidablesList[i]);

                            if (player.boundingBox.Intersects(collidablesList[i].boundingBox)) {
                                if(Utility.CheckPerPixel(player.spriteMask, collidablesList[i].spriteMask,
                                    player.position, collidablesList[i].position)){
                                    if (player.playerState == 0) {
                                        player.Damage();
                                    }
                                }
                            }
                        }
                        catch (ArgumentOutOfRangeException) {
                            //Sometimes it tries to access a plane that's been destroyed.
                            //A hacky fix, but that's what LD48 is about.
                        }
                    }

                    for (int i = 0; i < collectiblesList.Count; i++) {
                        try { //Same as above, crappy fixes
                            collectiblesList[i].Update(gt, 8 * gameSpeed);

                            if (collectiblesList[i].position.Y > 900)
                                collectiblesList.Remove(collectiblesList[i]);

                            if (player.boundingBox.Intersects(collectiblesList[i].boundingBox)) {
                                player.PickupCollectible(collectiblesList[i].type);
                                collectiblesList[i].pickupSound.Play();
                                collectiblesList.Remove(collectiblesList[i]);
                            }
                        }
                        catch (ArgumentOutOfRangeException) {

                        }
                    }

                    if (player.health < 1) {
                        KillPlayer();
                    }

                    oldHeight = height;
                }
                else {
                    hud.Update(gt, kb, gs);
                }
            }

            base.Update(gt);
        }


        public void Draw(GameTime gt, SpriteBatch sb, int gs) {

            if (height < 12800) {
                for (int i = 0; i < bgList.Length; i++) {
                    sb.Draw(bgList[i], new Vector2(bgOffsetList[i], i * 100 + (height % 100) - 100), Color.White);
                }
            }
            else if (height < 104000) {
                for (int i = 0; i < bgList.Length; i++) {
                    sb.Draw(bgList[i], new Vector2(bgOffsetList[i], i * 100 + (height % 500 / 5) - 100), Color.White);
                }
            }
            else {
                for (int i = 0; i < bgList.Length; i++) {
                    sb.Draw(bgList[i], new Vector2(bgOffsetList[i], i * 100 + ((height % 100000) / 1000) - 100), Color.White);
                }
            }

            if (height > 15000000) {
                sb.Draw(moon, new Vector2(0, (0 + height % 15000000 / 10000) - moon.Height), Color.White);
            }

            //Dont draw ameplay things at the menus
            if (gs == 1 || gs == 3) {

                foreach (Collectible col in collectiblesList) {
                    col.Draw(sb, gt);
                }

                foreach (Collidable col in collidablesList) {
                    col.Draw(sb, gt);
                    if(col.position.X < (0 - col.spriteMask.Width) && col.direction == 1)
                        hud.DrawWarningLabel(sb, (int)(col.position.Y - col.spriteMask.Height/2), 1);
                    else if(col.position.X > 480 && col.direction == -1)
                        hud.DrawWarningLabel(sb, (int)(col.position.Y - col.spriteMask.Height/2), -1);
                }
                player.Draw(gt, sb);
            }

            if (!playerAlive) {
                hud.DrawMenu(sb, 9);
            }

            if (height > 18700000) {
                sb.Draw(resources.bigBlank, Vector2.Zero, Color.White * (float)(height % 18700000 / 1000000));
            }

        }

        public void ResetLevel() {
            height = 0;
            oldHeight = 0;
            InitializeBgArray();
            player.ResetPlayer();
            playerAlive = true;
            collidablesList.Clear();
            gameSpeed = 0.7f;
            transitionNum = 0;
        }

        private void InitializeBgArray() {
            bgList[0] = skyBg0;
            bgList[1] = skyBg0;
            bgList[2] = skyBg0;
            bgList[3] = skyBg3;
            bgList[4] = skyBg0;
            bgList[5] = skyBg1;
            bgList[6] = skyBg0;
            bgList[7] = skyBg0;
            bgList[8] = skyBg2;
        }

        private void AddRandomBg(String type) {

            Random r = new Random();
            int rNum = r.Next(100);
            Texture2D bgToAdd = null;

            //If our list is full up, remove the one on the end, shift em down
            if(bgList[8] != null){
                bgList[8] = null;
                bgOffsetList[8] = 0;
                for(int i = 8; i > 0; i--){
                    bgList[i] = bgList[i-1];
                    bgOffsetList[i] = bgOffsetList[i-1];
                }
            }

            if (type == "sky") {
                if (rNum > 30)
                    bgToAdd = skyBg0;
                else if (rNum > 20)
                    bgToAdd = skyBg1;
                else if (rNum > 10)
                    bgToAdd = skyBg2;
                else
                    bgToAdd = skyBg3;
            }
            else if (type == "evening") {
                if (rNum > 30)
                    bgToAdd = eveningBg0;
                else if (rNum > 20)
                    bgToAdd = eveningBg1;
                else if (rNum > 10)
                    bgToAdd = eveningBg2;
                else
                    bgToAdd = eveningBg3;
            }
            else if (type == "space") {
                if (rNum > 50)
                    bgToAdd = spaceBg0;
                else if (rNum > 35)
                    bgToAdd = spaceBg1;
                else if (rNum > 20)
                    bgToAdd = spaceBg2;
                else
                    bgToAdd = spaceBg3;
            }

            rNum = r.Next(200);

            bgList[0] = bgToAdd;
            bgOffsetList[0] = (rNum *= -1);

        }

        private void TransitionBg(string type) {

            Texture2D bgToAdd = eveningBg0;

            bgList[8] = null;
            bgOffsetList[8] = 0;
            for(int i = 8; i > 0; i--){
                bgList[i] = bgList[i-1];
                bgOffsetList[i] = bgOffsetList[i-1];
            }

            if (type == "evening") {

                if (transitionNum == 0)
                    bgToAdd = eveningTransitionBg0;
                else if (transitionNum == 1)
                    bgToAdd = eveningTransitionBg1;
                else if (transitionNum == 2)
                    bgToAdd = eveningTransitionBg2;
                else if (transitionNum == 3)
                    bgToAdd = eveningTransitionBg3;
                else if (transitionNum == 4)
                    bgToAdd = eveningTransitionBg4;
                else if (transitionNum == 5)
                    bgToAdd = eveningTransitionBg5;
                else if (transitionNum == 6)
                    bgToAdd = eveningTransitionBg6;
                else if (transitionNum == 7) {
                    bgToAdd = eveningTransitionBg7;
                    player.EveningTransition();
                    transitionNum++;
                }

                transitionNum++;
            }
            else if (type == "space") {

                if (transitionNum == 0) {
                    bgToAdd = spaceTransitionBg0;
                    player.SpaceTransition();
                }
                else if (transitionNum == 1)
                    bgToAdd = spaceTransitionBg1;
                else if (transitionNum == 2)
                    bgToAdd = spaceTransitionBg2;
                else if (transitionNum == 3)
                    bgToAdd = spaceTransitionBg3;
                else if (transitionNum == 4)
                    bgToAdd = spaceTransitionBg4;
                else if (transitionNum == 5)
                    bgToAdd = spaceTransitionBg5;
                else if (transitionNum == 6)
                    bgToAdd = spaceTransitionBg6;
                else if (transitionNum == 7) {
                    bgToAdd = spaceTransitionBg7;
                    transitionNum++;
                }

                transitionNum++;
            }

            bgList[0] = bgToAdd;

        }

        private Collidable CreateRandomCollidable() {

            Collidable retCollidable = null;
            Random r = new Random();
            int rNum;
            int d;
            Vector2 p;
            float spd;
            Texture2D tex;
            AnimatedSprite aSprt;
            SoundEffect se;

            //Determine left of screen or right
            rNum = r.Next(10);
            if (rNum % 2 == 1) {
                d = 1;
            }
            else {
                d = -1;
            }
            //Determine speed
            rNum = r.Next((int)(8 * (gameSpeed*1.5)), (int)(12 * (gameSpeed*1.5)));
            spd = rNum;
            //Determine position
            if (d == 1) {
                p = new Vector2(r.Next((int)(-90 * spd), (int)(-60 * spd)), r.Next((int)player.position.Y - (600 + (int)(gameSpeed * 150)), (int)player.position.Y - (int)(gameSpeed * 150)));
            }
            else {
                p = new Vector2(r.Next((int)(80 * spd), (int)(110 * spd)), r.Next((int)player.position.Y - (600 + (int)(gameSpeed * 150)), (int)player.position.Y - (int)(gameSpeed*150)));
            }

            if (height < 12000) {
                tex = resources.ReturnRandomCollidableMask("sky");
            }
            else if (height < 100000) {
                tex = resources.ReturnRandomCollidableMask("evening");
            }
            else {
                tex = resources.ReturnRandomCollidableMask("space");
            }
            aSprt = resources.ReturnAnimatedSpriteFromMask(tex);
            se = resources.ReturnSoundEffectFromMask(tex);

            retCollidable = new Collidable(tex, aSprt, spd, p, d, se);

            return retCollidable;

        }

        private void KillPlayer(){
            playerAlive = false;
            //HUD reset for pause is the same for death
            hud.ResetHUD(3);
        }
    }
}
