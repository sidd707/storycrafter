﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Story_Crafter {
    class FillTool: EditingTool {

        public override string Name { get { return "Fill"; } }

        Pen cursor = new Pen(Color.Orchid);

        public override void Paint(ICanvas canvas, TileLayer layer, TileSelection selection, Point paintLocation, int brushSizeX, int brushSizeY, int tileset) {
            Tile target = layer.Tiles[Program.ScreenPointToIndex(paintLocation)].Clone();
            Tile replacement = new Tile();
            replacement.Tileset = tileset;
            replacement.Index = Program.TilesetPointToIndex(selection.MinX, selection.MinY);
            Fill(layer, target, replacement, paintLocation);
        }

        public override void Paint(ICanvas canvas, ObjectLayer layer, Point paintLocation, int brushSizeX, int brushSizeY, int bank, int idx) {
            Tile target = layer.Tiles[Program.ScreenPointToIndex(paintLocation)].Clone();
            Tile replacement = new Tile();
            replacement.Bank = bank;
            replacement.Index = idx;
            Fill(layer, target, replacement, paintLocation);
        }

        public override void DrawCursor(Graphics g, Point position, Selection selection, int brushSizeX, int brushSizeY, int layer) {
            g.DrawRectangle(cursor, position.X * 24, position.Y * 24, 23, 23);
        }

        private void Fill(Layer layer, Tile target, Tile replacement, Point start) {
            if(target.Equals(replacement)) return;
            Stack<Point> nodes = new Stack<Point>();
            nodes.Push(start);
            while(nodes.Count > 0) {
                Point p = nodes.Pop();
                Tile t = layer.Tiles[Program.ScreenPointToIndex(p)];
                if(t.Equals(target)) {
                    t.Set(replacement);
                    if(p.X > 0) nodes.Push(new Point(p.X - 1, p.Y));
                    if(p.X < Program.ScreenWidth - 1) nodes.Push(new Point(p.X + 1, p.Y));
                    if(p.Y > 0) nodes.Push(new Point(p.X, p.Y - 1));
                    if(p.Y < Program.ScreenHeight - 1) nodes.Push(new Point(p.X, p.Y + 1));
                }
            }
        }

    }
}
