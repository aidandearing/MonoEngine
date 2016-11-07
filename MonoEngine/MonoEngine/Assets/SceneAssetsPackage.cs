using System;
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
        public Dictionary<Type, List<string>> assets;

        public Scene parent;

        public SceneAssetsPackage()
        {
            assets = new Dictionary<Type, List<string>>();
        }

        public SceneAssetsPackage(Scene parent)
        {
            this.parent = parent;

            assets = new Dictionary<Type, List<string>>();
        }

        public void AddAsset(string name, Type type)
        {
            if (!assets.ContainsKey(type))
            {
                List<string> list = new List<string>();
                list.Add(name);
                assets.Add(type, list);
            }
            else
            {
                assets[type].Add(name);
            }
        }

        /// <summary>
        /// Given another SceneAssetsPackage this method will return a SceneAssetsPackage that contains only the assets that this SceneAssetsPackage does not have in common with the other (The ones this one has and the other doesn't)
        /// </summary>
        /// <param name="other">The SceneAssetsPackage to compare against</param>
        /// <returns>A SceneAssetsPackage with only the assets this package has and the other does not.</returns>
        public SceneAssetsPackage Difference(SceneAssetsPackage other)
        {
            SceneAssetsPackage package = new SceneAssetsPackage();

            foreach (KeyValuePair<Type, List<string>> assetList in assets)
            {
                foreach (string str in assetList.Value)
                {
                    if (!other.assets[assetList.Key].Contains(str))
                        package.AddAsset(str, assetList.Key);
                }
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

            Dictionary<Type, List<string>> allAssets = new Dictionary<Type, List<string>>();

            foreach(KeyValuePair<Type, List<string>> assets in this.assets)
            {
                allAssets.Add(assets.Key, assets.Value);
            }

            foreach(KeyValuePair<Type, List<string>> assets in other.assets)
            {
                if (allAssets.ContainsKey(assets.Key))
                {
                    allAssets[assets.Key].AddRange(assets.Value);
                }
                else
                {
                    allAssets.Add(assets.Key, assets.Value);
                }
            }

            foreach(KeyValuePair<Type, List<string>> assets in allAssets)
            {
                foreach(string asset in assets.Value)
                {
                    if (!other.assets[assets.Key].Contains(asset))
                    {
                        packages[0].AddAsset(asset, assets.Key);
                    }
                    else if (this.assets[assets.Key].Contains(asset))
                    {
                        packages[1].AddAsset(asset, assets.Key);
                    }
                    else
                    {
                        packages[2].AddAsset(asset, assets.Key);
                    }
                }
            }

            return packages;
        } 
    }
}
