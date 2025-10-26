using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SimpleGame;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Texture2D[] playerFrames = new Texture2D[3];
    private Vector2 playerPosition = new Vector2(350, 200);
    private Vector2 playerScale = new Vector2(80, 80);
    private int playerFrame = 0;
    private float playerTimeToChangeFrame = 0.3f;
    private const float PLAYER_FRAME_TIME = 0.3f;
    private PlayerDirection playerVerticalDirection = PlayerDirection.No;
    private PlayerDirection playerHorizontalDirection = PlayerDirection.No;
    private int playerSpeed = 10;
    private const int SPEED_MOD = 25;
    private bool looksRight = true;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
        for(int i = 0; i < playerFrames.Length; i++)
        {  
            playerFrames[i] = Content.Load<Texture2D>($"Sprites/Player/player{i}");
        }
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
        || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        GetInput();


        MovePlayer(playerVerticalDirection, playerHorizontalDirection, gameTime);

        if (playerHorizontalDirection != PlayerDirection.No
        || playerVerticalDirection != PlayerDirection.No)
        {
            AnimatePlayer(gameTime);
        }
        else playerFrame = 0;

        base.Update(gameTime);
    }
    
    private void GetInput()
    {
        var keyboardState = Keyboard.GetState();
        playerVerticalDirection = PlayerDirection.No;
        playerHorizontalDirection = PlayerDirection.No;

        if (keyboardState.IsKeyDown(Keys.W)) playerVerticalDirection = PlayerDirection.Up;
        if (keyboardState.IsKeyDown(Keys.A)) playerHorizontalDirection = PlayerDirection.Left;
        if (keyboardState.IsKeyDown(Keys.S)) playerVerticalDirection = PlayerDirection.Down;
        if (keyboardState.IsKeyDown(Keys.D)) playerHorizontalDirection = PlayerDirection.Right;
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        Rectangle playerRectangle = new Rectangle
        (
            (int)playerPosition.X, (int)playerPosition.Y,

            (int)playerScale.X, (int)playerScale.Y
        );

        SpriteEffects flip = (!looksRight)
        ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

        _spriteBatch.Draw
        (
            playerFrames[playerFrame], playerRectangle,
            null, Color.White, 0f, Vector2.Zero,
            flip, 0f
        );

        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private void MovePlayer(PlayerDirection vDir, PlayerDirection hDir, GameTime gameTime)
    {
        int toMove = (int)(playerSpeed * SPEED_MOD * gameTime.ElapsedGameTime.TotalSeconds);

        if (looksRight && hDir == PlayerDirection.Left) FlipPlayer();
        else if (!looksRight && hDir == PlayerDirection.Right) FlipPlayer();

        switch (vDir)
        {
            case PlayerDirection.Up: playerPosition.Y -= toMove; break;
            case PlayerDirection.Down: playerPosition.Y += toMove; break;
        }

        switch (hDir)
        {
            case PlayerDirection.Right: playerPosition.X += toMove; break;
            case PlayerDirection.Left: playerPosition.X -= toMove; break;
        }
    }

    private void AnimatePlayer(GameTime gameTime)
    {
        if (playerFrame == 0) playerFrame = 1;
        playerTimeToChangeFrame -= (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (playerTimeToChangeFrame <= 0)
        {
            if (playerFrame == 1) playerFrame = 2;
            else if (playerFrame == 2) playerFrame = 1;
            playerTimeToChangeFrame = PLAYER_FRAME_TIME;
        }
    }

    private void FlipPlayer()
    {
        looksRight = !looksRight;
    }

    enum PlayerDirection
    {
        Up, Down, Right, Left, No
    }
}
