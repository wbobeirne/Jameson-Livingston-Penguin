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
    public class HUD {

        private Texture2D healthIconEmpty;
        private Texture2D healthIconFull;
        private Texture2D selectionArrow;
        private Texture2D titleScreenLogo;
        private Texture2D warning;
        private Texture2D endScreen;
        private SpriteFont heightFont;
        private Texture2D instructions;

        private Vector2 selectionArrowPosition;

        //Used to allow the menu to not constantly change when holding a key down
        private KeyboardState oldKb;

        private Level level;

        private SoundEffect menuMove;
        private Color textColor;

        //Because when I create level, I need to give it HUD and vice versa. Need to manually insert.
        public void SetLevel(Level l) {
            level = l;
        }

        public HUD(Level l) {
            level = l;

            selectionArrowPosition = new Vector2(40, 500);

            textColor = Color.Black;
        }

        public void LoadContent(SpriteBatch sb, ContentManager content) {

            heightFont = content.Load<SpriteFont>("Sprites/Fonts/HUDFont");
            selectionArrow = content.Load<Texture2D>("Sprites/UI/selectionArrow");
            titleScreenLogo = content.Load<Texture2D>("Sprites/UI/titleScreenLogo");
            healthIconEmpty = content.Load<Texture2D>("Sprites/UI/healthEmpty");
            healthIconFull = content.Load<Texture2D>("Sprites/UI/healthFull");
            warning = content.Load<Texture2D>("Sprites/UI/warning");
            endScreen = content.Load<Texture2D>("Sprites/UI/endscreen");
            instructions = content.Load<Texture2D>("Sprites/UI/instructions");

            menuMove = content.Load<SoundEffect>("Audio/UI/menuMove");

        }

        public void Update(GameTime gt, KeyboardState kb, int gs) {

            if (level.height > 100000) {
                textColor = Color.White;
            }
            else {
                textColor = Color.Black;
            }

            if ((kb.IsKeyDown(Keys.Up) || kb.IsKeyDown(Keys.W))
                && !(oldKb.IsKeyDown(Keys.Up) || oldKb.IsKeyDown(Keys.W))) {

                if (gs == 0) {
                    menuMove.Play();
                    if (selectionArrowPosition.Y > 500) {
                        selectionArrowPosition.Y -= 50;
                    }
                    else {
                        selectionArrowPosition.Y += 100;
                    }
                }
                //Pause and death are the same
                if (gs == 3 || gs == 4 || gs == 5) {
                    menuMove.Play();
                    if (selectionArrowPosition.Y == 300)
                        selectionArrowPosition.Y += 50;
                    else
                        selectionArrowPosition.Y -= 50;
                }

            }
            if ((kb.IsKeyDown(Keys.Down) || kb.IsKeyDown(Keys.S)) 
                && !(oldKb.IsKeyDown(Keys.Down) || oldKb.IsKeyDown(Keys.S))) {

                if (gs == 0) {
                    menuMove.Play();
                    if (selectionArrowPosition.Y < 600) {
                        selectionArrowPosition.Y += 50;
                    }
                    else {
                        selectionArrowPosition.Y -= 100;
                    }
                }
                //Pause and death are the same
                if (gs == 3 || gs == 4 || gs == 5) {
                    menuMove.Play();
                    if (selectionArrowPosition.Y == 300)
                        selectionArrowPosition.Y += 50;
                    else
                        selectionArrowPosition.Y -= 50;
                }
            }
            

            oldKb = kb;

        }

        public void Draw(GameTime gt, SpriteBatch sb) {

            if(level.height < 18500000){
                int h = 0;
                string heightString = null;

                if (level.height < 100000) {
                    h = (int)level.height;
                    heightString = h.ToString() + "m";
                }
                else{
                    h = (int)level.height/1000;
                    heightString = h.ToString() + "km";
                }

                //Height counter
                sb.DrawString(heightFont, heightString, new Vector2(470 - heightString.Length*24, 10), Color.Red); 

                //Health bars
                for (int i = 0; i < 3; i++) {
                    if (level.player.health > i) {
                        sb.Draw(healthIconFull, new Vector2(10 + (healthIconFull.Width * i) + 4 * i, 20), Color.White);
                    }
                    else {
                        sb.Draw(healthIconEmpty, new Vector2(10 + (healthIconFull.Width * i) + 4 * i, 20), Color.White);
                    }
                }
            }
        }

        public void DrawMenu(SpriteBatch sb, int gs) {

            if (gs == 0) {
                string start = "START";
                string exit = "EXIT";
                string inst = "INSTRUCTIONS";

                sb.Draw(titleScreenLogo, new Vector2(50, 50), Color.White);

                sb.DrawString(heightFont, start, new Vector2(240 - (start.Length * 24)/2, 500), textColor);
                sb.DrawString(heightFont, inst, new Vector2(240 - (inst.Length * 24)/2, 550), textColor);
                sb.DrawString(heightFont, exit, new Vector2(240 - (exit.Length * 24)/2, 600), textColor);

                sb.Draw(selectionArrow, 
                    new Vector2(selectionArrowPosition.X + selectionArrow.Width/2,
                        selectionArrowPosition.Y + selectionArrow.Height/4), Color.White);
            }

            if (gs == 2) {
                string menu = "RETURN TO MENU";

                sb.Draw(instructions, Vector2.Zero, Color.White);

                sb.DrawString(heightFont, menu, new Vector2(255 - (menu.Length * 24) / 2, 650), textColor);

                sb.Draw(selectionArrow,
                    new Vector2(selectionArrowPosition.X + selectionArrow.Width / 2,
                        selectionArrowPosition.Y + selectionArrow.Height / 4), Color.White) ;
            }

            if (gs == 3) {
                string pause = "PAUSE";
                string resume = "RESUME";
                string menu = "RETURN TO MENU";

                sb.DrawString(heightFont, pause, new Vector2(240 - (pause.Length * 24) / 2, 100), textColor);
                sb.DrawString(heightFont, resume, new Vector2(240 - (resume.Length * 24) / 2, 300), textColor);
                sb.DrawString(heightFont, menu, new Vector2(255 - (menu.Length * 24) / 2, 350), textColor);

                sb.Draw(selectionArrow,
                    new Vector2(selectionArrowPosition.X + selectionArrow.Width / 2,
                        selectionArrowPosition.Y + selectionArrow.Height / 4), Color.White);
            }

            if (gs == 4) {
                string restart = "RESTART";
                string menu = "RETURN TO MENU";
                string dead = "YOU HAVE DIED";

                sb.DrawString(heightFont, dead, new Vector2(255 - (dead.Length * 24) / 2, 100), textColor);
                sb.DrawString(heightFont, restart, new Vector2(240 - (restart.Length * 24) / 2, 300), textColor);
                sb.DrawString(heightFont, menu, new Vector2(255 - (menu.Length * 24) / 2, 350), textColor);

                sb.Draw(selectionArrow,
                    new Vector2(selectionArrowPosition.X + selectionArrow.Width / 2,
                        selectionArrowPosition.Y + selectionArrow.Height / 4), Color.White);
            }

            if (gs == 5) {
                string restart = "RESTART";
                string menu = "RETURN TO MENU";
                string dead = "CONGRATULATIONS!";

                sb.Draw(endScreen, Vector2.Zero, Color.White);

                sb.DrawString(heightFont, dead, new Vector2(255 - (dead.Length * 24) / 2, 100), Color.White);
                sb.DrawString(heightFont, restart, new Vector2(240 - (restart.Length * 24) / 2, 300), Color.White);
                sb.DrawString(heightFont, menu, new Vector2(255 - (menu.Length * 24) / 2, 350), Color.White);

                sb.Draw(selectionArrow,
                    new Vector2(selectionArrowPosition.X + selectionArrow.Width / 2,
                        selectionArrowPosition.Y + selectionArrow.Height / 4), Color.White);
            }
        }

        public void DrawWarningLabel(SpriteBatch sb, int yPos, int dir){

            int xPos;
            SpriteEffects se = SpriteEffects.None;

            if(dir == 1){
                xPos = 0;
                se = SpriteEffects.None;
            }
            else{
                xPos = 480 - warning.Width;
                se = SpriteEffects.FlipHorizontally;
            }

            if (yPos < 50)
                yPos = 50;

            sb.Draw(warning, new Vector2(xPos, yPos), null, Color.White, 0f, Vector2.Zero, 1f, se, 0.5f);

        }

        public int GetSelectedState(int gs) {

            int retState = 0;

            if (gs == 0) {
                if (selectionArrowPosition.Y == 500) {
                    retState = 1;
                }
                else if (selectionArrowPosition.Y == 550) {
                    retState = 2;
                }
                else {
                    //99 is the exit state
                    retState = 99;
                }
            }
            //Death and pause selection are the same
            else if (gs == 3 || gs == 4) {
                if (selectionArrowPosition.Y == 300) {
                    retState = 1;
                }
                if (selectionArrowPosition.Y == 350) {
                    retState = 0;
                }
            }

            return retState;
        }

        public void ResetHUD(int gs) {
            if (gs == 0) {
                selectionArrowPosition = new Vector2(40, 500);
            }
            else if (gs == 1) {

            }
            else if (gs == 2) {
                selectionArrowPosition = new Vector2(20, 650);
            }
            else {
                selectionArrowPosition = new Vector2(20, 300);
            }
        }
    }
}
