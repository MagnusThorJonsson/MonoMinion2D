using System.Xml.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using System;
using Microsoft.Xna.Framework;
using System.Xml;

namespace MonoMinion.Graphics
{
    /// <summary>
    /// Manages the Tilesheet content
    /// </summary>
    public static class TileSheetManager
    {
        private static Dictionary<string, TileSheet> tilesheets = new Dictionary<string, TileSheet>();

        
        /// <summary>
        /// Gets a tilesheet
        /// </summary>
        /// <param name="path">The path to the tilesheet</param>
        /// <returns>The tilesheet</returns>
        public static TileSheet Tilesheet(string name)
        {
            // if it already exists we return the loaded one
            if (tilesheets.ContainsKey(name))
                return tilesheets[name];

            //"Graphics\\Tilesheets\\"
            tilesheets[name] = new TileSheet(name, Minion.Instance.Content.Load<Texture2D>(Minion.TILESHEET_PATH + name));
            return tilesheets[name];
        }


        /// <summary>
        /// Loads a tilesheet from a mts file
        /// </summary>
        /// <param name="path">The path to the file</param>
        /// <param name="tilesetXml">The tileset XML document</param>
        /// <returns>The tilesheet that was loaded</returns>
        public static TileSheet Load(string path, XDocument tilesetXml)
        {
            try
            {
                // Loads the file and creates the parser
                XElement tileset = tilesetXml.Element("tileset");
                TileSheet sheet = new TileSheet(
                    tileset.Attribute("name").Value,
                    Minion.Instance.Content.Load<Texture2D>(path + "\\" + tileset.Attribute("texture").Value)
                );

                // Extract tile size
                int tWidth = int.Parse(tileset.Attribute("tilewidth").Value);
                int tHeight = int.Parse(tileset.Attribute("tileheight").Value);

                // Loop through the tilegroups and create the tiles
                foreach (XElement tilegroup in tileset.Elements("tilegroup"))
                {
                    foreach (XElement tile in tilegroup.Elements("tile"))
                    {
                        sheet.AddTile(
                            tile.Attribute("name").Value,
                            tilegroup.Attribute("name").Value,
                            new Rectangle(
                                int.Parse(tile.Attribute("x").Value) * tWidth,
                                int.Parse(tile.Attribute("y").Value) * tWidth,
                                tWidth,
                                tHeight
                            )
                        );
                    }
                }

                return sheet;
            }
            catch (Exception e)
            {
                // TODO: Handle better
                Console.WriteLine("TILESHEET ERROR: " + e.Message);
            }

            return null;
        }
    }
}
