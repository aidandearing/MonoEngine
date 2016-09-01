using System.Collections.Generic;
using MonoEngine.Render;
using Microsoft.Xna.Framework.Graphics;
using MonoEngine.Game;

namespace MonoEngine.Assets
{
    /// <summary>
    /// A SceneAssetsPackage contains lists of the names of all referenced assets the parent scene has
    /// </summary>
    public class SceneAssetsPackage
    {
        public List<string> gameObjects;
        public List<string> fonts;
        public List<string> models;
        public List<string> texture2Ds;
        public List<string> renderTarget2Ds;
        public List<string> uiWidgets;

        public Scene parent;

        internal SceneAssetsPackage()
        { }

        public SceneAssetsPackage(Scene parent)
        {
            this.parent = parent;
        }

        /// <summary>
        /// Given another SceneAssetsPackage this method will return a SceneAssetsPackage that contains only the assets that this SceneAssetsPackage does not have in common with the other (The ones this one has and the other doesn't)
        /// </summary>
        /// <param name="other">The SceneAssetsPackage to compare against</param>
        /// <returns>A SceneAssetsPackage with only the assets this package has and the other does not.</returns>
        public SceneAssetsPackage Difference(SceneAssetsPackage other)
        {
            SceneAssetsPackage package = new SceneAssetsPackage();

            foreach(string str in gameObjects)
            {
                if (!other.gameObjects.Contains(str))
                    package.gameObjects.Add(str);
            }

            foreach (string str in fonts)
            {
                if (!other.fonts.Contains(str))
                    package.fonts.Add(str);
            }

            foreach (string str in models)
            {
                if (!other.models.Contains(str))
                    package.models.Add(str);
            }

            foreach (string str in texture2Ds)
            {
                if (!other.texture2Ds.Contains(str))
                    package.texture2Ds.Add(str);
            }

            foreach (string str in renderTarget2Ds)
            {
                if (!other.renderTarget2Ds.Contains(str))
                    package.renderTarget2Ds.Add(str);
            }

            foreach (string str in uiWidgets)
            {
                if (!other.uiWidgets.Contains(str))
                    package.uiWidgets.Add(str);
            }

            return package;
        }

        /// <summary>
        /// Given another SceneAssetsPackage this method will return an array of SceneAssetsPackages that contains the differences and commonalities between the 2 asset packages.
        /// This method is faster than checking the differences and commonalities seperately, but overkill if you only need to know one or the other
        /// </summary>
        /// <param name="other">The SceneAssetsPackage to check against</param>
        /// <returns>An array who's [0] contains only the assets found in this package, who's [1] contains the commonalities, and who's [2] contains only the assets found in the other package</returns>
        public SceneAssetsPackage[] Compare(SceneAssetsPackage other)
        {
            SceneAssetsPackage[] packages = new SceneAssetsPackage[3];

            List<string> allGameObjects = new List<string>();
            allGameObjects.AddRange(gameObjects);
            allGameObjects.AddRange(other.gameObjects);

            List<string> allFonts = new List<string>();
            allFonts.AddRange(gameObjects);
            allFonts.AddRange(other.gameObjects);

            List<string> allModels = new List<string>();
            allModels.AddRange(gameObjects);
            allModels.AddRange(other.gameObjects);

            List<string> allTextures = new List<string>();
            allTextures.AddRange(gameObjects);
            allTextures.AddRange(other.gameObjects);

            List<string> allTargets = new List<string>();
            allTargets.AddRange(gameObjects);
            allTargets.AddRange(other.gameObjects);

            List<string> allWidgets = new List<string>();
            allWidgets.AddRange(gameObjects);
            allWidgets.AddRange(other.gameObjects);

            foreach (string str in allGameObjects)
            {
                if (!other.gameObjects.Contains(str))
                    packages[0].gameObjects.Add(str);
                else if (gameObjects.Contains(str))
                    packages[1].gameObjects.Add(str);
                else
                    packages[2].gameObjects.Add(str);
            }

            foreach (string str in allFonts)
            {
                if (!other.fonts.Contains(str))
                    packages[0].fonts.Add(str);
                else if (fonts.Contains(str))
                    packages[1].fonts.Add(str);
                else
                    packages[2].fonts.Add(str);
            }

            foreach (string str in allModels)
            {
                if (!other.models.Contains(str))
                    packages[0].models.Add(str);
                else if (models.Contains(str))
                    packages[1].models.Add(str);
                else
                    packages[2].models.Add(str);
            }

            foreach (string str in allTextures)
            {
                if (!other.texture2Ds.Contains(str))
                    packages[0].texture2Ds.Add(str);
                else if (texture2Ds.Contains(str))
                    packages[1].texture2Ds.Add(str);
                else
                    packages[2].texture2Ds.Add(str);
            }

            foreach (string str in allTargets)
            {
                if (!other.renderTarget2Ds.Contains(str))
                    packages[0].renderTarget2Ds.Add(str);
                else if (renderTarget2Ds.Contains(str))
                    packages[1].renderTarget2Ds.Add(str);
                else
                    packages[2].renderTarget2Ds.Add(str);
            }

            foreach (string str in allWidgets)
            {
                if (!other.uiWidgets.Contains(str))
                    packages[0].uiWidgets.Add(str);
                else if (uiWidgets.Contains(str))
                    packages[1].uiWidgets.Add(str);
                else
                    packages[2].uiWidgets.Add(str);
            }

            return packages;
        } 
    }
}
