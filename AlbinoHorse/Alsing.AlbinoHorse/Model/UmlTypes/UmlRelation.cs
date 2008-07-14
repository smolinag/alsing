﻿using System;
using System.Drawing;
using System.Windows.Forms;
using AlbinoHorse.Infrastructure;
using AlbinoHorse.Model.Settings;
using Brushes=AlbinoHorse.Model.Settings.Brushes;
using Pens=AlbinoHorse.Model.Settings.Pens;

namespace AlbinoHorse.Model
{
    public delegate void LineDrawer(float x1, float y1, float x2, float y2);


    public class UmlRelation : Shape
    {
        protected object EndPortIdentifier = new object();
        protected object StartPortIdentifier = new object();

        #region Property Start

        public Shape Start
        {
            get { return DataSource.Start; }
        }

        #endregion

        #region Property End

        public Shape End
        {
            get { return DataSource.End; }
        }

        #endregion

        public IUmlRelationData DataSource { get; set; }

        public override void Draw(RenderInfo info)
        {
        }

        public override void DrawBackground(RenderInfo info)
        {
            Shape start = Start;
            Shape end = End;

            if (start == null || end == null)
                return;

            Rectangle startBounds = start.Bounds;
            Rectangle endBounds = end.Bounds;


            PointF startPoint = GetPoint(startBounds, DataSource.StartPortOffset, DataSource.StartPortSide);
            PointF endPoint = GetPoint(endBounds, DataSource.EndPortOffset, DataSource.EndPortSide);

            Pen pen;

            if (DataSource.AssociationType == UmlRelationType.Inheritance)
                pen = Pens.InheritanceLine;
            else if (DataSource.AssociationType == UmlRelationType.None)
                pen = Pens.FakeLine;
            else
                pen = Pens.AssociationLine;

            //   DrawAssociation(info, startPoint, endPoint, Settings.Pens.AssociationBorder);
            DrawRelationBackground(info, startPoint, endPoint);
            if (Selected)
            {
                DrawRelation(info, startPoint, endPoint, Pens.AssociationBorder);
            }
            DrawRelation(info, startPoint, endPoint, pen);
            DrawPortSelectionHandles(info, DataSource.StartPortSide, startPoint);
            DrawPortSelectionHandles(info, DataSource.EndPortSide, endPoint);
        }


        private void DrawRelation(RenderInfo info, PointF startPoint, PointF endPoint, Pen pen)
        {
            //start
            if (DataSource.AssociationType == UmlRelationType.Aggregation)
            {
                DrawAggregatePort(info, DataSource.StartPortSide, startPoint, pen);
            }
            else
            {
                DrawPort(info, DataSource.StartPortSide, startPoint, pen);
            }

            //middle            
            DrawLine(info, pen, startPoint.X, startPoint.Y, endPoint.X, endPoint.Y);

            //end
            if (DataSource.AssociationType == UmlRelationType.Association)
                DrawArrowPort(info, DataSource.EndPortSide, endPoint, pen);
            if (DataSource.AssociationType == UmlRelationType.Aggregation)
                DrawArrowPort(info, DataSource.EndPortSide, endPoint, pen);
            if (DataSource.AssociationType == UmlRelationType.None)
                DrawPort(info, DataSource.EndPortSide, endPoint, pen);
            if (DataSource.AssociationType == UmlRelationType.Inheritance)
                DrawInheritancePort(info, DataSource.EndPortSide, endPoint, pen);
        }

        private void DrawRelationBackground(RenderInfo info, PointF startPoint, PointF endPoint)
        {
            DrawLineBackground(info, startPoint.X, startPoint.Y, endPoint.X, endPoint.Y);
            DrawPortBackground(info, DataSource.StartPortSide, startPoint, StartPortIdentifier);
            DrawPortBackground(info, DataSource.EndPortSide, endPoint, EndPortIdentifier);
        }

        private void DrawPortSelectionHandles(RenderInfo info, UmlPortSide portSide, PointF point)
        {
            if (Selected)
            {
                const int marginSize = Margins.RelationPortMargin;
                if (portSide == UmlPortSide.Left)
                {
                    DrawSelectionHandle(info, new Point((int) point.X + marginSize, (int) point.Y));
                }

                if (portSide == UmlPortSide.Right)
                {
                    DrawSelectionHandle(info, new Point((int) point.X - marginSize, (int) point.Y));
                }

                if (portSide == UmlPortSide.Top)
                {
                    DrawSelectionHandle(info, new Point((int) point.X, (int) point.Y + marginSize));
                }

                if (portSide == UmlPortSide.Bottom)
                {
                    DrawSelectionHandle(info, new Point((int) point.X, (int) point.Y - marginSize));
                }
            }
        }

        private void DrawPortBackground(RenderInfo info, UmlPortSide portSide, PointF point, object portIdentifier)
        {
            const int marginSize = Margins.RelationPortMargin;
            if (portSide == UmlPortSide.Left)
            {
                DrawPortSelector(info, point.X, point.Y, point.X + marginSize, point.Y, portIdentifier);
            }

            if (portSide == UmlPortSide.Right)
            {
                DrawPortSelector(info, point.X, point.Y, point.X - marginSize, point.Y, portIdentifier);
            }

            if (portSide == UmlPortSide.Top)
            {
                DrawPortSelector(info, point.X, point.Y, point.X, point.Y + marginSize, portIdentifier);
            }

            if (portSide == UmlPortSide.Bottom)
            {
                DrawPortSelector(info, point.X, point.Y, point.X, point.Y - marginSize, portIdentifier);
            }
        }


        private static void DrawPort(RenderInfo info, UmlPortSide portSide, PointF point, Pen pen)
        {
            const int marginSize = Margins.RelationPortMargin;
            if (portSide == UmlPortSide.Left)
            {
                info.Graphics.DrawLine(pen, point.X, point.Y, point.X + marginSize, point.Y);
            }

            if (portSide == UmlPortSide.Right)
            {
                info.Graphics.DrawLine(pen, point.X, point.Y, point.X - marginSize, point.Y);
            }

            if (portSide == UmlPortSide.Top)
            {
                info.Graphics.DrawLine(pen, point.X, point.Y, point.X, point.Y + marginSize);
            }

            if (portSide == UmlPortSide.Bottom)
            {
                info.Graphics.DrawLine(pen, point.X, point.Y, point.X, point.Y - marginSize);
            }
        }

        private static void DrawArrowPort(RenderInfo info, UmlPortSide portSide, PointF point, Pen pen)
        {
            const int marginSize = Margins.RelationPortMargin;
            const int arrowSize = Margins.RelationArrowSize;

            if (portSide == UmlPortSide.Left)
            {
                info.Graphics.DrawLine(pen, point.X, point.Y, point.X + marginSize, point.Y);
                info.Graphics.DrawLine(pen, point.X + arrowSize, point.Y - Margins.RelationArrowSize, point.X + marginSize, point.Y);
                info.Graphics.DrawLine(pen, point.X + arrowSize, point.Y + Margins.RelationArrowSize, point.X + marginSize, point.Y);
            }

            if (portSide == UmlPortSide.Right)
            {
                info.Graphics.DrawLine(pen, point.X, point.Y, point.X - marginSize, point.Y);
                info.Graphics.DrawLine(pen, point.X - arrowSize, point.Y - Margins.RelationArrowSize, point.X - marginSize, point.Y);
                info.Graphics.DrawLine(pen, point.X - arrowSize, point.Y + Margins.RelationArrowSize, point.X - marginSize, point.Y);
            }

            if (portSide == UmlPortSide.Top)
            {
                info.Graphics.DrawLine(pen, point.X, point.Y, point.X, point.Y + marginSize);
                info.Graphics.DrawLine(pen, point.X - Margins.RelationArrowSize, point.Y + arrowSize, point.X, point.Y + marginSize);
                info.Graphics.DrawLine(pen, point.X + Margins.RelationArrowSize, point.Y + arrowSize, point.X, point.Y + marginSize);
            }

            if (portSide == UmlPortSide.Bottom)
            {
                info.Graphics.DrawLine(pen, point.X, point.Y, point.X, point.Y - marginSize);
                info.Graphics.DrawLine(pen, point.X - Margins.RelationArrowSize, point.Y - arrowSize, point.X, point.Y - marginSize);
                info.Graphics.DrawLine(pen, point.X + Margins.RelationArrowSize, point.Y - arrowSize, point.X, point.Y - marginSize);
            }
        }

        private static void DrawInheritancePort(RenderInfo info, UmlPortSide portSide, PointF point, Pen pen)
        {
            const int marginSize = Margins.RelationPortMargin;
            const int arrowSize = Margins.RelationArrowSize;
            var x = (int) point.X;
            var y = (int) point.Y;
            if (portSide == UmlPortSide.Left)
            {
                info.Graphics.DrawLine(pen, point.X, point.Y, point.X + marginSize, point.Y);

                var points = new[]
                                 {
                                     new Point(x + marginSize, y), new Point(x + arrowSize, y + Margins.RelationArrowSize),
                                     new Point(x + arrowSize, y - Margins.RelationArrowSize)
                                 };
                info.Graphics.FillPolygon(System.Drawing.Brushes.White, points);
                info.Graphics.DrawPolygon(pen, points);
            }

            if (portSide == UmlPortSide.Right)
            {
                info.Graphics.DrawLine(pen, point.X, point.Y, point.X - marginSize, point.Y);

                var points = new[]
                                 {
                                     new Point(x - marginSize, y), new Point(x - arrowSize, y + Margins.RelationArrowSize),
                                     new Point(x - arrowSize, y - Margins.RelationArrowSize)
                                 };
                info.Graphics.FillPolygon(System.Drawing.Brushes.White, points);
                info.Graphics.DrawPolygon(pen, points);
            }

            if (portSide == UmlPortSide.Top)
            {
                info.Graphics.DrawLine(pen, point.X, point.Y, point.X, point.Y + marginSize);

                var points = new[]
                                 {
                                     new Point(x, y + marginSize), new Point(x + Margins.RelationArrowSize, y + arrowSize),
                                     new Point(x - Margins.RelationArrowSize, y + arrowSize)
                                 };
                info.Graphics.FillPolygon(System.Drawing.Brushes.White, points);
                info.Graphics.DrawPolygon(pen, points);
            }

            if (portSide == UmlPortSide.Bottom)
            {
                info.Graphics.DrawLine(pen, point.X, point.Y, point.X, point.Y - marginSize);
                var points = new[]
                                 {
                                     new Point(x, y - marginSize), new Point(x + Margins.RelationArrowSize, y - arrowSize),
                                     new Point(x - Margins.RelationArrowSize, y - arrowSize)
                                 };
                info.Graphics.FillPolygon(System.Drawing.Brushes.White, points);
                info.Graphics.DrawPolygon(pen, points);
            }
        }

        private static void DrawAggregatePort(RenderInfo info, UmlPortSide portSide, PointF point, Pen pen)
        {
            const int marginSize = 16;
            var x = (int) point.X;
            var y = (int) point.Y;
            if (portSide == UmlPortSide.Left)
            {
                info.Graphics.DrawLine(pen, point.X, point.Y, point.X + marginSize, point.Y);

                var points = new[]
                                 {
                                     new Point(x + marginSize, y), new Point(x + marginSize/2, y + Margins.RelationArrowSize),
                                     new Point(x + 0, y),
                                     new Point(x + marginSize/2, y - Margins.RelationArrowSize)
                                 };

                info.Graphics.FillPolygon(System.Drawing.Brushes.White, points);
                info.Graphics.DrawPolygon(pen, points);
            }

            if (portSide == UmlPortSide.Right)
            {
                info.Graphics.DrawLine(pen, point.X, point.Y, point.X - marginSize, point.Y);

                var points = new[]
                                 {
                                     new Point(x - marginSize, y), new Point(x - marginSize/2, y + Margins.RelationArrowSize),
                                     new Point(x - 0, y),
                                     new Point(x - marginSize/2, y - Margins.RelationArrowSize)
                                 };
                info.Graphics.FillPolygon(System.Drawing.Brushes.White, points);
                info.Graphics.DrawPolygon(pen, points);
            }

            if (portSide == UmlPortSide.Top)
            {
                info.Graphics.DrawLine(pen, point.X, point.Y, point.X, point.Y + marginSize);

                var points = new[]
                                 {
                                     new Point(x, y + marginSize), new Point(x + Margins.RelationArrowSize, y + marginSize/2), new Point(x, y),
                                     new Point(x - Margins.RelationArrowSize, y + marginSize/2)
                                 };

                info.Graphics.FillPolygon(System.Drawing.Brushes.White, points);
                info.Graphics.DrawPolygon(pen, points);
            }

            if (portSide == UmlPortSide.Bottom)
            {
                info.Graphics.DrawLine(pen, point.X, point.Y, point.X, point.Y - marginSize);

                var points = new[]
                                 {
                                     new Point(x, y - marginSize), new Point(x + Margins.RelationArrowSize, y - marginSize/2), new Point(x, y),
                                     new Point(x - Margins.RelationArrowSize, y - marginSize/2)
                                 };

                info.Graphics.FillPolygon(System.Drawing.Brushes.White, points);
                info.Graphics.DrawPolygon(pen, points);
            }
        }

        //private static void DrawLine(RenderInfo info, PointF startPoint, PointF endPoint, Pen pen)
        //{
        //    info.Graphics.DrawLine(pen, startPoint, endPoint);
        //}

        private static PointF GetPoint(Rectangle bounds, int portOffset, UmlPortSide portSide)
        {
            const int marginSize = 20;

            var point = new PointF();
            if (portSide == UmlPortSide.Left)
            {
                point.X = bounds.Left - marginSize;
                point.Y = bounds.Top + portOffset;
            }
            else if (portSide == UmlPortSide.Right)
            {
                point.X = bounds.Right + marginSize;
                point.Y = bounds.Top + portOffset;
            }
            else if (portSide == UmlPortSide.Top)
            {
                point.X = bounds.Left + portOffset;
                point.Y = bounds.Top - marginSize;
            }
            else if (portSide == UmlPortSide.Bottom)
            {
                point.X = bounds.Left + portOffset;
                point.Y = bounds.Bottom + marginSize;
            }
            else
            {
                throw new NotSupportedException();
            }

            return point;
        }

        public override void PreviewDrawBackground(RenderInfo info)
        {
            Shape start = Start;
            Shape end = End;

            if (start == null || end == null)
                return;

            Rectangle startBounds = start.Bounds;
            Rectangle endBounds = end.Bounds;

            float x1 = startBounds.X + startBounds.Width/2;
            float y1 = startBounds.Y + startBounds.Height/2;


            float x2 = endBounds.X + endBounds.Width/2;
            float y2 = endBounds.Y + endBounds.Height/2;

            info.Graphics.DrawLine(System.Drawing.Pens.DarkGray, x1, y1, x2, y2);
        }

        private static void RouteLine(LineDrawer drawLine, float x1, float y1, float x2, float y2)
        {
            drawLine(x1, y1, x2, y2);
        }

        private static void DrawLine(RenderInfo info, Pen pen, float x1, float y1, float x2, float y2)
        {
            LineDrawer drawLine = (xx1, yy1, xx2, yy2) => info.Graphics.DrawLine(pen, xx1, yy1, xx2, yy2);
            RouteLine(drawLine, x1, y1, x2, y2);
        }

        private void DrawLineBackground(RenderInfo info, float x1, float y1, float x2, float y2)
        {
            LineDrawer drawLine = (xx1, yy1, xx2, yy2) =>
                                      {
                                          #region Add BBox

                                          var bbox = new BoundingBox {Target = this, Data = this};
                                          var tmp = new Rectangle((int) Math.Min(xx1, xx2), (int) Math.Min(yy1, yy2),
                                                                  (int) Math.Abs(xx2 - xx1),
                                                                  (int) Math.Abs(yy2 - yy1));
                                          tmp.Inflate(6, 6);
                                          bbox.Bounds = tmp;
                                          info.BoundingBoxes.Add(bbox);

                                          #endregion

                                          if (Selected)
                                          {
                                              info.Graphics.DrawLine(Pens.Selection, xx1, yy1, xx2, yy2);
                                          }
                                      };

            RouteLine(drawLine, x1, y1, x2, y2);
        }

        private void DrawPortSelector(RenderInfo info, float x1, float y1, float x2, float y2, object portIdentifier)
        {
            #region Add BBox

            var bbox = new BoundingBox {Target = this, Data = portIdentifier};
            var tmp = new Rectangle((int) Math.Min(x1, x2), (int) Math.Min(y1, y2), (int) Math.Abs(x2 - x1),
                                    (int) Math.Abs(y2 - y1));

            tmp.Inflate(6, 6);

            bbox.Bounds = tmp;
            info.BoundingBoxes.Add(bbox);

            #endregion

            if (Selected)
            {
                tmp.Inflate(-2, -2);
                info.Graphics.FillRectangle(Brushes.Selection, tmp);
            }
        }

        public override void OnMouseDown(ShapeMouseEventArgs args)
        {
            args.Sender.ClearSelection();
            Selected = true;
            args.Redraw = true;
        }

        public override void OnMouseMove(ShapeMouseEventArgs args)
        {
            if (args.BoundingBox.Data == StartPortIdentifier && args.Button == MouseButtons.Left)
            {
                Rectangle bounds = DataSource.Start.Bounds;

                int offset = 0;
                UmlPortSide side = DataSource.StartPortSide;

                int oppositeOffset = DataSource.EndPortOffset;
                UmlPortSide oppositeSide = DataSource.EndPortSide;


                MovePort(args, bounds, ref offset, ref side, oppositeOffset, oppositeSide);

                DataSource.StartPortSide = side;
                DataSource.StartPortOffset = offset;
                args.Redraw = true;
            }

            if (args.BoundingBox.Data == EndPortIdentifier && args.Button == MouseButtons.Left)
            {
                Rectangle bounds = DataSource.End.Bounds;

                int offset = 0;
                UmlPortSide side = DataSource.EndPortSide;

                int oppositeOffset = DataSource.StartPortOffset;
                UmlPortSide oppositeSide = DataSource.StartPortSide;


                MovePort(args, bounds, ref offset, ref side, oppositeOffset, oppositeSide);

                DataSource.EndPortSide = side;
                DataSource.EndPortOffset = offset;
                args.Redraw = true;
            }
        }

        private static void MovePort(ShapeMouseEventArgs args, Rectangle bounds, ref int offset, ref UmlPortSide side,
                                     int oppositeOffset, UmlPortSide oppositeSide)
        {
            int x = args.X;
            int y = args.Y;

            int half = bounds.Width/2;
            int center = bounds.X + half;
            int xd = Math.Abs(x - center);
            int top = bounds.Top + half - xd;
            int bottom = bounds.Bottom - half + xd;


            if (x < center)
            {
                //left of                
                side = UmlPortSide.Left;
            }
            else
            {
                //right of
                side = UmlPortSide.Right;
            }

            if (y < top)
                side = UmlPortSide.Top;

            if (y > bottom)
                side = UmlPortSide.Bottom;


            if (side == UmlPortSide.Top || side == UmlPortSide.Bottom)
            {
                offset = args.X - bounds.Left;

                if (args.X < bounds.Left)
                    offset = 0;

                if (args.X > bounds.Right)
                    offset = bounds.Width;
            }

            if (side == UmlPortSide.Left || side == UmlPortSide.Right)
            {
                offset = args.Y - bounds.Top;

                if (args.Y < bounds.Top)
                    offset = 0;

                if (args.Y > bounds.Bottom)
                    offset = bounds.Height;
            }

            if ((side == UmlPortSide.Left || side == UmlPortSide.Right) &&
                (oppositeSide == UmlPortSide.Left || oppositeSide == UmlPortSide.Right))
            {
                if (Math.Abs(offset - oppositeOffset) < 10)
                    offset = oppositeOffset;
            }

            if ((side == UmlPortSide.Top || side == UmlPortSide.Bottom) &&
                (oppositeSide == UmlPortSide.Top || oppositeSide == UmlPortSide.Bottom))
            {
                if (Math.Abs(offset - oppositeOffset) < 10)
                    offset = oppositeOffset;
            }
        }
    }
}