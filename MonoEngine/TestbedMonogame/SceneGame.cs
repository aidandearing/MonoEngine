using MonoEngine;
using MonoEngine.Assets;
using MonoEngine.Game;
using MonoEngine.Render;
using MonoEngine.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace TestbedMonogame
{
    class SceneGame : Scene
    {
        enum Rooms { kitchen, dining, living, hall, bedroom, garage, walkway, road, bathroom, study };
        enum Walls { corner_top_right, corner_top_left, corner_bottom_left, corner_bottom_right, wall_left, wall_bottom, wall_right, wall_top, window_single_left, window_single_bottom, window_single_right, window_single_top, window_far_top, window_far_left, window_far_bottom, window_far_right, window_near_top, window_near_left, window_near_bottom, window_near_right, window_middle_top, window_middle_left, window_middle_bottom, window_middle_right };

        public Dictionary<string, Material> materials;

        Dictionary<Vector2, Rooms> floors = new Dictionary<Vector2, Rooms>();
        Dictionary<Vector2, Walls> walls = new Dictionary<Vector2, Walls>();

        List<Vector2> doors;
        Vector2 room_position;
        int room_width;
        int room_height;

        List<Vector2> empty;
        List<Vector2> windows_single;
        List<Vector2> windows_side_far;
        List<Vector2> windows_side_near;
        List<Vector2> windows_middle;

        public bool isInit = false;

        public SceneGame() : base()
        {

        }

        public void Initialise()
        {
            if (isInit)
                return;

            isInit = true;

            // Defaults ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            // All the mostly generic scene set up, that I hope to be able to get rid of completely with the prefab system.
            // TODO Update this comment after completing the prefab system

            GameObjectManager.AddGameObject(Camera.Isometric("mainCamera", new Vector3(0, 0, 0f), 2, -10000.0f, 10000.0f));

            floors = new Dictionary<Vector2, Rooms>();
            walls = new Dictionary<Vector2, Walls>();

            materials = new Dictionary<string, Material>();

            RenderTargetBatch batch = RenderManager.GetRenderTargetBatch("default") as RenderTargetBatch;
            batch.settings.sampler = SamplerState.PointClamp;

            materials.Add("floor_grass", new Material(new BasicEffect(GraphicsHelper.graphicsDevice)));
            BasicEffect effect = materials["floor_grass"].effect as BasicEffect;
            effect.TextureEnabled = true;
            effect.Texture = Resources.LoadAsset(new Sprite().GetType(), "floor_grass", this) as Sprite;

            // Kitchen -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            // All the set up necessary for the world data to know how to look in the kitchen
            // floors 0 <- The int value returned by the floors dictionary when passed any of the values within this rooms boundaries

            // Setup: Floor -----------------------------------------------------------------------------
            // This code sets up the floor data for world generation, and assets
            // Make a material for the floor
            materials.Add("floor_kitchen", new Material(new BasicEffect(GraphicsHelper.graphicsDevice)));
            effect = materials["floor_kitchen"].effect as BasicEffect;
            effect.TextureEnabled = true;
            // Load the floor texture for it
            effect.Texture = Resources.LoadAsset(new Sprite().GetType(), "floor_kitchen", this) as Sprite;

            // Setup: Walls -----------------------------------------------------------------------------
            // Make a material for the wall
            materials.Add("wall_kitchen", new Material(new BasicEffect(GraphicsHelper.graphicsDevice)));
            effect = materials["wall_kitchen"].effect as BasicEffect;
            effect.TextureEnabled = true;
            // Load the wall texture for it
            effect.Texture = Resources.LoadAsset(new Sprite().GetType(), "wall_kitchen", this) as Sprite;

            // Setup: Rooms -----------------------------------------------------------------------------
            // The top left corner of the room
            room_position = new Vector2(9, 6);
            // Its dimensions
            room_width = 4;
            room_height = 3;

            // Doors
            doors = new List<Vector2>()
            {
                // Put the exact spot (integer please) where you want a door to go, on the room grid
            };

            // Empty walls (for open spaces, or floating walls)
            empty = new List<Vector2>()
            {
                new Vector2(12,6),
                new Vector2(12,7),
                new Vector2(12,8),
            };

            // Windows single
            windows_single = new List<Vector2>()
            {
                new Vector2(9,7),
            };

            // Windows wide far side 
            #region COMMENT: detailed
            //   /  /|
            //  /  / |
            // |\ /  |
            // | |   |
            //  \|/| |
            //   | | |
            //   | | |
            //  / \| |
            // |\ / /
            // | | /
            //  \|/
            #endregion
            windows_side_far = new List<Vector2>()
            {

            };

            // Windows wide near side
            #region COMMENT: detailed
            //    / \
            //   /  /|
            //  /  / |
            // |\ / /
            // | | |
            // | | |
            // | | |\
            // | | |/|
            // | |   |
            // | |  /
            // | | /
            #endregion
            windows_side_near = new List<Vector2>()
            {

            };

            // Windows wide middle part 
            windows_middle = new List<Vector2>()
            {

            };

            // Now make the room data
            Helper_InitialiseRoomData(Rooms.kitchen, true);

            // Dining Room ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            // floors 1

            // Setup: Floor -----------------------------------------------------------------------------
            // This code sets up the floor data for world generation, and assets
            // Make a material for the floor
            materials.Add("floor_dining", new Material(new BasicEffect(GraphicsHelper.graphicsDevice)));
            effect = materials["floor_dining"].effect as BasicEffect;
            effect.TextureEnabled = true;
            // Load the floor texture for it
            effect.Texture = Resources.LoadAsset(new Sprite().GetType(), "floor_dining", this) as Sprite;

            // Setup: Walls -----------------------------------------------------------------------------
            // Make a material for the wall
            materials.Add("wall_dining", new Material(new BasicEffect(GraphicsHelper.graphicsDevice)));
            effect = materials["wall_dining"].effect as BasicEffect;
            effect.TextureEnabled = true;
            // Load the wall texture for it
            effect.Texture = Resources.LoadAsset(new Sprite().GetType(), "wall_dining", this) as Sprite;

            // Setup: Rooms -----------------------------------------------------------------------------
            // The top left corner of the room
            room_position = new Vector2(13, 6);
            // Its dimensions
            room_width = 5;
            room_height = 6;

            // Doors
            doors = new List<Vector2>()
            {

            };

            // Empty walls (for open spaces, or floating walls)
            empty = new List<Vector2>()
            {

            };

            // Windows single
            windows_single = new List<Vector2>()
            {

            };

            // Windows wide far side
            windows_side_far = new List<Vector2>()
            {

            };

            // Windows wide near side
            windows_side_near = new List<Vector2>()
            {

            };

            // Windows wide middle part 
            windows_middle = new List<Vector2>()
            {

            };

            // Now make the room data
            Helper_InitialiseRoomData(Rooms.dining, true);

            // Living Room ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            // floors 2

            // Setup: Floor -----------------------------------------------------------------------------
            // This code sets up the floor data for world generation, and assets
            // Make a material for the floor
            materials.Add("floor_living", new Material(new BasicEffect(GraphicsHelper.graphicsDevice)));
            effect = materials["floor_living"].effect as BasicEffect;
            effect.TextureEnabled = true;
            // Load the floor texture for it
            effect.Texture = Resources.LoadAsset(new Sprite().GetType(), "floor_hall", this) as Sprite;

            // Setup: Walls -----------------------------------------------------------------------------
            // Make a material for the wall
            materials.Add("wall_living", new Material(new BasicEffect(GraphicsHelper.graphicsDevice)));
            effect = materials["wall_living"].effect as BasicEffect;
            effect.TextureEnabled = true;
            // Load the wall texture for it
            effect.Texture = Resources.LoadAsset(new Sprite().GetType(), "wall_living", this) as Sprite;

            // Setup: Rooms -----------------------------------------------------------------------------
            // The top left corner of the room
            room_position = new Vector2(12, 13);
            // Its dimensions
            room_width = 6;
            room_height = 4;

            // Doors
            doors = new List<Vector2>()
            {

            };

            // Empty walls (for open spaces, or floating walls)
            empty = new List<Vector2>()
            {

            };

            // Windows single
            windows_single = new List<Vector2>()
            {

            };

            // Windows wide far side 
            windows_side_far = new List<Vector2>()
            {

            };

            // Windows wide near side
            windows_side_near = new List<Vector2>()
            {

            };

            // Windows wide middle part 
            windows_middle = new List<Vector2>()
            {

            };

            // Now make the room data
            Helper_InitialiseRoomData(Rooms.living, true);

            // Hallways ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            // floors 3

            // Setup: Floor -----------------------------------------------------------------------------
            // This code sets up the floor data for world generation, and assets
            // Make a material for the floor
            materials.Add("floor_hall", new Material(new BasicEffect(GraphicsHelper.graphicsDevice)));
            effect = materials["floor_hall"].effect as BasicEffect;
            effect.TextureEnabled = true;
            // Load the floor texture for it
            effect.Texture = Resources.LoadAsset(new Sprite().GetType(), "floor_hall", this) as Sprite;

            // Setup: Walls -----------------------------------------------------------------------------
            // Make a material for the wall
            materials.Add("wall_hall", new Material(new BasicEffect(GraphicsHelper.graphicsDevice)));
            effect = materials["wall_hall"].effect as BasicEffect;
            effect.TextureEnabled = true;
            // Load the wall texture for it
            effect.Texture = Resources.LoadAsset(new Sprite().GetType(), "wall_hall", this) as Sprite;

            // Setup: Rooms -----------------------------------------------------------------------------
            // The top left corner of the room
            room_position = new Vector2(18, 8);
            // Its dimensions
            room_width = 1;
            room_height = 9;

            // Doors
            doors = new List<Vector2>()
            {

            };

            // Empty walls (for open spaces, or floating walls)
            empty = new List<Vector2>()
            {

            };

            // Windows single
            windows_single = new List<Vector2>()
            {

            };

            // Windows wide far side 
            windows_side_far = new List<Vector2>()
            {

            };

            // Windows wide near side
            windows_side_near = new List<Vector2>()
            {

            };

            // Windows wide middle part 
            windows_middle = new List<Vector2>()
            {

            };

            // Now make the room data
            Helper_InitialiseRoomData(Rooms.hall, true);

            // Setup: Rooms -----------------------------------------------------------------------------
            // The top left corner of the room
            room_position = new Vector2(9, 12);
            // Its dimensions
            room_width = 9;
            room_height = 1;

            // Doors
            doors = new List<Vector2>()
            {

            };

            // Empty walls (for open spaces, or floating walls)
            empty = new List<Vector2>()
            {

            };

            // Windows single
            windows_single = new List<Vector2>()
            {

            };

            // Windows wide far side 
            windows_side_far = new List<Vector2>()
            {

            };

            // Windows wide near side
            windows_side_near = new List<Vector2>()
            {

            };

            // Windows wide middle part 
            windows_middle = new List<Vector2>()
            {

            };

            // Now make the room data
            Helper_InitialiseRoomData(Rooms.hall, true);

            // Bedroom -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            // floors 4

            // Setup: Floor -----------------------------------------------------------------------------
            // This code sets up the floor data for world generation, and assets
            // Make a material for the floor
            materials.Add("floor_bedroom", new Material(new BasicEffect(GraphicsHelper.graphicsDevice)));
            effect = materials["floor_bedroom"].effect as BasicEffect;
            effect.TextureEnabled = true;
            // Load the floor texture for it
            effect.Texture = Resources.LoadAsset(new Sprite().GetType(), "floor_hall", this) as Sprite;

            // Setup: Walls -----------------------------------------------------------------------------
            // Make a material for the wall
            materials.Add("wall_bedroom", new Material(new BasicEffect(GraphicsHelper.graphicsDevice)));
            effect = materials["wall_bedroom"].effect as BasicEffect;
            effect.TextureEnabled = true;
            // Load the wall texture for it
            effect.Texture = Resources.LoadAsset(new Sprite().GetType(), "wall_bedroom", this) as Sprite;

            // Setup: Rooms -----------------------------------------------------------------------------
            // The top left corner of the room
            room_position = new Vector2(19, 8);
            // Its dimensions
            room_width = 4;
            room_height = 4;

            // Doors
            doors = new List<Vector2>()
            {

            };

            // Empty walls (for open spaces, or floating walls)
            empty = new List<Vector2>()
            {

            };

            // Windows single
            windows_single = new List<Vector2>()
            {

            };

            // Windows wide far side
            windows_side_far = new List<Vector2>()
            {

            };

            // Windows wide near side
            windows_side_near = new List<Vector2>()
            {

            };

            // Windows wide middle part 
            windows_middle = new List<Vector2>()
            {

            };

            // Now make the room data
            Helper_InitialiseRoomData(Rooms.bedroom, true);

            // Garage --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            // floors 5

            // Setup: Floor -----------------------------------------------------------------------------
            // This code sets up the floor data for world generation, and assets
            // Make a material for the floor
            materials.Add("floor_garage", new Material(new BasicEffect(GraphicsHelper.graphicsDevice)));
            effect = materials["floor_garage"].effect as BasicEffect;
            effect.TextureEnabled = true;
            // Load the floor texture for it
            effect.Texture = Resources.LoadAsset(new Sprite().GetType(), "floor_cement_tile", this) as Sprite;

            // Setup: Walls -----------------------------------------------------------------------------
            // Make a material for the wall
            materials.Add("wall_garage", new Material(new BasicEffect(GraphicsHelper.graphicsDevice)));
            effect = materials["wall_garage"].effect as BasicEffect;
            effect.TextureEnabled = true;
            // Load the wall texture for it
            effect.Texture = Resources.LoadAsset(new Sprite().GetType(), "wall_garage", this) as Sprite;

            // Setup: Rooms -----------------------------------------------------------------------------
            // The top left corner of the room
            room_position = new Vector2(19, 12);
            // Its dimensions
            room_width = 4;
            room_height = 5;

            // Doors
            doors = new List<Vector2>()
            {

            };

            // Empty walls (for open spaces, or floating walls)
            empty = new List<Vector2>()
            {

            };

            // Windows single
            windows_single = new List<Vector2>()
            {

            };

            // Windows wide far side
            windows_side_far = new List<Vector2>()
            {

            };

            // Windows wide near side
            windows_side_near = new List<Vector2>()
            {

            };

            // Windows wide middle part 
            windows_middle = new List<Vector2>()
            {

            };

            // Now make the room data
            Helper_InitialiseRoomData(Rooms.garage, true);

            // Walkway -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            // floors 6

            // Setup: Floor -----------------------------------------------------------------------------
            // This code sets up the floor data for world generation, and assets
            // Make a material for the floor
            materials.Add("floor_walk", new Material(new BasicEffect(GraphicsHelper.graphicsDevice)));
            effect = materials["floor_walk"].effect as BasicEffect;
            effect.TextureEnabled = true;
            // Load the floor texture for it
            effect.Texture = Resources.LoadAsset(new Sprite().GetType(), "floor_cement_tile", this) as Sprite;

            // Setup: Rooms -----------------------------------------------------------------------------
            // The top left corner of the room
            room_position = new Vector2(18, 6);
            // Its dimensions
            room_width = 5;
            room_height = 2;

            // Doors
            doors = new List<Vector2>() { };

            // Empty walls (for open spaces, or floating walls)
            empty = new List<Vector2>() { };

            // Windows single
            windows_single = new List<Vector2>() { };

            // Windows wide far side
            windows_side_far = new List<Vector2>() { };

            // Windows wide near side
            windows_side_near = new List<Vector2>() { };

            // Windows wide middle part 
            windows_middle = new List<Vector2>() { };

            // Now make the room data
            Helper_InitialiseRoomData(Rooms.walkway, false);

            // Setup: Rooms -----------------------------------------------------------------------------
            // The top left corner of the room
            room_position = new Vector2(16, 17);
            // Its dimensions
            room_width = 3;
            room_height = 2;

            // Doors
            doors = new List<Vector2>() { };

            // Empty walls (for open spaces, or floating walls)
            empty = new List<Vector2>() { };

            // Windows single
            windows_single = new List<Vector2>() { };

            // Windows wide far side
            windows_side_far = new List<Vector2>() { };

            // Windows wide near side
            windows_side_near = new List<Vector2>() { };

            // Windows wide middle part 
            windows_middle = new List<Vector2>() { };

            // Now make the room data
            Helper_InitialiseRoomData(Rooms.walkway, false);

            // Setup: Rooms -----------------------------------------------------------------------------
            // The top left corner of the room
            room_position = new Vector2(18, 19);
            // Its dimensions
            room_width = 1;
            room_height = 6;

            // Doors
            doors = new List<Vector2>() { };

            // Empty walls (for open spaces, or floating walls)
            empty = new List<Vector2>() { };

            // Windows single
            windows_single = new List<Vector2>() { };

            // Windows wide far side
            windows_side_far = new List<Vector2>() { };

            // Windows wide near side
            windows_side_near = new List<Vector2>() { };

            // Windows wide middle part 
            windows_middle = new List<Vector2>() { };

            // Now make the room data
            Helper_InitialiseRoomData(Rooms.walkway, false);

            // Setup: Rooms -----------------------------------------------------------------------------
            // The top left corner of the room
            room_position = new Vector2(0, 25);
            // Its dimensions
            room_width = 32;
            room_height = 1;

            // Doors
            doors = new List<Vector2>() { };

            // Empty walls (for open spaces, or floating walls)
            empty = new List<Vector2>() { };

            // Windows single
            windows_single = new List<Vector2>() { };

            // Windows wide far side
            windows_side_far = new List<Vector2>() { };

            // Windows wide near side
            windows_side_near = new List<Vector2>() { };

            // Windows wide middle part 
            windows_middle = new List<Vector2>() { };

            // Now make the room data
            Helper_InitialiseRoomData(Rooms.walkway, false);

            // Road ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            // floors 7

            // Setup: Floor -----------------------------------------------------------------------------
            // This code sets up the floor data for world generation, and assets
            // Make a material for the floor
            materials.Add("floor_road", new Material(new BasicEffect(GraphicsHelper.graphicsDevice)));
            effect = materials["floor_road"].effect as BasicEffect;
            effect.TextureEnabled = true;
            // Load the floor texture for it
            effect.Texture = Resources.LoadAsset(new Sprite().GetType(), "floor_road", this) as Sprite;

            // Setup: Rooms -----------------------------------------------------------------------------
            // The top left corner of the room
            room_position = new Vector2(0, 26);
            // Its dimensions
            room_width = 32;
            room_height = 6;

            // Doors
            doors = new List<Vector2>() { };

            // Empty walls (for open spaces, or floating walls)
            empty = new List<Vector2>() { };

            // Windows single
            windows_single = new List<Vector2>() { };

            // Windows wide far side
            windows_side_far = new List<Vector2>() { };

            // Windows wide near side
            windows_side_near = new List<Vector2>() { };

            // Windows wide middle part 
            windows_middle = new List<Vector2>() { };

            // Now make the room data
            Helper_InitialiseRoomData(Rooms.road, false);

            // Setup: Rooms -----------------------------------------------------------------------------
            // The top left corner of the room
            room_position = new Vector2(19, 17);
            // Its dimensions
            room_width = 4;
            room_height = 7;

            // Doors
            doors = new List<Vector2>() { };

            // Empty walls (for open spaces, or floating walls)
            empty = new List<Vector2>() { };

            // Windows single
            windows_single = new List<Vector2>() { };

            // Windows wide far side
            windows_side_far = new List<Vector2>() { };

            // Windows wide near side
            windows_side_near = new List<Vector2>() { };

            // Windows wide middle part 
            windows_middle = new List<Vector2>() { };

            // Now make the room data
            Helper_InitialiseRoomData(Rooms.road, false);

            // Bathroom ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            // floors 8

            // Setup: Floor -----------------------------------------------------------------------------
            // This code sets up the floor data for world generation, and assets
            // Make a material for the floor
            materials.Add("floor_bath", new Material(new BasicEffect(GraphicsHelper.graphicsDevice)));
            effect = materials["floor_bath"].effect as BasicEffect;
            effect.TextureEnabled = true;
            // Load the floor texture for it
            effect.Texture = Resources.LoadAsset(new Sprite().GetType(), "floor_bath", this) as Sprite;

            // Setup: Walls -----------------------------------------------------------------------------
            // Make a material for the wall
            materials.Add("wall_hall", new Material(new BasicEffect(GraphicsHelper.graphicsDevice)));
            effect = materials["wall_hall"].effect as BasicEffect;
            effect.TextureEnabled = true;
            // Load the wall texture for it
            effect.Texture = Resources.LoadAsset(new Sprite().GetType(), "wall_hall", this) as Sprite;

            // Setup: Rooms -----------------------------------------------------------------------------
            // The top left corner of the room
            room_position = new Vector2(9, 9);
            // Its dimensions
            room_width = 4;
            room_height = 3;

            // Doors
            doors = new List<Vector2>()
            {

            };

            // Empty walls (for open spaces, or floating walls)
            empty = new List<Vector2>()
            {

            };

            // Windows single
            windows_single = new List<Vector2>()
            {

            };

            // Windows wide far side
            windows_side_far = new List<Vector2>()
            {

            };

            // Windows wide near side
            windows_side_near = new List<Vector2>()
            {

            };

            // Windows wide middle part 
            windows_middle = new List<Vector2>()
            {

            };

            // Now make the room data
            Helper_InitialiseRoomData(Rooms.bathroom, true);

            // Study ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            // floors 9

            // Setup: Floor -----------------------------------------------------------------------------
            // This code sets up the floor data for world generation, and assets
            // Make a material for the floor
            materials.Add("floor_study", new Material(new BasicEffect(GraphicsHelper.graphicsDevice)));
            effect = materials["floor_study"].effect as BasicEffect;
            effect.TextureEnabled = true;
            // Load the floor texture for it
            effect.Texture = Resources.LoadAsset(new Sprite().GetType(), "floor_study", this) as Sprite;

            // Setup: Walls -----------------------------------------------------------------------------
            // Make a material for the wall
            materials.Add("wall_hall", new Material(new BasicEffect(GraphicsHelper.graphicsDevice)));
            effect = materials["wall_hall"].effect as BasicEffect;
            effect.TextureEnabled = true;
            // Load the wall texture for it
            effect.Texture = Resources.LoadAsset(new Sprite().GetType(), "wall_hall", this) as Sprite;

            // Setup: Rooms -----------------------------------------------------------------------------
            // The top left corner of the room
            room_position = new Vector2(9, 13);
            // Its dimensions
            room_width = 3;
            room_height = 4;

            // Doors
            doors = new List<Vector2>()
            {

            };

            // Empty walls (for open spaces, or floating walls)
            empty = new List<Vector2>()
            {

            };

            // Windows single
            windows_single = new List<Vector2>()
            {

            };

            // Windows wide far side
            windows_side_far = new List<Vector2>()
            {

            };

            // Windows wide near side
            windows_side_near = new List<Vector2>()
            {

            };

            // Windows wide middle part 
            windows_middle = new List<Vector2>()
            {

            };

            // Now make the room data
            Helper_InitialiseRoomData(Rooms.study, true);

            // World generation ----------------------------------------------------------------------------------------------------------------------------------------------------------------------
            // 

            int width = 32;
            int height = 32;
            float width_h = width / 2.0f;
            float height_h = height / 2.0f;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    ModelRenderer floor = ModelRenderer.MakeModelRenderer("FloorTile");
                    floor.Name = "floor" + x.ToString() + "," + y.ToString();
                    floor.transform.Position = new Vector3((x - width_h) / 2.0f, 0, (y - height_h) / 2.0f);

                    Vector2 pos = new Vector2(x, y);
                    if (floors.ContainsKey(pos))
                    {
                        switch (floors[pos])
                        {
                            case Rooms.kitchen:
                                floor.material = materials["floor_kitchen"];
                                break;
                            case Rooms.dining:
                                floor.material = materials["floor_dining"];
                                break;
                            case Rooms.living:
                                floor.material = materials["floor_living"];
                                break;
                            case Rooms.hall:
                                floor.material = materials["floor_hall"];
                                break;
                            case Rooms.bedroom:
                                floor.material = materials["floor_bedroom"];
                                break;
                            case Rooms.garage:
                                floor.material = materials["floor_garage"];
                                break;
                            case Rooms.walkway:
                                floor.material = materials["floor_walk"];
                                break;
                            case Rooms.road:
                                floor.material = materials["floor_road"];
                                break;
                            case Rooms.bathroom:
                                floor.material = materials["floor_bath"];
                                break;
                            case Rooms.study:
                                floor.material = materials["floor_study"];
                                break;
                        }
                    }
                    else
                    {
                        floor.material = materials["floor_grass"];
                    }

                    GameObjectManager.AddGameObject(floor);
                }
            }
        }

        public override void Update()
        {
            Initialise();
        }

        private void Helper_InitialiseRoomData(Rooms floor, bool hasWalls)
        {
            // Now make the room data
            for (int x = 0; x < room_width; x++)
            {
                for (int y = 0; y < room_height; y++)
                {
                    Vector2 position = new Vector2(x, y) + room_position;
                    // Data: Wall
                    // Now this needs to have some hard coded 'door' values inserted before here in order to do what it needs to
                    // Otherwise this code would just wall every room in
                    // It will also need 'window' values, and 'empty' values for wide open spaces
                    // Check: If the empty contains this position no wall of any sort should be generated here

                    if (hasWalls) { Helper_InitialiseWallData(position); }

                    floors.Add(position, floor);
                }
            }
        }

        private void Helper_InitialiseWallData(Vector2 position)
        {
            if (!empty.Contains(position))
            {
                // Top Left Corner Wall
                if (position.X == room_position.X && position.Y == room_position.Y)
                {
                    walls.Add(position, Walls.corner_top_left);
                }
                // Bottom Left Corner Wall
                else if (position.X == room_position.X && position.Y == room_position.Y + room_height - 1)
                {
                    walls.Add(position, Walls.corner_bottom_left);
                }
                // Bottom Right Corner Wall
                else if (position.X == room_position.X + room_width - 1 && position.Y == room_position.Y + room_height - 1)
                {
                    walls.Add(position, Walls.corner_bottom_right);
                }
                // Top Right Corner Wall
                else if (position.X == room_position.X + room_width - 1 && position.Y == room_position.Y)
                {
                    walls.Add(position, Walls.corner_top_right);
                }
                // Now all of the below are special, they can have windows and the like
                else
                {
                    // Walls can be windows so all of them need to be in that context
                    // Left Wall
                    if (position.X == room_position.X)
                    {
                        if (windows_single.Contains(position))
                        {
                            // Make this wall a single window
                            walls.Add(position, Walls.window_single_left);
                        }
                        else if (windows_side_far.Contains(position))
                        {
                            // Make this wall a far window
                            walls.Add(position, Walls.window_far_left);
                        }
                        else if (windows_side_near.Contains(position))
                        {
                            // Make this wall a near window
                            walls.Add(position, Walls.window_near_left);
                        }
                        else if (windows_middle.Contains(position))
                        {
                            // Make this wall a middle window
                            walls.Add(position, Walls.window_middle_left);
                        }
                        else
                        {
                            // Make this a left wall
                            walls.Add(position, Walls.wall_left);
                        }
                    }
                    // Top Wall
                    else if (position.Y == room_position.Y)
                    {
                        if (windows_single.Contains(position))
                        {
                            // Make this wall a single window
                            walls.Add(position, Walls.window_single_top);
                        }
                        else if (windows_side_far.Contains(position))
                        {
                            // Make this wall a far window
                            walls.Add(position, Walls.window_far_top);
                        }
                        else if (windows_side_near.Contains(position))
                        {
                            // Make this wall a near window
                            walls.Add(position, Walls.window_near_top);
                        }
                        else if (windows_middle.Contains(position))
                        {
                            // Make this wall a middle window
                            walls.Add(position, Walls.window_middle_top);
                        }
                        else
                        {
                            // Make this a top wall
                            walls.Add(position, Walls.wall_top);
                        }
                    }
                    // Right Wall
                    else if (position.X == room_position.X + room_width - 1)
                    {
                        if (windows_single.Contains(position))
                        {
                            // Make this wall a single window
                            walls.Add(position, Walls.window_single_right);
                        }
                        else if (windows_side_far.Contains(position))
                        {
                            // Make this wall a far window
                            walls.Add(position, Walls.window_far_right);
                        }
                        else if (windows_side_near.Contains(position))
                        {
                            // Make this wall a near window
                            walls.Add(position, Walls.window_near_right);
                        }
                        else if (windows_middle.Contains(position))
                        {
                            // Make this wall a middle window
                            walls.Add(position, Walls.window_middle_right);
                        }
                        else
                        {
                            // Make this a right wall
                            walls.Add(position, Walls.wall_right);
                        }
                    }
                    // Bottom Wall
                    else if (position.Y == room_position.Y + room_height - 1)
                    {
                        if (windows_single.Contains(position))
                        {
                            // Make this wall a single window
                            walls.Add(position, Walls.window_single_bottom);
                        }
                        else if (windows_side_far.Contains(position))
                        {
                            // Make this wall a far window
                            walls.Add(position, Walls.window_far_bottom);
                        }
                        else if (windows_side_near.Contains(position))
                        {
                            // Make this wall a near window
                            walls.Add(position, Walls.window_near_bottom);
                        }
                        else if (windows_middle.Contains(position))
                        {
                            // Make this wall a middle window
                            walls.Add(position, Walls.window_middle_bottom);
                        }
                        else
                        {
                            // Make this a bottom wall
                            walls.Add(position, Walls.wall_bottom);
                        }
                    }
                }
            }
        }
    }
}
