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
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public int gameState;   //0 is main menu
                                //1 is playing
                                //2 is help menu
                                //3 is paused in game
                                //4 is player death

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Level level;
        HUD hud;

        KeyboardState kbOld;

        SoundEffect menuSelect;

        public Game1(){

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            gameState = 0;

            graphics.PreferredBackBufferHeight = 800;
            graphics.PreferredBackBufferWidth = 480;
            graphics.ApplyChanges();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize(){

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent(){

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            menuSelect = Content.Load<SoundEffect>("Audio/UI/menuSelect");

            hud = new HUD(level);
            level = new Level(this, hud);
            hud.SetLevel(level);

            level.LoadContent(spriteBatch, Content);
            hud.LoadContent(spriteBatch, Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent(){
            
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime){

            KeyboardState kb = Keyboard.GetState();

            level.Update(gameTime, kb, gameState);
            hud.Update(gameTime, kb, gameState);

            if (gameState == 1) {
                if (!level.playerAlive) {
                    ChangeState(4);
                }
            }

            if (kb.IsKeyDown(Keys.Enter) && !(kbOld.IsKeyDown(Keys.Enter))) {

                if (gameState == 0 || gameState == 3 || gameState == 4) {
                    ChangeState(hud.GetSelectedState(gameState));
                    menuSelect.Play();
                }
                else if (gameState == 2) {
                    ChangeState(0);
                    menuSelect.Play();
                }
            }

            if (kb.IsKeyDown(Keys.Escape) && !(kbOld.IsKeyDown(Keys.Escape))){
                if(gameState == 0){
                    this.Exit();
                }
                else if(gameState == 1){
                    ChangeState(3);
                }
                else if (gameState == 2) {
                    ChangeState(0);
                }
                else if (gameState == 3) {
                    ChangeState(1);
                }
            }

            if (level.height > 20200000) {
                ChangeState(5);
            }

            kbOld = kb;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime){

            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);

            //Title menu
            if (gameState == 0) {
                level.Draw(gameTime, spriteBatch, gameState);
                hud.DrawMenu(spriteBatch, gameState);
            }
            //Playing mode
            else if (gameState == 1) {
                level.Draw(gameTime, spriteBatch, gameState);
                hud.Draw(gameTime, spriteBatch);
            }
            //Instructions
            else if (gameState == 2) {
                level.Draw(gameTime, spriteBatch, gameState);
                hud.DrawMenu(spriteBatch, gameState);
            }
            //Pause screen in game
            else if (gameState == 5) {
                hud.DrawMenu(spriteBatch, gameState);
            }
            else {
                level.Draw(gameTime, spriteBatch, gameState);
                hud.DrawMenu(spriteBatch, gameState);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void ChangeState(int newState) {
            if (newState == 0) {
                level.ResetLevel();
                hud.ResetHUD(newState);
                gameState = 0;
            }
            else if (newState == 1) {
                if(gameState == 0 || gameState == 4)
                    level.ResetLevel();
                hud.ResetHUD(newState);
                gameState = 1;
            }
            else if (newState == 2) {
                hud.ResetHUD(newState);
                gameState = 2;
            }
            else if (newState == 3) {
                hud.ResetHUD(newState);
                gameState = 3;
            }
            else if (newState == 4) {
                hud.ResetHUD(newState);
                gameState = 4;
            }
            else if (newState == 5) {
                gameState = 5;
                level.player.jetpackSoundEffect.Stop();
                level.ResetLevel();
                hud.ResetHUD(newState);
            }
            //Exit option
            else {
                this.Exit();
            }
        }
    }
}
