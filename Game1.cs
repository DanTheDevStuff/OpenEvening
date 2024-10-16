using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;

namespace OpenEveningProject;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Song sfxExplosion;
    private Song sfxVictory;
    private SpriteFont _font;
    private string _text = "";

    //MenuOptions
    private enum MenuState { MainMenu, Play, Options, Exit }
    private MenuState _currentMenuState = MenuState.MainMenu;
    private SpriteFont _menuFont;
    private string[] _menuItems = { "Play", "Options", "Exit" };
    private int _selectedIndex = 0;

    //Options Options
    private string[] _optionsItems = { "Difficulty", "Back" };
    private int _selectedOptionsIndex = 0;
    private int _difficulty = 0;

    private double _inputCooldown;
    private double _inputCooldownTimer;

    Texture2D squareTexture;
   	Color squareColour;
   	Rectangle squareRectangle;
   	Random random;

    bool alive;
    bool started;
    int r, g, b;
    int ar, ag, ab;
    int rr, rg, rb;
    short bounds;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

    }

    public Color GenerateRandomKillingColor(Random random)
	{
	    // You can define ranges for "killing" colors.
	    // For example, we might use bright reds, greens, or yellows.
	    int r, g, b;

	    // Randomly decide which color range to generate from
	    int colorType = random.Next(1, 5); // Generate a random number between 1 and 4

	    switch (colorType)
	    {
	        case 1: // Bright Red
	            r = random.Next(200, 256); // High red
	            g = random.Next(0, 100);   // Low green
	            b = random.Next(0, 100);   // Low blue
	            break;
	        case 2: // Bright Green
	            r = random.Next(0, 100);   // Low red
	            g = random.Next(200, 256); // High green
	            b = random.Next(0, 100);   // Low blue
	            break;
	        case 3: // Bright Yellow
	            r = random.Next(200, 256); // High red
	            g = random.Next(200, 256); // High green
	            b = random.Next(0, 100);   // Low blue
	            break;
	        case 4: // Deep Purple
	            r = random.Next(100, 156); // Medium red
	            g = random.Next(0, 100);   // Low green
	            b = random.Next(100, 256); // High blue
	            break;
	        default:
	            r = 0; g = 0; b = 0; // Fallback to black if something goes wrong
	            break;
    }

    return new Color(r, g, b); // Return the generated color
	}	

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
		r=0;
		g=0;
		b=0;

		random = new Random();

		rr = random.Next(256);
		rg = random.Next(256);
		rb = random.Next(256);

		ar = random.Next(256);
		ag = random.Next(256);
		ab = random.Next(256);

		_inputCooldownTimer = 0;
    	_inputCooldown = 0.2;

		bounds = 15;

		alive = true;
		started = false;
		Window.Title = "RGB Roulette";
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        sfxExplosion = Content.Load<Song>("medium-explosion-40472");
        sfxVictory = Content.Load<Song>("victory-96688");
        _font = Content.Load<SpriteFont>("title");
        _menuFont = Content.Load<SpriteFont>("menu");

        squareTexture = new Texture2D(GraphicsDevice, 1, 1);
        squareTexture.SetData(new Color[] { Color.White });

        squareColour = new Color(rr, rg, rb);

        int squareSize = 100;
        squareRectangle = new Rectangle(
        	(GraphicsDevice.Viewport.Width - squareSize) / 2,
        	(GraphicsDevice.Viewport.Height - squareSize) / 2,
        	squareSize,
        	squareSize
        );
    }

    protected override void Update(GameTime gameTime)
    {	
    	GamePadState _player1 = GamePad.GetState(PlayerIndex.One);
    	GamePadState _player2 = GamePad.GetState(PlayerIndex.Two);
    	GamePadState _selPlayer = _player1;
    	Random random = new Random();

    	KeyboardState keyboardState = Keyboard.GetState();
    	_inputCooldownTimer += gameTime.ElapsedGameTime.TotalSeconds;



        if (_currentMenuState == MenuState.MainMenu)
            {
                HandleMenuInput(keyboardState);
            }
        else if (_currentMenuState == MenuState.Options)
        	HandleOptionsInput();

        if (_selPlayer.Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)){
            _currentMenuState = MenuState.MainMenu;
            started = false;
        }


        if (started) 
	        {
	        	if (alive) {
	        	        	//Add Green
	        				if (_selPlayer.Buttons.A == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.G))
	        					if (g < 255)
	        						g++;
	        				//Add Red
	        				if (_selPlayer.Buttons.B == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.R))
	        					if (r < 255)
	        						r++;
	        				//Add Blue
	        				if (_selPlayer.Buttons.X == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.B))
	        					if (b < 255)
	        						b++;
	        				//Add Yellow
	        				if (_selPlayer.Buttons.Y == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Y)){
	        					if (r < 255)
	        						r++;
	        					
	        					if (g < 255)
	        						g++;
	        				}
	        
	        				//Remove Yellow
	        				if (_selPlayer.DPad.Up == ButtonState.Pressed || (Keyboard.GetState().IsKeyDown(Keys.LeftShift) && Keyboard.GetState().IsKeyDown(Keys.Y))){
	        					if (r > 0)
	        						r--;
	        					if (g > 0)
	        						g--;
	        				}
	        				//Remove Green
	        				if (_selPlayer.DPad.Down == ButtonState.Pressed || (Keyboard.GetState().IsKeyDown(Keys.LeftShift) && Keyboard.GetState().IsKeyDown(Keys.G)))
	        					if (g > 0)
	        						g--;
	        				//Remove Red
	        				if (_selPlayer.DPad.Right == ButtonState.Pressed || (Keyboard.GetState().IsKeyDown(Keys.LeftShift) && Keyboard.GetState().IsKeyDown(Keys.R)))
	        					if (r > 0)
	        						r--;
	        				//Remove Blue
	        				if (_selPlayer.DPad.Left == ButtonState.Pressed || (Keyboard.GetState().IsKeyDown(Keys.LeftShift) && Keyboard.GetState().IsKeyDown(Keys.B)))
	        					if (b > 0)
	        						b--;
	        				
	        				
	        			}
	        				//Fail
	        				if (r == 255 && g == 255 && b == 255)
	        					{
	        						if (MediaPlayer.State != MediaState.Playing) {
	        							MediaPlayer.Play(sfxExplosion);
	        							MediaPlayer.IsRepeating = false;
	        							r = 0;
	        							g = 0;
	        							b = 0;
	        							_text = "You died";
	        							alive = false;
	        							}
	        					}
	        				//Reset colours to black
	        			if (_selPlayer.Buttons.Start == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.S)) {
	        				r = 0;
	        				g = 0;
	        				b = 0;
	        				_text = "Match the colour of the screen with that in the middle!";
	        				alive = true;
	        			}
	        
	        			//Fail
	        			Color randomKillingColor = GenerateRandomKillingColor(random);
	        
	        			if (r == randomKillingColor.R && g == randomKillingColor.G && b == randomKillingColor.B){
	        				if (MediaPlayer.State != MediaState.Playing) {
	        					MediaPlayer.Play(sfxExplosion);
	        					MediaPlayer.IsRepeating = false;
	        					r = 0;
	        					g = 0;
	        					b = 0;
	        					_text = "You died";
	        					alive = false;
	        					}
	        			}
	        
	        			//Check if won
	        			if (Math.Abs(r -rr) <= bounds && Math.Abs(g- rg) <= bounds && Math.Abs(b- rb) <= bounds) {
	        				_text = "You Won!!";
	        				if (MediaPlayer.State != MediaState.Playing) {
	        							MediaPlayer.Play(sfxVictory);
	        							MediaPlayer.IsRepeating = false;
	        				}
	        				
	        			}}
        base.Update(gameTime);
	}
	private void HandleMenuInput(KeyboardState _keyboardState)
{
    // Check if enough time has passed for the next input
    if (_inputCooldownTimer >= _inputCooldown)
    {
        if (_keyboardState.IsKeyDown(Keys.Up))
        {
            _selectedIndex--;
            if (_selectedIndex < 0) _selectedIndex = _menuItems.Length - 1;
            _inputCooldownTimer = 0; // Reset the timer after input is handled
        }

        if (_keyboardState.IsKeyDown(Keys.Down))
        {
            _selectedIndex++;
            if (_selectedIndex >= _menuItems.Length) _selectedIndex = 0;
            _inputCooldownTimer = 0; // Reset the timer after input is handled
        }

        if (_keyboardState.IsKeyDown(Keys.Enter))
        {
            switch (_selectedIndex)
            {
                case 0:
                    _currentMenuState = MenuState.Play;
                    break;
                case 1:
                    _currentMenuState = MenuState.Options;
                    break;
                case 2:
                    Exit();
                    break;
            }
            _inputCooldownTimer = 0; // Reset the timer after input is handled
        }
    }
}

	private void HandleOptionsInput() 
	{
		if (_inputCooldownTimer	>= _inputCooldown)
		{
			if (_keyboardState.IsKeyDown(Keys.Up))
			{
				_selectedOptionsIndex--;
				if (_selectedOptionsIndex < 0)
					_selectedOptionsIndex = _selectedOptionsIndex = 0
				_inputCooldownTimer = 0;
			}

			if (_keyboardState.IsKeyDown(Keys.Down))
			{
				_selectedOptionsIndex++;
				if (_selectedOptionsIndex >= _optionsItems.Length)
					_selectedOptionsIndex = 0;
				_inputCooldownTimer = 0;
			}
			if 
		}
	}


    protected override void Draw(GameTime gameTime)
    {

		Color colourOne = new Color(r, g, b);
        GraphicsDevice.Clear(colourOne );
        _spriteBatch.Begin();
        if (_currentMenuState == MenuState.MainMenu)
        	DrawMenu();
        else if (_currentMenuState == MenuState.Play) {
        	_text = "Make the colour of the screen the same as the middle colour";
        	started=true;
        }
        else if (_currentMenuState == MenuState.Options)
        	_spriteBatch.DrawString(_menuFont, "Options Menu", new Vector2(100, 100), Color.White);
        _spriteBatch.DrawString(_font, _text, new Vector2(250,100), Color.White);
        _spriteBatch.Draw(squareTexture, squareRectangle, squareColour);
        _spriteBatch.End();

        // TODO: Add your drawing code here

        base.Draw(gameTime);
    }
    private void DrawMenu()
    {
    	for (int i = 0; i < _menuItems.Length; i++){
    		Color colour = (i == _selectedIndex) ? Color.Yellow : Color.White;
    		_spriteBatch.DrawString(_menuFont, _menuItems[i], new Vector2(100, 100 + i * 30), colour);
    	}
    }
}