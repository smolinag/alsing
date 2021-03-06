﻿using System;
using GenArt.Classes;
using GenArt.Core.Classes;

namespace GenArt.AST
{
    [Serializable]
    public class DnaPoint
    {
        public int X { get; set; }
        public int Y { get; set; }

        public void Init(DnaDrawing drawing,JobInfo info)
        {
            X = info.GetRandomNumber(0, info.SourceImage.Width);
            Y = info.GetRandomNumber(0, info.SourceImage.Height);
        }

        public DnaPoint Clone()
        {
            return new DnaPoint
                       {
                           X = X,
                           Y = Y,
                       };
        }

        public void Mutate(DnaDrawing drawing, JobInfo info)
        {
            if (!info.Settings.MuteMovePointMax)
            {
                if (info.WillMutate(info.Settings.MovePointMaxMutationRate))
                {
                    X = info.GetRandomNumber(0, info.SourceImage.Width);
                    Y = info.GetRandomNumber(0, info.SourceImage.Height);
                    drawing.SetDirty();
                }
            }

            if (!info.Settings.MuteMovePointMid)
            {
                if (info.WillMutate(info.Settings.MovePointMidMutationRate))
                {

                    X = X
                        .Randomize(info ,- info.Settings.MovePointRangeMid, info.Settings.MovePointRangeMid)
                        .Max(0)
                        .Min(info.SourceImage.Width);

                    Y = Y
                        .Randomize(info, -info.Settings.MovePointRangeMid, info.Settings.MovePointRangeMid)
                        .Max(0)
                        .Min(info.SourceImage.Height);

                    drawing.SetDirty();
                }
            }

            if (!info.Settings.MuteMovePointMin)
            {
                if (info.WillMutate(info.Settings.MovePointMinMutationRate))
                {
                    X = X
                        .Randomize(info, -info.Settings.MovePointRangeMin, info.Settings.MovePointRangeMin)
                        .Max(0)
                        .Min(info.SourceImage.Width);

                    Y = Y
                        .Randomize(info, -info.Settings.MovePointRangeMin, info.Settings.MovePointRangeMin)
                        .Max(0)
                        .Min(info.SourceImage.Height);

                    drawing.SetDirty();
                }
           }
        }

        public override string ToString()
        {
            return string.Format("Point({0},{1})", X, Y);
        }
    }
}
