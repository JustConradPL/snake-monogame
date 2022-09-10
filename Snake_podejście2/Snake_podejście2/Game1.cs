using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Timers;
using System;
using System.Collections.Generic;
using System.IO;
namespace Snake
{

    struct Snake
    {
       public int x, y;       
    }
    struct apple
    {
        public int x, y;
    }
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        int minutes = int.Parse(File.ReadAllText("czas.txt"));
        int seconds = 0;
        List<int> wyniki = new List<int>();
        int wynik = 0;
        apple app;
        Timer aTime = new Timer();
        Timer aTime2 = new Timer(1000);
        Snake []s; 
        Texture2D wąż;
        Texture2D background;
        Texture2D apple;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        int size = 16;
        int dir,num;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Window.AllowUserResizing = true;
            Window.AllowAltF4 = false;
            base.Initialize();
        }
        int M, N;
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content. 
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            M = GraphicsDevice.Viewport.Height - size; ;
            N = GraphicsDevice.Viewport.Width - size ;
            wynik = 0;
            dir = 0;
            num = 1;
            Jabłko();
            apple = Content.Load<Texture2D>("kwadrat_czerwony");
            background = Content.Load<Texture2D>("kwadrat_szary");
            wąż = Content.Load<Texture2D>("kwadrat");
            s=new Snake[1000];
            aTime = new Timer(50);
            aTime.AutoReset = true;
            aTime.Enabled = true;
            aTime.Elapsed += Tick;
            aTime2.AutoReset = true;
            aTime2.Enabled = true;
            aTime2.Elapsed += Seconds;
            sf = Content.Load<SpriteFont>("score");
        }
        private void Seconds(object source,ElapsedEventArgs e)
        {
            if (minutes == 0 && seconds == 0)
            {
                aTime.Stop();
                aTime2.Stop();
                while (true)
                { }
            }
            if (seconds == 0)
            {
                minutes--;
                seconds = 59;
            }
            seconds--;
        }
        private void Tick(object source, ElapsedEventArgs e)
        {
         
          
            for (int i = num; i >0; --i)
            {
                s[i].x = s[i - 1].x;
                s[i].y = s[i - 1].y;
            }
            if (dir == 0) s[0].y += size;
            if (dir == 1) s[0].x -= size;
            if (dir == 2) s[0].x += size;
            if (dir == 3) s[0].y -= size;
        }
        private void Lose()
        {
            s = new Snake[1000];
            LoadContent();
        }
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
        private void Jabłko()
        {
            Random rnd = new Random();
            app.x = rnd.Next(0,GraphicsDevice.Viewport.Width / size)*size;
            app.y = rnd.Next(0, GraphicsDevice.Viewport.Height / size) * size ;

        }
        
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
            // TODO: Add your update logic here
           

            if (s[0].x == app.x && s[0].y == app.y) { wynik += 100; num+=1; Jabłko(); }
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.D)) dir = 2;
            if (state.IsKeyDown(Keys.A)) dir = 1;
            if (state.IsKeyDown(Keys.W)) dir = 3;
            if (state.IsKeyDown(Keys.S)) dir = 0;
            for (int i = 1; i <= num; i++)
            {
                if (s[0].x == s[i].x && s[0].y == s[i].y) 
                { wyniki.Add(wynik); wynik = 0; num = 1; }
            }
            if (s[0].x > N) s[0].x = 0; if (s[0].x < 0) s[0].x = N;
            if (s[0].y > M) s[0].y = 0; if (s[0].y < 0) s[0].y = M;
            base.Update(gameTime);
        }
        private SpriteFont sf;
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            
            spriteBatch.Begin();
            for (int Y = 0; Y < GraphicsDevice.Viewport.Height/size; Y++)
            {
                for (int X = 0; X < GraphicsDevice.Viewport.Width/size; X++)
                {
                    spriteBatch.Draw(background, new Rectangle(X * size, Y * size, size, size), Color.White);
                }
            }
            for (int i = 0; i < num; i++)
            {
                spriteBatch.Draw(wąż, new Rectangle(s[i].x, s[i].y, size, size), Color.Black);
            }
            spriteBatch.Draw(apple,new Rectangle(app.x,app.y,size,size), Color.White);
            spriteBatch.DrawString(sf, wynik.ToString(), new Vector2(GraphicsDevice.Viewport.Width / 2, 0), Color.DarkBlue);
            
            spriteBatch.DrawString(sf, "time left:", new Vector2(0, 0), Color.Red);
            spriteBatch.DrawString(sf, minutes.ToString() + ":" + seconds.ToString(), new Vector2(0, 32), Color.Red);
            /*
            for (int i = wyniki.Count-1; i >=0; i--)
            {
               
                spriteBatch.DrawString(sf,wyniki[i].ToString(),new Vector2(0,(a*(2*size))+(2*size)),Color.DarkBlue);
                a++;
            }
             * */
            spriteBatch.DrawString(sf,"Highscore",new Vector2(GraphicsDevice.Viewport.Width-150,0),Color.Salmon);
            spriteBatch.DrawString(sf, Max().ToString(), new Vector2(GraphicsDevice.Viewport.Width-100,size*3), Color.Salmon);
            spriteBatch.End();
            base.Draw(gameTime);
        }
        private int Max()
        {
            int max=0;
            foreach (int item in wyniki)
            {
                if (max < item) max = item;
            }
            return max;
        }
    }
}
