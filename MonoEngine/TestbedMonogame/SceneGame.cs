using MonoEngine;
using MonoEngine.Assets;
using MonoEngine.Game;
using MonoEngine.Physics;
using MonoEngine.Physics.Physics2D;
using MonoEngine.Render;
using MonoEngine.Shapes;
using MonoEngine.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace TestbedMonogame
{
    class SceneGame : Scene
    {
        enum Rooms { kitchen, dining, living, hall, bedroom, garage, walkway, road, bathroom, study };
        enum Walls { corner_top_right, corner_top_left, corner_bottom_left, corner_bottom_left_door_bottom, corner_bottom_right, wall_left, wall_bottom, wall_right, wall_top, window_single_left, window_single_bottom, window_single_right, window_single_top, window_far_top, window_far_left, window_far_bottom, window_far_right, window_near_top, window_near_left, window_near_bottom, window_near_right, window_middle_top, window_middle_left, window_middle_bottom, window_middle_right, corner_out_top_right, corner_out_top_left, corner_out_bottom_left, corner_out_bottom_right, door_left, door_top, door_right, door_bottom, hall_top_bottom, hall_left_right, hall_end_left, hall_end_top_door, hall_end_bottom_door, hall_left_right_corner_out_left, hall_left_right_corner_out_right };

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

        AABB wall_bounding_box_top_bottom = new AABB(1.0f, 0.1f);
        AABB wall_bounding_box_left_right = new AABB(0.1f, 1.0f);
        AABB door_bounding_box = new AABB(0.1f, 0.1f);


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

            Camera camera = Camera.Isometric("mainCamera", new Vector3(0, 0, 0f), 2, -10000.0f, 10000.0f);
            //CameraController controller = new CameraController("cameraController", PlayerIndex.One);
            //controller.camera = camera;
            //camera.AddComponent(controller);
            GameObjectManager.AddGameObject(camera);
            //GameObjectManager.AddGameObject(Camera.Perspective("mainCamera", new Vector3(1000, 2000, 1000f), Vector3.Zero, 0.1f, 10000.0f));

            floors = new Dictionary<Vector2, Rooms>();
            walls = new Dictionary<Vector2, Walls>();

            materials = new Dictionary<string, Material>();

            RenderTargetBatch batch = RenderManager.GetRenderTargetBatch("default") as RenderTargetBatch;
            batch.settings.sampler = SamplerState.PointClamp;

            materials.Add("floor_grass", new Material(new BasicEffect(GraphicsHelper.graphicsDevice)));
            BasicEffect effect = materials["floor_grass"].effect as BasicEffect;
            effect.TextureEnabled = true;
            effect.Texture = Resources.LoadAsset(new Sprite().GetType(), "floor_grass", this) as Sprite;

            materials.Add("wall_brick", new Material(new BasicEffect(GraphicsHelper.graphicsDevice)));
            effect = materials["wall_brick"].effect as BasicEffect;
            effect.TextureEnabled = true;
            effect.Texture = Resources.LoadAsset(new Sprite().GetType(), "wall_brick", this) as Sprite;

            // Kitchen -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            #region Kitchen
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

            // Special cases
            walls[new Vector2(12, 6)] = Walls.wall_top;
            walls[new Vector2(12, 8)] = Walls.wall_bottom;
            #endregion

            // Dining Room ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            #region Dining
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
                new Vector2(13,6),
                new Vector2(13,7),
                new Vector2(13,8),
                new Vector2(14,11),
                new Vector2(15,11),
                new Vector2(16,11),
            };

            // Windows single
            windows_single = new List<Vector2>()
            {
                new Vector2(14,6),
                new Vector2(16,6),
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

            // Special cases
            walls[new Vector2(13, 6)] = Walls.wall_top;
            walls[new Vector2(13, 8)] = Walls.corner_out_bottom_left;
            walls[new Vector2(14, 11)] = Walls.corner_out_bottom_left;
            walls[new Vector2(16, 11)] = Walls.corner_out_bottom_right;
            #endregion

            // Living Room ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            #region Living
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
            effect.Texture = Resources.LoadAsset(new Sprite().GetType(), "wall_hall", this) as Sprite;

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
                new Vector2(14,13),
                new Vector2(15,13),
                new Vector2(16,13),
            };

            // Windows single
            windows_single = new List<Vector2>()
            {
                
            };

            // Windows wide far side 
            windows_side_far = new List<Vector2>()
            {
                new Vector2(13,16),
                new Vector2(15,16),
            };

            // Windows wide near side
            windows_side_near = new List<Vector2>()
            {
                new Vector2(14,16),
                new Vector2(17,16),
            };

            // Windows wide middle part 
            windows_middle = new List<Vector2>()
            {

            };

            // Now make the room data
            Helper_InitialiseRoomData(Rooms.living, true);

            // Special cases
            walls[new Vector2(14, 13)] = Walls.corner_out_top_left;
            walls[new Vector2(16, 13)] = Walls.corner_out_top_right;
            #endregion

            // Hallways ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            #region Hallway
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
            Helper_InitialiseRoomData(Rooms.hall, false);

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
            Helper_InitialiseRoomData(Rooms.hall, false);
            #endregion

            // Bedroom -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            #region Bedroom
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
            effect.Texture = Resources.LoadAsset(new Sprite().GetType(), "wall_hall", this) as Sprite;

            // Setup: Rooms -----------------------------------------------------------------------------
            // The top left corner of the room
            room_position = new Vector2(19, 8);
            // Its dimensions
            room_width = 4;
            room_height = 4;

            // Doors
            doors = new List<Vector2>()
            {
                new Vector2(19,10),
            };

            // Empty walls (for open spaces, or floating walls)
            empty = new List<Vector2>()
            {

            };

            // Windows single
            windows_single = new List<Vector2>()
            {
                new Vector2(21,8),
                new Vector2(22,9),
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
            #endregion

            // Garage --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            #region Garage
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
            effect.Texture = Resources.LoadAsset(new Sprite().GetType(), "wall_hall", this) as Sprite;

            // Setup: Rooms -----------------------------------------------------------------------------
            // The top left corner of the room
            room_position = new Vector2(19, 12);
            // Its dimensions
            room_width = 4;
            room_height = 5;

            // Doors
            doors = new List<Vector2>()
            {
                new Vector2(19,15),
            };

            // Empty walls (for open spaces, or floating walls)
            empty = new List<Vector2>()
            {
                new Vector2(19,16),
                new Vector2(20,16),
                new Vector2(21,16),
                new Vector2(22,16),
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

            // Special cases
            walls[new Vector2(19, 16)] = Walls.wall_left;
            walls[new Vector2(22, 16)] = Walls.wall_right;
            #endregion

            // Walkway -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            #region Walkway
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
            #endregion

            // Road ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            #region Road
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
            #endregion

            // Bathroom ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            #region Bathroom
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
            materials.Add("wall_bath", new Material(new BasicEffect(GraphicsHelper.graphicsDevice)));
            effect = materials["wall_bath"].effect as BasicEffect;
            effect.TextureEnabled = true;
            // Load the wall texture for it
            effect.Texture = Resources.LoadAsset(new Sprite().GetType(), "wall_bath", this) as Sprite;

            // Setup: Rooms -----------------------------------------------------------------------------
            // The top left corner of the room
            room_position = new Vector2(9, 9);
            // Its dimensions
            room_width = 4;
            room_height = 3;

            // Doors
            doors = new List<Vector2>()
            {
                new Vector2(12,11),
            };

            // Empty walls (for open spaces, or floating walls)
            empty = new List<Vector2>()
            {

            };

            // Windows single
            windows_single = new List<Vector2>()
            {
                new Vector2(9,10),
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
            #endregion

            // Study ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            #region Study
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
            materials.Add("wall_study", new Material(new BasicEffect(GraphicsHelper.graphicsDevice)));
            effect = materials["wall_study"].effect as BasicEffect;
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
                new Vector2(10,13),
            };

            // Empty walls (for open spaces, or floating walls)
            empty = new List<Vector2>()
            {

            };

            // Windows single
            windows_single = new List<Vector2>()
            {
                new Vector2(10,16),
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
            #endregion

            // Exterior ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            #region Exterior
            // Special case

            Vector2 posi = new Vector2(8, 5);
            int widi = 16;
            int hidi = 13;
            
            for (int x = 0; x < widi; x++)
            {
                for (int y = 0; y < hidi; y++)
                {
                    Vector2 position = new Vector2(x, y) + posi;
                    // Top Left Corner Wall
                    if (position.X == posi.X && position.Y == posi.Y)
                    {
                        walls.Add(position, Walls.corner_out_bottom_right);
                    }
                    // Bottom Left Corner Wall
                    else if (position.X == posi.X && position.Y == posi.Y + hidi - 1)
                    {
                        walls.Add(position, Walls.corner_out_top_right);
                    }
                    // Bottom Right Corner Wall
                    else if (position.X == posi.X + widi - 1 && position.Y == posi.Y + hidi - 1)
                    {
                        walls.Add(position, Walls.corner_out_top_left);
                    }
                    // Top Right Corner Wall
                    else if (position.X == posi.X + widi - 1 && position.Y == posi.Y)
                    {
                        walls.Add(position, Walls.corner_out_bottom_left);
                    }
                    // Now all of the below are special, they can have windows and the like
                    else
                    {
                        // Walls can be windows so all of them need to be in that context
                        // right Wall
                        if (position.X == posi.X)
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
                            else if (doors.Contains(position))
                            {
                                // Make this wall a door
                                walls.Add(position, Walls.door_right);
                            }
                            else
                            {
                                // Make this a right wall
                                walls.Add(position, Walls.wall_right);
                            }
                        }
                        // bottom Wall
                        else if (position.Y == posi.Y)
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
                            else if (doors.Contains(position))
                            {
                                // Make this wall a door
                                walls.Add(position, Walls.door_bottom);
                            }
                            else
                            {
                                // Make this a bottom wall
                                walls.Add(position, Walls.wall_bottom);
                            }
                        }
                        // left Wall
                        else if (position.X == posi.X + widi - 1)
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
                            else if (doors.Contains(position))
                            {
                                // Make this wall a door
                                walls.Add(position, Walls.door_left);
                            }
                            else
                            {
                                // Make this a left wall
                                walls.Add(position, Walls.wall_left);
                            }
                        }
                        // top Wall
                        else if (position.Y == posi.Y + hidi - 1)
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
                            else if (doors.Contains(position))
                            {
                                // Make this wall a door
                                walls.Add(position, Walls.door_top);
                            }
                            else
                            {
                                // Make this a top wall
                                walls.Add(position, Walls.wall_top);
                            }
                        }
                    }
                }
            }
            
            walls.Remove(new Vector2(19, 5));
            walls.Remove(new Vector2(20, 5));
            walls.Remove(new Vector2(21, 5));
            walls.Remove(new Vector2(22, 5));
            walls.Remove(new Vector2(23, 5));
            walls.Remove(new Vector2(23, 6));

            walls[new Vector2(18, 5)] = Walls.corner_out_bottom_left;
            walls[new Vector2(23, 7)] = Walls.corner_out_bottom_left;

            walls.Add(new Vector2(18, 6), Walls.wall_left);
            walls.Add(new Vector2(18, 7), Walls.corner_bottom_left_door_bottom);

            walls.Add(new Vector2(19, 7), Walls.wall_bottom);
            walls.Add(new Vector2(20, 7), Walls.wall_bottom);
            walls.Add(new Vector2(21, 7), Walls.wall_bottom);
            walls.Add(new Vector2(22, 7), Walls.wall_bottom);

            posi = new Vector2(10, 12);
            widi = 8;
            hidi = 1;
            for (int x = 0; x < widi; x++)
            {
                for (int y = 0; y < hidi; y++)
                {
                    Vector2 position = posi + new Vector2(x, y);
                    walls.Add(position, Walls.hall_top_bottom);
                }
            }

            posi = new Vector2(18, 9);
            widi = 1;
            hidi = 7;
            for (int x = 0; x < widi; x++)
            {
                for (int y = 0; y < hidi; y++)
                {
                    Vector2 position = posi + new Vector2(x, y);
                    walls.Add(position, Walls.hall_left_right);
                }
            }

            walls.Add(new Vector2(9, 12), Walls.hall_end_left);
            walls.Add(new Vector2(18, 8), Walls.hall_end_top_door);
            walls.Add(new Vector2(18, 16), Walls.hall_end_bottom_door);
            
            walls.Remove(new Vector2(15, 12));
            walls[new Vector2(14, 12)] = Walls.hall_left_right_corner_out_left;
            walls[new Vector2(16, 12)] = Walls.hall_left_right_corner_out_right;

            walls[new Vector2(18, 17)] = Walls.door_top;

            #endregion

            // World generation ----------------------------------------------------------------------------------------------------------------------------------------------------------------------
            #region World Generation
            // 

            int width = 32;
            int height = 32;
            float width_h = width / 2.0f;
            float height_h = height / 2.0f;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Vector3 position = new Vector3((x - width_h) / 2.0f, 0, (y - height_h) / 2.0f);
                    Vector2 pos = new Vector2(x, y);

                    // Walls
                    ModelRenderer wall = null;

                    PhysicsBody2D body = null;
                    
                    if (walls.ContainsKey(pos))
                    {
                        switch (walls[pos])
                        {
                            case Walls.corner_bottom_left:
                                // |
                                // |
                                // |_____
                                wall = ModelRenderer.MakeModelRenderer("BasicWallCorner");
                                wall.Name = "wall" + x.ToString() + "," + y.ToString();
                                wall.transform.Rotate(Vector3.Up, MathHelper.Pi);
                                wall.transform.Position = position;

                                // Body Definitions
                                body = new PhysicsBody2D(wall, "bounding_box_left", wall_bounding_box_left_right, new PhysicsMaterial(), PhysicsEngine.BodyType.STATIC);
                                body.transform.Position = new Vector3(-0.45f, 0.0f, 0.0f);
                                PhysicsEngine.AddPhysicsBody(body);
                                wall.AddComponent(body);

                                body = new PhysicsBody2D(wall, "bounding_box_bottom", wall_bounding_box_top_bottom, new PhysicsMaterial(), PhysicsEngine.BodyType.STATIC);
                                body.transform.Position = new Vector3(0.0f, 0.0f, 0.45f);
                                PhysicsEngine.AddPhysicsBody(body);
                                wall.AddComponent(body);
                                break;
                            case Walls.corner_bottom_right:
                                //      |
                                //      |
                                // _____|
                                wall = ModelRenderer.MakeModelRenderer("BasicWallCorner");
                                wall.Name = "wall" + x.ToString() + "," + y.ToString();
                                wall.transform.Rotate(Vector3.Up, MathHelper.Pi + MathHelper.PiOver2);
                                wall.transform.Position = position;

                                // Body Definitions
                                body = new PhysicsBody2D(wall, "bounding_box_right", wall_bounding_box_left_right, new PhysicsMaterial(), PhysicsEngine.BodyType.STATIC);
                                body.transform.Position = new Vector3(0.45f, 0.0f, 0.0f);
                                PhysicsEngine.AddPhysicsBody(body);
                                wall.AddComponent(body);

                                body = new PhysicsBody2D(wall, "bounding_box_bottom", wall_bounding_box_top_bottom, new PhysicsMaterial(), PhysicsEngine.BodyType.STATIC);
                                body.transform.Position = new Vector3(0.0f, 0.0f, 0.45f);
                                PhysicsEngine.AddPhysicsBody(body);
                                wall.AddComponent(body);
                                break;
                            case Walls.corner_top_left:
                                //  ______
                                // |
                                // |
                                // |
                                wall = ModelRenderer.MakeModelRenderer("BasicWallCorner");
                                wall.Name = "wall" + x.ToString() + "," + y.ToString();
                                wall.transform.Rotate(Vector3.Up, MathHelper.PiOver2);
                                wall.transform.Position = position;

                                // Body Definitions
                                body = new PhysicsBody2D(wall, "bounding_box_left", wall_bounding_box_left_right, new PhysicsMaterial(), PhysicsEngine.BodyType.STATIC);
                                body.transform.Position = new Vector3(-0.45f, 0.0f, 0.0f);
                                PhysicsEngine.AddPhysicsBody(body);
                                wall.AddComponent(body);

                                body = new PhysicsBody2D(wall, "bounding_box_top", wall_bounding_box_top_bottom, new PhysicsMaterial(), PhysicsEngine.BodyType.STATIC);
                                body.transform.Position = new Vector3(0.0f, 0.0f, -0.45f);
                                PhysicsEngine.AddPhysicsBody(body);
                                wall.AddComponent(body);
                                break;
                            case Walls.corner_top_right:
                                //  ______
                                //        |
                                //        |
                                //        |
                                wall = ModelRenderer.MakeModelRenderer("BasicWallCorner");
                                wall.Name = "wall" + x.ToString() + "," + y.ToString();
                                wall.transform.Position = position;

                                // Body Definitions
                                body = new PhysicsBody2D(wall, "bounding_box_right", wall_bounding_box_left_right, new PhysicsMaterial(), PhysicsEngine.BodyType.STATIC);
                                body.transform.Position = new Vector3(0.45f, 0.0f, 0.0f);
                                PhysicsEngine.AddPhysicsBody(body);
                                wall.AddComponent(body);

                                body = new PhysicsBody2D(wall, "bounding_box_top", wall_bounding_box_top_bottom, new PhysicsMaterial(), PhysicsEngine.BodyType.STATIC);
                                body.transform.Position = new Vector3(0.0f, 0.0f, -0.45f);
                                PhysicsEngine.AddPhysicsBody(body);
                                wall.AddComponent(body);
                                break;
                            case Walls.corner_bottom_left_door_bottom:
                                wall = ModelRenderer.MakeModelRenderer("DoorFrame");
                                wall.Name = "wall" + x.ToString() + "," + y.ToString();
                                wall.transform.Rotate(Vector3.Up, MathHelper.Pi);
                                wall.transform.Position = position;
                                wall.material = materials["wall_hall"];

                                // Body Definitions
                                body = new PhysicsBody2D(wall, "bounding_box_bottom_left", door_bounding_box, new PhysicsMaterial(), PhysicsEngine.BodyType.STATIC);
                                body.transform.Position = new Vector3(-0.45f, 0.0f, 0.45f);
                                PhysicsEngine.AddPhysicsBody(body);
                                wall.AddComponent(body);

                                body = new PhysicsBody2D(wall, "bounding_box_bottom_right", door_bounding_box, new PhysicsMaterial(), PhysicsEngine.BodyType.STATIC);
                                body.transform.Position = new Vector3(0.45f, 0.0f, 0.45f);
                                PhysicsEngine.AddPhysicsBody(body);
                                wall.AddComponent(body);

                                wall = ModelRenderer.MakeModelRenderer("BasicWallLeftSideShort");
                                wall.Name = "wall" + x.ToString() + "," + y.ToString();
                                wall.transform.Rotate(Vector3.Up, MathHelper.PiOver2);
                                wall.transform.Position = position;

                                // Body Definitions
                                body = new PhysicsBody2D(wall, "bounding_box_left", wall_bounding_box_left_right, new PhysicsMaterial(), PhysicsEngine.BodyType.STATIC);
                                body.transform.Position = new Vector3(-0.45f, 0.0f, 0.0f);
                                PhysicsEngine.AddPhysicsBody(body);
                                wall.AddComponent(body);
                                break;
                            case Walls.wall_bottom:
                                wall = ModelRenderer.MakeModelRenderer("BasicWall");
                                wall.Name = "wall" + x.ToString() + "," + y.ToString();
                                wall.transform.Rotate(Vector3.Up, MathHelper.Pi);
                                wall.transform.Position = position;

                                // Body Definitions
                                body = new PhysicsBody2D(wall, "bounding_box_bottom", wall_bounding_box_top_bottom, new PhysicsMaterial(), PhysicsEngine.BodyType.STATIC);
                                body.transform.Position = new Vector3(0.0f, 0.0f, 0.45f);
                                PhysicsEngine.AddPhysicsBody(body);
                                wall.AddComponent(body);
                                break;
                            case Walls.wall_left:
                                wall = ModelRenderer.MakeModelRenderer("BasicWall");
                                wall.Name = "wall" + x.ToString() + "," + y.ToString();
                                wall.transform.Rotate(Vector3.Up, MathHelper.PiOver2);
                                wall.transform.Position = position;

                                // Body Definitions
                                body = new PhysicsBody2D(wall, "bounding_box_left", wall_bounding_box_left_right, new PhysicsMaterial(), PhysicsEngine.BodyType.STATIC);
                                body.transform.Position = new Vector3(-0.45f, 0.0f, 0.0f);
                                PhysicsEngine.AddPhysicsBody(body);
                                wall.AddComponent(body);
                                break;
                            case Walls.wall_right:
                                wall = ModelRenderer.MakeModelRenderer("BasicWall");
                                wall.Name = "wall" + x.ToString() + "," + y.ToString();
                                wall.transform.Rotate(Vector3.Up, MathHelper.Pi + MathHelper.PiOver2);
                                wall.transform.Position = position;

                                // Body Definitions
                                body = new PhysicsBody2D(wall, "bounding_box_right", wall_bounding_box_left_right, new PhysicsMaterial(), PhysicsEngine.BodyType.STATIC);
                                body.transform.Position = new Vector3(0.45f, 0.0f, 0.0f);
                                PhysicsEngine.AddPhysicsBody(body);
                                wall.AddComponent(body);
                                break;
                            case Walls.wall_top:
                                wall = ModelRenderer.MakeModelRenderer("BasicWall");
                                wall.Name = "wall" + x.ToString() + "," + y.ToString();
                                wall.transform.Position = position;

                                // Body Definitions
                                body = new PhysicsBody2D(wall, "bounding_box_top", wall_bounding_box_top_bottom, new PhysicsMaterial(), PhysicsEngine.BodyType.STATIC);
                                body.transform.Position = new Vector3(0.0f, 0.0f, -0.45f);
                                PhysicsEngine.AddPhysicsBody(body);
                                wall.AddComponent(body);
                                break;
                            case Walls.hall_top_bottom:
                                wall = ModelRenderer.MakeModelRenderer("BasicWall");
                                wall.Name = "wall" + x.ToString() + "," + y.ToString();
                                wall.transform.Position = position;
                                wall.material = materials["wall_hall"];

                                // Body Definitions
                                body = new PhysicsBody2D(wall, "bounding_box_top", wall_bounding_box_top_bottom, new PhysicsMaterial(), PhysicsEngine.BodyType.STATIC);
                                body.transform.Position = new Vector3(0.0f, 0.0f, -0.45f);
                                PhysicsEngine.AddPhysicsBody(body);
                                wall.AddComponent(body);

                                wall = ModelRenderer.MakeModelRenderer("BasicWall");
                                wall.Name = "wall" + x.ToString() + "," + y.ToString();
                                wall.transform.Rotate(Vector3.Up, MathHelper.Pi);
                                wall.transform.Position = position;

                                // Body Definitions
                                body = new PhysicsBody2D(wall, "bounding_box_bottom", wall_bounding_box_top_bottom, new PhysicsMaterial(), PhysicsEngine.BodyType.STATIC);
                                body.transform.Position = new Vector3(0.0f, 0.0f, 0.45f);
                                PhysicsEngine.AddPhysicsBody(body);
                                wall.AddComponent(body);
                                break;
                            case Walls.hall_end_top_door:
                                wall = ModelRenderer.MakeModelRenderer("DoorFrame");
                                wall.Name = "wall" + x.ToString() + "," + y.ToString();
                                wall.transform.Position = position;
                                wall.material = materials["wall_hall"];

                                // Body Definitions
                                body = new PhysicsBody2D(wall, "bounding_box_top_left", door_bounding_box, new PhysicsMaterial(), PhysicsEngine.BodyType.STATIC);
                                body.transform.Position = new Vector3(-0.45f, 0.0f, -0.45f);
                                PhysicsEngine.AddPhysicsBody(body);
                                wall.AddComponent(body);

                                body = new PhysicsBody2D(wall, "bounding_box_top_right", door_bounding_box, new PhysicsMaterial(), PhysicsEngine.BodyType.STATIC);
                                body.transform.Position = new Vector3(0.45f, 0.0f, -0.45f);
                                PhysicsEngine.AddPhysicsBody(body);
                                wall.AddComponent(body);

                                wall = ModelRenderer.MakeModelRenderer("BasicWall");
                                wall.Name = "wall" + x.ToString() + "," + y.ToString();
                                wall.transform.Rotate(Vector3.Up, MathHelper.PiOver2);
                                wall.transform.Position = position;
                                wall.material = materials["wall_hall"];

                                // Body Definitions
                                body = new PhysicsBody2D(wall, "bounding_box_left", wall_bounding_box_left_right, new PhysicsMaterial(), PhysicsEngine.BodyType.STATIC);
                                body.transform.Position = new Vector3(-0.45f, 0.0f, 0.0f);
                                PhysicsEngine.AddPhysicsBody(body);
                                wall.AddComponent(body);

                                wall = ModelRenderer.MakeModelRenderer("BasicWall");
                                wall.Name = "wall" + x.ToString() + "," + y.ToString();
                                wall.transform.Rotate(Vector3.Up, MathHelper.Pi + MathHelper.PiOver2);
                                wall.transform.Position = position;

                                // Body Definitions
                                body = new PhysicsBody2D(wall, "bounding_box_right", wall_bounding_box_left_right, new PhysicsMaterial(), PhysicsEngine.BodyType.STATIC);
                                body.transform.Position = new Vector3(0.45f, 0.0f, 0.0f);
                                PhysicsEngine.AddPhysicsBody(body);
                                wall.AddComponent(body);
                                break;
                            case Walls.hall_end_bottom_door:
                                wall = ModelRenderer.MakeModelRenderer("DoorFrame");
                                wall.Name = "wall" + x.ToString() + "," + y.ToString();
                                wall.transform.Rotate(Vector3.Up, MathHelper.Pi);
                                wall.transform.Position = position;
                                wall.material = materials["wall_hall"];

                                // Body Definitions
                                body = new PhysicsBody2D(wall, "bounding_box_top_left", door_bounding_box, new PhysicsMaterial(), PhysicsEngine.BodyType.STATIC);
                                body.transform.Position = new Vector3(-0.45f, 0.0f, 0.45f);
                                PhysicsEngine.AddPhysicsBody(body);
                                wall.AddComponent(body);

                                body = new PhysicsBody2D(wall, "bounding_box_top_right", door_bounding_box, new PhysicsMaterial(), PhysicsEngine.BodyType.STATIC);
                                body.transform.Position = new Vector3(0.45f, 0.0f, 0.45f);
                                PhysicsEngine.AddPhysicsBody(body);
                                wall.AddComponent(body);

                                wall = ModelRenderer.MakeModelRenderer("BasicWall");
                                wall.Name = "wall" + x.ToString() + "," + y.ToString();
                                wall.transform.Rotate(Vector3.Up, MathHelper.PiOver2);
                                wall.transform.Position = position;
                                wall.material = materials["wall_hall"];

                                // Body Definitions
                                body = new PhysicsBody2D(wall, "bounding_box_left", wall_bounding_box_left_right, new PhysicsMaterial(), PhysicsEngine.BodyType.STATIC);
                                body.transform.Position = new Vector3(-0.45f, 0.0f, 0.0f);
                                PhysicsEngine.AddPhysicsBody(body);
                                wall.AddComponent(body);

                                wall = ModelRenderer.MakeModelRenderer("BasicWall");
                                wall.Name = "wall" + x.ToString() + "," + y.ToString();
                                wall.transform.Rotate(Vector3.Up, MathHelper.Pi + MathHelper.PiOver2);
                                wall.transform.Position = position;

                                // Body Definitions
                                body = new PhysicsBody2D(wall, "bounding_box_right", wall_bounding_box_left_right, new PhysicsMaterial(), PhysicsEngine.BodyType.STATIC);
                                body.transform.Position = new Vector3(0.45f, 0.0f, 0.0f);
                                PhysicsEngine.AddPhysicsBody(body);
                                wall.AddComponent(body);
                                break;
                            case Walls.hall_end_left:
                                wall = ModelRenderer.MakeModelRenderer("BasicWall");
                                wall.Name = "wall" + x.ToString() + "," + y.ToString();
                                wall.transform.Rotate(Vector3.Up, MathHelper.PiOver2);
                                wall.transform.Position = position;
                                wall.material = materials["wall_hall"];

                                // Body Definitions
                                body = new PhysicsBody2D(wall, "bounding_box_left", wall_bounding_box_left_right, new PhysicsMaterial(), PhysicsEngine.BodyType.STATIC);
                                body.transform.Position = new Vector3(-0.45f, 0.0f, 0.0f);
                                PhysicsEngine.AddPhysicsBody(body);
                                wall.AddComponent(body);

                                wall = ModelRenderer.MakeModelRenderer("BasicWallLeftSideShort");
                                wall.Name = "wall" + x.ToString() + "," + y.ToString();
                                wall.transform.Position = position;
                                wall.material = materials["wall_hall"];

                                // Body Definitions
                                body = new PhysicsBody2D(wall, "bounding_box_top", wall_bounding_box_top_bottom, new PhysicsMaterial(), PhysicsEngine.BodyType.STATIC);
                                body.transform.Position = new Vector3(0.0f, 0.0f, -0.45f);
                                PhysicsEngine.AddPhysicsBody(body);
                                wall.AddComponent(body);

                                wall = ModelRenderer.MakeModelRenderer("BasicWallRightSideShort");
                                wall.Name = "wall" + x.ToString() + "," + y.ToString();
                                wall.transform.Rotate(Vector3.Up, MathHelper.Pi);
                                wall.transform.Position = position;

                                // Body Definitions
                                body = new PhysicsBody2D(wall, "bounding_box_bottom", wall_bounding_box_top_bottom, new PhysicsMaterial(), PhysicsEngine.BodyType.STATIC);
                                body.transform.Position = new Vector3(0.0f, 0.0f, 0.45f);
                                PhysicsEngine.AddPhysicsBody(body);
                                wall.AddComponent(body);
                                break;
                            case Walls.hall_left_right:
                                wall = ModelRenderer.MakeModelRenderer("BasicWall");
                                wall.Name = "wall" + x.ToString() + "," + y.ToString();
                                wall.transform.Rotate(Vector3.Up, MathHelper.PiOver2);
                                wall.transform.Position = position;
                                wall.material = materials["wall_hall"];

                                // Body Definitions
                                body = new PhysicsBody2D(wall, "bounding_box_left", wall_bounding_box_left_right, new PhysicsMaterial(), PhysicsEngine.BodyType.STATIC);
                                body.transform.Position = new Vector3(-0.45f, 0.0f, 0.0f);
                                PhysicsEngine.AddPhysicsBody(body);
                                wall.AddComponent(body);

                                wall = ModelRenderer.MakeModelRenderer("BasicWall");
                                wall.Name = "wall" + x.ToString() + "," + y.ToString();
                                wall.transform.Rotate(Vector3.Up, MathHelper.Pi + MathHelper.PiOver2);
                                wall.transform.Position = position;

                                // Body Definitions
                                body = new PhysicsBody2D(wall, "bounding_box_right", wall_bounding_box_left_right, new PhysicsMaterial(), PhysicsEngine.BodyType.STATIC);
                                body.transform.Position = new Vector3(0.45f, 0.0f, 0.0f);
                                PhysicsEngine.AddPhysicsBody(body);
                                wall.AddComponent(body);
                                break;
                            case Walls.window_far_bottom:
                                wall = ModelRenderer.MakeModelRenderer("BasicDoubleWindowRight");
                                wall.Name = "wall" + x.ToString() + "," + y.ToString();
                                wall.transform.Rotate(Vector3.Up, MathHelper.Pi);
                                wall.transform.Position = position;

                                // Body Definitions
                                body = new PhysicsBody2D(wall, "bounding_box_bottom", wall_bounding_box_top_bottom, new PhysicsMaterial(), PhysicsEngine.BodyType.STATIC);
                                body.transform.Position = new Vector3(0.0f, 0.0f, 0.45f);
                                PhysicsEngine.AddPhysicsBody(body);
                                wall.AddComponent(body);
                                break;
                            case Walls.window_far_left:
                                wall = ModelRenderer.MakeModelRenderer("BasicDoubleWindowRight");
                                wall.Name = "wall" + x.ToString() + "," + y.ToString();
                                wall.transform.Rotate(Vector3.Up, MathHelper.PiOver2);
                                wall.transform.Position = position;

                                // Body Definitions
                                body = new PhysicsBody2D(wall, "bounding_box_left", wall_bounding_box_left_right, new PhysicsMaterial(), PhysicsEngine.BodyType.STATIC);
                                body.transform.Position = new Vector3(-0.45f, 0.0f, 0.0f);
                                PhysicsEngine.AddPhysicsBody(body);
                                wall.AddComponent(body);
                                break;
                            case Walls.window_far_right:
                                wall = ModelRenderer.MakeModelRenderer("BasicDoubleWindowRight");
                                wall.Name = "wall" + x.ToString() + "," + y.ToString();
                                wall.transform.Rotate(Vector3.Up, MathHelper.Pi + MathHelper.PiOver2);
                                wall.transform.Position = position;

                                // Body Definitions
                                body = new PhysicsBody2D(wall, "bounding_box_right", wall_bounding_box_left_right, new PhysicsMaterial(), PhysicsEngine.BodyType.STATIC);
                                body.transform.Position = new Vector3(0.45f, 0.0f, 0.0f);
                                PhysicsEngine.AddPhysicsBody(body);
                                wall.AddComponent(body);
                                break;
                            case Walls.window_far_top:
                                wall = ModelRenderer.MakeModelRenderer("BasicDoubleWindowRight");
                                wall.Name = "wall" + x.ToString() + "," + y.ToString();
                                wall.transform.Position = position;

                                // Body Definitions
                                body = new PhysicsBody2D(wall, "bounding_box_top", wall_bounding_box_top_bottom, new PhysicsMaterial(), PhysicsEngine.BodyType.STATIC);
                                body.transform.Position = new Vector3(0.0f, 0.0f, -0.45f);
                                PhysicsEngine.AddPhysicsBody(body);
                                wall.AddComponent(body);
                                break;
                            case Walls.window_middle_bottom:
                                wall = ModelRenderer.MakeModelRenderer("BasicWall");
                                wall.Name = "wall" + x.ToString() + "," + y.ToString();
                                wall.transform.Rotate(Vector3.Up, MathHelper.PiOver2);
                                wall.transform.Position = position;

                                // Body Definitions
                                body = new PhysicsBody2D(wall, "bounding_box_bottom", wall_bounding_box_top_bottom, new PhysicsMaterial(), PhysicsEngine.BodyType.STATIC);
                                body.transform.Position = new Vector3(0.0f, 0.0f, 0.45f);
                                PhysicsEngine.AddPhysicsBody(body);
                                wall.AddComponent(body);
                                break;
                            case Walls.window_middle_left:
                                wall = ModelRenderer.MakeModelRenderer("BasicWall");
                                wall.Name = "wall" + x.ToString() + "," + y.ToString();
                                wall.transform.Rotate(Vector3.Up, MathHelper.PiOver2);
                                wall.transform.Position = position;

                                // Body Definitions
                                body = new PhysicsBody2D(wall, "bounding_box_left", wall_bounding_box_left_right, new PhysicsMaterial(), PhysicsEngine.BodyType.STATIC);
                                body.transform.Position = new Vector3(-0.45f, 0.0f, 0.0f);
                                PhysicsEngine.AddPhysicsBody(body);
                                wall.AddComponent(body);
                                break;
                            case Walls.window_middle_right:
                                wall = ModelRenderer.MakeModelRenderer("BasicWall");
                                wall.Name = "wall" + x.ToString() + "," + y.ToString();
                                wall.transform.Rotate(Vector3.Up, MathHelper.PiOver2);
                                wall.transform.Position = position;

                                // Body Definitions
                                body = new PhysicsBody2D(wall, "bounding_box_right", wall_bounding_box_left_right, new PhysicsMaterial(), PhysicsEngine.BodyType.STATIC);
                                body.transform.Position = new Vector3(0.45f, 0.0f, 0.0f);
                                PhysicsEngine.AddPhysicsBody(body);
                                wall.AddComponent(body);
                                break;
                            case Walls.window_middle_top:
                                wall = ModelRenderer.MakeModelRenderer("BasicWall");
                                wall.Name = "wall" + x.ToString() + "," + y.ToString();
                                wall.transform.Position = position;

                                // Body Definitions
                                body = new PhysicsBody2D(wall, "bounding_box_top", wall_bounding_box_top_bottom, new PhysicsMaterial(), PhysicsEngine.BodyType.STATIC);
                                body.transform.Position = new Vector3(0.0f, 0.0f, -0.45f);
                                PhysicsEngine.AddPhysicsBody(body);
                                wall.AddComponent(body);
                                break;
                            case Walls.window_near_bottom:
                                wall = ModelRenderer.MakeModelRenderer("BasicDoubleWindowLeft");
                                wall.Name = "wall" + x.ToString() + "," + y.ToString();
                                wall.transform.Rotate(Vector3.Up, MathHelper.Pi);
                                wall.transform.Position = position;

                                // Body Definitions
                                body = new PhysicsBody2D(wall, "bounding_box_bottom", wall_bounding_box_top_bottom, new PhysicsMaterial(), PhysicsEngine.BodyType.STATIC);
                                body.transform.Position = new Vector3(0.0f, 0.0f, 0.45f);
                                PhysicsEngine.AddPhysicsBody(body);
                                wall.AddComponent(body);
                                break;
                            case Walls.window_near_left:
                                wall = ModelRenderer.MakeModelRenderer("BasicDoubleWindowLeft");
                                wall.Name = "wall" + x.ToString() + "," + y.ToString();
                                wall.transform.Rotate(Vector3.Up, MathHelper.PiOver2);
                                wall.transform.Position = position;

                                // Body Definitions
                                body = new PhysicsBody2D(wall, "bounding_box_left", wall_bounding_box_left_right, new PhysicsMaterial(), PhysicsEngine.BodyType.STATIC);
                                body.transform.Position = new Vector3(-0.45f, 0.0f, 0.0f);
                                PhysicsEngine.AddPhysicsBody(body);
                                wall.AddComponent(body);
                                break;
                            case Walls.window_near_right:
                                wall = ModelRenderer.MakeModelRenderer("BasicDoubleWindowLeft");
                                wall.Name = "wall" + x.ToString() + "," + y.ToString();
                                wall.transform.Rotate(Vector3.Up, MathHelper.Pi + MathHelper.PiOver2);
                                wall.transform.Position = position;

                                // Body Definitions
                                body = new PhysicsBody2D(wall, "bounding_box_right", wall_bounding_box_left_right, new PhysicsMaterial(), PhysicsEngine.BodyType.STATIC);
                                body.transform.Position = new Vector3(0.45f, 0.0f, 0.0f);
                                PhysicsEngine.AddPhysicsBody(body);
                                wall.AddComponent(body);
                                break;
                            case Walls.window_near_top:
                                wall = ModelRenderer.MakeModelRenderer("BasicDoubleWindowLeft");
                                wall.Name = "wall" + x.ToString() + "," + y.ToString();
                                wall.transform.Position = position;

                                // Body Definitions
                                body = new PhysicsBody2D(wall, "bounding_box_top", wall_bounding_box_top_bottom, new PhysicsMaterial(), PhysicsEngine.BodyType.STATIC);
                                body.transform.Position = new Vector3(0.0f, 0.0f, -0.45f);
                                PhysicsEngine.AddPhysicsBody(body);
                                wall.AddComponent(body);
                                break;
                            case Walls.window_single_bottom:
                                wall = ModelRenderer.MakeModelRenderer("BasicSingleWindow");
                                wall.Name = "wall" + x.ToString() + "," + y.ToString();
                                wall.transform.Rotate(Vector3.Up, MathHelper.Pi);
                                wall.transform.Position = position;

                                // Body Definitions
                                body = new PhysicsBody2D(wall, "bounding_box_bottom", wall_bounding_box_top_bottom, new PhysicsMaterial(), PhysicsEngine.BodyType.STATIC);
                                body.transform.Position = new Vector3(0.0f, 0.0f, 0.45f);
                                PhysicsEngine.AddPhysicsBody(body);
                                wall.AddComponent(body);
                                break;
                            case Walls.window_single_left:
                                wall = ModelRenderer.MakeModelRenderer("BasicSingleWindow");
                                wall.Name = "wall" + x.ToString() + "," + y.ToString();
                                wall.transform.Rotate(Vector3.Up, MathHelper.PiOver2);
                                wall.transform.Position = position;

                                // Body Definitions
                                body = new PhysicsBody2D(wall, "bounding_box_left", wall_bounding_box_left_right, new PhysicsMaterial(), PhysicsEngine.BodyType.STATIC);
                                body.transform.Position = new Vector3(-0.45f, 0.0f, 0.0f);
                                PhysicsEngine.AddPhysicsBody(body);
                                wall.AddComponent(body);
                                break;
                            case Walls.window_single_right:
                                wall = ModelRenderer.MakeModelRenderer("BasicSingleWindow");
                                wall.Name = "wall" + x.ToString() + "," + y.ToString();
                                wall.transform.Rotate(Vector3.Up, MathHelper.Pi + MathHelper.PiOver2);
                                wall.transform.Position = position;

                                // Body Definitions
                                body = new PhysicsBody2D(wall, "bounding_box_right", wall_bounding_box_left_right, new PhysicsMaterial(), PhysicsEngine.BodyType.STATIC);
                                body.transform.Position = new Vector3(0.45f, 0.0f, 0.0f);
                                PhysicsEngine.AddPhysicsBody(body);
                                wall.AddComponent(body);
                                break;
                            case Walls.window_single_top:
                                wall = ModelRenderer.MakeModelRenderer("BasicSingleWindow");
                                wall.Name = "wall" + x.ToString() + "," + y.ToString();
                                wall.transform.Position = position;

                                // Body Definitions
                                body = new PhysicsBody2D(wall, "bounding_box_top", wall_bounding_box_top_bottom, new PhysicsMaterial(), PhysicsEngine.BodyType.STATIC);
                                body.transform.Position = new Vector3(0.0f, 0.0f, -0.45f);
                                PhysicsEngine.AddPhysicsBody(body);
                                wall.AddComponent(body);
                                break;
                            case Walls.corner_out_bottom_left:
                                wall = ModelRenderer.MakeModelRenderer("FrontBasicWallCorner");
                                wall.Name = "wall" + x.ToString() + "," + y.ToString();
                                wall.transform.Rotate(Vector3.Up, MathHelper.Pi + MathHelper.PiOver2);
                                wall.transform.Position = position;

                                // Body Definitions
                                body = new PhysicsBody2D(wall, "bounding_box_right", wall_bounding_box_left_right, new PhysicsMaterial(), PhysicsEngine.BodyType.STATIC);
                                body.transform.Position = new Vector3(-0.45f, 0.0f, 0.45f);
                                PhysicsEngine.AddPhysicsBody(body);
                                wall.AddComponent(body);
                                break;
                            case Walls.corner_out_bottom_right:
                                wall = ModelRenderer.MakeModelRenderer("FrontBasicWallCorner");
                                wall.Name = "wall" + x.ToString() + "," + y.ToString();
                                //wall.transform.Rotate(Vector3.Up, MathHelper.PiOver2);
                                wall.transform.Position = position;

                                // Body Definitions
                                body = new PhysicsBody2D(wall, "bounding_box_right", wall_bounding_box_left_right, new PhysicsMaterial(), PhysicsEngine.BodyType.STATIC);
                                body.transform.Position = new Vector3(0.45f, 0.0f, 0.45f);
                                PhysicsEngine.AddPhysicsBody(body);
                                wall.AddComponent(body);
                                break;
                            case Walls.corner_out_top_left:
                                wall = ModelRenderer.MakeModelRenderer("FrontBasicWallCorner");
                                wall.Name = "wall" + x.ToString() + "," + y.ToString();
                                wall.transform.Rotate(Vector3.Up, MathHelper.Pi);
                                wall.transform.Position = position;

                                // Body Definitions
                                body = new PhysicsBody2D(wall, "bounding_box_left", wall_bounding_box_left_right, new PhysicsMaterial(), PhysicsEngine.BodyType.STATIC);
                                body.transform.Position = new Vector3(-0.45f, 0.0f, -0.45f);
                                PhysicsEngine.AddPhysicsBody(body);
                                wall.AddComponent(body);
                                break;
                            case Walls.corner_out_top_right:
                                wall = ModelRenderer.MakeModelRenderer("FrontBasicWallCorner");
                                wall.Name = "wall" + x.ToString() + "," + y.ToString();
                                wall.transform.Rotate(Vector3.Up, MathHelper.PiOver2);
                                wall.transform.Position = position;

                                // Body Definitions
                                body = new PhysicsBody2D(wall, "bounding_box_right", wall_bounding_box_left_right, new PhysicsMaterial(), PhysicsEngine.BodyType.STATIC);
                                body.transform.Position = new Vector3(0.45f, 0.0f, -0.45f);
                                PhysicsEngine.AddPhysicsBody(body);
                                wall.AddComponent(body);
                                break;
                            case Walls.hall_left_right_corner_out_left:
                                wall = ModelRenderer.MakeModelRenderer("FrontBasicWallCorner");
                                wall.Name = "wall" + x.ToString() + "," + y.ToString();
                                wall.transform.Rotate(Vector3.Up, MathHelper.Pi);
                                wall.transform.Position = position;
                                wall.material = materials["wall_hall"];

                                // Body Definitions
                                body = new PhysicsBody2D(wall, "bounding_box_left", wall_bounding_box_left_right, new PhysicsMaterial(), PhysicsEngine.BodyType.STATIC);
                                body.transform.Position = new Vector3(-0.45f, 0.0f, -0.45f);
                                PhysicsEngine.AddPhysicsBody(body);
                                wall.AddComponent(body);

                                wall = ModelRenderer.MakeModelRenderer("FrontBasicWallCorner");
                                wall.Name = "wall" + x.ToString() + "," + y.ToString();
                                wall.transform.Rotate(Vector3.Up, MathHelper.Pi + MathHelper.PiOver2);
                                wall.transform.Position = position;

                                // Body Definitions
                                body = new PhysicsBody2D(wall, "bounding_box_right", wall_bounding_box_left_right, new PhysicsMaterial(), PhysicsEngine.BodyType.STATIC);
                                body.transform.Position = new Vector3(-0.45f, 0.0f, 0.45f);
                                PhysicsEngine.AddPhysicsBody(body);
                                wall.AddComponent(body);
                                break;
                            case Walls.hall_left_right_corner_out_right:
                                wall = ModelRenderer.MakeModelRenderer("FrontBasicWallCorner");
                                wall.Name = "wall" + x.ToString() + "," + y.ToString();
                                wall.transform.Rotate(Vector3.Up, MathHelper.PiOver2);
                                wall.transform.Position = position;
                                wall.material = materials["wall_hall"];

                                // Body Definitions
                                body = new PhysicsBody2D(wall, "bounding_box_right", wall_bounding_box_left_right, new PhysicsMaterial(), PhysicsEngine.BodyType.STATIC);
                                body.transform.Position = new Vector3(0.45f, 0.0f, -0.45f);
                                PhysicsEngine.AddPhysicsBody(body);
                                wall.AddComponent(body);

                                wall = ModelRenderer.MakeModelRenderer("FrontBasicWallCorner");
                                wall.Name = "wall" + x.ToString() + "," + y.ToString();
                                //wall.transform.Rotate(Vector3.Up, MathHelper.PiOver2);
                                wall.transform.Position = position;

                                // Body Definitions
                                body = new PhysicsBody2D(wall, "bounding_box_right", wall_bounding_box_left_right, new PhysicsMaterial(), PhysicsEngine.BodyType.STATIC);
                                body.transform.Position = new Vector3(0.45f, 0.0f, 0.45f);
                                PhysicsEngine.AddPhysicsBody(body);
                                wall.AddComponent(body);
                                break;
                            case Walls.door_bottom:
                                wall = ModelRenderer.MakeModelRenderer("DoorFrame");
                                wall.Name = "wall" + x.ToString() + "," + y.ToString();
                                wall.transform.Rotate(Vector3.Up, MathHelper.Pi);
                                wall.transform.Position = position;

                                // Body Definitions
                                body = new PhysicsBody2D(wall, "bounding_box_bottom_left", door_bounding_box, new PhysicsMaterial(), PhysicsEngine.BodyType.STATIC);
                                body.transform.Position = new Vector3(-0.45f, 0.0f, 0.45f);
                                PhysicsEngine.AddPhysicsBody(body);
                                wall.AddComponent(body);

                                body = new PhysicsBody2D(wall, "bounding_box_bottom_right", door_bounding_box, new PhysicsMaterial(), PhysicsEngine.BodyType.STATIC);
                                body.transform.Position = new Vector3(0.45f, 0.0f, 0.45f);
                                PhysicsEngine.AddPhysicsBody(body);
                                wall.AddComponent(body);
                                break;
                            case Walls.door_left:
                                wall = ModelRenderer.MakeModelRenderer("DoorFrame");
                                wall.Name = "wall" + x.ToString() + "," + y.ToString();
                                wall.transform.Rotate(Vector3.Up, MathHelper.PiOver2);
                                wall.transform.Position = position;

                                // Body Definitions
                                body = new PhysicsBody2D(wall, "bounding_box_bottom_left", door_bounding_box, new PhysicsMaterial(), PhysicsEngine.BodyType.STATIC);
                                body.transform.Position = new Vector3(-0.45f, 0.0f, -0.45f);
                                PhysicsEngine.AddPhysicsBody(body);
                                wall.AddComponent(body);

                                body = new PhysicsBody2D(wall, "bounding_box_top_left", door_bounding_box, new PhysicsMaterial(), PhysicsEngine.BodyType.STATIC);
                                body.transform.Position = new Vector3(-0.45f, 0.0f, 0.45f);
                                PhysicsEngine.AddPhysicsBody(body);
                                wall.AddComponent(body);
                                break;
                            case Walls.door_right:
                                wall = ModelRenderer.MakeModelRenderer("DoorFrame");
                                wall.Name = "wall" + x.ToString() + "," + y.ToString();
                                wall.transform.Rotate(Vector3.Up, MathHelper.Pi + MathHelper.PiOver2);
                                wall.transform.Position = position;

                                // Body Definitions
                                body = new PhysicsBody2D(wall, "bounding_box_bottom_right", door_bounding_box, new PhysicsMaterial(), PhysicsEngine.BodyType.STATIC);
                                body.transform.Position = new Vector3(0.45f, 0.0f, -0.45f);
                                PhysicsEngine.AddPhysicsBody(body);
                                wall.AddComponent(body);

                                body = new PhysicsBody2D(wall, "bounding_box_top_right", door_bounding_box, new PhysicsMaterial(), PhysicsEngine.BodyType.STATIC);
                                body.transform.Position = new Vector3(0.45f, 0.0f, 0.45f);
                                PhysicsEngine.AddPhysicsBody(body);
                                wall.AddComponent(body);
                                break;
                            case Walls.door_top:
                                wall = ModelRenderer.MakeModelRenderer("DoorFrame");
                                wall.Name = "wall" + x.ToString() + "," + y.ToString();
                                wall.transform.Position = position;

                                // Body Definitions
                                body = new PhysicsBody2D(wall, "bounding_box_top_left", door_bounding_box, new PhysicsMaterial(), PhysicsEngine.BodyType.STATIC);
                                body.transform.Position = new Vector3(-0.45f, 0.0f, -0.45f);
                                PhysicsEngine.AddPhysicsBody(body);
                                wall.AddComponent(body);

                                body = new PhysicsBody2D(wall, "bounding_box_top_right", door_bounding_box, new PhysicsMaterial(), PhysicsEngine.BodyType.STATIC);
                                body.transform.Position = new Vector3(0.45f, 0.0f, -0.45f);
                                PhysicsEngine.AddPhysicsBody(body);
                                wall.AddComponent(body);
                                break;
                        }
                    }

                    //if (wall != null)
                    //    wall.transform.Rotate(new Vector3(0, 1, 0), Random.Range(MathHelper.Pi * 2));

                    ModelRenderer floor = ModelRenderer.MakeModelRenderer("FloorTile");
                    floor.Name = "floor" + x.ToString() + "," + y.ToString();
                    floor.transform.Position = position;
                    
                    if (floors.ContainsKey(pos))
                    {
                        switch (floors[pos])
                        {
                            case Rooms.kitchen:
                                floor.material = materials["floor_kitchen"];

                                if (wall != null)
                                    wall.material = materials["wall_kitchen"];
                                break;
                            case Rooms.dining:
                                floor.material = materials["floor_dining"];

                                if (wall != null)
                                    wall.material = materials["wall_dining"];
                                break;
                            case Rooms.living:
                                floor.material = materials["floor_living"];

                                if (wall != null)
                                    wall.material = materials["wall_hall"];
                                break;
                            case Rooms.hall:
                                floor.material = materials["floor_hall"];

                                if (wall != null)
                                    wall.material = materials["wall_hall"];
                                break;
                            case Rooms.bedroom:
                                floor.material = materials["floor_bedroom"];

                                if (wall != null)
                                    wall.material = materials["wall_hall"];
                                break;
                            case Rooms.garage:
                                floor.material = materials["floor_garage"];

                                if (wall != null)
                                    wall.material = materials["wall_hall"];
                                break;
                            case Rooms.walkway:
                                floor.material = materials["floor_walk"];

                                if (wall != null)
                                    wall.material = materials["wall_brick"];
                                break;
                            case Rooms.road:
                                floor.material = materials["floor_road"];

                                if (wall != null)
                                    wall.material = materials["wall_brick"];
                                break;
                            case Rooms.bathroom:
                                floor.material = materials["floor_bath"];

                                if (wall != null)
                                    wall.material = materials["wall_bath"];
                                break;
                            case Rooms.study:
                                floor.material = materials["floor_study"];

                                if (wall != null)
                                    wall.material = materials["wall_hall"];
                                break;
                        }
                    }
                    else
                    {
                        floor.material = materials["floor_grass"];

                        if (wall != null)
                            wall.material = materials["wall_brick"];
                    }

                    GameObjectManager.AddGameObject(floor);  

                    if (wall != null)
                    {
                        GameObjectManager.AddGameObject(wall);
                    }
                }
            }
            #endregion

            // Player generation ---------------------------------------------------------------------------------------------------------------------------------------------------------------------
            #region Player Generation
            //

            PlayerController player = new PlayerController("Player1", PlayerIndex.One);
            player.transform.Position = new Vector3(2, 0, 9);
            PhysicsBody2D player_body = new PhysicsBody2D(player, "Player1Body", new Circle(0.5f), new PhysicsMaterial(), PhysicsEngine.BodyType.KINEMATIC);
            player.AddComponent(player_body);
            ModelRenderer player_model = ModelRenderer.MakeModelRenderer("Player");
            player.AddComponent(player_model);

            player_body.RegisterCollisionCallback(new Collision2D.OnCollision(PlayerCollide));

            PhysicsEngine.AddPhysicsBody(player_body);
            GameObjectManager.AddGameObject(player);
            #endregion
        }

        public override void Update()
        {
            Initialise();
        }

        public void PlayerCollide(Collision2D collision)
        {
            float t = 0;
            t = (t == 0) ? 0 : 1;
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
                        else if (doors.Contains(position))
                        {
                            // Make this wall a door
                            walls.Add(position, Walls.door_left);
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
                        else if (doors.Contains(position))
                        {
                            // Make this wall a door
                            walls.Add(position, Walls.door_top);
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
                        else if (doors.Contains(position))
                        {
                            // Make this wall a door
                            walls.Add(position, Walls.door_right);
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
                        else if (doors.Contains(position))
                        {
                            // Make this wall a door
                            walls.Add(position, Walls.door_bottom);
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
