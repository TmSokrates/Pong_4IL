using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Pong4IL
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        int wysokoscOkna;
        int szerokoscOkna;

        Texture2D teksturaPaletki;
        Texture2D teksturaPaletkiPrzeciwnika;
        Rectangle paletka;
        Rectangle paletkaPrzeciwnika;

        int speed = 2;

        Texture2D teksturaSciana;
        Rectangle scianaPrawa;
        Rectangle scianaLewa;
        Rectangle scianaGorna;
        Rectangle scianaDolna;

        Texture2D teksturaAdriana;
        Texture2D teksturaDamiana;
        Texture2D aktualnaTeksturaPilki;

        Rectangle pilka;

        bool ruchPD = false;
        bool ruchPG = false;
        bool ruchLG = false;
        bool ruchLD = false;

        int dx = 2;
        int dy = 1;

        int ruchStart = 0;

        SpriteFont napisPunkty;
        int punktyGracza = 0;
        int punktyPrzeciwnika = 0;

        int pilkaPoczX;
        int pilkaPoczY;

        int paletkaGraczPoczX;
        int paletkaGraczPoczY;
        int paletkaPrzeciwnikaPoczX;
        int paletkaPrzeciwnikaPoczY;

        bool czyGramy = false;

        int cel = 10;

        string info = "Wygrales";

        SoundEffect effect;

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
            //paletka=new Rectangle()
            szerokoscOkna = GraphicsDevice.Viewport.Width;
            wysokoscOkna = GraphicsDevice.Viewport.Height;
            paletka = new Rectangle(szerokoscOkna / 10, wysokoscOkna / 2, szerokoscOkna / 100, wysokoscOkna / 10);
            paletkaGraczPoczX = paletka.X;
            paletkaGraczPoczY = paletka.Y;
            paletkaPrzeciwnika = new Rectangle(szerokoscOkna - (szerokoscOkna / 10), wysokoscOkna / 2, szerokoscOkna / 100, wysokoscOkna / 10);
            paletkaPrzeciwnikaPoczX = paletkaPrzeciwnika.X;
            paletkaPrzeciwnikaPoczY = paletkaPrzeciwnika.Y;
            scianaPrawa = new Rectangle((szerokoscOkna * 98) / 100, 0, (szerokoscOkna * 2) / 100, wysokoscOkna);
            scianaLewa = new Rectangle(0, 0, (szerokoscOkna * 2) / 100, wysokoscOkna);
            scianaGorna = new Rectangle(0, 0, szerokoscOkna, (wysokoscOkna * 4) / 100);
            scianaDolna = new Rectangle(0, (wysokoscOkna * 96) / 100, szerokoscOkna, (wysokoscOkna * 5) / 100);
            pilka = new Rectangle((szerokoscOkna / 2) - (paletka.Height / 4), wysokoscOkna / 10, paletka.Height / 3, paletka.Height / 3);
            pilkaPoczX = pilka.X;
            pilkaPoczY = pilka.Y;
            Random random = new Random();
            ruchStart = random.Next(0, 10);
            if (ruchStart % 2 == 0)
            {
                ruchPD = true;
            }
            else
            {
                ruchLD = true;
            }

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            teksturaPaletki = Content.Load<Texture2D>("paletka_pong");
            teksturaPaletkiPrzeciwnika = Content.Load<Texture2D>("paletka_pong");
            teksturaSciana = Content.Load<Texture2D>("paletka_pong");
            teksturaAdriana = Content.Load<Texture2D>("ball2");
            teksturaDamiana = Content.Load<Texture2D>("pileczka2");
            napisPunkty = Content.Load<SpriteFont>("czcionka_wynik");
            aktualnaTeksturaPilki = teksturaAdriana;
            effect = Content.Load<SoundEffect>("Damian");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
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
            if (Keyboard.GetState().IsKeyDown(Keys.Space) && punktyGracza < cel && punktyPrzeciwnika < cel)
            {
                czyGramy = true;
            }
            if (czyGramy)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.W))
                {
                    if (!paletka.Intersects(scianaGorna))
                        paletka.Y -= speed;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.S))
                {
                    if (!paletka.Intersects(scianaDolna))
                        paletka.Y += speed;
                }

                if (pilka.X >= szerokoscOkna / 2)
                {
                    if (ruchPD || ruchPG)
                    {
                        if (paletkaPrzeciwnika.Y < pilka.Y)
                        {
                            if (!paletkaPrzeciwnika.Intersects(scianaDolna))
                            {
                                paletkaPrzeciwnika.Y++;
                            }
                        }
                        else
                        {
                            if (!paletkaPrzeciwnika.Intersects(scianaGorna))
                            {
                                paletkaPrzeciwnika.Y--;
                            }
                        }
                    }
                }

                if (ruchPD)
                {
                    pilka.X += dx;
                    pilka.Y += dy;
                    if (pilka.Intersects(scianaPrawa))
                    {
                        ruchPD = false;
                        ruchLD = true;
                        punktyGracza++;
                        czyGramy = false;
                        ustawPozycjePoczatkowe();
                    }
                    if (pilka.Intersects(paletkaPrzeciwnika))
                    {
                        ruchPD = false;
                        ruchLD = true;
                    }
                    if (pilka.Intersects(scianaDolna))
                    {
                        ruchPD = false;
                        ruchPG = true;
                    }
                }

                if (ruchLD)
                {
                    pilka.X -= dx;
                    pilka.Y += dy;
                    if (pilka.Intersects(scianaLewa))
                    {
                        ruchLD = false;
                        ruchPD = true;
                        punktyPrzeciwnika++;
                        czyGramy = false;
                        ustawPozycjePoczatkowe();
                    }
                    if (pilka.Intersects(paletka))
                    {
                        ruchLD = false;
                        ruchPD = true;
                        pilka.X = paletka.X + paletka.Width + 1;
                    }
                    if (pilka.Intersects(scianaDolna))
                    {
                        ruchLD = false;
                        ruchLG = true;
                    }

                }

                if (ruchLG)
                {
                    pilka.X -= dx;
                    pilka.Y -= dy;
                    if (pilka.Intersects(scianaLewa))
                    {
                        ruchLG = false;
                        ruchPG = true;
                        punktyPrzeciwnika++;
                        czyGramy = false;
                        ustawPozycjePoczatkowe();
                    }
                    if (pilka.Intersects(scianaGorna))
                    {
                        ruchLG = false;
                        ruchLD = true;
                    }
                    if (pilka.Intersects(paletka))
                    {
                        ruchLG = false;
                        ruchPG = true;
                        pilka.X = paletka.X + paletka.Width + 1;
                    }

                }

                if (ruchPG)
                {
                    pilka.X += dx;
                    pilka.Y -= dy;
                    if (pilka.Intersects(scianaPrawa))
                    {
                        ruchPG = false;
                        ruchLG = true;
                        punktyGracza++;
                        czyGramy = false;
                        ustawPozycjePoczatkowe();
                    }
                    if (pilka.Intersects(scianaGorna))
                    {
                        ruchPG = false;
                        ruchPD = true;
                    }
                    if (pilka.Intersects(paletkaPrzeciwnika))
                    {
                        ruchPG = false;
                        ruchLG = true;
                    }
                }

                if (punktyGracza == cel)
                {
                    info = "Wygrales";
                    czyGramy = false;
                }
                if (punktyPrzeciwnika == cel)
                {
                    info = "Przegrales";
                    czyGramy = false;
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                if (punktyGracza == cel || punktyPrzeciwnika == cel)
                {
                    punktyGracza = 0;
                    punktyPrzeciwnika = 0;
                    czyGramy = true;
                }
            }
            if(Keyboard.GetState().IsKeyDown(Keys.P))
            {
                if(aktualnaTeksturaPilki==teksturaAdriana)
                {
                    aktualnaTeksturaPilki = teksturaDamiana;
                }
                else
                {
                    aktualnaTeksturaPilki = teksturaAdriana;
                }
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            spriteBatch.Draw(teksturaPaletki, paletka, Color.Green);
            spriteBatch.Draw(teksturaPaletkiPrzeciwnika, paletkaPrzeciwnika, Color.Yellow);
            spriteBatch.Draw(teksturaSciana, scianaPrawa, Color.Green);
            spriteBatch.Draw(teksturaSciana, scianaLewa, Color.Green);
            spriteBatch.Draw(teksturaSciana, scianaGorna, Color.Green);
            spriteBatch.Draw(teksturaSciana, scianaDolna, Color.Green);
            spriteBatch.Draw(aktualnaTeksturaPilki, pilka, Color.White);
            spriteBatch.DrawString(napisPunkty, punktyGracza.ToString(), new Vector2(szerokoscOkna / 4, wysokoscOkna / 10), Color.Red);
            spriteBatch.DrawString(napisPunkty, punktyPrzeciwnika.ToString(), new Vector2(szerokoscOkna - (szerokoscOkna / 4), wysokoscOkna / 10), Color.Red);
            if (!czyGramy && punktyGracza == 0 && punktyPrzeciwnika == 0)
                spriteBatch.DrawString(napisPunkty, "Wcisnij spacje, aby rozpoczac rozgrywke", new Vector2(paletka.X + (paletka.Width * 2) + 15, wysokoscOkna / 2), Color.Blue);
            else if (!czyGramy && punktyGracza < cel && punktyPrzeciwnika < cel)
                spriteBatch.DrawString(napisPunkty, "Wcisnij spacje, aby kontynowac rozgrywke", new Vector2(paletka.X + (paletka.Width * 2) + 15, wysokoscOkna / 2), Color.Blue);
            else if (!czyGramy)
            {
                spriteBatch.DrawString(napisPunkty, info, new Vector2((szerokoscOkna / 2) - 100, wysokoscOkna / 2), Color.Blue);
                spriteBatch.DrawString(napisPunkty, "Wcisnij R, aby rozpoczac nowa rozgrywke", new Vector2(paletka.X + paletka.Width, scianaDolna.Y - (scianaDolna.Height * 2)), Color.Blue);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void ustawPozycjePoczatkowe()
        {
            pilka.X = pilkaPoczX;
            pilka.Y = pilkaPoczY;
            paletka.X = paletkaGraczPoczX;
            paletka.Y = paletkaGraczPoczY;
            paletkaPrzeciwnika.X = paletkaPrzeciwnikaPoczX;
            paletkaPrzeciwnika.Y = paletkaPrzeciwnikaPoczY;
            effect.Play();
        }
    }
}
