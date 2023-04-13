using System;
using System.IO;
using System.Data;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Framework;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework.Design;
using Microsoft.Xna.Framework.Content;
//using System.Drawing;
//using System.Windows.Forms;
using System.Globalization;
using System.Linq;
using CellGame.Content;
using System.Collections.ObjectModel;

namespace CellGame
{
    public class Game1 : Game
    {
        
        private GraphicsDeviceManager _graphics;
        private Microsoft.Xna.Framework.Graphics.SpriteBatch _spriteBatch;
        Collisions Collide = new Collisions();
        System.Random rnd = new System.Random();

        Texture2D[] Blocks8x8, Blocks16x16, PlayerJumping32x32, PlayerJumping64x64, PlayerWalking32x32, PlayerWalking64x64;
        Texture2D PlayerIdle32x32, PlayerIdle64x64;

        public int[,] world;
        int blocksize, loadingareaX, loadingareaY;//BLOCK RANGE THE PLAYER CAN SEE IN ALL DIRECTIONS
        Texture2D[] Blocks;
        //Vector2[,] blockposition;
        Vector2 origin, middleposition;// = new Vector2(20, 20);
        Rectangle[,] blockrectangle;
        public const int width = 50, height = 30; //BLOCK SIZE OF THE MAP
        public string seed;

        const double G = 0.01;
        Planet[] planets;
        DoubleCoordinates PlanetForceOnMouse;
        int Numberofplanets;
        

        Texture2D[] PlayerWalking, PlayerJumping;
        Texture2D Player, PlayerIdle;
        Vector2 playerOffSet;
        Point Coords;
        int spiderSequenceNum = -1, maxvelocity = 5, spiderJumpNum = -1, playersize;
        double verticalVelocity, horizontalVelocity;
        bool Idle = true, falling = true, jumping = false, onground = true;
        float scale = 1f, rotation = 0f, layerDepth = 0f;
        SpriteEffects spriteEffects;
        Nullable<Rectangle> sourceRectangle = null;
        Rectangle playerrectangle;

        Inventory Inventory = new Inventory();

        Color colorclear = Color.White;
        ColorRGB Rainbow;

        MouseState mouseState;
        Point MousePosition, MousePositionInRelationToMap, SelectedBlock;
        Boolean Paused = true, Infomode = false, screensizechanged = false, MouseLeftPressed = false, MoveOneFrame = false, ChatOn = true, ButtonSelected; 

        KeyBoardKey NewMonsters = new KeyBoardKey(), KillMonsters = new KeyBoardKey();
        KeyBoardKey InfoKey = new KeyBoardKey(), EscapeKey = new KeyBoardKey(), DebugButton1 = new KeyBoardKey(), DebugButton2 = new KeyBoardKey(), ChatButton = new KeyBoardKey();
        KeyBoardKey Jump = new KeyBoardKey(), Switch = new KeyBoardKey();
        KeyBoardKey MoveRight = new KeyBoardKey(); //, MoveLeft = new KeyBoardKey();
        int SelectedBlockType = 0;
        public int Time = 0;
        int SlowMo = 15;

        Texture2D[] Letters;
        Color[] LetterColors;
        String[] ColorString;
        Text Coordinates, Extratext, Info;

        Chat Chat = new Chat();

        Texture2D[,] MonsterSprites;
        String[] MonsterList = { "Fly" };
        int maxMonsters = -1;
        int numberOfMonsters = 0;
        Monster[] monster;

        Color[] miniMapColors;
        int minimapsize;
        Texture2D pixel;

        static Texture2D circle, circletransparent;
        Button[] Buttons;
        const int PLAYBUTTON = 0;
        const int CREATENEWWORLDBUTTON = 1;
        const int DEBUGBUTTON = 2;
        const int SETTINGSBUTTON =3;
        const int RESETBUTTON= 4;
        const int NEWWORLDBUTTON = 5;

        Boolean MainMenuInitilised = false;
        const int MAINMENU = 0;
        const int LOADING = 1;
        const int GAME = 2;
        const int DEBUGLOADING = 3;
        const int DEBUG = 4;
        int CurrentlyDisplaying = MAINMENU;
        //Dictionary<int, Button> Buttons = new Dictionary<int, Button>();
        //Texture2D canvas;
        //Rectangle tracedSize;
        //UInt32[] pixels;
        bool StartDrawing = false;
        bool ReadytoDraw = false;
        DoubleCoordinates FinalCoords, FinalCoords2, FinalCoords3, FinalCoordsTest, FinalCoordsTest2, FinalCoordsTest3;
        DoubleCoordinates StartPixel, EndPixel;
        Vector2 PlayerLocation, Player2Location, Player3Location;
        DoubleCoordinates[] TestVectricesX, TestVectricesY, Test2VectricesX, Test2VectricesY, Test3VectricesX, Test3VectricesY;
        DoubleCoordinates[] SecondTestVectricesX, SecondTestVectricesY, SecondTest2VectricesX, SecondTest2VectricesY, SecondTest3VectricesX, SecondTest3VectricesY;
        DoubleCoordinates PlayerVelocity, Player2Velocity, Player3Velocity;
        bool PlayerPlaying = false, PlayerPlaying3 = false, Player1 = true, sharedspeedtest = true;
        int playernum = 0;
        bool IschosenpointX, IschosenpointX2;
        int DebugintersectpointchosenY = 0, DebugintersectpointchosenX = 0, DebugintersectpointchosenY2 = 0, DebugintersectpointchosenX2 = 0, XOffsetPlayer1, YOffsetPlayer1, XOffsetPlayer2, YOffsetPlayer2, XOffset, YOffset, XOffsetPlayerVelocity1, YOffsetPlayerVelocity1, XOffsetPlayerVelocity2, YOffsetPlayerVelocity2;
        Point[] DebugIntersectBlocksX, DebugIntersectBlocksY, DebugIntersectBlocksX2, DebugIntersectBlocksY2;
        //Vector2[] Debugintersectpoints;
        Rectangle DebugDetectrange, DebugDetectrange2;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            //_graphics.ToggleFullScreen();
            _graphics.PreferredBackBufferWidth = 1280;  // set this value to the desired width of your window
            _graphics.PreferredBackBufferHeight = 720;   // set this value to the desired height of your window
            _graphics.ApplyChanges();
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += Window_ClientSizeChanged;
        }

        protected override void Initialize()
        {

            // TODO: Add your initialization logic here


            //playerposition = new Vector2(-playersize / 2, -playersize);
            NewMonsters.NewKeyBoardKey();
            NewMonsters.Key = Keys.N;
            KillMonsters.NewKeyBoardKey();
            KillMonsters.Key = Keys.B;
            InfoKey.Key = Keys.F3;
            EscapeKey.Key = Keys.Escape;
            DebugButton1.NewKeyBoardKey();
            DebugButton1.Key = Keys.C;
            DebugButton2.NewKeyBoardKey();
            DebugButton2.Key = Keys.V;
            Jump.Key = Keys.Space;
            ChatButton.Key = Keys.OemQuestion;
            MoveRight.Key = Keys.Right;
            Switch.Key = Keys.L;
            base.Initialize();
            PlayerVelocity = new DoubleCoordinates(32, 32);
            Player2Velocity = new DoubleCoordinates(-32, -32);
        }
        
        protected override void LoadContent()
        {
            // TODO: use this.Content to load your game content here
            _spriteBatch = new Microsoft.Xna.Framework.Graphics.SpriteBatch(GraphicsDevice);

            Blocks16x16 = new Texture2D[4];
            Blocks16x16[1] = Content.Load<Texture2D>("Blocks16x16/Clay16x16");
            Blocks16x16[2] = Resize(Content.Load<Texture2D>("Blocks/Dirt"),2);
            Blocks16x16[3] = Resize(Content.Load<Texture2D>("Blocks/Stone"),2);
            Blocks8x8 = new Texture2D[4];
            Blocks8x8[1] = Content.Load<Texture2D>("Blocks/Clay");
            Blocks8x8[2] = Content.Load<Texture2D>("Blocks/Dirt");
            Blocks8x8[3] = Content.Load<Texture2D>("Blocks/Stone");

            Letters = new Texture2D[128];
            for (int i = 48; i < 58; i++)
            {
                Letters[i] = Content.Load<Texture2D>("Numbers/" + (i - 48).ToString());
            }
            for (int i = 65; i < 91; i++)
            {
                Letters[i] = Content.Load<Texture2D>("Letters/Letters" + ((Char)i).ToString());
            }
            for (int i = 97; i < 123; i++)
            {
                Letters[i] = Content.Load<Texture2D>("Letters/Letters_" + ((Char)i).ToString());
            }
            //MyExtension().run;
            PlayerJumping32x32 = new Texture2D[3];
            PlayerJumping32x32[0] = Content.Load<Texture2D>("Player/32x32/SpiderJump32x32_1");
            PlayerJumping32x32[1] = Content.Load<Texture2D>("Player/32x32/SpiderJump32x32_2");
            PlayerJumping32x32[2] = Content.Load<Texture2D>("Player/32x32/SpiderJump32x32_3");
            PlayerJumping64x64 = new Texture2D[3];
            PlayerJumping64x64[0] = Content.Load<Texture2D>("Player/64x64/SpiderJump64x64_1");
            PlayerJumping64x64[1] = Content.Load<Texture2D>("Player/64x64/SpiderJump64x64_2");
            PlayerJumping64x64[2] = Content.Load<Texture2D>("Player/64x64/SpiderJump64x64_3");

            PlayerWalking32x32 = new Texture2D[4];
            PlayerWalking32x32[0] = Content.Load<Texture2D>("Player/32x32/SpiderWalking32x32_1");
            PlayerWalking32x32[1] = Content.Load<Texture2D>("Player/32x32/SpiderWalking32x32_2");
            PlayerWalking32x32[2] = Content.Load<Texture2D>("Player/32x32/SpiderWalking32x32_3");
            PlayerWalking32x32[3] = Content.Load<Texture2D>("Player/32x32/SpiderWalking32x32_4");
            PlayerWalking64x64 = new Texture2D[4];
            PlayerWalking64x64[0] = Content.Load<Texture2D>("Player/64x64/SpiderWalking64x64_1");
            PlayerWalking64x64[1] = Content.Load<Texture2D>("Player/64x64/SpiderWalking64x64_2");
            PlayerWalking64x64[2] = Content.Load<Texture2D>("Player/64x64/SpiderWalking64x64_3");
            PlayerWalking64x64[3] = Content.Load<Texture2D>("Player/64x64/SpiderWalking64x64_4");
            PlayerIdle32x32 = Content.Load<Texture2D>("Player/32x32/SpiderIdle32x32_1");
            PlayerIdle64x64 = Content.Load<Texture2D>("Player/64x64/SpiderIdle64x64_1");
            MonsterSprites = new Texture2D[1, 2];
            MonsterSprites[0, 0] = Content.Load<Texture2D>("Monsters/Fly/BlackFly1");
            MonsterSprites[0, 1] = Content.Load<Texture2D>("Monsters/Fly/BlackFly2");

            string test = "";
            for (int i = 32; i < 129; i++)
            {
                test += (i.ToString() + "'" + ((char)i).ToString() + "', ");

            }
            System.Windows.Forms.MessageBox.Show(test);

            circle = Resize(Getborders(CreateCircle(16), 1, Color.Black, false), 2, 2);
            circletransparent = Getborders(CreateCircle(16), 1, Color.White, true);

            miniMapColors = new Color[4];
            miniMapColors[0] = Color.White;
            miniMapColors[1] = Color.Brown;
            miniMapColors[2] = Color.Gray;
            miniMapColors[3] = Color.DarkGray;
            pixel = new Texture2D(this.GraphicsDevice, 1, 1);
            pixel.SetData<Color>(new Color[1] { Color.White });

            LetterColors = new Color[21];
            LetterColors[0] = Color.White;
            LetterColors[1] = Color.Red;
            LetterColors[2] = Color.Yellow;
            LetterColors[3] = Color.Lime;
            LetterColors[4] = Color.Cyan;
            LetterColors[5] = Color.Blue;
            LetterColors[6] = Color.Magenta;
            LetterColors[7] = Color.Black;
            LetterColors[8] = Color.DarkGray;
            LetterColors[9] = Color.DarkRed;
            LetterColors[10] = Color.DarkOrange;
            LetterColors[11] = Color.Green;
            LetterColors[12] = Color.DarkCyan;
            LetterColors[13] = Color.DarkBlue;
            LetterColors[14] = Color.DarkMagenta;
            LetterColors[15] = Color.DimGray;

            ColorString = new string[21];
            ColorString[0] = "Ωa";
            ColorString[1] = "Ωb";
            ColorString[2] = "Ωc";
            ColorString[3] = "Ωd";
            ColorString[4] = "Ωe";
            ColorString[5] = "Ωf";
            ColorString[6] = "Ωg";
            ColorString[7] = "Ωh";
            ColorString[8] = "Ωi";
            ColorString[9] = "Ωj";
            ColorString[10] = "Ωk";
            ColorString[11] = "Ωl";
            ColorString[12] = "Ωm";
            ColorString[13] = "Ωn";
            ColorString[14] = "Ωo";
            ColorString[15] = "Ωp";
            //ColorRGB Rainbow = new ColorRGB();
            Rainbow.R = 255;
            Rainbow.G = 0;
            Rainbow.B = 0;
            Rainbow.A = 255;
            LetterColors[20] = Rainbow.col;
            ColorString[20] = "Ωz";

            InitializeMainMenu();
            RedoScreenVars();
        }

        protected override void UnloadContent()
        {

        }
        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here

            if (screensizechanged)
                RedoScreenVars();

            mouseState = Mouse.GetState();
            MousePosition = new Point(mouseState.X, mouseState.Y);   
            MousePositionInRelationToMap = MousePosition + Coords - new Point(Convert.ToInt32(middleposition.X), Convert.ToInt32(middleposition.Y));


            if (CurrentlyDisplaying == MAINMENU)
            {
                if (!MainMenuInitilised)
                    InitializeMainMenu();

                if (mouseState.LeftButton == ButtonState.Released && MouseLeftPressed)
                    MouseLeftPressed = false;
                if (mouseState.LeftButton == ButtonState.Pressed && !MouseLeftPressed)
                {
                    MouseLeftPressed = true;
                    for (int b = 0; b < Buttons.Count(); b++)
                    {
                        if (Buttons[b].Selected)
                        {
                            ButtonClick(b);
                        }
                    }
                }

            }

            else if (CurrentlyDisplaying == LOADING)
            {
                //width = 50;
                //height = 30;
                double randomFillPercent = 0.001;
                int smoothIndex = 5;
                //seed = "";
                blocksize = 32;
                playersize = 64;
                playerOffSet = new Vector2(-playersize / 2, -playersize/32 * 9 * 2);
                Coords.X = width * blocksize / 2 + blocksize/2;
                Coords.Y = height * blocksize / 2 - playersize/2;
                //Coords.X  = width * blocksize / 2 - playersize / 2 + blocksize / 2;
                //Coords.Y = height * blocksize / 2 - playersize / 32 * 23 * 2;

                playerrectangle.Location = new Point(Coords.X + Convert.ToInt32(playerOffSet.X), Coords.Y + Convert.ToInt32(playerOffSet.Y) + playersize / 32 * 9);
                playerrectangle.Size = new Point(playersize, playersize / 32 * 23);
                Numberofplanets = 0;
                planets = new Planet[Numberofplanets];
                blockrectangle = new Rectangle[width, height];
                RedoScreenVars();

                //MIGHT HAVE TO TURN THIS BACK ON IF THERE ARE ERRORS
                //if (loadingareaX > width / 2)
                //{
                //    System.Windows.Forms.MessageBox.Show("The loadingareaX: " + loadingareaX.ToString() + ", is too big, setting it to: " + (width / 2).ToString());
                //    loadingareaX = width / 2;
                //}
                //if (loadingareaY > height / 2)
                //{
                //    System.Windows.Forms.MessageBox.Show("The loadingareaY: " + loadingareaY.ToString() + ", is too, big setting it to: " + (height / 2).ToString());
                //    loadingareaY = height / 2;
                //}

                WorldGen mapGen = new WorldGen();
                (world, seed) = mapGen.Start(width, height, seed, randomFillPercent, smoothIndex);
                
                for (int x = 0; x < width; x++)
                    for (int y = 0; y < height; y++)
                    {
                        //if (world[x, y] != 0 & world[x, y] != 3)
                        //{
                        //    NewPlanet(x, y, world[x, y]);
                        //    world[x, y] = 0;

                        //}
                        blockrectangle[x, y].Location = new Point(x * blocksize, y * blocksize);
                        blockrectangle[x, y].Size = new Point(blocksize, blocksize);


                    }

                CurrentlyDisplaying = GAME;
                PlayerJumping = new Texture2D[3];
                PlayerWalking = new Texture2D[4];
                Blocks = new Texture2D[4];
                if (playersize == 64)
                {
                    PlayerIdle = PlayerIdle64x64;
                    //PlayerIdle = Resize(PlayerIdle64x64, 6,1);
                    //PlayerIdle = Resize(PlayerIdle, 10, 10);
                    //System.Windows.Forms.MessageBox.Show(PlayerIdle.Width.ToString());
                    for (int i = 0; i < PlayerJumping.Length; i++)
                        PlayerJumping[i] = PlayerJumping64x64[i];
                    for (int i = 0; i < PlayerWalking.Length; i++)
                        PlayerWalking[i] = PlayerWalking64x64[i];
                }
                else
                {
                    PlayerIdle = PlayerIdle32x32;
                    for (int i = 0; i < PlayerJumping.Length; i++)
                        PlayerJumping[i] = PlayerJumping32x32[i];
                    for (int i = 0; i < PlayerWalking.Length; i++)
                        PlayerWalking[i] = PlayerWalking32x32[i];
                }
                if (blocksize == 16)
                    for (int i = 1; i < Blocks.Length; i++)
                        Blocks[i] = Blocks16x16[i];
                else
                    for (int i = 1; i < Blocks.Length; i++)
                        Blocks[i] = Resize(Blocks8x8[i], Convert.ToDouble(blocksize)/8);
                Inventory.Items = new Inventory.ItemProperties[Blocks.Length * 2 + 1];
                //Inventory.Items[0].image = new Texture2D;
                for (int i = 1; i < Blocks.Length; i++)
                {
                    Inventory.Items[i + 1].image = Blocks[i];
                    Inventory.Items[i + 1].autoswing = true;
                }
                Inventory.Items[1].image = Letters[Convert.ToInt32(char.Parse("0"))];
                Inventory.Items[1].autoswing = true;
                Inventory.Items[Blocks.Length + 1].image = Letters[Convert.ToInt32(char.Parse("T"))];
                for (int i = 1; i < Blocks.Length; i++)
                {
                    Inventory.Items[i + 1 + Blocks.Length].image = Resize(Blocks[i],2);
                    Inventory.Items[i + 1 + Blocks.Length].autoswing = false;
                }
                loadingareaX = this.GraphicsDevice.Viewport.Width / blocksize / 2 + 1;
                loadingareaY = this.GraphicsDevice.Viewport.Height / blocksize / 2 + blocksize / 8 * 2;

                Player = PlayerIdle;
                minimapsize = 75;
                //monster = new Monster[maxMonsters];

                Inventory.InitializeInv();
                Buttons[SETTINGSBUTTON].visible = true;//SETTINGS BUTTON
                Chat.Initialize();
                Chat.NewLine("You Joined The World", Time);
                if (PlayerPlaying || PlayerPlaying3)
                {
                    TestVectricesX = new DoubleCoordinates[4];
                    TestVectricesY = new DoubleCoordinates[4];
                    Test2VectricesX = new DoubleCoordinates[4];
                    Test2VectricesY = new DoubleCoordinates[4];
                    Test3VectricesX = new DoubleCoordinates[4];
                    Test3VectricesY = new DoubleCoordinates[4];
                    SecondTestVectricesX = new DoubleCoordinates[4];
                    SecondTestVectricesY = new DoubleCoordinates[4];
                    SecondTest2VectricesX = new DoubleCoordinates[4];
                    SecondTest2VectricesY = new DoubleCoordinates[4];
                    SecondTest3VectricesX = new DoubleCoordinates[4];
                    SecondTest3VectricesY = new DoubleCoordinates[4];
                    PlayerLocation.X = Coords.X;
                    PlayerLocation.Y = Coords.Y;
                    Player2Location.X = Coords.X;
                    Player2Location.Y = Coords.Y;
                    Player3Location.X = Coords.X;
                    Player3Location.Y = Coords.Y;
                }
            }
            else if (CurrentlyDisplaying == GAME)
            {
                if (EscapeKey.CheckKeyPress())
                {
                    PauseGame();

                }

                if (InfoKey.CheckKeyPress())
                    Infomode = !Infomode;



                if (ChatButton.CheckKeyPress())
                    ChatOn = !ChatOn;
                //Exit();


                if (NewMonsters.CheckKeyPress())
                    maxMonsters += 1;
                if (KillMonsters.CheckKeyPress() && numberOfMonsters > 0)
                {
                    maxMonsters -= 1;
                    int removedmonster = rnd.Next(0, numberOfMonsters);
                    numberOfMonsters -= 1;
                    //Monster savedmonster = new Monster();
                    //savedmonster = monster[removedmonster];
                    for (int i = 0; i < numberOfMonsters; i++)
                        if (i >= removedmonster)
                        {
                            monster[i] = monster[i + 1];
                        }
                    System.Array.Resize(ref monster, numberOfMonsters);
                }

                if (Paused == false || MoveOneFrame)
                {
                    MoveOneFrame = false;
                    //if (numberOfMonsters < maxMonsters)
                    //{
                    //    SummonMonster();
                    //}

                    //MoveMonster();


                    //MovePlayer();
                    Moveplanets();

                    Time += 1;

                }
                else
                {

                    if (MoveRight.CheckKeyPress())
                        MoveOneFrame = true;


                    //if (l.size.Contains(MousePosition - new Point(this.GraphicsDevice.Viewport.Width - SettingsButton.size.Width - 20, 20)))
                    //    settingsButtonSelected = true;
                    //else
                    //    settingsButtonSelected = false;
                    //if (g.size.Contains(MousePosition - new Point(this.GraphicsDevice.Viewport.Width - SettingsButton.size.Width - 20, 20)))
                    //    settingsButtonSelected = true;
                    //else
                    //    settingsButtonSelected = false;

                }

                if (DebugButton1.CheckKeyPress())
                {
                    //blocksize += 1;
                    //for (int i = 1; i < Blocks.Length; i++)
                    //    Blocks[i] = Resize(Blocks8x8[i], Convert.ToDouble(blocksize) / 8);

                    //Coords.X = Convert.ToInt32(((MousePositionInRelationToMap.X) *Convert.ToDouble(blocksize) / (blocksize - 1)) - MousePosition.X + middleposition.X);
                    //Coords.Y = Convert.ToInt32(((MousePositionInRelationToMap.Y) * Convert.ToDouble(blocksize) / (blocksize - 1)) - MousePosition.Y + middleposition.Y);
                    //RedoLoadingarea();

                    ////Coords.X = MousePosition.X + Coords.X - this.GraphicsDevice.Viewport.Width / 2 - playersize / 2 + blocksize / 2;
                    //// Coords.Y = MousePosition.Y + Coords.Y - this.GraphicsDevice.Viewport.Height / 2 - playersize / 32 * 23 / 2;


                }

                if (DebugButton2.CheckKeyPress())
                {
                    //if (blocksize > 1)
                    //{
                    //    blocksize -= 1;
                    //    for (int i = 1; i < Blocks.Length; i++)
                    //        Blocks[i] = Resize(Blocks8x8[i], Convert.ToDouble(blocksize) / 8);
                    //    Coords.X = Convert.ToInt32(((MousePositionInRelationToMap.X) * Convert.ToDouble(blocksize) / (blocksize + 1)) - MousePosition.X + middleposition.X);
                    //    Coords.Y = Convert.ToInt32(((MousePositionInRelationToMap.Y) * Convert.ToDouble(blocksize) / (blocksize + 1)) - MousePosition.Y + middleposition.Y);
                    //    RedoLoadingarea();
                    //}
                    sharedspeedtest = !sharedspeedtest;
                    //Chat.NewLine("SharedSpeedTest = " + sharedspeedtest.ToString(), Time);
                }

                PlanetForceOnMouse = new DoubleCoordinates(0, 0);
                DoubleCoordinates mousedouble = new DoubleCoordinates(MousePositionInRelationToMap.X / Convert.ToDouble(blocksize), MousePositionInRelationToMap.Y / Convert.ToDouble(blocksize));
                for (int thisplanet = 0; thisplanet < Numberofplanets; thisplanet++)
                {
                    PlanetForceOnMouse.X += ((planets[thisplanet].position.X - mousedouble.X) * (G * planets[thisplanet].weight)) / ((planets[thisplanet].position.X - mousedouble.X) * (planets[thisplanet].position.X - mousedouble.X) + (planets[thisplanet].position.Y - mousedouble.Y) * (planets[thisplanet].position.Y - mousedouble.Y));
                    PlanetForceOnMouse.Y += ((planets[thisplanet].position.Y - mousedouble.Y) * (G * planets[thisplanet].weight)) / ((planets[thisplanet].position.X - mousedouble.X) * (planets[thisplanet].position.X - mousedouble.X) + (planets[thisplanet].position.Y - mousedouble.Y) * (planets[thisplanet].position.Y - mousedouble.Y));
                }


                if (PlayerPlaying)
                {
                    if (Switch.CheckKeyPress())
                    {
                        if (Player1)
                        {
                            Coords.X = (int)Player2Location.X;
                            Coords.Y = (int)Player2Location.Y;
                        }
                        else
                        {
                            Coords.X = (int)PlayerLocation.X;
                            Coords.Y = (int)PlayerLocation.Y;
                        }
                        Player1 = !Player1;

                    }
                    if (Player1)
                    {
                        if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Right))
                            PlayerVelocity.X += 1;
                        if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Left))
                            PlayerVelocity.X -= 1;
                        if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Up))
                            PlayerVelocity.Y -= 1;
                        if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Down))
                            PlayerVelocity.Y += 1;

                        if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D))
                            PlayerLocation.X += 1;
                        if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A))
                            PlayerLocation.X -= 1;
                        if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W))
                            PlayerLocation.Y -= 1;
                        if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S))
                            PlayerLocation.Y += 1;
                    }
                    else
                    {
                        if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Right))
                            Player2Velocity.X += 1;
                        if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Left))
                            Player2Velocity.X -= 1;
                        if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Up))
                            Player2Velocity.Y -= 1;
                        if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Down))
                            Player2Velocity.Y += 1;

                        if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D))
                            Player2Location.X += 1;
                        if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A))
                            Player2Location.X -= 1;
                        if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W))
                            Player2Location.Y -= 1;
                        if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S))
                            Player2Location.Y += 1;
                    }

                    //PlayerDebug(new DoubleCoordinates(middleposition.X, middleposition.Y), new DoubleCoordinates(middleposition.X + PlayerVelocity.X, middleposition.Y + PlayerVelocity.Y));
                    //(StartPixel, EndPixel) = PlayerDebug(new DoubleCoordinates(PlayerLocation.X, PlayerLocation.Y), new DoubleCoordinates(Coords.X + PlayerVelocity.X, Coords.Y + PlayerVelocity.Y));
                    MultiplePlayerDebug(new DoubleCoordinates(PlayerLocation.X, PlayerLocation.Y), new DoubleCoordinates(PlayerLocation.X + PlayerVelocity.X, PlayerLocation.Y + PlayerVelocity.Y), new DoubleCoordinates(Player2Location.X, Player2Location.Y), new DoubleCoordinates(Player2Location.X + Player2Velocity.X, Player2Location.Y + Player2Velocity.Y));


                }

                else if (PlayerPlaying3)
                {
                    if (Switch.CheckKeyPress())
                    {
                        if (playernum == 0)
                        {
                            Coords.X = (int)Player2Location.X;
                            Coords.Y = (int)Player2Location.Y;
                            playernum = 1;
                        }
                        else if (playernum == 1)
                        {
                            Coords.X = (int)Player3Location.X;
                            Coords.Y = (int)Player3Location.Y;
                            playernum = 2;
                        }
                        else
                        {
                            Coords.X = (int)PlayerLocation.X;
                            Coords.Y = (int)PlayerLocation.Y;
                            playernum = 0;
                        }


                    }
                    if (playernum == 0)
                    {
                        if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Right))
                            PlayerVelocity.X += 1;
                        if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Left))
                            PlayerVelocity.X -= 1;
                        if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Up))
                            PlayerVelocity.Y -= 1;
                        if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Down))
                            PlayerVelocity.Y += 1;

                        if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D))
                            PlayerLocation.X += 1;
                        if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A))
                            PlayerLocation.X -= 1;
                        if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W))
                            PlayerLocation.Y -= 1;
                        if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S))
                            PlayerLocation.Y += 1;
                    }
                    else if (playernum == 1)
                    {
                        if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Right))
                            Player2Velocity.X += 1;
                        if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Left))
                            Player2Velocity.X -= 1;
                        if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Up))
                            Player2Velocity.Y -= 1;
                        if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Down))
                            Player2Velocity.Y += 1;

                        if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D))
                            Player2Location.X += 1;
                        if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A))
                            Player2Location.X -= 1;
                        if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W))
                            Player2Location.Y -= 1;
                        if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S))
                            Player2Location.Y += 1;
                    }
                    else if (playernum == 2)
                    {
                        if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Right))
                            Player3Velocity.X += 1;
                        if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Left))
                            Player3Velocity.X -= 1;
                        if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Up))
                            Player3Velocity.Y -= 1;
                        if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Down))
                            Player3Velocity.Y += 1;

                        if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D))
                            Player3Location.X += 1;
                        if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A))
                            Player3Location.X -= 1;
                        if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W))
                            Player3Location.Y -= 1;
                        if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S))
                            Player3Location.Y += 1;
                    }

                    (FinalCoordsTest, FinalCoordsTest2, FinalCoordsTest3) = TriplePlayerDebug(new DoubleCoordinates(PlayerLocation.X, PlayerLocation.Y), new DoubleCoordinates(PlayerLocation.X + PlayerVelocity.X, PlayerLocation.Y + PlayerVelocity.Y), new DoubleCoordinates(Player2Location.X, Player2Location.Y), new DoubleCoordinates(Player2Location.X + Player2Velocity.X, Player2Location.Y + Player2Velocity.Y), new DoubleCoordinates(Player3Location.X, Player3Location.Y), new DoubleCoordinates(Player3Location.X + Player3Velocity.X, Player3Location.Y + Player3Velocity.Y));



                }

                if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D))
                    Coords.X += 1;
                if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A))
                    Coords.X -= 1;
                if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W))
                    Coords.Y -= 1;
                if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S))
                    Coords.Y += 1;
                
                if (MousePosition.X > 0 && MousePosition.X < this.GraphicsDevice.Viewport.Width && MousePosition.Y > 0 && MousePosition.Y < this.GraphicsDevice.Viewport.Height)
                {
                    SelectedBlock = new Point((MousePosition.X + Coords.X - this.GraphicsDevice.Viewport.Width / 2) / blocksize, (MousePosition.Y + Coords.Y - this.GraphicsDevice.Viewport.Height / 2) / blocksize);
                    if ((SelectedBlock.X > -1) && (SelectedBlock.X < width) && (SelectedBlock.Y > -1) && (SelectedBlock.Y < height))
                        SelectedBlockType = world[SelectedBlock.X, SelectedBlock.Y];
                    else
                        SelectedBlockType = 0;
                }

                bool mouseOnInv = false;
                int mouseOnSlot = 0;
                for (int s = 0; s < Inventory.Slots.Count(); s++)
                {
                    if (Inventory.Slots[s].visible)
                    {
                        if (Inventory.Slots[s].Rectangle.Contains(MousePosition))
                        {
                            Inventory.Slots[s].Selected = true;
                            mouseOnInv = true;
                            mouseOnSlot = s;
                        }

                        else if (s != Inventory.Selected)
                            Inventory.Slots[s].Selected = false;
                    }
                }

                if (mouseState.LeftButton == ButtonState.Released && MouseLeftPressed)
                    MouseLeftPressed = false;
                if (mouseState.LeftButton == ButtonState.Pressed && (!MouseLeftPressed || Inventory.Items[Inventory.Slots[Inventory.Selected].Item.number].autoswing == true))
                {
                    if (mouseOnInv)
                    {
                        if (!MouseLeftPressed)
                        {
                            Inventory.Slots[Inventory.Selected].Selected = false;
                            Inventory.Slots[mouseOnSlot].Selected = true;
                            Inventory.Selected = mouseOnSlot;
                        }
                    }







                    else if (ButtonSelected)
                    {

                        if (!MouseLeftPressed)
                        {
                            for (int b = 0; b < Buttons.Count(); b++)
                            {
                                if (Buttons[b].Selected)
                                {
                                    ButtonClick(b);
                                }
                            }
                        }
                    }
                    else if (Inventory.Slots[Inventory.Selected].Item.number != 0)
                    {
                        if (Inventory.Slots[Inventory.Selected].Item.number == Blocks.Length + 1)
                        {
                            Coords.X = MousePosition.X + Coords.X - this.GraphicsDevice.Viewport.Width / 2 - playersize / 2 + blocksize / 2;
                            Coords.Y = MousePosition.Y + Coords.Y - this.GraphicsDevice.Viewport.Height / 2 - playersize / 32 * 23 / 2;
                        }
                        if (Inventory.Slots[Inventory.Selected].Item.number > 0 && Inventory.Slots[Inventory.Selected].Item.number < Blocks.Length + 1)
                        {
                            if (SelectedBlock.X < width && SelectedBlock.Y < height && SelectedBlock.X > -1 && SelectedBlock.Y > -1)
                            {
                                world[SelectedBlock.X, SelectedBlock.Y] = Inventory.Slots[Inventory.Selected].Item.number - 1;
                                SelectedBlockType = Inventory.Slots[Inventory.Selected].Item.number - 1;
                            }
                        }
                        if (Inventory.Slots[Inventory.Selected].Item.number > Blocks.Length + 1 && Inventory.Slots[Inventory.Selected].Item.number < Blocks.Length*2 + 2)
                        {
                            if (SelectedBlock.X < width && SelectedBlock.Y < height && SelectedBlock.X > -1 && SelectedBlock.Y > -1)
                            {
                                NewPlanet((MousePosition.X + Coords.X - this.GraphicsDevice.Viewport.Width / 2)/blocksize, (MousePosition.Y + Coords.Y - this.GraphicsDevice.Viewport.Height / 2)/blocksize, Inventory.Selected - Blocks.Length);
                                //System.Array.Resize(ref planets, Numberofplanets + 1);
                                //planets[Numberofplanets].position.X  = (MousePosition.X  + Coords.X  - this.GraphicsDevice.Viewport.Width / 2);
                                //planets[Numberofplanets].position.Y = (MousePosition.Y + Coords.Y - this.GraphicsDevice.Viewport.Height / 2);
                                //planets[Numberofplanets].blocktype = Inventory.Selected - Blocks.Length;
                                //Numberofplanets += 1;
                            }
                        }
                    }
                    MouseLeftPressed = true;
                }
                playerrectangle.Location = new Point(Coords.X + Convert.ToInt32(playerOffSet.X), Coords.Y + Convert.ToInt32(playerOffSet.Y) + playersize / 32 * 9);

            }
            else if (CurrentlyDisplaying == DEBUG)
            {
                if (InfoKey.CheckKeyPress())
                    Infomode = !Infomode;
                if (mouseState.LeftButton == ButtonState.Released && MouseLeftPressed)
                {
                    MouseLeftPressed = false;
                    if (StartDrawing)
                    {
                        EndPixel = new DoubleCoordinates((MousePosition.X + Coords.X - middleposition.X), (MousePosition.Y + Coords.Y - middleposition.Y));
                        StartDrawing = false;
                    }
                }


                if (mouseState.LeftButton == ButtonState.Pressed && (!MouseLeftPressed))
                {
                    StartPixel = new DoubleCoordinates((MousePosition.X + Coords.X - middleposition.X), (MousePosition.Y + Coords.Y - middleposition.Y));
                    StartDrawing = true;
                }

                if ((StartDrawing && Math.Abs(StartPixel.X - (MousePosition.X + Coords.X - middleposition.X)) >= blocksize && Math.Abs(StartPixel.Y - (MousePosition.Y + Coords.Y - middleposition.Y)) >= blocksize) || (Math.Abs(StartPixel.X - EndPixel.X) >= blocksize && Math.Abs(StartPixel.Y - EndPixel.Y) >= blocksize))
                {
                    DoubleCoordinates EndingPixel = EndPixel;
                    int XModifier = blocksize;
                    int YModifier = blocksize;
                    if (StartDrawing)
                        EndingPixel = new DoubleCoordinates((MousePosition.X + Coords.X - middleposition.X), (MousePosition.Y + Coords.Y - middleposition.Y));
                    if (StartPixel.X > EndingPixel.X)
                        XModifier = -blocksize;
                    if (StartPixel.Y > EndingPixel.Y)
                        YModifier = -blocksize;
                    //TestVectrices[0] = StartPixel;
                    //TestVectrices[1] = new DoubleCoordinates(StartPixel.X  + XModifier, StartPixel.Y);
                    //TestVectrices[2] = new DoubleCoordinates(EndingPixel.X , EndingPixel.Y - YModifier);
                    //TestVectrices[3] = new DoubleCoordinates(EndingPixel.X , EndingPixel.Y);
                    //TestVectrices[4] = new DoubleCoordinates(EndingPixel.X  - XModifier, EndingPixel.Y);
                    //TestVectrices[5] = new DoubleCoordinates(StartPixel.X , StartPixel.Y + YModifier);

                    TestVectricesX[0] = new DoubleCoordinates(StartPixel.X + XModifier, StartPixel.Y + YModifier);
                    TestVectricesX[1] = new DoubleCoordinates(EndingPixel.X, EndingPixel.Y);
                    TestVectricesX[2] = new DoubleCoordinates(EndingPixel.X - XModifier, EndingPixel.Y);
                    TestVectricesX[3] = new DoubleCoordinates(StartPixel.X, StartPixel.Y + YModifier);

                    TestVectricesY[0] = new DoubleCoordinates(StartPixel.X + XModifier, StartPixel.Y + YModifier);
                    TestVectricesY[1] = new DoubleCoordinates(EndingPixel.X, EndingPixel.Y);
                    TestVectricesY[2] = new DoubleCoordinates(EndingPixel.X, EndingPixel.Y - YModifier);
                    TestVectricesY[3] = new DoubleCoordinates(StartPixel.X + XModifier, StartPixel.Y);
                    ReadytoDraw = true;

                    XOffset = 0;
                    YOffset = 0;
                    if ((StartPixel.X + XModifier) > EndingPixel.X)
                        XOffset = blocksize;
                    if ((StartPixel.Y + YModifier) > EndingPixel.Y)
                        YOffset = blocksize;

                    double BiggerXThisPlanet = StartPixel.X > EndingPixel.X ? StartPixel.X : EndingPixel.X;
                    double BiggerYThisPlanet = StartPixel.Y > EndingPixel.Y ? StartPixel.Y : EndingPixel.Y;
                    double SmallerXThisPlanet = StartPixel.X > EndingPixel.X ? EndingPixel.X : StartPixel.X;
                    double SmallerYThisPlanet = StartPixel.Y > EndingPixel.Y ? EndingPixel.Y : StartPixel.Y;
                    //int operatordirectionY = planets[thisplanet].velocity.Y > 0 ? 1 : -1;
                    //int operatordirectionX = planets[thisplanet].velocity.X  > 0 ? 1 : -1;

                    //DoubleCoordinates[] vertices;
                    //vertices = GetPlanetVectors(BiggerXThisPlanet, BiggerYThisPlanet, SmallerXThisPlanet, SmallerYThisPlanet, (planets[thisplanet].velocity.Y / planets[thisplanet].velocity.X ));
                    double SmallestTime = 10000;
                    DebugintersectpointchosenY = 0;
                    DebugintersectpointchosenX = 0;
                    IschosenpointX = true;
                    int Orientationofbig = 0;
                    int numberofintersectpointsX = 0;
                    int numberofintersectpointsY = 0;
                    //Debugintersectpoints = new Vector2[0];
                    DebugIntersectBlocksX = new Point[0];
                    DebugIntersectBlocksY= new Point[0];
                    DebugDetectrange = new Rectangle(Convert.ToInt32((SmallerXThisPlanet) / blocksize) - 1, Convert.ToInt32((SmallerYThisPlanet) / blocksize) - 1, Convert.ToInt32((BiggerXThisPlanet) / blocksize) - Convert.ToInt32((SmallerXThisPlanet) / blocksize) + 2, Convert.ToInt32((BiggerYThisPlanet) / blocksize) - Convert.ToInt32((SmallerYThisPlanet) / blocksize) + 2);
                    for (int Y = Convert.ToInt32((SmallerYThisPlanet) / blocksize) - 1; Y < Convert.ToInt32((BiggerYThisPlanet) / blocksize) + 2; Y++)
                    {
                        for (int X = Convert.ToInt32((SmallerXThisPlanet) / blocksize) - 1; X < Convert.ToInt32((BiggerXThisPlanet) / blocksize) + 2; X++)
                        {
                            if ((X > -1) && (X < width) && (Y > -1) && (Y < height))
                                if (world[X, Y] != 0)
                                {

                                    //(bool collision, bool inside, int[] orientation, DoubleCoordinates[] intersectpoints) = Collide.polyRect(TestVectrices, Coords.X  * blocksize, Coords.Y * blocksize, blocksize, blocksize);
                                    ////if (!collision && inside)
                                    ////System.Windows.Forms.MessageBox.Show("bruh" + Coords.X .ToString() + ", " + Coords.Y.ToString());
                                    //if (collision)
                                    //{

                                    //    for (int i = 0; i < orientation.Length; i++)
                                    //    {
                                    //        if (orientation[i] > 0)
                                    //        {
                                    //            Array.Resize(ref Debugintersectpoints, numberofintersectpoints + 1);
                                    //            Debugintersectpoints[numberofintersectpoints] = new Vector2(Convert.ToInt32(intersectpoints[i].X ), Convert.ToInt32(intersectpoints[i].Y));
                                    //            numberofintersectpoints += 1;
                                    //            double NewTime = 10000;
                                    //            if (orientation[i] == 2 && StartPixel.X  > EndingPixel.X )//RIGHT
                                    //                NewTime = (intersectpoints[i].X  - StartPixel.X ) / (EndingPixel.X  - StartPixel.X );
                                    //            else if (orientation[i] == 4 && StartPixel.Y > EndingPixel.Y)//BOTTOM 
                                    //                NewTime = (intersectpoints[i].Y - StartPixel.Y) / (EndingPixel.Y - StartPixel.Y);
                                    //            else if (orientation[i] == 1 && StartPixel.X  < EndingPixel.X )//LEFT
                                    //                NewTime = (intersectpoints[i].X  - (StartPixel.X  + blocksize)) / (EndingPixel.X  - StartPixel.X );
                                    //            else if (orientation[i] == 3 && StartPixel.Y < EndingPixel.Y)//TOP 
                                    //                NewTime = (intersectpoints[i].Y - (StartPixel.Y + blocksize)) / (EndingPixel.Y - StartPixel.Y);

                                    //            //System.Windows.Forms.MessageBox.Show("orientation" + (orientation[i]).ToString() + "old:" + (SmallestTime).ToString() + "new" + (NewTime).ToString());
                                    //            //if (NewTime == 0)
                                    //            //System.Windows.Forms.MessageBox.Show("New orientation: " + (orientation[i]).ToString() + " 0 = " + intersectpoints[i].X .ToString() + " or " + (intersectpoints[i].X ).ToString() + " - " + (planets[thisplanet].position.X ).ToString() + " or " + (planets[thisplanet].position.Y).ToString() + " + (" + blocksize.ToString() + ") / " + planets[thisplanet].velocity.X .ToString() + " or " + planets[thisplanet].velocity.Y.ToString());

                                    //            if (SmallestTime == NewTime && NewTime != 10000 && NewTime >= 0 && orientation[i] != Orientationofbig)
                                    //                System.Windows.Forms.MessageBox.Show("New orientation: " + (orientation[i]).ToString() + " new time: " + (NewTime).ToString() + " old orientation: " + (Orientationofbig).ToString() + " old time: " + (SmallestTime).ToString());
                                    //            else if (SmallestTime > NewTime && NewTime != 10000 && NewTime >= 0)
                                    //            {
                                    //                SmallestTime = NewTime;
                                    //                Debugintersectpointchosen = numberofintersectpoints - 1;
                                    //                Orientationofbig = orientation[i];
                                    //            }
                                    //        }
                                    //    }
                                    //}
                                    //NEW PROPOSITION
                                    //Line Along the Coords.Y axis
                                    if (Collide.PolyLineGood(TestVectricesY, X * blocksize + XOffset, Y * blocksize, X * blocksize + XOffset, Y * blocksize + blocksize)) 
                                    {
                                        Array.Resize(ref DebugIntersectBlocksY, numberofintersectpointsY + 1);
                                        DebugIntersectBlocksY[numberofintersectpointsY] = new Point(X,Y);
                                        //if (((XOffset == 0 && DebugIntersectBlocksY[numberofintersectpointsY].X  < DebugIntersectBlocksY[DebugintersectpointchosenY].X ) && ((YOffset == 0 && DebugIntersectBlocksY[numberofintersectpointsY].Y <= DebugIntersectBlocksY[DebugintersectpointchosenY].Y) || (YOffset == blocksize && DebugIntersectBlocksY[numberofintersectpointsY].Y >= DebugIntersectBlocksY[DebugintersectpointchosenY].Y))) || ((XOffset == blocksize && DebugIntersectBlocksY[numberofintersectpointsY].X  > DebugIntersectBlocksY[DebugintersectpointchosenY].X ) && ((YOffset == 0 && DebugIntersectBlocksY[numberofintersectpointsY].Y <= DebugIntersectBlocksY[DebugintersectpointchosenY].Y) || (YOffset == blocksize && DebugIntersectBlocksY[numberofintersectpointsY].Y >= DebugIntersectBlocksY[DebugintersectpointchosenY].Y))) || ((YOffset == 0 && DebugIntersectBlocksY[numberofintersectpointsY].Y < DebugIntersectBlocksY[DebugintersectpointchosenY].Y) && ((XOffset == 0 && DebugIntersectBlocksY[numberofintersectpointsY].X  <= DebugIntersectBlocksY[DebugintersectpointchosenY].X ) || (XOffset == blocksize && DebugIntersectBlocksY[numberofintersectpointsY].X  >= DebugIntersectBlocksY[DebugintersectpointchosenY].X ))) || ((YOffset == blocksize && DebugIntersectBlocksY[numberofintersectpointsY].Y > DebugIntersectBlocksY[DebugintersectpointchosenY].Y) && ((XOffset == 0 && DebugIntersectBlocksY[numberofintersectpointsY].X  <= DebugIntersectBlocksY[DebugintersectpointchosenY].X ) || (XOffset == blocksize && DebugIntersectBlocksY[numberofintersectpointsY].X  >= DebugIntersectBlocksY[DebugintersectpointchosenY].X ))))
                                        if ((XOffset == 0 && DebugIntersectBlocksY[numberofintersectpointsY].X < DebugIntersectBlocksY[DebugintersectpointchosenY].X) || (XOffset == blocksize && DebugIntersectBlocksY[numberofintersectpointsY].X > DebugIntersectBlocksY[DebugintersectpointchosenY].X))
                                            DebugintersectpointchosenY = numberofintersectpointsY; 
                                        numberofintersectpointsY += 1;
                                        IschosenpointX = false;
                                    }//
                                    //Line Along the Coords.X  axis
                                    if (Collide.PolyLineGood(TestVectricesX, X * blocksize, Y * blocksize + YOffset, X * blocksize + blocksize, Y * blocksize + YOffset)) 
                                    {
                                        Array.Resize(ref DebugIntersectBlocksX, numberofintersectpointsX + 1);
                                        DebugIntersectBlocksX[numberofintersectpointsX] = new Point(X, Y);
                                        //if (((XOffset == 0 && DebugIntersectBlocksX[numberofintersectpointsX].X  < DebugIntersectBlocksX[DebugintersectpointchosenX].X ) && ((YOffset == 0 && DebugIntersectBlocksX[numberofintersectpointsX].Y <= DebugIntersectBlocksX[DebugintersectpointchosenX].Y) || (YOffset == blocksize && DebugIntersectBlocksX[numberofintersectpointsX].Y >= DebugIntersectBlocksX[DebugintersectpointchosenX].Y))) || ((XOffset == blocksize && DebugIntersectBlocksX[numberofintersectpointsX].X  > DebugIntersectBlocksX[DebugintersectpointchosenX].X ) && ((YOffset == 0 && DebugIntersectBlocksX[numberofintersectpointsX].Y <= DebugIntersectBlocksX[DebugintersectpointchosenX].Y) || (YOffset == blocksize && DebugIntersectBlocksX[numberofintersectpointsX].Y >= DebugIntersectBlocksX[DebugintersectpointchosenX].Y))) || ((YOffset == 0 && DebugIntersectBlocksX[numberofintersectpointsX].Y < DebugIntersectBlocksX[DebugintersectpointchosenX].Y) && ((XOffset == 0 && DebugIntersectBlocksX[numberofintersectpointsX].X  <= DebugIntersectBlocksX[DebugintersectpointchosenX].X ) || (XOffset == blocksize && DebugIntersectBlocksX[numberofintersectpointsX].X  >= DebugIntersectBlocksX[DebugintersectpointchosenX].X ))) || ((YOffset == blocksize && DebugIntersectBlocksX[numberofintersectpointsX].Y > DebugIntersectBlocksX[DebugintersectpointchosenX].Y) && ((XOffset == 0 && DebugIntersectBlocksX[numberofintersectpointsX].X  <= DebugIntersectBlocksX[DebugintersectpointchosenX].X ) || (XOffset == blocksize && DebugIntersectBlocksX[numberofintersectpointsX].X  >= DebugIntersectBlocksX[DebugintersectpointchosenX].X ))))
                                        if ((YOffset == 0 && DebugIntersectBlocksX[numberofintersectpointsX].Y < DebugIntersectBlocksX[DebugintersectpointchosenX].Y) || (YOffset == blocksize && DebugIntersectBlocksX[numberofintersectpointsX].Y > DebugIntersectBlocksX[DebugintersectpointchosenX].Y))
                                            DebugintersectpointchosenX = numberofintersectpointsX;
                                        numberofintersectpointsX += 1;
                                        IschosenpointX = true;
                                    }
                                    if (numberofintersectpointsX > 0 && numberofintersectpointsY > 0)
                                    {
                                        IschosenpointX = (((DebugIntersectBlocksX[DebugintersectpointchosenX].Y * blocksize + YOffset - (StartPixel.Y + YModifier)) / (EndingPixel.Y - (StartPixel.Y + YModifier))) < ((DebugIntersectBlocksY[DebugintersectpointchosenY].X * blocksize + XOffset - (StartPixel.X + XModifier)) / (EndingPixel.X - (StartPixel.X + XModifier))));
                                    //    if (((DebugIntersectBlocksY[DebugintersectpointchosenY].Y * blocksize + YOffset - (StartPixel.Y - YOffset + blocksize)) / (EndingPixel.Y - (StartPixel.Y + YModifier)) <= 0))
                                    //        System.Windows.Forms.MessageBox.Show("bruh Coords.Y: (" + DebugIntersectBlocksY[DebugintersectpointchosenY].Y.ToString() + " * " + blocksize.ToString()  + " + " + YOffset.ToString() + " - (" + StartPixel.Y.ToString() + " - " + YOffset.ToString() + " + " + blocksize.ToString() + " )) / (" + EndingPixel.Y.ToString() + " - (" + StartPixel.Y.ToString() + " + " + YModifier.ToString() + " )) = " + ((DebugIntersectBlocksY[DebugintersectpointchosenY].Y * blocksize + YOffset - (StartPixel.Y - YOffset + blocksize)) / (EndingPixel.Y - (StartPixel.Y + YModifier))).ToString());
                                    //    if (((DebugIntersectBlocksX[DebugintersectpointchosenX].X  * blocksize + XOffset - (StartPixel.X  + XModifier)) / (EndingPixel.X  - (StartPixel.X  + XModifier)) <= 0))
                                    //        System.Windows.Forms.MessageBox.Show("bruh Coords.X : (" + DebugIntersectBlocksX[DebugintersectpointchosenX].X .ToString() + " * " + blocksize.ToString() + " + " + XOffset.ToString() + " - (" + StartPixel.X .ToString() + " - " + XOffset.ToString() + " + " + blocksize.ToString() + " )) / (" + EndingPixel.X .ToString() + " - (" + StartPixel.X .ToString() + " + " + XModifier.ToString() + " )) = " + ((DebugIntersectBlocksX[DebugintersectpointchosenX].X  * blocksize + XOffset - (StartPixel.X  - XOffset + blocksize)) / (EndingPixel.X  - (StartPixel.X  + XModifier))).ToString());
                                    }
                                    
                                }
                        }

                    }
                }
                Time += 1;
            }
            else if (CurrentlyDisplaying == DEBUGLOADING)
            {
                Blocks = new Texture2D[4];
                blocksize = 20;
                //width = 50;
                //height = 30;
                Coords.X  = width * blocksize / 2 + blocksize / 2;
                Coords.Y = height * blocksize / 2 - playersize / 2;
                int randomFillPercent = 500;
                int smoothIndex = 0;
                blockrectangle = new Rectangle[width, height];
                blockrectangle = new Rectangle[width, height];
                WorldGen mapGen = new WorldGen();
                (world, seed) = mapGen.Start(width, height, seed, randomFillPercent, smoothIndex);

                for (int x = 0; x < width; x++)
                    for (int y = 0; y < height; y++)
                    {
                        blockrectangle[x, y].Location = new Point(x * blocksize, y * blocksize);
                        blockrectangle[x, y].Size = new Point(blocksize, blocksize);
                    }

                CurrentlyDisplaying = DEBUG;
                loadingareaX = this.GraphicsDevice.Viewport.Width / blocksize / 2 + 1;
                loadingareaY = this.GraphicsDevice.Viewport.Height / blocksize / 2 + blocksize / 8 * 2;

                for (int i = 1; i < Blocks.Length; i++)
                    Blocks[i] = Resize(Blocks8x8[i], Convert.ToDouble(blocksize) / 8);
            }
            ButtonSelected = false;
            for (int b = 0; b < Buttons.Count(); b++)
            {
                if (Buttons[b].visible)
                {
                    if (Buttons[b].Rectangle.Contains(MousePosition))
                    {
                        Buttons[b].Selected = true;
                        ButtonSelected = true;
                    }
                    else
                        Buttons[b].Selected = false;
                }
            }

            DoLettersOnScreen();


            //pixels[rnd.Next(pixels.Length)] = 0xFF00FF00;
            //canvas.SetData<UInt32>(pixels, 0, tracedSize.Width * tracedSize.Height);

            // Last thing, do not move
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.SlateGray);
            Vector2 position = new Vector2(Coords.X , Coords.Y);

            //GraphicsDevice.Textures[0] = null;


            // TODO: Add your drawing code here

            _spriteBatch.Begin();

            //_spriteBatch.Draw(canvas, new Rectangle(0, 0, tracedSize.Width, tracedSize.Height), Color.White);
            if (CurrentlyDisplaying == MAINMENU)
            {


            }
            else if (CurrentlyDisplaying == LOADING)
            {
                GraphicsDevice.Clear(Color.Gray);
            }
            else if (CurrentlyDisplaying == GAME)
            {
                Color blockcolor;
                for (int x = Coords.X / blocksize - loadingareaX; x < Coords.X / blocksize + loadingareaX; x++)
                    for (int y = Coords.Y / blocksize - loadingareaY; y < Coords.Y / blocksize + loadingareaY; y++)
                        if ((x > -1) && (x < width) && (y > -1) && (y < height))
                            if (world[x, y] > 0)
                            {
                                if (x == SelectedBlock.X && y == SelectedBlock.Y)
                                    blockcolor = Color.LightGray;
                                else
                                    blockcolor = colorclear;
                                _spriteBatch.Draw(Blocks[world[x, y]], new Vector2(x * blocksize, y * blocksize) - position + middleposition, blockcolor);
                            }

                for (int z = 0; z < Numberofplanets; z++)
                {
                    if ((planets[z].position.X - loadingareaX < planets[z].position.X + loadingareaX) && (planets[z].position.Y - loadingareaY < planets[z].position.Y + loadingareaY))
                    {
                        _spriteBatch.Draw(Blocks[planets[z].blocktype], new Vector2(Convert.ToInt32(planets[z].position.X * blocksize), Convert.ToInt32(planets[z].position.Y * blocksize)) - position + middleposition, colorclear);
                    }
                }



                for (int i = 0; i < numberOfMonsters; i++)
                {
                    _spriteBatch.Draw(monster[i].sprite, monster[i].position - position + middleposition, sourceRectangle, monster[i].color, monster[i].rotation, origin, monster[i].scale, monster[i].spriteEffects, layerDepth);
                    //System.Windows.Forms.MessageBox.Show(monster[i].position.ToString());
                }

                if (PlayerPlaying)
                {
                    _spriteBatch.Draw(Blocks[1],PlayerLocation - position + middleposition, colorclear);
                    _spriteBatch.Draw(Blocks[2], Player2Location - position + middleposition, colorclear);
                    if (ReadytoDraw)
                    {//Player1
                        SpriteBatchExtensions.DrawVertices(_spriteBatch, pixel, TestVectricesX, position, middleposition, Color.Green);
                        SpriteBatchExtensions.DrawVertices(_spriteBatch, pixel, TestVectricesY, position, middleposition, Color.Yellow);
                        RectangleSprite.DrawRectangle(_spriteBatch, new Rectangle(Convert.ToInt32(DebugDetectrange.X * blocksize - position.X + middleposition.X), Convert.ToInt32(DebugDetectrange.Y * blocksize - position.Y + middleposition.Y), Convert.ToInt32(DebugDetectrange.Width * blocksize), Convert.ToInt32(DebugDetectrange.Height * blocksize)), Color.Blue, 2);
                        for (int i = 0; i < DebugIntersectBlocksX.Length; i++)
                        {
                            Color circlecolor = LetterColors[i - (i / 16) * 16];
                            if (i == DebugintersectpointchosenX)
                                if (IschosenpointX) circlecolor = Time % 2 == 0 ? LetterColors[0] : LetterColors[7];
                                else circlecolor = Time % 2 == 0 ? LetterColors[1] : LetterColors[8];
                            //RectangleSprite.DrawRectangle(_spriteBatch, new Rectangle(Convert.ToInt32(DebugIntersectBlocksX[i].X - position.X + middleposition.X), Convert.ToInt32(DebugIntersectBlocksX[i].Y - position.Y + middleposition.Y + YOffsetPlayer1), Convert.ToInt32(blocksize), Convert.ToInt32(1)), circlecolor, 2);
                            RectangleSprite.DrawRectangle(_spriteBatch, new Rectangle(Convert.ToInt32(DebugIntersectBlocksX[i].X - position.X + middleposition.X + XOffsetPlayer2 - XOffsetPlayerVelocity2), Convert.ToInt32(DebugIntersectBlocksX[i].Y - position.Y + middleposition.Y), Convert.ToInt32(blocksize), Convert.ToInt32(1)), circlecolor, 2);

                            //_spriteBatch.Draw(circletransparent, new Vector2(Convert.ToInt32(Debugintersectpoints[i].X  - 8), Convert.ToInt32(Debugintersectpoints[i].Y) - 8) - position + middleposition, circlecolor);
                            //_spriteBatch.Draw(pixel, Debugintersectpoints[i] - position + middleposition, circlecolor);
                        }
                        for (int i = 0; i < DebugIntersectBlocksY.Length; i++)
                        {
                            Color circlecolor = LetterColors[i - (i / 16) * 16];
                            if (i == DebugintersectpointchosenY) // 
                                if (!IschosenpointX) circlecolor = Time % 2 == 0 ? LetterColors[0] : LetterColors[7];
                                else circlecolor = Time % 2 == 0 ? LetterColors[1] : LetterColors[8];
                            //RectangleSprite.DrawRectangle(_spriteBatch, new Rectangle(Convert.ToInt32(DebugIntersectBlocksY[i].X - position.X + middleposition.X + XOffsetPlayer1), Convert.ToInt32(DebugIntersectBlocksY[i].Y - position.Y + middleposition.Y), Convert.ToInt32(1), Convert.ToInt32(blocksize)), circlecolor, 2);
                            RectangleSprite.DrawRectangle(_spriteBatch, new Rectangle(Convert.ToInt32(DebugIntersectBlocksY[i].X - position.X + middleposition.X), Convert.ToInt32(DebugIntersectBlocksY[i].Y - position.Y + middleposition.Y + YOffsetPlayer2 - YOffsetPlayerVelocity2), Convert.ToInt32(1), Convert.ToInt32(blocksize)), circlecolor, 2);

                        }
                        //_spriteBatch.Draw(circletransparent, new Vector2(Convert.ToInt32(Debugintersectpoint.Y - 8), Convert.ToInt32(Debugintersectpoint.Y) - 8) - position + middleposition, Color.Black);

                        //Player2
                        SpriteBatchExtensions.DrawVertices(_spriteBatch, pixel, Test2VectricesX, position, middleposition, Color.Green);
                        SpriteBatchExtensions.DrawVertices(_spriteBatch, pixel, Test2VectricesY, position, middleposition, Color.Yellow);
                        RectangleSprite.DrawRectangle(_spriteBatch, new Rectangle(Convert.ToInt32(DebugDetectrange2.X * blocksize - position.X + middleposition.X), Convert.ToInt32(DebugDetectrange2.Y * blocksize - position.Y + middleposition.Y), Convert.ToInt32(DebugDetectrange2.Width * blocksize), Convert.ToInt32(DebugDetectrange2.Height * blocksize)), Color.Blue, 2);
                        for (int i = 0; i < DebugIntersectBlocksX2.Length; i++)
                        {
                            Color circlecolor = LetterColors[i - (i / 16) * 16];
                            if (i == DebugintersectpointchosenX2)
                                if (IschosenpointX2) circlecolor = Time % 2 == 0 ? LetterColors[0] : LetterColors[7];
                                else circlecolor = Time % 2 == 0 ? LetterColors[1] : LetterColors[8];
                            RectangleSprite.DrawRectangle(_spriteBatch, new Rectangle(Convert.ToInt32(DebugIntersectBlocksX2[i].X - position.X + middleposition.X + XOffsetPlayer1 - XOffsetPlayerVelocity1), Convert.ToInt32(DebugIntersectBlocksX2[i].Y - position.Y + middleposition.Y), Convert.ToInt32(blocksize), Convert.ToInt32(1)), circlecolor, 2);

                            //_spriteBatch.Draw(circletransparent, new Vector2(Convert.ToInt32(Debugintersectpoints[i].X  - 8), Convert.ToInt32(Debugintersectpoints[i].Y) - 8) - position + middleposition, circlecolor);
                            //_spriteBatch.Draw(pixel, Debugintersectpoints[i] - position + middleposition, circlecolor);
                        }
                        for (int i = 0; i < DebugIntersectBlocksY2.Length; i++)
                        {
                            Color circlecolor = LetterColors[i - (i / 16) * 16];
                            if (i == DebugintersectpointchosenY2) // 
                                if (!IschosenpointX2) circlecolor = Time % 2 == 0 ? LetterColors[0] : LetterColors[7];
                                else circlecolor = Time % 2 == 0 ? LetterColors[1] : LetterColors[8];
                            RectangleSprite.DrawRectangle(_spriteBatch, new Rectangle(Convert.ToInt32(DebugIntersectBlocksY2[i].X - position.X + middleposition.X), Convert.ToInt32(DebugIntersectBlocksY2[i].Y - position.Y + middleposition.Y + YOffsetPlayer1 - YOffsetPlayerVelocity1), Convert.ToInt32(1), Convert.ToInt32(blocksize)), circlecolor, 2);
                        }
                    }
                    DrawRectangle(new Rectangle((int)(FinalCoords2.X - position.X + middleposition.X - XOffsetPlayer2), (int)(FinalCoords2.Y - position.Y + middleposition.Y - YOffsetPlayer2), blocksize, blocksize), Color.Black * (float)0.5);
                    DrawRectangle(new Rectangle((int)(FinalCoords.X - position.X + middleposition.X - XOffsetPlayer1), (int)(FinalCoords.Y - position.Y + middleposition.Y - YOffsetPlayer1), blocksize, blocksize), Color.Black * (float)0.5);
                    DrawRectangle(new Rectangle((int)(FinalCoordsTest2.X - position.X + middleposition.X), (int)(FinalCoordsTest2.Y - position.Y + middleposition.Y), blocksize, blocksize), Color.Red * (float)0.5);
                    DrawRectangle(new Rectangle((int)(FinalCoordsTest.X - position.X + middleposition.X), (int)(FinalCoordsTest.Y - position.Y + middleposition.Y), blocksize, blocksize), Color.Red * (float)0.5);
                    //_spriteBatch.Draw(pixel, new Vector2((float)StartPixel.X, (float)StartPixel.Y) - position + middleposition, Color.Magenta);
                    //_spriteBatch.Draw(pixel, new Vector2((float)EndPixel.X, (float)EndPixel.Y) - position + middleposition, Color.Orange);
                }

                else if (PlayerPlaying3)
                {
                    _spriteBatch.Draw(Blocks[1], PlayerLocation - position + middleposition, colorclear);
                    _spriteBatch.Draw(Blocks[2], Player2Location - position + middleposition, colorclear);
                    _spriteBatch.Draw(Blocks[3], Player3Location - position + middleposition, colorclear);
                    if (ReadytoDraw)
                    {//Player1
                        SpriteBatchExtensions.DrawVertices(_spriteBatch, pixel, TestVectricesX, position, middleposition, Color.Green);
                        SpriteBatchExtensions.DrawVertices(_spriteBatch, pixel, TestVectricesY, position, middleposition, Color.Yellow);
                        //RectangleSprite.DrawRectangle(_spriteBatch, new Rectangle(Convert.ToInt32(DebugDetectrange.X * blocksize - position.X + middleposition.X), Convert.ToInt32(DebugDetectrange.Y * blocksize - position.Y + middleposition.Y), Convert.ToInt32(DebugDetectrange.Width * blocksize), Convert.ToInt32(DebugDetectrange.Height * blocksize)), Color.Blue, 2);
                       
                        //_spriteBatch.Draw(circletransparent, new Vector2(Convert.ToInt32(Debugintersectpoint.Y - 8), Convert.ToInt32(Debugintersectpoint.Y) - 8) - position + middleposition, Color.Black);

                        //Player2
                        SpriteBatchExtensions.DrawVertices(_spriteBatch, pixel, Test2VectricesX, position, middleposition, Color.Green);
                        SpriteBatchExtensions.DrawVertices(_spriteBatch, pixel, Test2VectricesY, position, middleposition, Color.Yellow);
                        //RectangleSprite.DrawRectangle(_spriteBatch, new Rectangle(Convert.ToInt32(DebugDetectrange2.X * blocksize - position.X + middleposition.X), Convert.ToInt32(DebugDetectrange2.Y * blocksize - position.Y + middleposition.Y), Convert.ToInt32(DebugDetectrange2.Width * blocksize), Convert.ToInt32(DebugDetectrange2.Height * blocksize)), Color.Blue, 2);

                        SpriteBatchExtensions.DrawVertices(_spriteBatch, pixel, Test3VectricesX, position, middleposition, Color.Green);
                        SpriteBatchExtensions.DrawVertices(_spriteBatch, pixel, Test3VectricesY, position, middleposition, Color.Yellow);

                        SpriteBatchExtensions.DrawVertices(_spriteBatch, pixel, SecondTestVectricesX, position, middleposition, Color.Red);
                        SpriteBatchExtensions.DrawVertices(_spriteBatch, pixel, SecondTestVectricesY, position, middleposition, Color.Blue);
                        SpriteBatchExtensions.DrawVertices(_spriteBatch, pixel, SecondTest2VectricesX, position, middleposition, Color.Red);
                        SpriteBatchExtensions.DrawVertices(_spriteBatch, pixel, SecondTest2VectricesY, position, middleposition, Color.Blue);
                        SpriteBatchExtensions.DrawVertices(_spriteBatch, pixel, SecondTest3VectricesX, position, middleposition, Color.Red);
                        SpriteBatchExtensions.DrawVertices(_spriteBatch, pixel, SecondTest3VectricesY, position, middleposition, Color.Blue);
                    }
                    
                    DrawRectangle(new Rectangle((int)(FinalCoordsTest3.X - position.X + middleposition.X), (int)(FinalCoordsTest3.Y - position.Y + middleposition.Y), blocksize, blocksize), Color.Red * (float)0.5);

                    DrawRectangle(new Rectangle((int)(FinalCoordsTest2.X - position.X + middleposition.X), (int)(FinalCoordsTest2.Y - position.Y + middleposition.Y), blocksize, blocksize), Color.Red * (float)0.5);
                    DrawRectangle(new Rectangle((int)(FinalCoordsTest.X - position.X + middleposition.X), (int)(FinalCoordsTest.Y - position.Y + middleposition.Y), blocksize, blocksize), Color.Red * (float)0.5);
                    
                    DrawRectangle(new Rectangle((int)(FinalCoords3.X - position.X + middleposition.X), (int)(FinalCoords3.Y - position.Y + middleposition.Y), blocksize, blocksize), Color.Black * (float)0.5);
                    DrawRectangle(new Rectangle((int)(FinalCoords2.X - position.X + middleposition.X), (int)(FinalCoords2.Y - position.Y + middleposition.Y), blocksize, blocksize), Color.Black * (float)0.5);
                    DrawRectangle(new Rectangle((int)(FinalCoords.X - position.X + middleposition.X), (int)(FinalCoords.Y - position.Y + middleposition.Y), blocksize, blocksize), Color.Black * (float)0.5);
                    //_spriteBatch.Draw(pixel, new Vector2((float)StartPixel.X, (float)StartPixel.Y) - position + middleposition, Color.Magenta);
                    //_spriteBatch.Draw(pixel, new Vector2((float)EndPixel.X, (float)EndPixel.Y) - position + middleposition, Color.Orange);
                }


                //_spriteBatch.Draw(Player, middleposition + playerOffSet, sourceRectangle, colorclear, rotation, origin, scale, spriteEffects, layerDepth);

                Color mappixelcolor;
                for (int x = Coords.X / blocksize - minimapsize; x < Coords.X / blocksize + minimapsize; x++)
                    for (int y = Coords.Y / blocksize - minimapsize; y < Coords.Y / blocksize + minimapsize; y++)
                    {
                        //System.Windows.Forms.MessageBox.Show((x - (Coords.X  / blocksize - mapsize)).ToString() + " " + (y - (Coords.Y / blocksize - mapsize)).ToString());
                        if ((x > -1) && (x < width) && (y > -1) && (y < height))
                            mappixelcolor = miniMapColors[world[x, y]];
                        else
                            mappixelcolor = miniMapColors[0];
                        if (x - (Coords.X / blocksize - minimapsize) >= minimapsize - playersize / blocksize / 2 && y - (Coords.Y / blocksize - minimapsize) < minimapsize + playersize / blocksize / 2 && y - (Coords.Y / blocksize - minimapsize) >= minimapsize - playersize / blocksize / 2 && x - (Coords.X / blocksize - minimapsize) < minimapsize + playersize / blocksize / 2)
                            mappixelcolor = Color.Black;
                        _spriteBatch.Draw(pixel, new Vector2(x - (Coords.X / blocksize - minimapsize) + this.GraphicsDevice.Viewport.Width - 50 - minimapsize * 2, y - (Coords.Y / blocksize - minimapsize) + this.GraphicsDevice.Viewport.Height - 50 - minimapsize * 2), mappixelcolor);
                    }

                for (int i = 0; i < Coordinates.chars.Length; i++)
                    _spriteBatch.Draw(Letters[Coordinates.chars[i]], Coordinates.coords[i] + new Vector2(this.GraphicsDevice.Viewport.Width - 50 - minimapsize - Coordinates.size.Width / 2, this.GraphicsDevice.Viewport.Height - 50 + 5), LetterColors[Coordinates.color[i]]);
                RectangleSprite.DrawRectangle(_spriteBatch, new Rectangle(this.GraphicsDevice.Viewport.Width - 50 - minimapsize - Coordinates.size.Width / 2, this.GraphicsDevice.Viewport.Height - 50 + 5, Coordinates.size.Width, Coordinates.size.Height), Color.Red, 1);

                for (int b = 0; b < Inventory.NumberOfInvSlots; b++)
                    if (Inventory.Slots[b].visible)
                    {
                        _spriteBatch.DrawRoundedRect(Inventory.Slots[b].Rectangle, circle, circle.Width / 2, Inventory.Slots[b].Selected ? Color.Red : Color.OrangeRed);
                        if (Inventory.Slots[b].Item.number != 0)
                            _spriteBatch.Draw(Inventory.Items[Inventory.Slots[b].Item.number].image, new Vector2(Inventory.Slots[b].Rectangle.X + Inventory.Slots[b].Rectangle.Width / 2 - Inventory.Items[Inventory.Slots[b].Item.number].image.Width / 2, Inventory.Slots[b].Rectangle.Y + Inventory.Slots[b].Rectangle.Height / 2 - Inventory.Items[Inventory.Slots[b].Item.number].image.Height / 2), colorclear);
                    }

                //if (Paused)
                //{
                //    Color settingsbuttonColor = settingsButtonSelected? Color.Red : colorclear;
                //    for (int i = 0; i < SettingsButton.chars.Length; i++)
                //        _spriteBatch.Draw(Letters[SettingsButton.chars[i]], SettingsButton.coords[i] + new Vector2(this.GraphicsDevice.Viewport.Width - SettingsButton.size.Width - 20, 20), settingsbuttonColor);
                //}
                for (int p = 0; p < Numberofplanets; p++)
                {
                    //RectangleSprite.DrawRectangle(_spriteBatch, new Rectangle(Convert.ToInt32(planets[p].position.X  - planets[p].velocity.X  * blocksize - position.X  + middleposition.X ), Convert.ToInt32(planets[p].position.Y - planets[p].velocity.Y * blocksize - position.Y + middleposition.Y), Convert.ToInt32(planets[p].velocity.X  * blocksize), Convert.ToInt32(planets[p].velocity.Y * blocksize)), Color.Red, 2);
                    int height = 1;
                    int width = 1;
                    int x = Convert.ToInt32(planets[p].position.X * blocksize - position.X + middleposition.X);
                    int y = Convert.ToInt32(planets[p].position.Y * blocksize - position.Y + middleposition.Y);
                    if (planets[p].overlap == 1 || planets[p].overlap == 2)
                    {
                        height = blocksize;
                        width = 2;
                        if (planets[p].overlap == 1)//LEFT
                        {
                            x += blocksize - width;

                        }
                    }
                    if (planets[p].overlap == 3 || planets[p].overlap == 4)
                    {
                        width = blocksize;
                        height = 2;
                        if (planets[p].overlap == 3)//TOP
                        {
                            y += blocksize - height;
                        }
                    }
                    if (planets[p].vertices != null)
                        SpriteBatchExtensions.DrawVertices(_spriteBatch, pixel, planets[p].vertices, position, middleposition, Color.Red);
                    _spriteBatch.Draw(pixel, new Rectangle(x, y, width, height), Color.Red);
                    //RectangleSprite.DrawRectangle(_spriteBatch, new Rectangle(Convert.ToInt32(planets[p].position.X  - position.X  + middleposition.X ), Convert.ToInt32(planets[p].position.Y - position.Y + middleposition.Y), Convert.ToInt32(planets[p].intersect.X ), Convert.ToInt32(planets[p].intersect.Y)), Color.Red, 1);
                    RectangleSprite.DrawRectangle(_spriteBatch, new Rectangle(Convert.ToInt32(planets[p].Detectrange.X * blocksize - position.X + middleposition.X), Convert.ToInt32(planets[p].Detectrange.Y * blocksize - position.Y + middleposition.Y), Convert.ToInt32(planets[p].Detectrange.Width * blocksize), Convert.ToInt32(planets[p].Detectrange.Height * blocksize)), Color.Blue, 2);

                }
            }
            else if (CurrentlyDisplaying == DEBUGLOADING)
            {

            }
            else if (CurrentlyDisplaying == DEBUG)
            {
                GraphicsDevice.Clear(Color.LightSteelBlue);
                Color blockcolor;
                for (int x = Coords.X / blocksize - loadingareaX; x < Coords.X / blocksize + loadingareaX; x++)
                    for (int y = Coords.Y / blocksize - loadingareaY; y < Coords.Y / blocksize + loadingareaY; y++)
                        if ((x > -1) && (x < width) && (y > -1) && (y < height))
                            if (world[x, y] > 0)
                            {
                                if (x == SelectedBlock.X && y == SelectedBlock.Y)
                                    blockcolor = Color.LightGray;
                                else
                                    blockcolor = colorclear;
                                _spriteBatch.Draw(Blocks[world[x, y]], new Vector2(x * blocksize, y * blocksize) - position + middleposition, blockcolor);
                            }
                if (ReadytoDraw)
                {
                    SpriteBatchExtensions.DrawVertices(_spriteBatch, pixel, TestVectricesX, position, middleposition, Color.Green);
                    SpriteBatchExtensions.DrawVertices(_spriteBatch, pixel, TestVectricesY, position, middleposition, Color.Green);
                    RectangleSprite.DrawRectangle(_spriteBatch, new Rectangle(Convert.ToInt32(DebugDetectrange.X * blocksize - position.X + middleposition.X), Convert.ToInt32(DebugDetectrange.Y * blocksize - position.Y + middleposition.Y), Convert.ToInt32(DebugDetectrange.Width * blocksize), Convert.ToInt32(DebugDetectrange.Height * blocksize)), Color.Blue, 2);
                    for (int i = 0; i < DebugIntersectBlocksX.Length; i++)
                    {
                        Color circlecolor = LetterColors[i - (i / 16)*16];
                        if (IschosenpointX == true &&  i == DebugintersectpointchosenX) //  
                            circlecolor = Time % 2 == 0 ? Color.White : Color.Black;
                        RectangleSprite.DrawRectangle(_spriteBatch, new Rectangle(Convert.ToInt32(DebugIntersectBlocksX[i].X * blocksize - position.X + middleposition.X), Convert.ToInt32(DebugIntersectBlocksX[i].Y * blocksize - position.Y + middleposition.Y + YOffset), Convert.ToInt32(blocksize), Convert.ToInt32(1)), circlecolor, 2);

                        //_spriteBatch.Draw(circletransparent, new Vector2(Convert.ToInt32(Debugintersectpoints[i].X  - 8), Convert.ToInt32(Debugintersectpoints[i].Y) - 8) - position + middleposition, circlecolor);
                        //_spriteBatch.Draw(pixel, Debugintersectpoints[i] - position + middleposition, circlecolor);
                    }
                    for (int i = 0; i < DebugIntersectBlocksY.Length; i++)
                    {
                        Color circlecolor = LetterColors[i - (i / 16) * 16];
                        if (IschosenpointX == false && i == DebugintersectpointchosenY) // 
                            circlecolor = Time % 2 == 0 ? Color.White : Color.Black;
                        RectangleSprite.DrawRectangle(_spriteBatch, new Rectangle(Convert.ToInt32(DebugIntersectBlocksY[i].X * blocksize - position.X + middleposition.X + XOffset), Convert.ToInt32(DebugIntersectBlocksY[i].Y * blocksize - position.Y + middleposition.Y), Convert.ToInt32(1), Convert.ToInt32(blocksize)), circlecolor, 2);
                    }
                    //_spriteBatch.Draw(circletransparent, new Vector2(Convert.ToInt32(Debugintersectpoint.Y - 8), Convert.ToInt32(Debugintersectpoint.Y) - 8) - position + middleposition, Color.Black);

                }




            }
            for (int i = 0; i < Extratext.chars.Length; i++)
                _spriteBatch.Draw(Letters[Extratext.chars[i]], Extratext.coords[i] + new Vector2(50, this.GraphicsDevice.Viewport.Height - 50), LetterColors[Extratext.color[i]]);


            //_spriteBatch.Draw(createCircleText(300), new Vector2(200, 300), Color.Yellow);
            //DrawRectangle(new Rectangle(700, 100, circle.Width, circle.Width), Color.Red);
            //_spriteBatch.Draw(circle, new Vector2(700, 100), colorclear);
            for (int b = 0; b < Buttons.Count(); b++)
                if (Buttons[b].visible)
                {
                    Color LetterColor = colorclear;
                    if (Buttons[b].TextOnly == false)
                    {
                        _spriteBatch.DrawRoundedRect(Buttons[b].Rectangle, // The coordinates of the Rectangle to be drawn
circle, // Texture for the whole rounded rectangle
circle.Width / 2, // Distance from the edges of the texture to the "middle" patch
Buttons[b].Selected ? Color.Red : Color.OrangeRed);
                    }
                    else
                    {
                        LetterColor = Buttons[b].Selected ? Color.Red : colorclear;
                    }
                    for (int i = 0; i < Buttons[b].text.chars.Length; i++)
                    {
                        if (LetterColor == colorclear)
                            LetterColor = LetterColors[Buttons[b].text.color[i]];
                        _spriteBatch.Draw(Letters[Buttons[b].text.chars[i]], Buttons[b].text.coords[i] + new Vector2(Buttons[b].Rectangle.X + Buttons[b].Rectangle.Width / 2 - Buttons[b].text.size.Width / 2, Buttons[b].Rectangle.Y + Buttons[b].Rectangle.Height / 2 - Buttons[b].text.size.Height / 2), LetterColor);
                    }
                        
                    //RectangleSprite.DrawRectangle(_spriteBatch, new Rectangle(playbutton.Rectangle.X  + playbutton.Rectangle.Width / 2 - playbutton.text.size.Width / 2, playbutton.Rectangle.Y + playbutton.Rectangle.Height / 2 - playbutton.text.size.Height / 2, playbutton.text.size.Width, playbutton.text.size.Height), Color.Red, 1);

                }



            if (Infomode)
                for (int i = 0; i < Info.chars.Length; i++)
                    _spriteBatch.Draw(Letters[Info.chars[i]], Info.coords[i] + new Vector2(25, 100), LetterColors[Info.color[i]]);
            
            if (ChatOn)
            {
                DrawRectangle(new Rectangle(25, this.GraphicsDevice.Viewport.Height - 50 - Chat.Lines * 16, Chat.data.size.Width, Chat.data.size.Height), Color.Black * Chat.OpacityOfRectangle);
                for (int i = 0; i < Chat.data.chars.Length; i++)
                    _spriteBatch.Draw(Letters[Chat.data.chars[i]], Chat.data.coords[i] + new Vector2(25, this.GraphicsDevice.Viewport.Height - 50 - Chat.Lines * 16), LetterColors[Chat.data.color[i]] * (float)(Convert.ToDouble(Chat.TimeDisplayed[Chat.TotalLines - (Chat.Lines - Chat.data.LinePerCharecter[i])] + Chat.TimePerLine - Time)/ Chat.LengthOfFade));
                //_spriteBatch.Draw(Chat.data.size, Chat.data.size, Color.Black * 0.5F);
                //RectangleSprite.DrawRectangle(_spriteBatch, new Rectangle(25, this.GraphicsDevice.Viewport.Height - 50 - Chat.Lines * 16, Chat.data.size.Width, Chat.data.size.Height), Color.Black * 0.5F, 1);
                
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }

        private void Window_ClientSizeChanged(object sender, System.EventArgs e)
        {
            //Window.ClientSizeChanged -= Window_ClientSizeChanged;
            //_graphics.PreferredBackBufferWidth = Window.ClientBounds.Width < 100 ? 100 : Window.ClientBounds.Width;
            //_graphics.PreferredBackBufferHeight = Window.ClientBounds.Height < 100 ? 100 : Window.ClientBounds.Height;
            //_graphics.ApplyChanges();
            //Window.ClientSizeChanged += Window_ClientSizeChanged;
            screensizechanged = true;
        }

        void MovePlayer()
        {
            //System.Threading.Thread.Sleep(SlowMo);
            if (((Keyboard.GetState().IsKeyUp(Microsoft.Xna.Framework.Input.Keys.Space)) || jumping == false) && verticalVelocity > 0)
            {
                if (spiderJumpNum > 0)
                {
                    Player = PlayerJumping[spiderJumpNum - 1];
                    spiderJumpNum -= 1;
                }
                else if (Keyboard.GetState().IsKeyUp(Microsoft.Xna.Framework.Input.Keys.A) && (Keyboard.GetState().IsKeyUp(Microsoft.Xna.Framework.Input.Keys.D)) || spiderJumpNum == 0)
                {
                    spiderJumpNum = -1;
                    Player = PlayerIdle;
                    Idle = true;
                }

            }
            else if ((Keyboard.GetState().IsKeyUp(Microsoft.Xna.Framework.Input.Keys.Space)) && verticalVelocity == 0 && (Keyboard.GetState().IsKeyUp(Microsoft.Xna.Framework.Input.Keys.A) && (Keyboard.GetState().IsKeyUp(Microsoft.Xna.Framework.Input.Keys.D)) && spiderJumpNum == -1 && !Idle))
            {
                Player = PlayerIdle;
                Idle = true;
            }
            playerrectangle.Location = new Point(Coords.X + Convert.ToInt32(playerOffSet.X), Coords.Y + Convert.ToInt32(playerOffSet.Y) - spiderJumpNum * (playersize / 32) - playersize / 32 + playersize / 32 * 9);
            if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A) && !Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D))
            {
                if (onground && (Time % 2) == 0)
                {
                    if (spiderSequenceNum < 3)
                    {
                        Player = PlayerWalking[spiderSequenceNum + 1];
                        spiderSequenceNum += 1;

                    }

                    else
                    {
                        Player = PlayerWalking[0];
                        spiderSequenceNum = 0;
                    }
                }

                if (Collide.CheckCollisionsLeft(world, width, height, blocksize, blockrectangle, playerrectangle))
                    Coords.X -= 1;

                if (spriteEffects == SpriteEffects.FlipHorizontally)
                    spriteEffects = SpriteEffects.None;
                Idle = false;
            }
            if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D) && !Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A))
            {
                if (horizontalVelocity < 10)
                horizontalVelocity += 0.1;
            }
            if (horizontalVelocity > 0)
                for (int v = 0; v < 1 + Time % Math.Ceiling(horizontalVelocity); v++)
                {
                    playerrectangle.Location = new Point(Coords.X + Convert.ToInt32(playerOffSet.X), Coords.Y + Convert.ToInt32(playerOffSet.Y) - spiderJumpNum * (playersize / 32) - playersize / 32 + playersize / 32 * 9);

                    if (onground && (Time % 2) == 0)
                    {
                        if (spiderSequenceNum < 3)
                        {
                            Player = PlayerWalking[spiderSequenceNum + 1];
                            spiderSequenceNum += 1;
                        }
                        else
                        {
                            Player = PlayerWalking[0];
                            spiderSequenceNum = 0;
                        }
                    }

                    if (Collide.CheckCollisionsRight(world, width, height, blocksize, blockrectangle, playerrectangle))
                    {
                        Coords.X += 1;
                    }
                    else
                        horizontalVelocity = 0;

                    if (spriteEffects != SpriteEffects.FlipHorizontally)
                        spriteEffects = SpriteEffects.FlipHorizontally;
                    Idle = false;
                }

            if (Jump.CheckKeyPress(true))
            {
                if (onground && !Jump.Pressed)
                {
                    verticalVelocity += -7;
                }
                else if (jumping)
                    verticalVelocity += -0.1;
                if (!Jump.Pressed && !falling)
                    Jump.Pressed = true;
            }
            for (int v = 0; v > verticalVelocity; v--)
            {
                playerrectangle.Location = new Point(Coords.X  + +Convert.ToInt32(playerOffSet.X), Coords.Y + Convert.ToInt32(playerOffSet.Y) - spiderJumpNum * (playersize / 32) - playersize / 32 + playersize / 32 * 9);
                if (Collide.CheckCollisionsUp(world, width, height, blocksize, blockrectangle, playerrectangle))
                {
                    if (spiderJumpNum < 2 && v == 0)
                    {
                        Player = PlayerJumping[spiderJumpNum + 1];
                        spiderJumpNum += 1;
                    }
                    else
                    {
                        Coords.Y -= 1;
                    }
                    onground = false;
                    Idle = false;
                    jumping = true;
                }
                else
                {
                    if (verticalVelocity < -1)
                        verticalVelocity += 0.5;
                    jumping = false;
                    break;

                }

            }

                playerrectangle.Location = new Point(Coords.X  + Convert.ToInt32(playerOffSet.X), Coords.Y + Convert.ToInt32(playerOffSet.Y) + playersize / 32 * 9);

            if (onground && spiderJumpNum > -1)
            {
                if (spiderJumpNum > 0)
                {
                    Player = PlayerJumping[spiderJumpNum - 1];
                    spiderJumpNum -= 1;
                    Coords.Y += -playersize / 32;
                }
                else
                {
                    Player = PlayerIdle;
                    spiderJumpNum -= 1;
                    Coords.Y += -playersize / 32;
                }

            }

            for (int v = 0; v < verticalVelocity + 0.5; v++)
            {
                playerrectangle.Location = new Point(Coords.X  + Convert.ToInt32(playerOffSet.X), Coords.Y + Convert.ToInt32(playerOffSet.Y) + playersize / 32 * 9);
                if (Collide.CheckCollisionsDown(world, width, height, blocksize, blockrectangle, playerrectangle))
                {
                    Coords.Y += 1;
                    onground = false;
                    falling = true;
                }
                else
                {
                    falling = false;
                    jumping = false;
                    verticalVelocity = 0;
                    onground = true;
                    break;
                }
            }
            if (verticalVelocity < 0 || falling)
            {
                if (verticalVelocity < maxvelocity)
                    verticalVelocity += 0.4;
            }
            playerrectangle.Location = new Point(Coords.X  + Convert.ToInt32(playerOffSet.X), Coords.Y + Convert.ToInt32(playerOffSet.Y) + playersize / 32 * 9);
        }

        (Char[], Vector2[], int[], Rectangle, int[], int) Getcoordinateschar(string data)
        {
            char[] chararrayprivate = data.ToCharArray(), chararray;
            chararray = chararrayprivate;
            int length = 0, newlength = 0, height = 16;
            //string test = "", test2 = "";
            Vector2[] CoordinatesString = new Vector2[chararrayprivate.Length];
            int[] ColorString = new int [chararrayprivate.Length];
            int[] numberofchararray = new int[chararrayprivate.Length];
            int[] LinesPerCharecter = new int[chararrayprivate.Length];
            int newLine = 0, charectersinpreviousline = 0, TotalWeirdChar = 0, SpacesPerLine = 0, WeirdCharPerLine = 0;
            Boolean ColorChar = false;
            int ColorOfLetter = 0;
            //System.Windows.Forms.MessageBox.Show((index).ToString());
            for (int i = 0; i < chararrayprivate.Length; i++)
            {
                char c;
                c = chararrayprivate[i];
                numberofchararray[i] = c;
                chararray[i - newLine - TotalWeirdChar] = chararrayprivate[i];
                //test2 += i.ToString() + "'" + numberofchararray[i].ToString() + "', ";
                if (ColorChar)
                {
                    if (numberofchararray[i] > 96 && numberofchararray[i] < 122)
                        ColorOfLetter = numberofchararray[i] - 97;
                    else if (numberofchararray[i] == 122)//z
                        ColorOfLetter = 20;
                    TotalWeirdChar += 1;
                    WeirdCharPerLine += 1;
                    ColorChar = false;
                }
                else if (numberofchararray[i] == 10)//enter or newline
                {
                    //System.Array.Resize(ref charectersperline, newLine + 1);
                    //charectersperline[newLine] = i + 1 - charectersinpreviousline;
                    newLine += 1;
                    charectersinpreviousline = i + 1;
                    SpacesPerLine = 0;
                    WeirdCharPerLine = 0;
                    height += 16;
                    length = length < newlength ? newlength : length;
                    newlength = 0;
                }
                else if (numberofchararray[i] == 32) //space
                {
                    SpacesPerLine += 1;
                    TotalWeirdChar += 1;
                    newlength += 8;
                }
                else if (numberofchararray[i] == 937) //Ω
                {
                    ColorChar = true;
                    TotalWeirdChar += 1;
                    WeirdCharPerLine += 1;
                }
                else
                {

                    if (numberofchararray[i] > 128)
                    {
                        System.Windows.Forms.MessageBox.Show(((char)numberofchararray[i]).ToString());
                        WeirdCharPerLine += 1;
                        TotalWeirdChar += 1;
                    }
                    else if (Letters[numberofchararray[i]] == null)
                    {
                        numberofchararray[i] = rnd.Next(97, 123);
                        chararray[i - newLine - TotalWeirdChar] = (char)numberofchararray[i];
                        //System.Windows.Forms.MessageBox.Show(chararray[i - newLine - TotalWeirdChar].ToString());
                    }
                    newlength += 16;
                    CoordinatesString[i - newLine - TotalWeirdChar] = new Vector2((i - SpacesPerLine - WeirdCharPerLine) * 16 - charectersinpreviousline * 16 + SpacesPerLine * 8, 0 + newLine * 16);
                    ColorString[i - newLine - TotalWeirdChar] = ColorOfLetter;
                    LinesPerCharecter[i - newLine - TotalWeirdChar] = newLine;
                    //test += (i.ToString() + "'" + ((char)c).ToString() + "', ");
                }

            }
            //System.Array.Resize(ref charectersperline, newLine + 1);
            //charectersperline[newLine] = chararrayprivate.Length - charectersinpreviousline;
            length = length < newlength ? newlength : length;
            //System.Windows.Forms.MessageBox.Show(test2);
            System.Array.Resize(ref chararray, chararrayprivate.Length - newLine - TotalWeirdChar);
            Rectangle size = new Rectangle(0, 0, length, height);
            return (chararray, CoordinatesString, ColorString, size, LinesPerCharecter, newLine + 1);
        }

        void DoLettersOnScreen()
        {
            //string data = "ABCDEFGHIJ1fFJKAL \n f" + System.Environment.NewLine + "f";
            //string data = "abcdefghi jklmn\nopqrstuv wxyz1234567890\nABCDEFGHIJKLMN OPQRSTUVWXYZ";
            // string data = ((blockrectangle[((Convert.ToInt32(monster[0].rectangle.X ) - monster[0].rectangle.Size.X  / 2 + 24) / blocksize) - 1, ((Convert.ToInt32(monster[0].rectangle.Y) - monster[0].rectangle.Size.Y) / blocksize) - 1].Bottom + height / 2 * blocksize) + " " + (monster[0].rectangle.Top).ToString());
            //string data = playerrectangle.X .ToString() + " " + playerrectangle.Y.ToString();
            //string data = (monster[0].rectangle.X ).ToString() + " " + (monster[0].rectangle.Y).ToString() + " " + (monster[0].rectangle.Size.X ).ToString() + " " + (monster[0].rectangle.Size.Y).ToString() + " " + (playerrectangle.X ).ToString() + " " + (playerrectangle.Y).ToString() + " " + (playerrectangle.Size.X ).ToString() + " " + (playerrectangle.Size.Y).ToString();
            //Extratext.text = (MousePosition.X .ToString() + ", " + MousePosition.Y.ToString() + ", " + SelectedBlock.X .ToString() + ", " + SelectedBlock.Y.ToString());
            Extratext.text = "Ωa0Ωb1Ωc2Ωd3Ωe4Ωf5Ωg6Ωh7\nΩi8Ωj9Ωk0Ωl1Ωm2Ωn3Ωo4Ωp5Ωz5  " + "Ωa" + Rainbow.R.ToString() + " " + Rainbow.G.ToString() + " " + Rainbow.B.ToString() + " " + Rainbow.A.ToString();
            Extratext.text = sharedspeedtest.ToString();
            Extratext = DoText(Extratext);
            //(Extratext.chars, Extratext.coords, Extratext.color, Extratext.size, Extratext.CharectersPerLine, Extratext.NumberOfLines) = Getcoordinateschar(textname.text);
            Chat.RefreshText(Time);
            Chat.data = DoText(Chat.data);


            if (CurrentlyDisplaying != MAINMENU)
            {
                Coordinates.text = "Ωg" + Coords.X .ToString() + ", " + Coords.Y.ToString();
                Coordinates = DoText(Coordinates);
                //System.Windows.Forms.MessageBox.Show(Coordinates.size.Width.ToString());
                if (Infomode)
                {

                    int SelectedPlanet = Inventory.Selected;
                    if (SelectedPlanet >= Numberofplanets)
                        SelectedPlanet = Numberofplanets - 1;
                    Info.text = "WORLD:" +
                        "\n  Width: " + width.ToString() + ", Height: " + height.ToString() + " LoadingareaX: " + loadingareaX.ToString() + " LoadingareaY: " + loadingareaY.ToString() + " Seed: " + seed;
                    if (PlayerPlaying) Info.text +=
                           "\n\nPLAYER:" +
                            "\n  ΩbPlayerVelocity X: " + PlayerVelocity.X + " Y: " + PlayerVelocity.Y;
                    if (planets != null && Numberofplanets > 0) Info.text +=
                          "\n\nPLANETS:" +
                          "\n  ΩbNumber of Planets: " + Numberofplanets.ToString() + " Selected Planet: " + SelectedPlanet.ToString() +
                          "\n  Selected Planet's Overlap: " + planets[SelectedPlanet].overlap.ToString() + " Type: " + planets[SelectedPlanet].blocktype.ToString() + " Velocity: " + (Math.Floor(planets[SelectedPlanet].velocity.X * 100) / 100).ToString() + ", " + (Math.Floor(planets[SelectedPlanet].velocity.Y * 100) / 100).ToString() +
                          //"\n Position: " + (Math.Floor(planets[SelectedPlanet].position.X / blocksize * 10) / 10).ToString() + ", " + (Math.Floor(planets[SelectedPlanet].position.Y / blocksize * 10) / 10).ToString() +
                          "\n Position: " + (Math.Floor(planets[SelectedPlanet].position.X * 100) / 100).ToString() + ", " + (Math.Floor(planets[SelectedPlanet].position.Y * 100) / 100).ToString() +
                          "\n Hit: " + planets[SelectedPlanet].Hit.ToString() + " Mass: " + planets[SelectedPlanet].weight.ToString() +
                          "\n intersect: " + planets[SelectedPlanet].intersectBlock.X.ToString() + ", " + planets[SelectedPlanet].intersectBlock.Y.ToString() +
                          "\n intersectPosition: " + (Math.Floor(planets[SelectedPlanet].intersectPosition.X / blocksize * 10) / 10).ToString() + ", " + (Math.Floor(planets[SelectedPlanet].intersectPosition.Y / blocksize * 10) / 10).ToString();
                    Info.text +=
                        "\n\nMOUSE:" +
                        "\n  Position X : " + MousePosition.X.ToString() + " Y: " + MousePosition.Y.ToString() +
                        "\n  Selected Block X : " + SelectedBlock.X.ToString() + " Y: " + SelectedBlock.Y.ToString() + " Type: " + SelectedBlockType.ToString() +
                        "\n  PlanetForce X : " + (Math.Floor(PlanetForceOnMouse.X * 10000) / 10000).ToString() + " Y: " + (Math.Floor(PlanetForceOnMouse.Y * 10000) / 10000).ToString();
                    if (maxMonsters >= 0) Info.text +=
                        "\n\nMONSTERS:" +
                        "\n  Number Of Monsters " + numberOfMonsters.ToString() + "/" + maxMonsters.ToString();
                    if (CurrentlyDisplaying == DEBUG || PlayerPlaying)
                    {
                        Info.text +=
                        "Ωb\n\nOFFSET:" +
                        "\n  XOffset: " + XOffsetPlayer1.ToString() + " YOffset: " + YOffsetPlayer1.ToString();
                        Info.text +=
                        "Ωb\n\nVERTICES:" +
                        "\n  Coords.X  Axis:";
                        for (int i = 0; i < TestVectricesX.Length; i++)
                            Info.text += "\n  Number: " + i.ToString() + " Position Coords.X : " + TestVectricesX[i].X.ToString() + " Position Coords.Y: " + TestVectricesX[i].Y.ToString();
                        Info.text +=
                        "\n  Coords.Y Axis:";
                        for (int i = 0; i < TestVectricesY.Length; i++)
                            Info.text += "\n  Number: " + i.ToString() + " Position Coords.X : " + TestVectricesY[i].X.ToString() + " Position Coords.Y: " + TestVectricesY[i].Y.ToString();
                        Info.text +=
                        "Ωb\n\nINTERSECTS:" +
                        "\n  Coords.X  Axis:";
                        for (int i = 0; i < DebugIntersectBlocksX.Length; i++)
                        {
                            string textcolor = ColorString[i - (i / 16) * 16];
                            if (i == DebugintersectpointchosenX)
                                if (IschosenpointX) textcolor = Time % 2 == 0 ? ColorString[0] : ColorString[7];
                            else textcolor = Time % 2 == 0 ? ColorString[1] : ColorString[8];
                            Info.text += textcolor + "\n  Number: " + i.ToString() + " Position Coords.X : " + DebugIntersectBlocksX[i].X.ToString() + " Position Coords.Y: " + DebugIntersectBlocksX[i].Y.ToString();
                        }
                        Info.text +=
                        "Ωb\n  Coords.Y Axis:";
                        for (int i = 0; i < DebugIntersectBlocksY.Length; i++)
                        {
                            string textcolor = ColorString[i - (i / 16) * 16];
                            if (i == DebugintersectpointchosenY)
                                if (!IschosenpointX) textcolor = Time % 2 == 0 ? ColorString[0] : ColorString[7];
                                else textcolor = Time % 2 == 0 ? ColorString[1] : ColorString[8];
                            Info.text += textcolor + "\n  Number: " + i.ToString() + " Position Coords.X : " + DebugIntersectBlocksY[i].X.ToString() + " Position Coords.Y: " + DebugIntersectBlocksY[i].Y.ToString();
                        }
                        //    for (int i = 0; i < Debugintersectpoints.Length; i++)
                        //    {
                        //        string textcolor = ColorString[i - (i / 16) * 16];
                        //        if (i == Debugintersectpointchosen) textcolor = Time % 2 == 0 ? ColorString[0] : ColorString[7];
                        //        Info.text += textcolor + "\n  Number: " + i.ToString() + " Position Coords.X : " + Debugintersectpoints[i].X .ToString() + " Position Coords.Y: " + Debugintersectpoints[i].Y.ToString();
                        //    }
                    }

                    Info.text +=
                        "\n\nΩbTIME: " + Time.ToString();
                    //string info = (MousePosition.X .ToString() + ", " + MousePosition.Y.ToString() + ", " + SelectedBlock.X .ToString() + ", " + SelectedBlock.Y.ToString());
                    Info = DoText(Info);
                }
            }
                if (Rainbow.R == 255 && Rainbow.G == 0 && Rainbow.B > 0)
                    Rainbow.B -= 5;
                else if (Rainbow.R == 255 && Rainbow.G < 255 && Rainbow.B == 0)
                    Rainbow.G += 5;
                else if (Rainbow.R > 0 && Rainbow.G == 255 && Rainbow.B == 0)
                    Rainbow.R -= 5;
                else if (Rainbow.R == 0 && Rainbow.G == 255 && Rainbow.B < 255)
                    Rainbow.B += 5;
                else if (Rainbow.R == 0 && Rainbow.G > 0 && Rainbow.B == 255)
                    Rainbow.G -= 5;
                else if (Rainbow.R < 255 && Rainbow.G == 0 && Rainbow.B == 255)
                    Rainbow.R += 5;
                else
                {
                    Rainbow.R = 255;
                    Rainbow.A = 255;
                }
            LetterColors[20] = Rainbow.col;
        }
        void RedoScreenVars()
        {
            middleposition = new Vector2(this.GraphicsDevice.Viewport.Width / 2, this.GraphicsDevice.Viewport.Height / 2);
            screensizechanged = false;
            if (CurrentlyDisplaying == GAME)
            {
                RedoLoadingarea();
            }
            //for (buttons)
            for (int b = 0; b < Buttons.Count(); b++)
                ResetButtonPosition(b);

            //tracedSize = GraphicsDevice.PresentationParameters.Bounds;
            //canvas = new Texture2D(GraphicsDevice, tracedSize.Width, tracedSize.Height, false, SurfaceFormat.Color);
            ////pixels = new UInt32[tracedSize.Width * tracedSize.Height];
            //System.Array.Resize(ref pixels, tracedSize.Width * tracedSize.Height);
        }
        void RedoLoadingarea()
        {
            loadingareaX = this.GraphicsDevice.Viewport.Width / blocksize / 2 + 1;
            loadingareaY = this.GraphicsDevice.Viewport.Height / blocksize / 2 + blocksize / 8 * 2;
        }

        (DoubleCoordinates, DoubleCoordinates) PlayerDebug(DoubleCoordinates StartPixel, DoubleCoordinates EndingPixel)
        {

            bool hit = false;
            int XModifier = -blocksize;
            int YModifier = -blocksize;
            int StartXModifier = blocksize;
            int StartYModifier = blocksize;
            XOffsetPlayer1 = blocksize;
            YOffsetPlayer1 = blocksize;
            //if (StartDrawing)
            //EndingPixel = new DoubleCoordinates((MousePosition.X + Coords.X - middleposition.X), (MousePosition.Y + Coords.Y - middleposition.Y));
            if (StartPixel.X  < EndingPixel.X)
            {
                XModifier = +blocksize;
                StartXModifier = 0;
               EndingPixel.X += blocksize ;
                StartPixel.X += blocksize;
                XOffsetPlayer1 = 0;
                //Chat.NewLine("Right", Time);
            }
            //else
            //    Chat.NewLine("Left", Time);
            if (StartPixel.Y < EndingPixel.Y)
            { 
                YModifier = +blocksize;
               StartYModifier = 0;
                EndingPixel.Y += blocksize;
                StartPixel.Y += blocksize;
                YOffsetPlayer1 = 0;
                //Chat.NewLine("Down", Time);
            }

            //StartPixel.Y += StartYModifier;
            //StartPixel.X += StartXModifier;
            //else
            //    Chat.NewLine("Up", Time);

            TestVectricesX[0] = new DoubleCoordinates(StartPixel.X, StartPixel.Y);
            TestVectricesX[1] = new DoubleCoordinates(EndingPixel.X, EndingPixel.Y);
            TestVectricesX[2] = new DoubleCoordinates(EndingPixel.X - XModifier, EndingPixel.Y);
            TestVectricesX[3] = new DoubleCoordinates(StartPixel.X - XModifier, StartPixel.Y);

            TestVectricesY[0] = new DoubleCoordinates(StartPixel.X, StartPixel.Y);
            TestVectricesY[1] = new DoubleCoordinates(EndingPixel.X, EndingPixel.Y);
            TestVectricesY[2] = new DoubleCoordinates(EndingPixel.X, EndingPixel.Y - YModifier);
            TestVectricesY[3] = new DoubleCoordinates(StartPixel.X, StartPixel.Y - YModifier);
            ReadytoDraw = true;

            //XOffset = 0;
            //YOffset = 0;
            //if ((StartPixel.X + XModifier) > EndingPixel.X)
            //    XOffset = blocksize;
            //if ((StartPixel.Y + YModifier) > EndingPixel.Y)
            //    YOffset = blocksize;

            double BiggerXThisPlanet = StartPixel.X > EndingPixel.X ? StartPixel.X : EndingPixel.X;
            double BiggerYThisPlanet = StartPixel.Y > EndingPixel.Y ? StartPixel.Y : EndingPixel.Y;
            double SmallerXThisPlanet = StartPixel.X > EndingPixel.X ? EndingPixel.X : StartPixel.X;
            double SmallerYThisPlanet = StartPixel.Y > EndingPixel.Y ? EndingPixel.Y : StartPixel.Y;
            double SmallestTime = 10000;
            DebugintersectpointchosenY = 0;
            DebugintersectpointchosenX = 0;
            IschosenpointX = true;
            int Orientationofbig = 0;
            int numberofintersectpointsX = 0;
            int numberofintersectpointsY = 0;

            DebugIntersectBlocksX = new Point[0];
            DebugIntersectBlocksY = new Point[0];
            //DebugDetectrange = new Rectangle(Convert.ToInt32((SmallerXThisPlanet) / blocksize) - 1, Convert.ToInt32((SmallerYThisPlanet) / blocksize) - 1, Convert.ToInt32((BiggerXThisPlanet) / blocksize) - Convert.ToInt32((SmallerXThisPlanet) / blocksize) + 2, Convert.ToInt32((BiggerYThisPlanet) / blocksize) - Convert.ToInt32((SmallerYThisPlanet) / blocksize) + 2);

            DebugDetectrange = new Rectangle((int)Math.Floor((SmallerXThisPlanet - blocksize + XOffsetPlayer1) / blocksize), (int)Math.Floor((SmallerYThisPlanet - blocksize + YOffsetPlayer1) / blocksize), (int)Math.Ceiling((BiggerXThisPlanet + XOffsetPlayer1) / blocksize) - (int)Math.Floor((SmallerXThisPlanet - blocksize + XOffsetPlayer1) / blocksize), (int)Math.Ceiling((BiggerYThisPlanet + YOffsetPlayer1) / blocksize) - (int)Math.Floor((SmallerYThisPlanet - blocksize + YOffsetPlayer1) / blocksize));
            for (int Y = (int)Math.Floor((SmallerYThisPlanet - blocksize + YOffsetPlayer1) / blocksize); Y < (int)Math.Ceiling((BiggerYThisPlanet + YOffsetPlayer1) / blocksize); Y++)
            {
                for (int X = (int)Math.Floor((SmallerXThisPlanet - blocksize + XOffsetPlayer1) / blocksize); X < (int)Math.Ceiling((BiggerXThisPlanet + XOffsetPlayer1) / blocksize); X++)
                {
                    if ((X > -1) && (X < width) && (Y > -1) && (Y < height))
                        if (world[X, Y] != 0)
                        {
                            //Line Along the Y axis
                            if (Collide.PolyLineGood(TestVectricesY, X * blocksize + XOffsetPlayer1, Y * blocksize, X * blocksize + XOffsetPlayer1, Y * blocksize + blocksize))
                            {
                                Array.Resize(ref DebugIntersectBlocksY, numberofintersectpointsY + 1);
                                DebugIntersectBlocksY[numberofintersectpointsY] = new Point(X, Y);
                                if (((XOffsetPlayer1 == 0 && DebugIntersectBlocksY[numberofintersectpointsY].X < DebugIntersectBlocksY[DebugintersectpointchosenY].X)) || ((XOffsetPlayer1 != 0 && DebugIntersectBlocksY[numberofintersectpointsY].X > DebugIntersectBlocksY[DebugintersectpointchosenY].X)))
                                    //if (DebugIntersectBlocksY[numberofintersectpointsY].X == DebugIntersectBlocksY[DebugintersectpointchosenY].X)
                                    //{
                                    //    if (((YOffset == 0 && DebugIntersectBlocksY[numberofintersectpointsY].Y < DebugIntersectBlocksY[DebugintersectpointchosenY].Y)) || ((YOffset != 0 && DebugIntersectBlocksY[numberofintersectpointsY].Y > DebugIntersectBlocksY[DebugintersectpointchosenY].Y)))
                                    //        DebugintersectpointchosenY = numberofintersectpointsY;
                                    //}
                                    ////if (((XOffset == 0 && DebugIntersectBlocksY[numberofintersectpointsY].X < DebugIntersectBlocksY[DebugintersectpointchosenY].X) && DebugIntersectBlocksY[numberofintersectpointsY].X > StartPixel.X + XModifier) || ((XOffset == blocksize && DebugIntersectBlocksY[numberofintersectpointsY].X > DebugIntersectBlocksY[DebugintersectpointchosenY].X) && DebugIntersectBlocksY[numberofintersectpointsY].X < StartPixel.X + XModifier))
                                    //else
                                        DebugintersectpointchosenY = numberofintersectpointsY;
                                numberofintersectpointsY += 1;
                                IschosenpointX = false;
                                hit = true;
                            }//
                             //Line Along the X axis
                            if (Collide.PolyLineGood(TestVectricesX, X * blocksize, Y * blocksize + YOffsetPlayer1, X * blocksize + blocksize, Y * blocksize + YOffsetPlayer1))
                            {
                                Array.Resize(ref DebugIntersectBlocksX, numberofintersectpointsX + 1);
                                DebugIntersectBlocksX[numberofintersectpointsX] = new Point(X, Y);
                                if (((YOffsetPlayer1 == 0 && DebugIntersectBlocksX[numberofintersectpointsX].Y < DebugIntersectBlocksX[DebugintersectpointchosenX].Y)) || ((YOffsetPlayer1 != 0 && DebugIntersectBlocksX[numberofintersectpointsX].Y > DebugIntersectBlocksX[DebugintersectpointchosenX].Y)))
                                    //if (DebugIntersectBlocksX[numberofintersectpointsX].Y == DebugIntersectBlocksX[DebugintersectpointchosenX].Y)
                                    //{
                                    //    if (((XOffset == 0 && DebugIntersectBlocksX[numberofintersectpointsX].X < DebugIntersectBlocksX[DebugintersectpointchosenX].X)) || ((XOffset != 0 && DebugIntersectBlocksX[numberofintersectpointsX].X > DebugIntersectBlocksX[DebugintersectpointchosenX].X)))
                                    //        DebugintersectpointchosenX = numberofintersectpointsX;
                                    //}
                                    ////if (((YOffset == 0 && DebugIntersectBlocksX[numberofintersectpointsX].Y < DebugIntersectBlocksX[DebugintersectpointchosenX].Y) && DebugIntersectBlocksX[numberofintersectpointsX].Y > StartPixel.Y + YModifier) || ((YOffset == blocksize && DebugIntersectBlocksX[numberofintersectpointsX].Y > DebugIntersectBlocksX[DebugintersectpointchosenX].Y)) && DebugIntersectBlocksX[numberofintersectpointsX].Y < StartPixel.Y + YModifier)
                                    //else
                                        DebugintersectpointchosenX = numberofintersectpointsX;
                                numberofintersectpointsX += 1;
                                IschosenpointX = true;
                                hit = true;
                            }


                        }
                }


            }
            if (numberofintersectpointsX > 0 && numberofintersectpointsY > 0)
            {
                if (((DebugIntersectBlocksX[DebugintersectpointchosenX].Y * blocksize + YOffsetPlayer1 - StartPixel.Y) / (EndingPixel.Y - StartPixel.Y)) == ((DebugIntersectBlocksY[DebugintersectpointchosenY].X * blocksize + XOffsetPlayer1 - StartPixel.X) / (EndingPixel.X - StartPixel.X))) // removeble
                {
                    IschosenpointX = true;
                    if (world[DebugIntersectBlocksX[DebugintersectpointchosenX].X, DebugIntersectBlocksX[DebugintersectpointchosenX].Y - YModifier / blocksize] != 0)
                        IschosenpointX = false;
                }
                else
                    IschosenpointX = (((DebugIntersectBlocksX[DebugintersectpointchosenX].Y * blocksize + YOffsetPlayer1 - (StartPixel.Y)) / (EndingPixel.Y - (StartPixel.Y))) < ((DebugIntersectBlocksY[DebugintersectpointchosenY].X * blocksize + XOffsetPlayer1 - (StartPixel.X)) / (EndingPixel.X - (StartPixel.X))));



                //if (((DebugIntersectBlocksX[DebugintersectpointchosenX].Y * blocksize + YOffset - (StartPixel.Y + YModifier)) / (EndingPixel.Y - (StartPixel.Y + YModifier))) >= 1)
                //    System.Windows.Forms.MessageBox.Show("bruh Coords.Y : (" + DebugIntersectBlocksY[DebugintersectpointchosenY].X.ToString() + " * " + blocksize.ToString() + " + " + YOffset.ToString() + " - (" + StartPixel.Y.ToString() + " + " + YModifier.ToString() + " )) / (" + EndingPixel.Y.ToString() + " - (" + StartPixel.Y.ToString() + " + " + YModifier.ToString() + " )) = " + ((DebugIntersectBlocksY[DebugintersectpointchosenY].X * blocksize + YOffset - (StartPixel.Y + YModifier)) / (EndingPixel.Y - (StartPixel.Y + YModifier))).ToString() + " test if " + ((DebugIntersectBlocksX[DebugintersectpointchosenX].Y * blocksize + YOffset - (StartPixel.Y + YModifier)) / (EndingPixel.Y - (StartPixel.Y + YModifier))).ToString());
                //if (((DebugIntersectBlocksY[DebugintersectpointchosenY].X * blocksize + XOffset - (StartPixel.X + XModifier)) / (EndingPixel.X - (StartPixel.X + XModifier))) >= 1)
                //    System.Windows.Forms.MessageBox.Show("bruh Coords.X : (" + DebugIntersectBlocksX[DebugintersectpointchosenX].Y.ToString() + " * " + blocksize.ToString() + " + " + XOffset.ToString() + " - (" + StartPixel.X.ToString() + " + " + XModifier.ToString() + " )) / (" + EndingPixel.X.ToString() + " - (" + StartPixel.X.ToString() + " + " + XModifier.ToString() + " )) = " + ((DebugIntersectBlocksX[DebugintersectpointchosenX].Y * blocksize + XOffset - (StartPixel.X + XModifier)) / (EndingPixel.X - (StartPixel.X + XModifier))).ToString() + " test if " + ((DebugIntersectBlocksY[DebugintersectpointchosenY].X * blocksize + XOffset - (StartPixel.X + XModifier)) / (EndingPixel.X - (StartPixel.X + XModifier))).ToString());


                Chat.NewLine(" XTime " + ((DebugIntersectBlocksX[DebugintersectpointchosenX].Y * blocksize + YOffsetPlayer1 - (StartPixel.Y)) / (EndingPixel.Y - (StartPixel.Y))).ToString(), Time); // removeble
                Chat.NewLine(" YTime " + ((DebugIntersectBlocksY[DebugintersectpointchosenY].X * blocksize + XOffsetPlayer1 - (StartPixel.X)) / (EndingPixel.X - (StartPixel.X))).ToString(), Time);
            }
            FinalCoords = new DoubleCoordinates(EndingPixel.X - blocksize + XOffsetPlayer1, EndingPixel.Y - blocksize + YOffsetPlayer1);
            bool Auxhit;
            int hitcoords;
            if (hit)
                if (IschosenpointX)
                {
                    FinalCoords.Y = DebugIntersectBlocksX[DebugintersectpointchosenX].Y * blocksize - YModifier;
                    int Ypos = (int)FinalCoords.Y / blocksize;
                    int operatordirection = -XModifier / blocksize;
                    (Auxhit, hitcoords) = Collide.CollisionWithAuxBlocksX(DebugIntersectBlocksX[DebugintersectpointchosenX].X * blocksize, EndingPixel.X, world, blocksize, width, height, operatordirection, Ypos);//
                    if (Auxhit)
                    {
                        FinalCoords.X = (hitcoords + 1 * operatordirection) * blocksize;
                    }
                }
                else
                {
                    FinalCoords.X = DebugIntersectBlocksY[DebugintersectpointchosenY].X *blocksize - XModifier;
                    int Xpos = (int)FinalCoords.X / blocksize;
                    int operatordirection = -YModifier / blocksize;
                    (Auxhit, hitcoords) = Collide.CollisionWithAuxBlocksY(DebugIntersectBlocksY[DebugintersectpointchosenY].Y * blocksize, EndingPixel.Y, world, blocksize, width, height, operatordirection, Xpos);//
                    if (Auxhit)
                    {
                        FinalCoords.Y = (hitcoords + 1 * operatordirection) * blocksize;
                    }
                }



            return (StartPixel, EndingPixel);
        }

        void MultiplePlayerDebug(DoubleCoordinates StartPixel, DoubleCoordinates EndingPixel, DoubleCoordinates StartPixel2, DoubleCoordinates EndingPixel2)
        {
            double mass1 = 1;
            double mass2 = 4;
            DoubleCoordinates useless;
            //(FinalCoordsTest, FinalCoordsTest2, useless, useless) = Collide.DoubleVectorCollision(StartPixel, EndingPixel, StartPixel2, EndingPixel2, mass1, mass2, blocksize);
            XOffsetPlayer1 = 0;
            XOffsetPlayer2 = 0;
            YOffsetPlayer1 = 0;
            YOffsetPlayer2 = 0;
            if (PlayerVelocity.X * Player2Velocity.X > 0)
                if (Math.Abs(PlayerVelocity.X) > Math.Abs(Player2Velocity.X))
                {
                    XOffsetPlayer1 = 0;    //add this to the start
                    //XOffsetPlayer2 = Player2Velocity.X > 0 ? -blocksize : blocksize;
                    if (PlayerVelocity.X > 0)//Opposit cuz smaller one can be 0
                        XOffsetPlayer2 = -blocksize;
                    else if (PlayerVelocity.X < 0)
                        XOffsetPlayer2 = blocksize;
                    
                }
                else
                {
                    //XOffsetPlayer1 = PlayerVelocity.X > 0 ? -blocksize : blocksize;
                    if (Player2Velocity.X > 0)
                        XOffsetPlayer1 = -blocksize;
                    else if (Player2Velocity.X < 0)
                        XOffsetPlayer1 = blocksize;
                    XOffsetPlayer2 = 0;
                }
            if (PlayerVelocity.Y * Player2Velocity.Y > 0)
                if (Math.Abs(PlayerVelocity.Y) > Math.Abs(Player2Velocity.Y))//For some reason the sign is oposite here, But it works, Actually it does not
                {
                    YOffsetPlayer1 = 0;
                    //YOffsetPlayer2 = Player2Velocity.Y > 0 ? -blocksize : blocksize;
                    if (PlayerVelocity.Y > 0)
                        YOffsetPlayer2 = -blocksize;
                    else if (PlayerVelocity.Y < 0)
                        YOffsetPlayer2 = blocksize;
                }
                else
                {
                    //YOffsetPlayer1 = PlayerVelocity.Y > 0 ? -blocksize : blocksize;
                    if (Player2Velocity.Y > 0)
                        YOffsetPlayer1 = -blocksize;
                    else if (Player2Velocity.Y < 0)
                        YOffsetPlayer1 = blocksize;
                    YOffsetPlayer2 = 0;
                }
            if (PlayerVelocity.X == 0 || Player2Velocity.X == 0)

                if (PlayerVelocity.X != 0 || Player2Velocity.X != 0)
                {
                    if (PlayerVelocity.X == 0)
                        XOffsetPlayer1 = Player2Velocity.X > 0 ? 0 : blocksize;
                    if (Player2Velocity.X == 0)
                        XOffsetPlayer2 = PlayerVelocity.X > 0 ? 0 : blocksize;
                }
                else
                {
                    XOffsetPlayer1 = 0;
                    XOffsetPlayer2 = blocksize;
                }


            if (PlayerVelocity.Y == 0 || Player2Velocity.Y == 0)
                if (PlayerVelocity.Y != 0 || Player2Velocity.Y != 0)
                {
                    if (PlayerVelocity.Y == 0)
                        YOffsetPlayer1 = Player2Velocity.Y > 0 ? 0 : blocksize;
                    if (Player2Velocity.Y == 0)
                        YOffsetPlayer2 = PlayerVelocity.Y > 0 ? 0 : blocksize;
                }
                else
                {
                    YOffsetPlayer1 = 0;
                    YOffsetPlayer2 = blocksize;
                }
            //StartPixel.X += XOffsetPlayer1;
            //StartPixel2.X += XOffsetPlayer2;
            //StartPixel.Y += YOffsetPlayer1;
            //StartPixel2.Y += YOffsetPlayer2;
            //EndingPixel.X += XOffsetPlayer1;
            //EndingPixel2.X += XOffsetPlayer2;
            //EndingPixel.Y += YOffsetPlayer1;
            //EndingPixel2.Y += YOffsetPlayer2;
            Chat.NewLine("XOffsetPlayer1: " + XOffsetPlayer1.ToString(), Time);
            Chat.NewLine("YOffsetPlayer1: " + YOffsetPlayer1.ToString(), Time);
            Chat.NewLine("XOffsetPlayer2: " + XOffsetPlayer2.ToString(), Time);
            Chat.NewLine("YOffsetPlayer2: " + YOffsetPlayer2.ToString(), Time);

            bool hit = false;
            int XModifier;
            int YModifier;
            int XModifier2;
            int YModifier2;
            //int XOffsetPlayerVelocity1;
            //int YOffsetPlayerVelocity1;
            //int XOffsetPlayerVelocity2;
            //int YOffsetPlayerVelocity2;
            (StartPixel, EndingPixel, XModifier, YModifier, XOffsetPlayerVelocity1, YOffsetPlayerVelocity1, TestVectricesX, TestVectricesY) = CreateVertices(StartPixel, EndingPixel, TestVectricesX, TestVectricesY, blocksize, true, XOffsetPlayer1, YOffsetPlayer1);
            (StartPixel2, EndingPixel2, XModifier2, YModifier2, XOffsetPlayerVelocity2, YOffsetPlayerVelocity2, Test2VectricesX, Test2VectricesY) = CreateVertices(StartPixel2, EndingPixel2, Test2VectricesX, Test2VectricesY, blocksize, true, XOffsetPlayer2, YOffsetPlayer2);
            //if (XOffsetPlayerVelocity1 == -1)
            //{
            //    XOffsetPlayerVelocity1 = XOffsetPlayerVelocity2 == -1 ? 0 : blocksize - XOffsetPlayerVelocity2;
            //    XOffsetPlayerVelocity2 = 0;
            //}
            //if (XOffsetPlayerVelocity2 == -1)
            //{
            //    XOffsetPlayerVelocity2 = XOffsetPlayerVelocity1 == -1 ? 0 : blocksize - XOffsetPlayerVelocity1;
            //    XOffsetPlayerVelocity1 = 0;
            //}
            //if (YOffsetPlayerVelocity1 == -1)
            //{
            //    YOffsetPlayerVelocity1 = YOffsetPlayerVelocity2 == -1 ? 0 : blocksize - YOffsetPlayerVelocity2;
            //    YOffsetPlayerVelocity2 = 0;
            //}
            //if (YOffsetPlayerVelocity2 == -1)
            //{
            //   YOffsetPlayerVelocity2 = YOffsetPlayerVelocity1 == -1 ? 0 : blocksize - YOffsetPlayerVelocity1;
            //    YOffsetPlayerVelocity1 = 0;
            //}

            Chat.NewLine("XOffsetPlayerVelocity1: " + XOffsetPlayerVelocity1.ToString(), Time);
            Chat.NewLine("YOffsetPlayerVelocity1: " + YOffsetPlayerVelocity1.ToString(), Time);
            Chat.NewLine("XOffsetPlayerVelocity2: " + XOffsetPlayerVelocity2.ToString(), Time);
            Chat.NewLine("YOffsetPlayerVelocity2: " + YOffsetPlayerVelocity2.ToString(), Time);
            ReadytoDraw = true;
            //XOffsetPlayer1 = +XOffsetPlayerVelocity1 - blocksize;
            //YOffsetPlayer1 = +YOffsetPlayerVelocity1 - blocksize;
            //XOffsetPlayer2 = +XOffsetPlayerVelocity2 - blocksize;
            //YOffsetPlayer2 = +YOffsetPlayerVelocity2 - blocksize;
            double BiggerXThisPlanet = StartPixel.X >= EndingPixel.X ? StartPixel.X - XModifier : EndingPixel.X;
            double BiggerYThisPlanet = StartPixel.Y >= EndingPixel.Y ? StartPixel.Y - YModifier : EndingPixel.Y;
            double SmallerXThisPlanet = StartPixel.X >= EndingPixel.X ? EndingPixel.X : StartPixel.X - XModifier;
            double SmallerYThisPlanet = StartPixel.Y >= EndingPixel.Y ? EndingPixel.Y : StartPixel.Y - YModifier;

            double BiggerXThisPlanet2 = StartPixel2.X >= EndingPixel2.X ? StartPixel2.X - XModifier2 : EndingPixel2.X;
            double BiggerYThisPlanet2 = StartPixel2.Y >= EndingPixel2.Y ? StartPixel2.Y - YModifier2 : EndingPixel2.Y;
            double SmallerXThisPlanet2 = StartPixel2.X >= EndingPixel2.X ? EndingPixel2.X : StartPixel2.X - XModifier2;
            double SmallerYThisPlanet2 = StartPixel2.Y >= EndingPixel2.Y ? EndingPixel2.Y : StartPixel2.Y - YModifier2;

            DebugintersectpointchosenY = 0;
            DebugintersectpointchosenX = 0;
            IschosenpointX = true;
            int numberofintersectpointsX = 0;
            int numberofintersectpointsY = 0;

            DebugIntersectBlocksX = new Point[1];
            DebugIntersectBlocksY = new Point[1];
            DebugIntersectBlocksX2 = new Point[1];
            DebugIntersectBlocksY2 = new Point[1];

            DebugDetectrange = new Rectangle((int)Math.Floor((SmallerXThisPlanet - XOffsetPlayer1) / blocksize), (int)Math.Floor((SmallerYThisPlanet - YOffsetPlayer1) / blocksize), (int)Math.Ceiling((BiggerXThisPlanet - XOffsetPlayer1) / blocksize) - (int)Math.Floor((SmallerXThisPlanet - XOffsetPlayer1) / blocksize), (int)Math.Ceiling((BiggerYThisPlanet - YOffsetPlayer1) / blocksize) - (int)Math.Floor((SmallerYThisPlanet - YOffsetPlayer1) / blocksize));
            DebugDetectrange2 = new Rectangle((int)Math.Floor((SmallerXThisPlanet2 - XOffsetPlayer2) / blocksize), (int)Math.Floor((SmallerYThisPlanet2 - YOffsetPlayer2) / blocksize), (int)Math.Ceiling((BiggerXThisPlanet2 - XOffsetPlayer2) / blocksize) - (int)Math.Floor((SmallerXThisPlanet2 - XOffsetPlayer2) / blocksize), (int)Math.Ceiling((BiggerYThisPlanet2 - YOffsetPlayer2) / blocksize) - (int)Math.Floor((SmallerYThisPlanet2 - YOffsetPlayer2) / blocksize));

            //DebugDetectrange2 = new Rectangle((int)Math.Floor((SmallerXThisPlanet2 - blocksize + XOffsetPlayerVelocity2) / blocksize), (int)Math.Floor((SmallerYThisPlanet2 - blocksize + YOffsetPlayerVelocity2) / blocksize), (int)Math.Ceiling((BiggerXThisPlanet2 + XOffsetPlayerVelocity2) / blocksize) - (int)Math.Floor((SmallerXThisPlanet2 - blocksize + XOffsetPlayerVelocity2) / blocksize), (int)Math.Ceiling((BiggerYThisPlanet2 + YOffsetPlayerVelocity2) / blocksize) - (int)Math.Floor((SmallerYThisPlanet2 - blocksize + YOffsetPlayerVelocity2) / blocksize));

            FinalCoords = new DoubleCoordinates(EndingPixel.X - blocksize + XOffsetPlayerVelocity1, EndingPixel.Y - blocksize + YOffsetPlayerVelocity1);
            FinalCoords2 = new DoubleCoordinates(EndingPixel2.X - blocksize + XOffsetPlayerVelocity2, EndingPixel2.Y - blocksize + YOffsetPlayerVelocity2);

            



            DebugIntersectBlocksX[numberofintersectpointsX] = new Point(0, 0);
            DebugIntersectBlocksX2[numberofintersectpointsX] = new Point(0, 0);
            DebugIntersectBlocksY[numberofintersectpointsY] = new Point(0, 0);
            DebugIntersectBlocksY2[numberofintersectpointsY] = new Point(0, 0);

            bool Xhit = false;
            bool Yhit = false;
            if (DebugDetectrange.Intersects(DebugDetectrange2))
            {
                Chat.NewLine("intersects", Time);
                //DebugIntersectBlocksY[numberofintersectpointsY] = new Point((int)PlayerLocation.X, (int)PlayerLocation.Y);
                //DebugIntersectBlocksX[numberofintersectpointsX] = new Point((int)PlayerLocation.X, (int)PlayerLocation.Y);
                //DebugIntersectBlocksY2[numberofintersectpointsY] = new Point((int)Player2Location.X, (int)Player2Location.Y);
                //DebugIntersectBlocksX2[numberofintersectpointsX] = new Point((int)Player2Location.X, (int)Player2Location.Y);
                //if (PlayerVelocity.X != 0 && PlayerVelocity.Y != 0 && Player2Velocity.X != 0 && Player2Velocity.Y != 0)
                if (!(PlayerVelocity.X == Player2Velocity.X && PlayerVelocity.Y == Player2Velocity.Y))
                {
                    double IntersectAlongY = ((StartPixel2.X * EndingPixel.X - StartPixel.X * EndingPixel2.X) / (StartPixel2.X + EndingPixel.X - StartPixel.X - EndingPixel2.X));
                    double IntersectAlongX = ((StartPixel2.Y * EndingPixel.Y - StartPixel.Y * EndingPixel2.Y) / (StartPixel2.Y + EndingPixel.Y - StartPixel.Y - EndingPixel2.Y));
                    Chat.NewLine("IntersectAlongY " + IntersectAlongY.ToString(), Time);
                    Chat.NewLine("IntersectAlongX " + IntersectAlongX.ToString(), Time);
                    double TimeAlongY = 1;
                    double TimeAlongX = 1;
                    if (PlayerVelocity.X != 0)
                        TimeAlongY = (IntersectAlongY - StartPixel.X) / (EndingPixel.X - StartPixel.X);
                    else if (Player2Velocity.X != 0)
                        TimeAlongY = (IntersectAlongY - StartPixel2.X) / (EndingPixel2.X - StartPixel2.X);
                    if (PlayerVelocity.Y != 0)
                        TimeAlongX = (IntersectAlongX - StartPixel.Y) / (EndingPixel.Y - StartPixel.Y);
                    else if (Player2Velocity.Y != 0)
                        TimeAlongX = (IntersectAlongX - StartPixel2.Y) / (EndingPixel2.Y - StartPixel2.Y);
                    //double bForIntersectAlongY = StartPixel.Y - (StartPixel.Y - EndingPixel.Y) / (StartPixel.X - EndingPixel.X) * StartPixel.X;
                    //double YposForIntersectAlongY = (StartPixel.Y - EndingPixel.Y) / (StartPixel.X - EndingPixel.X) * IntersectAlongY + bForIntersectAlongY;

                    //double bForIntersectAlongX = StartPixel.Y - (StartPixel.X - EndingPixel.X) / (StartPixel.Y - EndingPixel.Y) * StartPixel.Y;
                    //double XposForIntersectAlongX =  (IntersectAlongX - bForIntersectAlongY)/ ((StartPixel.Y - EndingPixel.Y) / (StartPixel.X - EndingPixel.X));
                    Chat.NewLine("TimeAlongY " + TimeAlongY.ToString(), Time);
                    if (TimeAlongY >= 0 && 1 > TimeAlongY)
                    {
                        double YposForIntersectAlongY = StartPixel.Y + (EndingPixel.Y - StartPixel.Y) * TimeAlongY;
                        double YposForIntersectAlongY2 = StartPixel2.Y + (EndingPixel2.Y - StartPixel2.Y) * TimeAlongY;
                        DebugIntersectBlocksY[numberofintersectpointsY] = new Point((int)IntersectAlongY, (int)YposForIntersectAlongY);
                        DebugIntersectBlocksY2[numberofintersectpointsY] = new Point((int)IntersectAlongY, (int)YposForIntersectAlongY2);
                        Chat.NewLine("TimeAlongY " + TimeAlongY.ToString(), Time);
                        if (YposForIntersectAlongY + blocksize - YOffsetPlayerVelocity2 + YOffsetPlayer2 >= YposForIntersectAlongY2 - YOffsetPlayerVelocity1 + YOffsetPlayer1 && YposForIntersectAlongY - YOffsetPlayerVelocity2 + YOffsetPlayer2 <= YposForIntersectAlongY2 + blocksize - YOffsetPlayerVelocity1 + YOffsetPlayer1)
                        {
                            Chat.NewLine("Y Good", Time);
                            Yhit = true;
                            //FinalCoords.X -= (EndingPixel.X - IntersectAlongY)*2;
                            //FinalCoords2.X -= (EndingPixel2.X - IntersectAlongY)*2;
                        }
                    }
                    Chat.NewLine("TimeAlongX " + TimeAlongX.ToString(), Time);
                    if (TimeAlongX >= 0 && 1 > TimeAlongX)
                    {
                        double XposForIntersectAlongX = StartPixel.X + (EndingPixel.X - StartPixel.X) * TimeAlongX;
                        double XposForIntersectAlongX2 = StartPixel2.X + (EndingPixel2.X - StartPixel2.X) * TimeAlongX;
                        DebugIntersectBlocksX[numberofintersectpointsX] = new Point((int)XposForIntersectAlongX, (int)IntersectAlongX);
                        DebugIntersectBlocksX2[numberofintersectpointsX] = new Point((int)XposForIntersectAlongX2, (int)IntersectAlongX);
                        Chat.NewLine("TimeAlongX " + TimeAlongX.ToString(), Time);
                        if (XposForIntersectAlongX + blocksize - XOffsetPlayerVelocity2 + XOffsetPlayer2 >= XposForIntersectAlongX2 - XOffsetPlayerVelocity1 + XOffsetPlayer1 && XposForIntersectAlongX - XOffsetPlayerVelocity2 + XOffsetPlayer2 <= XposForIntersectAlongX2 + blocksize - XOffsetPlayerVelocity1 + XOffsetPlayer1)
                        {
                            Chat.NewLine("X Good", Time);
                            Xhit = true;
                            //FinalCoords.Y += EndingPixel.X - IntersectAlongX;
                            //FinalCoords2.Y += EndingPixel2.X - IntersectAlongX;
                        }
                    }

                    if (Xhit == true && Yhit != true || (TimeAlongX < TimeAlongY || TimeAlongY == TimeAlongX) && Yhit == true && Xhit == true)
                    {

                        double totalspeed1 = EndingPixel.Y - StartPixel.Y;//also total distance
                        double totalspeed2 = EndingPixel2.Y - StartPixel2.Y;//also total distance

                        double Speed1 = (mass1 - mass2) / (mass1 + mass2) * totalspeed1 + (mass2 * 2) / (mass1 + mass2) * totalspeed2;// this will also be the final speed
                        double Speed2 = (mass2 - mass1) / (mass1 + mass2) * totalspeed2 + (mass1 * 2) / (mass1 + mass2) * totalspeed1;
                        FinalCoords.Y += -(EndingPixel.Y - IntersectAlongX) + (Speed1) * (1 - TimeAlongX);
                        FinalCoords2.Y += -(EndingPixel2.Y - IntersectAlongX) + (Speed2) * (1 - TimeAlongX);
                    }
                    if (Yhit == true && Xhit != true || (TimeAlongY < TimeAlongX || TimeAlongY == TimeAlongX) && Xhit == true && Yhit == true )
                    {
                        //FinalCoords.X -= (EndingPixel.X - IntersectAlongY) * 2;
                        //FinalCoords2.X -= (EndingPixel2.X - IntersectAlongY) * 2;

                        double totalspeed1 = EndingPixel.X - StartPixel.X;//also total distance
                        double totalspeed2 = EndingPixel2.X - StartPixel2.X;//also total distance

                        double Speed1 = (mass1 - mass2) / (mass1 + mass2) * totalspeed1 + (mass2 * 2) / (mass1 + mass2) * totalspeed2;// this will also be the final speed
                        double Speed2 = (mass2 - mass1) / (mass1 + mass2) * totalspeed2 + (mass1 * 2) / (mass1 + mass2) * totalspeed1;
                        FinalCoords.X += -(EndingPixel.X - IntersectAlongY) + (Speed1) * (1 - TimeAlongY);
                        FinalCoords2.X += -(EndingPixel2.X - IntersectAlongY) + (Speed2) * (1 - TimeAlongY);
                    }
                    //else if (TimeAlongY == TimeAlongX && Xhit == true && Yhit == true)


                }



            }






        }

        (DoubleCoordinates, DoubleCoordinates, DoubleCoordinates) TriplePlayerDebug(DoubleCoordinates StartPixel, DoubleCoordinates EndingPixel, DoubleCoordinates StartPixel2, DoubleCoordinates EndingPixel2, DoubleCoordinates StartPixel3, DoubleCoordinates EndingPixel3)
        {
            int Numberofplanets = 3;
            Planet[] planets = new Planet[3];
            planets[0].position = StartPixel;
            planets[0].velocity = Collide.AddDoubleCoordinates(EndingPixel, StartPixel, true);
            planets[0].weight = 1;
            //planets[0].oldvelocity = 0;
            planets[1].position = StartPixel2;
            planets[1].velocity = Collide.AddDoubleCoordinates(EndingPixel2, StartPixel2, true);
            planets[1].weight = 1;
            planets[2].position = StartPixel3;
            planets[2].velocity = Collide.AddDoubleCoordinates(EndingPixel3, StartPixel3, true);
            planets[2].weight = 1;

            int XModifier;
            int YModifier;
            int XModifier2;
            int YModifier2;
            (StartPixel, EndingPixel, XModifier, YModifier, XOffsetPlayerVelocity1, YOffsetPlayerVelocity1, TestVectricesX, TestVectricesY) = CreateVertices(StartPixel, EndingPixel, TestVectricesX, TestVectricesY, blocksize);
            (StartPixel2, EndingPixel2, XModifier2, YModifier2, XOffsetPlayerVelocity2, YOffsetPlayerVelocity2, Test2VectricesX, Test2VectricesY) = CreateVertices(StartPixel2, EndingPixel2, Test2VectricesX, Test2VectricesY, blocksize);
            (StartPixel3, EndingPixel3, XModifier2, YModifier2, XOffsetPlayerVelocity2, YOffsetPlayerVelocity2, Test3VectricesX, Test3VectricesY) = CreateVertices(StartPixel3, EndingPixel3, Test3VectricesX, Test3VectricesY, blocksize);

            ReadytoDraw = true;
            PlanetCollisions[] PlanetCollisions = new PlanetCollisions[Numberofplanets];
            BlockCollisions[] BlockCollisions = new BlockCollisions[Numberofplanets];
            int fastestCollision = 0;
            double previousFastestCollision = 0;
            double previouspreviousFastestCollision = 0;
            bool Planethit = true;
            bool firsttime = true;
            int round = 0;
            FinalCoords = planets[0].position;
            FinalCoords2 = planets[1].position;
            FinalCoords3 = planets[2].position;
            while (Planethit)
            {


                Planethit = false;
                for (int thisplanet = 0; thisplanet < Numberofplanets; thisplanet++)
                {
                    bool alreadyhit = false;
                    if (planets[thisplanet].Collided || firsttime)
                    {
                        if (firsttime)
                            PlanetCollisions[thisplanet].velocitystarttime = 0;

                        (BlockCollisions[thisplanet].hit, BlockCollisions[thisplanet].RelativeTime, BlockCollisions[thisplanet].hitPosition, BlockCollisions[thisplanet].HitX, BlockCollisions[thisplanet].HitY) = Collide.CollisionWithBlocks(new DoubleCoordinates(planets[thisplanet].position.X, planets[thisplanet].position.Y), new DoubleCoordinates(planets[thisplanet].position.X + planets[thisplanet].velocity.X * (1 - previousFastestCollision), planets[thisplanet].position.Y + planets[thisplanet].velocity.Y * (1 - previousFastestCollision)), world, blocksize, width, height);
                        BlockCollisions[thisplanet].time = BlockCollisions[thisplanet].RelativeTime * (1 - previousFastestCollision) + previousFastestCollision;
                        //BlockCollisions[thisplanet].RelativeTime = previousFastestCollision;
                        for (int otherplanet = 0; otherplanet < Numberofplanets; otherplanet++)
                        {
                            if (thisplanet != otherplanet)
                            {
                                bool hit;
                                PlanetCollisions Temporary;
                                Temporary.otherplanet = otherplanet;
                                Temporary.thisplanet = thisplanet;
                              
                                (hit, Temporary.time, Temporary.thisposition, Temporary.otherposition, Temporary.thisvelocity, Temporary.othervelocity, Temporary.changedvelocityX, Temporary.changedvelocityY) = Collide.DoubleVectorCollision(new DoubleCoordinates(planets[thisplanet].position.X, planets[thisplanet].position.Y), new DoubleCoordinates(planets[thisplanet].position.X + planets[thisplanet].velocity.X * (1 - previousFastestCollision), planets[thisplanet].position.Y + planets[thisplanet].velocity.Y * (1 - previousFastestCollision)), new DoubleCoordinates(planets[otherplanet].position.X, planets[otherplanet].position.Y), new DoubleCoordinates(planets[otherplanet].position.X + planets[otherplanet].velocity.X * (1 - previousFastestCollision), planets[otherplanet].position.Y + planets[otherplanet].velocity.Y * (1 - previousFastestCollision)), planets[thisplanet].oldvelocity, planets[otherplanet].oldvelocity, planets[thisplanet].weight, planets[otherplanet].weight, blocksize);
                                Temporary.time = Temporary.time * (1 - previousFastestCollision) + previousFastestCollision;
                                Temporary.velocitystarttime = PlanetCollisions[thisplanet].velocitystarttime;
                                //Temporary.velocitystarttime = Temporary.time;
                                if (hit)
                                {
                                    //Chat.NewLine(previousFastestCollision.ToString(), Time);
                                    //System.Windows.Forms.MessageBox.Show(test);
                                    //Chat.NewLine(alreadyhit.ToString(), Time);
                                    if (alreadyhit)
                                    {
                                        //Chat.NewLine(previousFastestCollision.ToString(), Time);
                                        //System.Windows.Forms.MessageBox.Show("test");
                                        //Paused = true;
                                        if (Temporary.time < PlanetCollisions[thisplanet].time)
                                            PlanetCollisions[thisplanet] = Temporary;
                                    }
                                    else
                                    {
                                        PlanetCollisions[thisplanet] = Temporary;
                                        alreadyhit = true;
                                    }
                                    //Chat.NewLine("planet: " + thisplanet.ToString() + " hit " + otherplanet.ToString() + " " + Temporary.thisvelocity.X.ToString() + ", " + Temporary.thisvelocity.Y.ToString() + " " + Temporary.time.ToString() + " " + Time.ToString() + " " + round.ToString(), Time);

                                    Planethit = true;

                                }
                                if (!alreadyhit)
                                    PlanetCollisions[thisplanet] = Temporary;

                            }
                        }
                    }
                    if (PlanetCollisions[thisplanet].time < PlanetCollisions[fastestCollision].time && alreadyhit)
                    {
                        fastestCollision = thisplanet;
                    }
                    if (Numberofplanets <= 1)
                        PlanetCollisions[thisplanet].thisposition = planets[thisplanet].position;
                    planets[thisplanet].Collided = false;
                }
                firsttime = false;
                if (Planethit)//from this point forward has to be fixed
                {
                    //if (PlanetCollisions[fastestCollision].time < 1)
                    //{
                    previouspreviousFastestCollision = previousFastestCollision;
                    previousFastestCollision = PlanetCollisions[fastestCollision].time;
                    //}
                    bool blockbeforehit = false;
                    for (int thiscollision = 0; thiscollision < Numberofplanets; thiscollision++)
                        if (BlockCollisions[thiscollision].hit && BlockCollisions[thiscollision].time <= PlanetCollisions[fastestCollision].time)
                        {
                            planets[thiscollision].velocity.X *= BlockCollisions[thiscollision].HitX;
                            planets[thiscollision].velocity.Y *= BlockCollisions[thiscollision].HitY;
                            planets[thiscollision].position = new DoubleCoordinates(BlockCollisions[thiscollision].hitPosition.X - planets[thiscollision].velocity.X * (1 - PlanetCollisions[fastestCollision].time), BlockCollisions[thiscollision].hitPosition.Y - planets[thiscollision].velocity.Y * (1 - PlanetCollisions[fastestCollision].time));
                            planets[thiscollision].velocity.X /= (1 - previouspreviousFastestCollision);
                            planets[thiscollision].velocity.Y /= (1 - previouspreviousFastestCollision);
                            planets[thiscollision].oldvelocity = planets[thiscollision].velocity;
                            planets[thiscollision].Collided = true;
                            BlockCollisions[thiscollision].hit = false;
                            planets[thiscollision].finalhitblock = true;
                            if (thiscollision == fastestCollision || thiscollision == PlanetCollisions[fastestCollision].otherplanet)
                                blockbeforehit = true;
                        }
                    if (!blockbeforehit)
                    {
                        planets[fastestCollision].position = new DoubleCoordinates(PlanetCollisions[fastestCollision].thisposition.X - PlanetCollisions[fastestCollision].thisvelocity.X, PlanetCollisions[fastestCollision].thisposition.Y - PlanetCollisions[fastestCollision].thisvelocity.Y);
                        planets[PlanetCollisions[fastestCollision].otherplanet].position = new DoubleCoordinates(PlanetCollisions[fastestCollision].otherposition.X - PlanetCollisions[fastestCollision].othervelocity.X, PlanetCollisions[fastestCollision].otherposition.Y - PlanetCollisions[fastestCollision].othervelocity.Y);

                        planets[fastestCollision].velocity = new DoubleCoordinates(PlanetCollisions[fastestCollision].thisvelocity.X / (1 - previouspreviousFastestCollision), PlanetCollisions[fastestCollision].thisvelocity.Y / (1 - previouspreviousFastestCollision));
                        planets[PlanetCollisions[fastestCollision].otherplanet].velocity = new DoubleCoordinates(PlanetCollisions[fastestCollision].othervelocity.X / (1 - previouspreviousFastestCollision), PlanetCollisions[fastestCollision].othervelocity.Y / (1 - previouspreviousFastestCollision));
                        
                        planets[fastestCollision].position = new DoubleCoordinates(planets[fastestCollision].position.X + planets[fastestCollision].velocity.X * (PlanetCollisions[fastestCollision].time - PlanetCollisions[fastestCollision].velocitystarttime), planets[fastestCollision].position.Y + planets[fastestCollision].velocity.Y * (PlanetCollisions[fastestCollision].time - PlanetCollisions[fastestCollision].velocitystarttime));
                        planets[PlanetCollisions[fastestCollision].otherplanet].position = new DoubleCoordinates(planets[PlanetCollisions[fastestCollision].otherplanet].position.X + planets[PlanetCollisions[fastestCollision].otherplanet].velocity.X * (PlanetCollisions[fastestCollision].time - PlanetCollisions[PlanetCollisions[fastestCollision].otherplanet].velocitystarttime), planets[PlanetCollisions[fastestCollision].otherplanet].position.Y + planets[PlanetCollisions[fastestCollision].otherplanet].velocity.Y * (PlanetCollisions[fastestCollision].time - PlanetCollisions[PlanetCollisions[fastestCollision].otherplanet].velocitystarttime));


                        planets[fastestCollision].oldvelocity = new DoubleCoordinates(planets[fastestCollision].velocity.X * PlanetCollisions[fastestCollision].changedvelocityX, planets[fastestCollision].velocity.Y * PlanetCollisions[fastestCollision].changedvelocityY);
                        planets[PlanetCollisions[fastestCollision].otherplanet].oldvelocity = new DoubleCoordinates(planets[PlanetCollisions[fastestCollision].otherplanet].velocity.X * PlanetCollisions[PlanetCollisions[fastestCollision].otherplanet].changedvelocityX, planets[PlanetCollisions[fastestCollision].otherplanet].velocity.Y * PlanetCollisions[PlanetCollisions[fastestCollision].otherplanet].changedvelocityY);
                        
                        planets[PlanetCollisions[fastestCollision].otherplanet].Collided = true;
                        planets[fastestCollision].Collided = true;
                        planets[fastestCollision].finalhitblock = false;
                        planets[PlanetCollisions[fastestCollision].otherplanet].finalhitblock = false;
                        Chat.NewLine("planet " + fastestCollision.ToString() + " and " + PlanetCollisions[fastestCollision].otherplanet.ToString() + " Resolved " + Time.ToString(), Time);
                        FinalCoords = planets[PlanetCollisions[fastestCollision].otherplanet].position;
                        FinalCoords2 = planets[fastestCollision].position;
                        PlanetCollisions[PlanetCollisions[fastestCollision].otherplanet].velocitystarttime = previousFastestCollision;
                        PlanetCollisions[fastestCollision].velocitystarttime = previousFastestCollision;
                        (StartPixel, EndingPixel, XModifier, YModifier, XOffsetPlayerVelocity1, YOffsetPlayerVelocity1, SecondTestVectricesX, SecondTestVectricesY) = CreateVertices(planets[fastestCollision].position, new DoubleCoordinates(planets[fastestCollision].position.X + planets[fastestCollision].velocity.X *(1 - PlanetCollisions[fastestCollision].time), planets[fastestCollision].position.Y + planets[fastestCollision].velocity.Y *(1 - PlanetCollisions[fastestCollision].time)), SecondTestVectricesX, SecondTestVectricesY, blocksize);
                        (StartPixel, EndingPixel, XModifier, YModifier, XOffsetPlayerVelocity1, YOffsetPlayerVelocity1, SecondTest2VectricesX, SecondTest2VectricesY) = CreateVertices(planets[PlanetCollisions[fastestCollision].otherplanet].position, new DoubleCoordinates(planets[PlanetCollisions[fastestCollision].otherplanet].position.X + planets[PlanetCollisions[fastestCollision].otherplanet].velocity.X * (1 - PlanetCollisions[fastestCollision].time), planets[PlanetCollisions[fastestCollision].otherplanet].position.Y + planets[PlanetCollisions[fastestCollision].otherplanet].velocity.Y * (1 - PlanetCollisions[fastestCollision].time)), SecondTest2VectricesX, SecondTest2VectricesY, blocksize);
                        


                        //(StartPixel2, EndingPixel2, XModifier2, YModifier2, XOffsetPlayerVelocity2, YOffsetPlayerVelocity2, SecondTest2VectricesX, SecondTest2VectricesY) = CreateVertices(planets[1].position, Collide.AddDoubleCoordinates(planets[1].position, planets[1].velocity), SecondTest2VectricesX, SecondTest2VectricesY, blocksize);
                        //(StartPixel3, EndingPixel3, XModifier2, YModifier2, XOffsetPlayerVelocity2, YOffsetPlayerVelocity2, SecondTest3VectricesX, SecondTest3VectricesY) = CreateVertices(planets[2].position, Collide.AddDoubleCoordinates(planets[2].position, planets[2].velocity), SecondTest3VectricesX, SecondTest3VectricesY, blocksize);
                        PlanetCollisions[fastestCollision].time = 1;
                        PlanetCollisions[PlanetCollisions[fastestCollision].otherplanet].time = 1; // change
                    }


                    for (int thisplanet = 0; thisplanet < Numberofplanets; thisplanet++)
                    {
                        if (planets[thisplanet].Collided == false)
                        {
                            if (sharedspeedtest)
                            {
                                sharedspeedtest = !sharedspeedtest;
                            }
                                
                            planets[thisplanet].position.X += planets[thisplanet].velocity.X * (previousFastestCollision - PlanetCollisions[thisplanet].velocitystarttime);
                            planets[thisplanet].position.Y += planets[thisplanet].velocity.Y * (previousFastestCollision - PlanetCollisions[thisplanet].velocitystarttime);
                            PlanetCollisions[thisplanet].velocitystarttime = previousFastestCollision;
                            FinalCoords3 = planets[thisplanet].position;
                            (StartPixel, EndingPixel, XModifier, YModifier, XOffsetPlayerVelocity1, YOffsetPlayerVelocity1, SecondTest3VectricesX, SecondTest3VectricesY) = CreateVertices(planets[thisplanet].position, new DoubleCoordinates(planets[thisplanet].position.X + planets[thisplanet].velocity.X * (1 - previousFastestCollision), planets[thisplanet].position.Y + planets[thisplanet].velocity.Y * (1 - previousFastestCollision)), SecondTest3VectricesX, SecondTest3VectricesY, blocksize);
                        }
                    }


                }
                else
                {
                    for (int thiscollision = 0; thiscollision < Numberofplanets; thiscollision++)
                        if (BlockCollisions[thiscollision].hit)
                        {
                            planets[thiscollision].velocity.X *= BlockCollisions[thiscollision].HitX;
                            planets[thiscollision].velocity.Y *= BlockCollisions[thiscollision].HitY;
                            planets[thiscollision].oldvelocity = planets[thiscollision].velocity;
                            planets[thiscollision].position = BlockCollisions[thiscollision].hitPosition;
                            //BlockCollisions[thiscollision].hit = false;
                        }
                    for (int thisplanet = 0; thisplanet < Numberofplanets; thisplanet++)
                    {
                        if (!BlockCollisions[thisplanet].hit)
                        {
                            if (planets[fastestCollision].finalhitblock)
                            {
                                planets[thisplanet].position = BlockCollisions[thisplanet].hitPosition;

                            }
                            else
                            {
                                //planets[thisplanet].position = PlanetCollisions[thisplanet].thisposition;
                                planets[thisplanet].position = new DoubleCoordinates(planets[thisplanet].position.X + planets[thisplanet].velocity.X * (1 - PlanetCollisions[thisplanet].velocitystarttime), planets[thisplanet].position.Y + planets[thisplanet].velocity.Y * (1 - PlanetCollisions[thisplanet].velocitystarttime));
                                Chat.NewLine("planet " + thisplanet.ToString() + " Resolved " + Time.ToString(), Time);
                            }
                        }
                    }
                }
                round += 1;
            }
            return (planets[0].position, planets[1].position, planets[2].position);
        }


        (DoubleCoordinates, DoubleCoordinates, int, int, int, int, DoubleCoordinates[], DoubleCoordinates[]) CreateVertices(DoubleCoordinates StartPixel, DoubleCoordinates EndingPixel, DoubleCoordinates[] TestVectricesX, DoubleCoordinates[] TestVectricesY, int blocksize, bool idotDirection = false, int IdiotXOffset = 0, int IdiotYOffset = 0)
        {
            int XModifier = -blocksize;
            int YModifier = -blocksize;
            int XOffset = blocksize;
            int YOffset = blocksize;
            //if (StartDrawing)
            //EndingPixel = new DoubleCoordinates((MousePosition.X + Coords.X - middleposition.X), (MousePosition.Y + Coords.Y - middleposition.Y));
            if (StartPixel.X < EndingPixel.X)
            {
                XModifier = +blocksize;
                EndingPixel.X += blocksize;
                StartPixel.X += blocksize;
                XOffset = 0;
                //Chat.NewLine("Right", Time);
            }
            else if (StartPixel.X == EndingPixel.X)
            {
                //XModifier = 0;
                //EndingPixel.X += blocksize;
                //StartPixel.X += blocksize;
                //XOffset = -1;
               // XOffset = -blocksize;
            }
            //else
            //    Chat.NewLine("Left", Time);
            if (StartPixel.Y < EndingPixel.Y)
            {
                YModifier = +blocksize;
                EndingPixel.Y += blocksize;
                StartPixel.Y += blocksize;
                YOffset = 0;
                //Chat.NewLine("Down", Time);
            }
            else if (StartPixel.Y == EndingPixel.Y)
            {
                //YModifier = 0;
                //EndingPixel.Y += blocksize;
                //StartPixel.Y += blocksize;
                //YOffset = -blocksize;

            }
            //StartPixel.Y += StartYModifier;
            //StartPixel.X += StartXModifier;
            //else
            //    Chat.NewLine("Up", Time);


            TestVectricesX = new DoubleCoordinates[4];
            TestVectricesX[0] = new DoubleCoordinates(StartPixel.X + IdiotXOffset, StartPixel.Y + IdiotYOffset);
            TestVectricesX[1] = new DoubleCoordinates(EndingPixel.X + IdiotXOffset, EndingPixel.Y + IdiotYOffset);
            //TestVectricesX[0] = new DoubleCoordinates(StartPixel.X, StartPixel.Y);
            //TestVectricesX[1] = new DoubleCoordinates(EndingPixel.X, EndingPixel.Y);
            TestVectricesX[2] = new DoubleCoordinates(EndingPixel.X - XModifier - IdiotXOffset, EndingPixel.Y + IdiotYOffset);
            TestVectricesX[3] = new DoubleCoordinates(StartPixel.X - XModifier - IdiotXOffset, StartPixel.Y + IdiotYOffset);
            TestVectricesY = new DoubleCoordinates[4];
            TestVectricesY[0] = new DoubleCoordinates(StartPixel.X + IdiotXOffset, StartPixel.Y + IdiotYOffset);
            TestVectricesY[1] = new DoubleCoordinates(EndingPixel.X + IdiotXOffset, EndingPixel.Y + IdiotYOffset);
            //TestVectricesY[0] = new DoubleCoordinates(StartPixel.X, StartPixel.Y);
            //TestVectricesY[1] = new DoubleCoordinates(EndingPixel.X, EndingPixel.Y);
            TestVectricesY[2] = new DoubleCoordinates(EndingPixel.X + IdiotXOffset, EndingPixel.Y - YModifier - IdiotYOffset);
            TestVectricesY[3] = new DoubleCoordinates(StartPixel.X + IdiotXOffset, StartPixel.Y - YModifier - IdiotYOffset);
            StartPixel.X += IdiotXOffset;

            StartPixel.Y += IdiotYOffset;

            EndingPixel.X += IdiotXOffset;

            EndingPixel.Y += IdiotYOffset;


            return (StartPixel, EndingPixel, XModifier, YModifier, XOffset, YOffset, TestVectricesX, TestVectricesY);
        }

        public struct Planet
        {
            public DoubleCoordinates position;
            public int blocktype;
            public int weight;
            public DoubleCoordinates velocity;
            public DoubleCoordinates oldvelocity;
            public DoubleCoordinates Acceleration;
            public int overlap;
            public Point intersectBlock;
            public DoubleCoordinates intersectPosition;
            public Rectangle Detectrange;
            public DoubleCoordinates[] vertices;
            public bool Hit;
            public bool merge;
            public bool Collided;
            public bool finalhitblock;
            //public int overlapL;
            //public int overlapD;
            //public int overlapU;

        }
        void Moveplanets()
        {
            //DoubleCoordinates Oldposition;
            int merge = 0;

            for (int thisplanet = 0; thisplanet < Numberofplanets; thisplanet++)
            {
                if (planets[thisplanet].merge)
                {
                    merge += 1;
                }

                else
                {

                    for (int otherplanet = thisplanet + 1; otherplanet < Numberofplanets; otherplanet++)
                    {
                        if (planets[thisplanet].position.X == planets[otherplanet].position.X && planets[thisplanet].position.Y == planets[otherplanet].position.Y)
                        {
                            planets[thisplanet].velocity.X = (planets[thisplanet].velocity.X * planets[thisplanet].weight + planets[otherplanet].velocity.X * planets[otherplanet].weight) / (planets[thisplanet].weight + planets[otherplanet].weight);
                            planets[thisplanet].velocity.Y = (planets[thisplanet].velocity.Y * planets[thisplanet].weight + planets[otherplanet].velocity.Y * planets[otherplanet].weight) / (planets[thisplanet].weight + planets[otherplanet].weight);
                            planets[thisplanet].weight += planets[otherplanet].weight;
                            //if (planets[thisplanet].merge == 0) 
                            planets[otherplanet].merge = true;
                        }
                        else
                        {
                            planets[thisplanet].oldvelocity = planets[thisplanet].velocity;
                            planets[thisplanet].velocity.X += ((planets[otherplanet].position.X - planets[thisplanet].position.X) * (G * planets[otherplanet].weight)) / ((planets[otherplanet].position.X - planets[thisplanet].position.X) * (planets[otherplanet].position.X - planets[thisplanet].position.X) + (planets[otherplanet].position.Y - planets[thisplanet].position.Y) * (planets[otherplanet].position.Y - planets[thisplanet].position.Y));// * planets[thisplanet].weight / Math.Abs(planets[thisplanet].weight);
                            planets[thisplanet].velocity.Y += ((planets[otherplanet].position.Y - planets[thisplanet].position.Y) * (G * planets[otherplanet].weight)) / ((planets[otherplanet].position.X - planets[thisplanet].position.X) * (planets[otherplanet].position.X - planets[thisplanet].position.X) + (planets[otherplanet].position.Y - planets[thisplanet].position.Y) * (planets[otherplanet].position.Y - planets[thisplanet].position.Y));// * planets[thisplanet].weight / Math.Abs(planets[thisplanet].weight);

                            planets[otherplanet].oldvelocity = planets[otherplanet].velocity;
                            planets[otherplanet].velocity.X += ((planets[thisplanet].position.X - planets[otherplanet].position.X) * (G * planets[thisplanet].weight)) / ((planets[thisplanet].position.X - planets[otherplanet].position.X) * (planets[thisplanet].position.X - planets[otherplanet].position.X) + (planets[thisplanet].position.Y - planets[otherplanet].position.Y) * (planets[thisplanet].position.Y - planets[otherplanet].position.Y));// * planets[otherplanet].weight / Math.Abs(planets[otherplanet].weight);
                            planets[otherplanet].velocity.Y += ((planets[thisplanet].position.Y - planets[otherplanet].position.Y) * (G * planets[thisplanet].weight)) / ((planets[thisplanet].position.X - planets[otherplanet].position.X) * (planets[thisplanet].position.X - planets[otherplanet].position.X) + (planets[thisplanet].position.Y - planets[otherplanet].position.Y) * (planets[thisplanet].position.Y - planets[otherplanet].position.Y));// * planets[otherplanet].weight / Math.Abs(planets[otherplanet].weight);


                        }
                    }
                    planets[thisplanet - merge] = planets[thisplanet];
                }

            }
            Numberofplanets -= merge;
            System.Array.Resize(ref planets, Numberofplanets);

            PlanetCollisions[] PlanetCollisions = new PlanetCollisions[Numberofplanets];
            BlockCollisions[] BlockCollisions = new BlockCollisions[Numberofplanets];
            int fastestCollision = 0;
            double previousFastestCollision = 0;
            double previouspreviousFastestCollision = 0;
            bool Planethit = true;
            bool firsttime = true;
            int round = 0;
            while (Planethit)
            {


                Planethit = false;
                for (int thisplanet = 0; thisplanet < Numberofplanets; thisplanet++)
                {
                    bool alreadyhit = false;
                    if (planets[thisplanet].Collided || firsttime)
                    {
                        if (firsttime)
                            PlanetCollisions[thisplanet].velocitystarttime = 0;

                        (BlockCollisions[thisplanet].hit, BlockCollisions[thisplanet].RelativeTime, BlockCollisions[thisplanet].hitPosition, BlockCollisions[thisplanet].HitX, BlockCollisions[thisplanet].HitY) = Collide.CollisionWithBlocks(new DoubleCoordinates(planets[thisplanet].position.X, planets[thisplanet].position.Y), new DoubleCoordinates(planets[thisplanet].position.X + planets[thisplanet].velocity.X * (1 - previousFastestCollision), planets[thisplanet].position.Y + planets[thisplanet].velocity.Y * (1 - previousFastestCollision)), world, 1, width, height);
                        BlockCollisions[thisplanet].time = BlockCollisions[thisplanet].RelativeTime * (1 - previousFastestCollision) + previousFastestCollision;
                        //BlockCollisions[thisplanet].RelativeTime = previousFastestCollision;
                        for (int otherplanet = 0; otherplanet < Numberofplanets; otherplanet++)
                        {
                            if (thisplanet != otherplanet)
                            {
                                bool hit;
                                PlanetCollisions Temporary;
                                Temporary.otherplanet = otherplanet;
                                Temporary.thisplanet = thisplanet;
                                //Temporary.velocitystarttime = 0;
                                (hit, Temporary.time, Temporary.thisposition, Temporary.otherposition, Temporary.thisvelocity, Temporary.othervelocity, Temporary.changedvelocityX, Temporary.changedvelocityY) = Collide.DoubleVectorCollision(new DoubleCoordinates(planets[thisplanet].position.X, planets[thisplanet].position.Y), new DoubleCoordinates(planets[thisplanet].position.X + planets[thisplanet].velocity.X * (1 - previousFastestCollision), planets[thisplanet].position.Y + planets[thisplanet].velocity.Y * (1 - previousFastestCollision)), new DoubleCoordinates(planets[otherplanet].position.X, planets[otherplanet].position.Y), new DoubleCoordinates(planets[otherplanet].position.X + planets[otherplanet].velocity.X * (1 - previousFastestCollision), planets[otherplanet].position.Y + planets[otherplanet].velocity.Y * (1 - previousFastestCollision)), Collide.MultiplyDoubleCoordinates(planets[thisplanet].oldvelocity, (1 - previousFastestCollision)), Collide.MultiplyDoubleCoordinates(planets[otherplanet].oldvelocity, (1 - previousFastestCollision)), planets[thisplanet].weight, planets[otherplanet].weight, 1);
                                Temporary.time = Temporary.time * (1 - previousFastestCollision) + previousFastestCollision;
                                Temporary.velocitystarttime = PlanetCollisions[thisplanet].velocitystarttime;
                                if (hit)
                                {
                                    if (alreadyhit)
                                    {
                                        if (Temporary.time < PlanetCollisions[thisplanet].time)
                                            PlanetCollisions[thisplanet] = Temporary;
                                    }
                                    else
                                    {
                                        PlanetCollisions[thisplanet] = Temporary;
                                        alreadyhit = true;
                                    }
                                    Chat.NewLine("planet: " + thisplanet.ToString() + " hit " + otherplanet.ToString() + " " + Temporary.thisvelocity.X.ToString() + ", " + Temporary.thisvelocity.Y.ToString() + " " + Temporary.time.ToString() + " " + Time.ToString() + " " + round.ToString(), Time);

                                    Planethit = true;

                                }
                                if (!alreadyhit)
                                    PlanetCollisions[thisplanet] = Temporary;

                            }
                        }
                    }


                    if (PlanetCollisions[thisplanet].time < PlanetCollisions[fastestCollision].time && alreadyhit)
                    {
                        fastestCollision = thisplanet;
                    }
                    if (Numberofplanets <= 1)
                        PlanetCollisions[thisplanet].thisposition = planets[thisplanet].position;
                    planets[thisplanet].Collided = false;

                }
                firsttime = false;
                if (Planethit)//from this point forward has to be fixed
                {
                    //if (PlanetCollisions[fastestCollision].time < 1)
                    //{
                    previouspreviousFastestCollision = previousFastestCollision;
                    previousFastestCollision = PlanetCollisions[fastestCollision].time;
                    //}
                    bool blockbeforehit = false;
                    for (int thiscollision = 0; thiscollision < Numberofplanets; thiscollision++)
                        if (BlockCollisions[thiscollision].hit && BlockCollisions[thiscollision].time <= PlanetCollisions[fastestCollision].time)
                        {
                            planets[thiscollision].velocity.X *= BlockCollisions[thiscollision].HitX;
                            planets[thiscollision].velocity.Y *= BlockCollisions[thiscollision].HitY;
                            planets[thiscollision].position = new DoubleCoordinates(BlockCollisions[thiscollision].hitPosition.X - planets[thiscollision].velocity.X * (1 - PlanetCollisions[fastestCollision].time), BlockCollisions[thiscollision].hitPosition.Y - planets[thiscollision].velocity.Y * (1 - PlanetCollisions[fastestCollision].time));
                            planets[thiscollision].velocity.X /= (1 - previouspreviousFastestCollision);
                            planets[thiscollision].velocity.Y /= (1 - previouspreviousFastestCollision);
                            planets[thiscollision].oldvelocity = planets[thiscollision].velocity;
                            planets[thiscollision].Collided = true;
                            BlockCollisions[thiscollision].hit = false;
                            planets[thiscollision].finalhitblock = true;
                            if (thiscollision == fastestCollision || thiscollision == PlanetCollisions[fastestCollision].otherplanet)
                                blockbeforehit = true;
                        }


                    if (!blockbeforehit)
                    {

                        planets[fastestCollision].position = new DoubleCoordinates(PlanetCollisions[fastestCollision].thisposition.X - PlanetCollisions[fastestCollision].thisvelocity.X, PlanetCollisions[fastestCollision].thisposition.Y - PlanetCollisions[fastestCollision].thisvelocity.Y);
                        planets[PlanetCollisions[fastestCollision].otherplanet].position = new DoubleCoordinates(PlanetCollisions[fastestCollision].otherposition.X - PlanetCollisions[fastestCollision].othervelocity.X, PlanetCollisions[fastestCollision].otherposition.Y - PlanetCollisions[fastestCollision].othervelocity.Y);
                        //planets[fastestCollision].position = PlanetCollisions[fastestCollision].thisposition;
                        //planets[PlanetCollisions[fastestCollision].otherplanet].position = PlanetCollisions[PlanetCollisions[fastestCollision].otherplanet].thisposition;


                        planets[fastestCollision].velocity = new DoubleCoordinates(PlanetCollisions[fastestCollision].thisvelocity.X / (1 - previouspreviousFastestCollision), PlanetCollisions[fastestCollision].thisvelocity.Y / (1 - previouspreviousFastestCollision));
                        planets[PlanetCollisions[fastestCollision].otherplanet].velocity = new DoubleCoordinates(PlanetCollisions[fastestCollision].othervelocity.X / (1 - previouspreviousFastestCollision), PlanetCollisions[fastestCollision].othervelocity.Y / (1 - previouspreviousFastestCollision));

                        planets[fastestCollision].position = new DoubleCoordinates(planets[fastestCollision].position.X + planets[fastestCollision].velocity.X * (PlanetCollisions[fastestCollision].time - PlanetCollisions[fastestCollision].velocitystarttime), planets[fastestCollision].position.Y + planets[fastestCollision].velocity.Y * (PlanetCollisions[fastestCollision].time - PlanetCollisions[fastestCollision].velocitystarttime));
                        planets[PlanetCollisions[fastestCollision].otherplanet].position = new DoubleCoordinates(planets[PlanetCollisions[fastestCollision].otherplanet].position.X + planets[PlanetCollisions[fastestCollision].otherplanet].velocity.X * (PlanetCollisions[fastestCollision].time - PlanetCollisions[PlanetCollisions[fastestCollision].otherplanet].velocitystarttime), planets[PlanetCollisions[fastestCollision].otherplanet].position.Y + planets[PlanetCollisions[fastestCollision].otherplanet].velocity.Y * (PlanetCollisions[fastestCollision].time - PlanetCollisions[PlanetCollisions[fastestCollision].otherplanet].velocitystarttime));


                        planets[fastestCollision].oldvelocity = planets[fastestCollision].velocity;
                        planets[PlanetCollisions[fastestCollision].otherplanet].oldvelocity = planets[PlanetCollisions[fastestCollision].otherplanet].velocity;

                        planets[PlanetCollisions[fastestCollision].otherplanet].Collided = true;
                        planets[fastestCollision].Collided = true;
                        planets[fastestCollision].finalhitblock = false;
                        planets[PlanetCollisions[fastestCollision].otherplanet].finalhitblock = false;
                        PlanetCollisions[PlanetCollisions[fastestCollision].otherplanet].velocitystarttime = previousFastestCollision;
                        PlanetCollisions[fastestCollision].velocitystarttime = previousFastestCollision;
                        PlanetCollisions[fastestCollision].time = 1;
                        PlanetCollisions[PlanetCollisions[fastestCollision].otherplanet].time = 1; // change
                        Chat.NewLine("planet " + fastestCollision.ToString() + " and " + PlanetCollisions[fastestCollision].otherplanet.ToString() + " Resolved " + Time.ToString(), Time);

                    }


                    for (int thisplanet = 0; thisplanet < Numberofplanets; thisplanet++)
                    {
                        if (planets[thisplanet].Collided == false)
                        {
                            planets[thisplanet].position.X += planets[thisplanet].velocity.X * (previousFastestCollision - PlanetCollisions[thisplanet].velocitystarttime);
                            planets[thisplanet].position.Y += planets[thisplanet].velocity.Y * (previousFastestCollision - PlanetCollisions[thisplanet].velocitystarttime);
                            PlanetCollisions[thisplanet].velocitystarttime = previousFastestCollision;
                        }
                    }
                    for (int thisplanet = 0; thisplanet < Numberofplanets; thisplanet++)
                    {
                        planets[thisplanet].position.X = Math.Round(planets[thisplanet].position.X, 15);
                        planets[thisplanet].position.Y = Math.Round(planets[thisplanet].position.Y, 15);
                        Chat.NewLine("planet " + thisplanet.ToString() + " posX " + planets[thisplanet].position.X + " posY " + planets[thisplanet].position.Y + Time.ToString(), Time);
                    }



                    for (int thisplanet = 0; thisplanet < Numberofplanets; thisplanet++)//test
                    {
                        for (int otherplanet = thisplanet + 1; otherplanet < Numberofplanets; otherplanet++)
                        {
                            //Rectangle OverlapDetectrange = new Rectangle(planets[thisplanet].position.X, planets[thisplanet].position.Y, blocksize, blocksize);
                            if (Collide.rectRect(planets[thisplanet].position.X, planets[thisplanet].position.Y, 1, 1, planets[otherplanet].position.X, planets[otherplanet].position.Y, 1, 1))
                            {
                                Chat.NewLine("During planet " + thisplanet.ToString() + " and " + otherplanet.ToString() + " Overlap " + Time.ToString(), Time);
                                Chat.NewLine("planet " + thisplanet.ToString() + " posX " + planets[thisplanet].position.X + " posY " + planets[thisplanet].position.Y + Time.ToString(), Time);
                                Chat.NewLine("planet " + otherplanet.ToString() + " posX " + planets[otherplanet].position.X + " posY " + planets[otherplanet].position.Y + Time.ToString(), Time);
                                //PauseGame();
                            }
                        }
                    }//end test


                }

                else
                {
                    for (int thiscollision = 0; thiscollision < Numberofplanets; thiscollision++)
                        if (BlockCollisions[thiscollision].hit)
                        {
                            planets[thiscollision].velocity.X *= BlockCollisions[thiscollision].HitX;
                            planets[thiscollision].velocity.Y *= BlockCollisions[thiscollision].HitY;
                            planets[thiscollision].oldvelocity = planets[thiscollision].velocity;
                            planets[thiscollision].position = BlockCollisions[thiscollision].hitPosition;
                            //BlockCollisions[thiscollision].hit = false;
                        }
                    for (int thisplanet = 0; thisplanet < Numberofplanets; thisplanet++)
                    {
                        if (!BlockCollisions[thisplanet].hit)
                        {
                            if (planets[fastestCollision].finalhitblock)
                            {
                                planets[thisplanet].position = BlockCollisions[thisplanet].hitPosition;

                            }
                            else
                            {
                                planets[thisplanet].position = new DoubleCoordinates(planets[thisplanet].position.X + planets[thisplanet].velocity.X * (1 - PlanetCollisions[thisplanet].velocitystarttime), planets[thisplanet].position.Y + planets[thisplanet].velocity.Y * (1 - PlanetCollisions[thisplanet].velocitystarttime));
                                Chat.NewLine("planet " + thisplanet.ToString() + " Resolved " + Time.ToString(), Time);

                            }
                        }
                    }
                }
                round += 1;
            }
            for (int thisplanet = 0; thisplanet < Numberofplanets; thisplanet++)
            {
                for (int otherplanet = thisplanet + 1; otherplanet < Numberofplanets; otherplanet++)
                {
                    //Rectangle OverlapDetectrange = new Rectangle(planets[thisplanet].position.X, planets[thisplanet].position.Y, blocksize, blocksize);
                    if (Collide.rectRect(planets[thisplanet].position.X, planets[thisplanet].position.Y, 1, 1, planets[otherplanet].position.X, planets[otherplanet].position.Y, 1, 1))
                    {
                        Chat.NewLine("planet " + thisplanet.ToString() + " and " + otherplanet.ToString() + " Overlap " + Time.ToString(), Time);
                        Chat.NewLine("planet " + thisplanet.ToString() + " posX " + planets[thisplanet].position.X + " posY " + planets[thisplanet].position.Y + Time.ToString(), Time);
                        Chat.NewLine("planet " + otherplanet.ToString() + " posX " + planets[otherplanet].position.X + " posY " + planets[otherplanet].position.Y + Time.ToString(), Time);
                        PauseGame();
                    }
                }
            }
            
        }

        public struct PlanetCollisions
        {
            public double time;
            public double velocitystarttime;
            public int thisplanet;
            public int otherplanet;
            public int changedvelocityX;
            public int changedvelocityY;
            public DoubleCoordinates thisvelocity;
            public DoubleCoordinates othervelocity;
            public DoubleCoordinates thisposition;
            public DoubleCoordinates otherposition;
        }
        public struct BlockCollisions
        {
            public bool hit;
            public double time;
            public double RelativeTime;
            public int HitX;
            public int HitY;
            public DoubleCoordinates hitPosition;
        }

        void NewPlanet(int x, int y, int blocktype, int weight = 0)
        {
            System.Array.Resize(ref planets, Numberofplanets + 1);
            planets[Numberofplanets].position.X = (x);
            planets[Numberofplanets].position.Y = (y);
            planets[Numberofplanets].blocktype = blocktype;
            //if (blocktype == 2)
            //    blocktype = -10;
            planets[Numberofplanets].weight = weight == 0 ? blocktype : weight;
            Numberofplanets += 1;
        }

        public DoubleCoordinates[] GetPlanetVectors(double bigX, double bigY, double smallX, double smallY, double slope)
        {
            DoubleCoordinates[] vertices;
            vertices = new DoubleCoordinates[6];
            if (slope > 0)
            {
                vertices = new DoubleCoordinates[6];
                vertices[0] = new DoubleCoordinates(smallX, smallY);
                vertices[1] = new DoubleCoordinates(smallX + blocksize, smallY);
                vertices[2] = new DoubleCoordinates(bigX + blocksize, bigY);
                vertices[3] = new DoubleCoordinates(bigX + blocksize, bigY + blocksize);
                vertices[4] = new DoubleCoordinates(bigX, bigY + blocksize);
                vertices[5] = new DoubleCoordinates(smallX, smallY + blocksize);
            }
            else if (slope < 0)
            {
                vertices = new DoubleCoordinates[6];
                vertices[0] = new DoubleCoordinates(smallX, bigY + blocksize);
                vertices[1] = new DoubleCoordinates(smallX, bigY);
                vertices[2] = new DoubleCoordinates(bigX, smallY);
                vertices[3] = new DoubleCoordinates(bigX + blocksize, smallY);
                vertices[4] = new DoubleCoordinates(bigX + blocksize, smallY + blocksize);
                vertices[5] = new DoubleCoordinates(smallX + blocksize, bigY + blocksize);
            }
            else
            {
                vertices = new DoubleCoordinates[4];
                vertices[0] = new DoubleCoordinates(smallX, smallY);
                vertices[1] = new DoubleCoordinates(bigX + blocksize, smallY);
                vertices[2] = new DoubleCoordinates(bigX + blocksize, bigY + blocksize);
                vertices[3] = new DoubleCoordinates(smallX, bigY + blocksize);
            }
            return vertices;
        }

        DoubleCoordinates GetWorldGravity(DoubleCoordinates PositionOfObject, int startx = 0, int starty = 0, int endx = width, int endy = height)
        {
            DoubleCoordinates WorldGravity = new DoubleCoordinates(0,0);
            for (int x = startx; x < endx; x++)
                for (int y = starty; y < endy; y++)
                    if (world[x, y] != 0)
                    {
                        WorldGravity.X += ((x - PositionOfObject.X) * (G * world[x, y])) / ((x - PositionOfObject.X) * (x - PositionOfObject.X) + (y - PositionOfObject.Y) * (y - PositionOfObject.Y));
                        WorldGravity.Y += ((y - PositionOfObject.Y) * (G * world[x, y])) / ((x - PositionOfObject.X) * (x - PositionOfObject.X) + (y - PositionOfObject.Y) * (y - PositionOfObject.Y));
                    }
            return WorldGravity;
        }

        public struct Monster
        {
            public Texture2D sprite;
            public Vector2 position;
            public Color color;
            public SpriteEffects spriteEffects;
            public Rectangle rectangle;
            public float scale, rotation;
            public int type;
            public int level;
            public double verticalVelocity, HorizontalVelocity;

        }

        void SummonMonster()
        {
            System.Array.Resize(ref monster, numberOfMonsters + 1);
            //Monster[] monster = new Monster[maxMonsters];
            monster[numberOfMonsters].type = 0;
            monster[numberOfMonsters].sprite = MonsterSprites[monster[numberOfMonsters].type, 0];
            int SizeX = 32, SizeY = 32;
            monster[numberOfMonsters].position = NewMonsterpos(SizeX, SizeY);
            if (monster[numberOfMonsters].position != new Vector2(-1, -1))
            {
                monster[numberOfMonsters].color = Color.White;
                monster[numberOfMonsters].spriteEffects = SpriteEffects.None;
                monster[numberOfMonsters].rectangle = new Rectangle(Convert.ToInt32(monster[numberOfMonsters].position.X), Convert.ToInt32(monster[numberOfMonsters].position.Y), SizeX, SizeY);
                monster[numberOfMonsters].scale = 1;
                monster[numberOfMonsters].rotation = 0;

                numberOfMonsters += 1;
            }

        }

        void MoveMonster()
        {
            MonsterList[0] = "Fly";
            for (int i = 0; i < numberOfMonsters; i++)
            {
                //var type = typeof(Game1);
                //var method = type.GetMethod((MonsterList[monster[i].type]) + "AI");
                //method.Invoke(null, new object[] { i });
                if (monster[i].type == 0)
                    FlyAI(i);



            }
        }
        void Refreshmonsterpos(int i) 
        {
            monster[i].rectangle.Location = new Point(Convert.ToInt32(monster[i].position.X),Convert.ToInt32(monster[i].position.Y));
            //-monster[i].rectangle.Size.Y / 2 +  -monster[i].rectangle.Size.Y + 
        }
        Vector2 NewMonsterpos(int SizeX, int SizeY)
        {
            int blockx = rnd.Next(-20 + Coords.X /blocksize, 21 + Coords.X  / blocksize);
            int blocky = rnd.Next(-20 + Coords.Y / blocksize, 21 + Coords.Y / blocksize);

            for (int y = blocky; y < blocky + SizeY / blocksize; y++)
                for (int x = blockx; x < blockx + SizeX / blocksize; x++)
                {
                    if (x >= width || y >= height || x < 0 || y < 0 || world[x, y] != 0)
                        return new Vector2(-1, -1);
                }
            return new Vector2(blockx*blocksize, blocky*blocksize);

        }
        void FlyAI(int i)
        {
            if (monster[i].sprite == MonsterSprites[0, 1])
                monster[i].sprite = MonsterSprites[0, 0];
            else
                monster[i].sprite = MonsterSprites[0, 1];
            //if (monster[i].verticalVelocity == 0)
            //monster[i].verticalVelocity = 7;


            if (monster[i].verticalVelocity < -25)
                monster[i].verticalVelocity = 75;
            if (monster[i].verticalVelocity == 0)
                monster[i].verticalVelocity = 500;
            if (monster[i].verticalVelocity == 475)
                monster[i].verticalVelocity = -1;
            if (monster[i].verticalVelocity > 0)
            {
                if (monster[i].verticalVelocity < 76)
                {
                    Refreshmonsterpos(i);
                    if (Collide.CheckCollisionsDown(world, width, height, blocksize, blockrectangle, monster[i].rectangle))
                        monster[i].position.Y += 1;
                }
                monster[i].verticalVelocity -= 1;
            }

            if (monster[i].verticalVelocity < 0)
            {
                Refreshmonsterpos(i);
                if (Collide.CheckCollisionsUp(world, width, height, blocksize, blockrectangle, monster[i].rectangle))             
                    monster[i].position.Y -= 1;
                else
                    monster[i].verticalVelocity -= 1;
            }

            if (monster[i].HorizontalVelocity == 0)
                monster[i].HorizontalVelocity = 1;

            if (monster[i].HorizontalVelocity > 0)
                if (Collide.CheckCollisionsRight(world, width, height, blocksize, blockrectangle, monster[i].rectangle))
                {
                    monster[i].position.X += 1;
                    if (monster[i].spriteEffects != SpriteEffects.FlipHorizontally)
                        monster[i].spriteEffects = SpriteEffects.FlipHorizontally;
                }

                else
                    monster[i].HorizontalVelocity = -1;
            if (monster[i].HorizontalVelocity < 0)
                if (Collide.CheckCollisionsLeft(world, width, height, blocksize, blockrectangle, monster[i].rectangle))
                {
                    monster[i].position.X -= 1;
                    if (monster[i].spriteEffects == SpriteEffects.FlipHorizontally)
                        monster[i].spriteEffects = SpriteEffects.None;
                }

                else
                    monster[i].HorizontalVelocity = 1;


            Refreshmonsterpos(i);
            if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Right) && Collide.CheckCollisionsRight(world, width, height, blocksize, blockrectangle, monster[i].rectangle))
            {
                if (monster[i].spriteEffects != SpriteEffects.FlipHorizontally)
                    monster[i].spriteEffects = SpriteEffects.FlipHorizontally;
                monster[i].position.X += 1;
            }
            Refreshmonsterpos(i);
            if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Left) && Collide.CheckCollisionsLeft(world, width, height, blocksize, blockrectangle, monster[i].rectangle))
            {
                if (monster[i].spriteEffects == SpriteEffects.FlipHorizontally)
                    monster[i].spriteEffects = SpriteEffects.None;
                monster[i].position.X -= 1;
            }
            Refreshmonsterpos(i);
            if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Up) && Collide.CheckCollisionsUp(world, width, height, blocksize, blockrectangle, monster[i].rectangle))
                monster[i].position.Y -= 1;
            Refreshmonsterpos(i);
            if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Down) && Collide.CheckCollisionsDown(world, width, height, blocksize, blockrectangle, monster[i].rectangle))
                monster[i].position.Y += 1;
            Refreshmonsterpos(i);
        }

        void PauseGame()
        {
            Paused = !Paused;
            if (Paused)
                Buttons[SETTINGSBUTTON].visible = true;
            else
            {
                Buttons[SETTINGSBUTTON].visible = false;
                Buttons[RESETBUTTON].visible = false;
                Buttons[NEWWORLDBUTTON].visible = false;
            }
        }

        public struct Button
        {
            public Rectangle Rectangle;
            public Text text;
            public Boolean Selected;
            public Boolean visible;
            public Boolean TextOnly;
            public string PositionAnchor;
            public Vector2 Position;
        }

        void ButtonClick(int buttonNum)
        {
            Buttons[buttonNum].Selected = false;
            if (buttonNum == PLAYBUTTON)//PLAY BUTTON
            {
                Buttons[PLAYBUTTON].visible = false;
                Buttons[CREATENEWWORLDBUTTON].visible = true;
                Buttons[DEBUGBUTTON].visible = true;
            }
            if (buttonNum == CREATENEWWORLDBUTTON)//CREATE NEW WORLD BUTTON
            {
                Buttons[CREATENEWWORLDBUTTON].visible = false;
                Buttons[DEBUGBUTTON].visible = false;
                CurrentlyDisplaying = LOADING;
            }
            if (buttonNum == DEBUGBUTTON)//DEBUG BUTTON
            {
                Buttons[CREATENEWWORLDBUTTON].visible = false;
                Buttons[DEBUGBUTTON].visible = false;
                CurrentlyDisplaying = DEBUGLOADING;
                //TestVectrices = new DoubleCoordinates[6];
                TestVectricesX = new DoubleCoordinates[4];
                TestVectricesY = new DoubleCoordinates[4];
            }
                if (buttonNum == SETTINGSBUTTON)//SETTINGS BUTTON
            {
                Buttons[NEWWORLDBUTTON].visible = !Buttons[NEWWORLDBUTTON].visible;
                Buttons[RESETBUTTON].visible = !Buttons[RESETBUTTON].visible;
            }
            if (buttonNum == RESETBUTTON)//RESET BUTTON
            {
                CurrentlyDisplaying = LOADING;
            }
            if (buttonNum == NEWWORLDBUTTON)//NEWWORLD BUTTON
            {
                seed = "";
                CurrentlyDisplaying = LOADING;
            }
        }
        
        void NewButton(int buttonNum, string ButtonText, string PositionAnchor, int RecX, int RecY, Boolean TextOnly = true, int RecWidth = 200, int RecHeight = 50, Boolean ButtonVisible = false)
        {
            Buttons[buttonNum].Position = new Vector2(RecX, RecY);
            Buttons[buttonNum].PositionAnchor = PositionAnchor;
            Buttons[buttonNum].text.text = ButtonText;
            Buttons[buttonNum].text = DoText(Buttons[buttonNum].text);
            if (TextOnly)
            {
                Buttons[buttonNum].TextOnly = TextOnly;
                RecWidth = Buttons[buttonNum].text.size.Width;
                RecHeight = Buttons[buttonNum].text.size.Height;
            }    
            Buttons[buttonNum].Rectangle = new Rectangle(RecX, RecY, RecWidth, RecHeight);
            ResetButtonPosition(buttonNum);
            Buttons[buttonNum].visible = ButtonVisible;
        }
        void ResetButtonPosition(int buttonNum)
        {
            Vector2 PositionModifier = new Vector2(0,0);
            if (Buttons[buttonNum].PositionAnchor == "MiddleX")
                PositionModifier.X = middleposition.X;
            if (Buttons[buttonNum].PositionAnchor == "RightSide")
                PositionModifier.X = this.GraphicsDevice.Viewport.Width - Buttons[buttonNum].Rectangle.Width;
            Buttons[buttonNum].Rectangle = new Rectangle(Convert.ToInt32(PositionModifier.X + Buttons[buttonNum].Position.X), Convert.ToInt32(PositionModifier.Y + Buttons[buttonNum].Position.Y), Buttons[buttonNum].Rectangle.Width, Buttons[buttonNum].Rectangle.Height);
        }
        public Text DoText(Text textname, int startindex = 0)
        {
            (textname.chars, textname.coords, textname.color, textname.size, textname.LinePerCharecter, textname.NumberOfLines) = Getcoordinateschar(textname.text.Substring(startindex));
            return textname;
        }
        void InitializeMainMenu()
        {
            Buttons = new Button[6];
            NewButton(0, "Play", "MiddleX", -100, 200, false, 200, 50);
            NewButton(1, "Create New World", "MiddleX", -150, 100, false, 300, 50);
            NewButton(2, "Debug", "MiddleX", -150, 200, false, 300, 50);
            NewButton(3, "Settings", "RightSide", -20, 20);
            NewButton(4, "Reset", "RightSide", -20, 50);
            NewButton(5, "New World", "RightSide", -20, 70);

            Buttons[PLAYBUTTON].visible = true;
            MainMenuInitilised = true;
        }

        Texture2D CreateCircle(int diameter)
        {
            Texture2D texture = new Texture2D(GraphicsDevice, diameter, diameter);
            Color[] colorData = new Color[diameter * diameter];

            float radius = diameter / 2f;
            float radiussq = radius * radius;

            for (int x = 0; x < diameter; x++)
            {
                for (int y = 0; y < diameter; y++)
                {
                    int index = x * diameter + y;
                    Vector2 pos = new Vector2(x - radius, y - radius);
                    Vector2 pos2 = new Vector2(x - radius + 1, y - radius);
                    Vector2 pos3 = new Vector2(x - radius, y - radius + 1);
                    Vector2 pos4 = new Vector2(x - radius + 1, y - radius + 1);
                    if (pos.LengthSquared() <= radiussq || pos2.LengthSquared() <= radiussq || pos3.LengthSquared() <= radiussq || pos4.LengthSquared() <= radiussq)
                    {
                        //if (y == 2 || x== 2 || y == radius - 2 || x == radius - 2)
                        //    colorData[index] = Color.Black;
                        //else
                            colorData[index] = Color.White;
                    }
                    else
                    {
                        colorData[index] = Color.Transparent;
                    }
                }
            }
            texture.SetData(colorData);
            return texture;
        }

        Texture2D Getborders(Texture2D texture, int bordersize, Color bordercolor, bool onlyBorder)
        {
            Color actualbordercolor = bordercolor;
            if (onlyBorder) 
            {
                bordercolor = Color.Black;
            }
            Color[] colorData = new Color[texture.Width * texture.Height];
            texture.GetData(colorData);
            for (int x = 0; x < texture.Width; x++)
                for (int y = 0; y < texture.Height; y++)
                {
                    int index = x * texture.Width + y;
                    if (colorData[index] == Color.White)
                        for (int neighborX = x - bordersize; neighborX <= x + bordersize; neighborX++)
                            for (int neighborY = y - bordersize; neighborY <= y + bordersize; neighborY++)
                                if (neighborX != x || neighborY != y)
                                {
                                    int indexNeighbor = neighborX * texture.Width + neighborY;
                                    if (indexNeighbor < 0 || indexNeighbor >= texture.Width * texture.Height || neighborY < 0 || neighborY >= texture.Height)
                                        colorData[index] = bordercolor;
                                    else if (colorData[indexNeighbor] == Color.Transparent)
                                    {
                                        colorData[index] = bordercolor;
                                    }
                                }
                }
            if (onlyBorder)
                for (int x = 0; x < texture.Width; x++)
                    for (int y = 0; y < texture.Height; y++)
                    {
                        int index = x * texture.Width + y;
                        if (colorData[index] == Color.White)
                            colorData[index] = Color.Transparent;
                        else if (colorData[index] == Color.Black)
                            colorData[index] = actualbordercolor;
                    }



            texture.SetData(colorData);
            return texture;
        }

        Texture2D Resize(Texture2D texture, double ResizeamountX, double ResizeamountY = 0)
        {
            if (ResizeamountY == 0) ResizeamountY = ResizeamountX;
            Texture2D newtexture = new Texture2D(GraphicsDevice, Convert.ToInt32(texture.Width * ResizeamountX), Convert.ToInt32(texture.Height * ResizeamountY));
            Color[] newcolorData = new Color[Convert.ToInt32(texture.Width*ResizeamountX) * Convert.ToInt32(texture.Height*ResizeamountY)];

            Color[] colorData = new Color[texture.Width * texture.Height];
            texture.GetData(colorData);
            for (int x = 0; x < texture.Height; x++)
                for (int y = 0; y < texture.Width; y++)
                {
                    int index = x * texture.Width + y;
                    for (int newx = Convert.ToInt32(x * ResizeamountY); newx < Convert.ToInt32(x * ResizeamountY + ResizeamountY); newx++)
                        for (int newy = Convert.ToInt32(y * ResizeamountX); newy < Convert.ToInt32(y * ResizeamountX + ResizeamountX); newy++)
                        {
                            int newindex = Convert.ToInt32(newx * newtexture.Width + newy);
                            newcolorData[newindex] = colorData[index];
                        }
                }

            newtexture.SetData(newcolorData);
            return newtexture;
        }

        private static Texture2D rect;

        private void DrawRectangle(Rectangle coords, Color color)
        {
            if (rect == null)
            {
                rect = new Texture2D(GraphicsDevice, 1, 1);
                rect.SetData(new[] { Color.White });
            }
            _spriteBatch.Draw(rect, coords, color);
        }

        public struct ColorRGB
        {
            internal Microsoft.Xna.Framework.Color col;

            public byte R
            {
                get { return col.R; }
                set { col.R = value; }
            }
            public byte G
            {
                get { return col.G; }
                set { col.G = value; }
            }
            public byte B
            {
                get { return col.B; }
                set { col.B = value; }
            }
            public byte A
            {
                get { return col.A; }
                set { col.A = value; }
            }
        }


    }
    class KeyBoardKey
    {
        public bool Pressed;
        public bool Holdable;
        public int Continue;
        public int MaxContinueTime;
        public int MaxContinueTimeOriginal;
        public int InitialWaitTime;
        public int InitialWaitTimeOriginal;
        public int MinWaitTime;
        public Keys Key;
        public void NewKeyBoardKey(int thisMaxContinueTime = 10, int thisMinWaitTime = 5, int thisInitialWaitTime = 25)
        {
            MaxContinueTimeOriginal = thisMaxContinueTime;
            MaxContinueTime = MaxContinueTimeOriginal;
            MinWaitTime = thisMinWaitTime;
            InitialWaitTimeOriginal = thisInitialWaitTime - (MaxContinueTime + MinWaitTime);
            InitialWaitTime = InitialWaitTimeOriginal;
            Holdable = true;
        }
        public Boolean CheckKeyPress(Boolean Overide = false)
        {
            bool Canactivate = false;
            if (Keyboard.GetState().IsKeyUp(Key) && (Pressed == true))
            {
                Pressed = false;
                if (Holdable)
                {
                    Continue = 0;
                    MaxContinueTime = MaxContinueTimeOriginal;
                    InitialWaitTime = InitialWaitTimeOriginal;
                }
            }

            if ((Keyboard.GetState().IsKeyDown(Key)))
            {
                if (!Overide)
                {
                    if (Holdable)
                    {
                        Continue += 1;
                        if (Pressed == false || Continue > (MaxContinueTime + MinWaitTime + InitialWaitTime))
                        {
                            Canactivate = true;
                            Continue = 0;
                            if (MaxContinueTime > 0)
                                MaxContinueTime -= 1;
                            if (Pressed == true)
                                InitialWaitTime = 0;
                        }
                    }
                    else if (Pressed == false)
                        Canactivate = true;
                    Pressed = true;
                }
                else
                    Canactivate = true;

            }
            return Canactivate;
        }
    }
    class Inventory
    {
        public int NumberOfInvSlots = 10;
        public Slot[] Slots;
        public ItemProperties[] Items;
        public int Selected;
        public void InitializeInv()
        {
            Slots = new Slot[NumberOfInvSlots];
            for (int i = 0; i < NumberOfInvSlots; i++)
            {
                Slots[i].Rectangle = new Rectangle(i * 50 + 5, 5, 50, 50);
                Slots[i].visible = true;
                Slots[i].Item.number = 0;
                //Slots[i].Item.image = Items[0];

            }
            Selected = 0;
            Slots[Selected].Selected = true;
            for (int i = 0; i < Items.Length - 1; i++)
            {
                Slots[i].Item.number = i + 1;
                //Slots[i].Item.image = Items[Slots[i].Item.number].image;
            }

            //InvInitilised = true;
        }
        public struct Slot
        {
            public Rectangle Rectangle;
            public Itemstrc Item;
            public Boolean Selected;
            public Boolean visible;
        }
        public struct Itemstrc
        {
            public int number;
            //public ItemProperties properties;
            public int amount;
        }
        public struct ItemProperties
        {
            public Texture2D image;
            public bool autoswing;
        }

    }
    class RectangleSprite
    {
        static Texture2D _pointTexture;
        public static void DrawRectangle(SpriteBatch spriteBatch, Rectangle rectangle, Color color, int lineWidth)
        {
            if (_pointTexture == null)
            {
                _pointTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                _pointTexture.SetData<Color>(new Color[] { Color.White });
            }

            spriteBatch.Draw(_pointTexture, new Rectangle(rectangle.X, rectangle.Y, lineWidth, rectangle.Height + lineWidth), color);
            spriteBatch.Draw(_pointTexture, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width + lineWidth, lineWidth), color);
            spriteBatch.Draw(_pointTexture, new Rectangle(rectangle.X + rectangle.Width, rectangle.Y, lineWidth, rectangle.Height + lineWidth), color);
            spriteBatch.Draw(_pointTexture, new Rectangle(rectangle.X, rectangle.Y + rectangle.Height, rectangle.Width + lineWidth, lineWidth), color);
        }
    }

    class Chat
    {
        public Text data;
        public const int LineLimit = 20;
        public const int TimePerLine = 150;
        //public int StartChar;
        public int TotalLines;
        public int Lines;
        public int[] TimeDisplayed;
        public string[] Texts;
        public int LengthOfFade = 25;
        public int maxLengthOfchat = 300;
        public float OpacityOfRectangle = 0.5F;


        public void Initialize()
        {
            TimeDisplayed = new int[maxLengthOfchat];
            Texts = new string[maxLengthOfchat];
        }

        public void NewLine(string Newtext, int TimeToDisplay)
        {
            //System.Array.Resize(ref TimeDisplayed, TotalLines + 1);
            //System.Array.Resize(ref Texts, TotalLines + 1);

            if (TotalLines + 1 >= maxLengthOfchat)
                TotalLines = 0;
            Texts[TotalLines] = "Ωa" + Newtext;
            TimeDisplayed[TotalLines] = TimeToDisplay;
            TotalLines += 1;
        }
        public void RefreshText(int time)
        {
            Lines = 0;
            data.text = "";
            for (int i = TotalLines - 1; i >= 0; i--)
            {
                if (TimeDisplayed[i] <= time && time <= TimeDisplayed[i] + TimePerLine)
                {
                    string newline = "";
                    if (Lines > 0)
                        newline = "\n";
                    data.text = Texts[i] + newline +  data.text;
                    Lines += 1; 
                }
                else
                    break;
                if (Lines >= LineLimit)
                    break;
            }
            
        }
    }
    public struct Text
    {
        public string text;
        public char[] chars;
        public Vector2[] coords;
        public int[] color;
        public Rectangle size;
        public int NumberOfLines;
        public int[] LinePerCharecter;
    }
    public static class SpriteBatchExtensions
    {
        public static void DrawRoundedRect(this SpriteBatch spriteBatch, Rectangle destinationRectangle, Texture2D texture, int border, Color color)
        {
            // Top left
            spriteBatch.Draw(
                texture,
                new Rectangle(destinationRectangle.Location, new Point(border)),
                new Rectangle(0, 0, border, border),
                color);

            // Top
            spriteBatch.Draw(
                texture,
                new Rectangle(destinationRectangle.Location + new Point(border, 0),
                    new Point(destinationRectangle.Width - border * 2, border)),
                new Rectangle(border, 0, texture.Width - border * 2, border),
                color);

            // Top right
            spriteBatch.Draw(
                texture,
                new Rectangle(destinationRectangle.Location + new Point(destinationRectangle.Width - border, 0), new Point(border)),
                new Rectangle(texture.Width - border, 0, border, border),
                color);

            // Middle left
            spriteBatch.Draw(
                texture,
                new Rectangle(destinationRectangle.Location + new Point(0, border), new Point(border, destinationRectangle.Height - border * 2)),
                new Rectangle(0, border, border, texture.Height - border * 2),
                color);

            // Middle
            spriteBatch.Draw(
                texture,
                new Rectangle(destinationRectangle.Location + new Point(border), destinationRectangle.Size - new Point(border * 2)),
                new Rectangle(border, border, texture.Width - border * 2, texture.Height - border * 2),
                color);

            // Middle right
            spriteBatch.Draw(
                texture,
                new Rectangle(destinationRectangle.Location + new Point(destinationRectangle.Width - border, border),
                    new Point(border, destinationRectangle.Height - border * 2)),
                new Rectangle(texture.Width - border, border, border, texture.Height - border * 2),
                color);

            // Bottom left
            spriteBatch.Draw(
                texture,
                new Rectangle(destinationRectangle.Location + new Point(0, destinationRectangle.Height - border), new Point(border)),
                new Rectangle(0, texture.Height - border, border, border),
                color);

            // Bottom
            spriteBatch.Draw(
                texture,
                new Rectangle(destinationRectangle.Location + new Point(border, destinationRectangle.Height - border),
                    new Point(destinationRectangle.Width - border * 2, border)),
                new Rectangle(border, texture.Height - border, texture.Width - border * 2, border),
                color);

            // Bottom right
            spriteBatch.Draw(
                texture,
                new Rectangle(destinationRectangle.Location + destinationRectangle.Size - new Point(border), new Point(border)),
                new Rectangle(texture.Width - border, texture.Height - border, border, border),
                color);
        }

        public static void DrawVertices(this SpriteBatch spriteBatch, Texture2D pixel, DoubleCoordinates[] TestVectrices, Vector2 position, Vector2 middleposition, Color color)
        {
            int next = 0;
            for (int current = 0; current < TestVectrices.Length; current++)
            {
                next = current + 1;
                if (next == TestVectrices.Length) next = 0;
                DoubleCoordinates vc = TestVectrices[current];    // c for "current"
                DoubleCoordinates vn = TestVectrices[next];       // n for "next"
                double pente = (vn.Y - vc.Y) / (vn.X - vc.X);
                double b = vc.Y - pente * vc.X;
                double Forvalue1 = vc.X;
                double Forvalue2 = vn.X;
                bool ForLoopY = false;

                if (Math.Abs(vc.Y - vn.Y) > Math.Abs(vc.X - vn.X))
                {
                    Forvalue1 = vc.Y;
                    Forvalue2 = vn.Y;
                    ForLoopY = true;
                }
                if (Forvalue1 > Forvalue2)
                {
                    double savedvalue = Forvalue1;
                    Forvalue1 = Forvalue2;
                    Forvalue2 = savedvalue;
                }
                for (int i = Convert.ToInt32(Forvalue1); i < Convert.ToInt32(Forvalue2); i++)
                {
                    if (ForLoopY)
                    {
                        //pente = (Forvalue2 - Forvalue1) / (vc.X  - vn.X );
                        int speciealy;
                        if (pente > 1000000 || pente < -1000000)
                            speciealy = Convert.ToInt32(vn.X);
                        else
                            speciealy = Convert.ToInt32((i - b) / pente);


                        spriteBatch.Draw(pixel, new Vector2(speciealy, i) - position + middleposition, color);
                    }
                    else
                    {
                        int speciealx;
                        if (pente > 1000000 || pente < -1000000)
                            speciealx = Convert.ToInt32(vn.Y);
                        else
                            speciealx = Convert.ToInt32(pente * i + b);


                        spriteBatch.Draw(pixel, new Vector2(i, speciealx) - position + middleposition, color);

                    }

                }

            }
        }
    }

    
}




