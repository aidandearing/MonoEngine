using System;
using System.Xml;
using Microsoft.Xna.Framework;
using MonoEngine.Assets;

namespace MonoEngine
{
    public class Transform
    {
        private enum ReaderFormat { vectors, transformations, none };
        public static Transform XmlToTransform(XmlReader reader)
        {
            Transform newTransform = null;

            int depth = reader.Depth;
            int depth_inner_1;

            int currentVector = -1;

            ReaderFormat format = ReaderFormat.none;

            Vector4 v1 = new Vector4();
            Vector4 v2 = new Vector4();
            Vector4 v3 = new Vector4();
            Vector4 v4 = new Vector4();

            while (reader.Read() && depth < reader.Depth)
            {
                if (reader.IsStartElement())
                {
                    switch(reader.Name)
                    {
                        case "vector4":
                            if (format == ReaderFormat.none)
                                format = ReaderFormat.vectors;
                            else if (format == ReaderFormat.transformations)
                                throw new AssetExceptions.TransformFromXMLFormat("The transform xml is incorrectly formatted, and mixes both transformation format (translation, rotation, scale) with vector format.");

                            currentVector++;
                            depth_inner_1 = reader.Depth;

                            while (reader.Read() && depth_inner_1 < reader.Depth)
                            {
                                if (reader.IsStartElement())
                                {
                                    switch(reader.Name)
                                    {
                                        case "x":
                                            reader.Read();
                                            if (currentVector == 0)
                                            {
                                                v1.X = float.Parse(reader.Value);
                                            }
                                            else if (currentVector == 1)
                                            {
                                                v2.X = float.Parse(reader.Value);
                                            }
                                            else if (currentVector == 2)
                                            {
                                                v3.X = float.Parse(reader.Value);
                                            }
                                            else
                                            {
                                                v4.X = float.Parse(reader.Value);
                                            }
                                            break;
                                        case "y":
                                            reader.Read();
                                            if (currentVector == 0)
                                            { 
                                                v1.Y = float.Parse(reader.Value);
                                            }
                                            else if (currentVector == 1)
                                            {
                                                v2.Y = float.Parse(reader.Value);
                                            }
                                            else if (currentVector == 2)
                                            {
                                                v3.Y = float.Parse(reader.Value);
                                            }
                                            else
                                            {
                                                v4.Y = float.Parse(reader.Value);
                                            }
                                            break;
                                        case "z":
                                            reader.Read();
                                            if (currentVector == 0)
                                            {
                                                v1.Z = float.Parse(reader.Value);
                                            }
                                            else if (currentVector == 1)
                                            {
                                                v2.Z = float.Parse(reader.Value);
                                            }
                                            else if (currentVector == 2)
                                            {
                                                v3.Z = float.Parse(reader.Value);
                                            }
                                            else
                                            {
                                                v4.Z = float.Parse(reader.Value);
                                            }
                                            break;
                                        case "w":
                                            reader.Read();
                                            if (currentVector == 0)
                                            {
                                                v1.W = float.Parse(reader.Value);
                                            }
                                            else if (currentVector == 1)
                                            {
                                                v2.W = float.Parse(reader.Value);
                                            }
                                            else if (currentVector == 2)
                                            {
                                                v3.W = float.Parse(reader.Value);
                                            }
                                            else
                                            {
                                                v4.W = float.Parse(reader.Value);
                                            }
                                            break;
                                    }
                                }
                            }
                            break;
                        case "vector3":
                            if (format == ReaderFormat.none)
                                format = ReaderFormat.vectors;
                            else if (format == ReaderFormat.transformations)
                                throw new AssetExceptions.TransformFromXMLFormat("The transform xml is incorrectly formatted, and mixes both transformation format (translation, rotation, scale) with vector format.");

                            currentVector++;
                            depth_inner_1 = reader.Depth;

                            while (reader.Read() && depth_inner_1 < reader.Depth)
                            {
                                if (reader.IsStartElement())
                                {
                                    switch (reader.Name)
                                    {
                                        case "x":
                                            reader.Read();
                                            if (currentVector == 0)
                                            {
                                                v1.X = float.Parse(reader.Value);
                                            }
                                            else if (currentVector == 1)
                                            {
                                                v2.X = float.Parse(reader.Value);
                                            }
                                            else if (currentVector == 2)
                                            {
                                                v3.X = float.Parse(reader.Value);
                                            }
                                            else
                                            {
                                                v4.X = float.Parse(reader.Value);
                                            }
                                            break;
                                        case "y":
                                            reader.Read();
                                            if (currentVector == 0)
                                            {
                                                v1.Y = float.Parse(reader.Value);
                                            }
                                            else if (currentVector == 1)
                                            {
                                                v2.Y = float.Parse(reader.Value);
                                            }
                                            else if (currentVector == 2)
                                            {
                                                v3.Y = float.Parse(reader.Value);
                                            }
                                            else
                                            {
                                                v4.Y = float.Parse(reader.Value);
                                            }
                                            break;
                                        case "z":
                                            reader.Read();
                                            if (currentVector == 0)
                                            {
                                                v1.Z = float.Parse(reader.Value);
                                            }
                                            else if (currentVector == 1)
                                            {
                                                v2.Z = float.Parse(reader.Value);
                                            }
                                            else if (currentVector == 2)
                                            {
                                                v3.Z = float.Parse(reader.Value);
                                            }
                                            else
                                            {
                                                v4.Z = float.Parse(reader.Value);
                                            }
                                            break;
                                    }
                                }
                            }
                            break;
                        case "vector2":
                            if (format == ReaderFormat.none)
                                format = ReaderFormat.vectors;
                            else if (format == ReaderFormat.transformations)
                                throw new AssetExceptions.TransformFromXMLFormat("The transform xml is incorrectly formatted, and mixes both transformation format (translation, rotation, scale) with vector format.");

                            currentVector++;
                            depth_inner_1 = reader.Depth;

                            while (reader.Read() && depth_inner_1 < reader.Depth)
                            {
                                if (reader.IsStartElement())
                                {
                                    switch (reader.Name)
                                    {
                                        case "x":
                                            reader.Read();
                                            if (currentVector == 0)
                                            {
                                                v1.X = float.Parse(reader.Value);
                                            }
                                            else if (currentVector == 1)
                                            {
                                                v2.X = float.Parse(reader.Value);
                                            }
                                            else if (currentVector == 2)
                                            {
                                                v3.X = float.Parse(reader.Value);
                                            }
                                            else
                                            {
                                                v4.X = float.Parse(reader.Value);
                                            }
                                            break;
                                        case "y":
                                            reader.Read();
                                            if (currentVector == 0)
                                            {
                                                v1.Y = float.Parse(reader.Value);
                                            }
                                            else if (currentVector == 1)
                                            {
                                                v2.Y = float.Parse(reader.Value);
                                            }
                                            else if (currentVector == 2)
                                            {
                                                v3.Y = float.Parse(reader.Value);
                                            }
                                            else
                                            {
                                                v4.Y = float.Parse(reader.Value);
                                            }
                                            break;
                                    }
                                }
                            }
                            break;
                    }
                }
            }

            Matrix m = new Matrix(v1, v2, v3, v4);
            newTransform = new Transform(m);

            return newTransform;
        }

        public Transform parent;

        private Matrix transformation;

        public Matrix Transformation
        {
            get
            {
                if (parent == null)
                {
                    return transformation;
                }
                else
                {
                    return parent.Transformation + transformation;
                }
            }
            set
            {
                transformation = value;
            }
        }

        public Vector3 Position
        {
            get
            {
                if (parent == null)
                {
                    return transformation.Translation;
                }
                else
                {
                    return parent.Position + transformation.Translation;
                }
            }
            set
            {
                transformation.Translation = value;
            }
        }

        public Quaternion Rotation
        {
            get
            {
                if (parent == null)
                {
                    return transformation.Rotation;
                }
                else
                {
                    return parent.Rotation + transformation.Rotation;
                }
            }
        }
        
        public Vector3 Forward
        {
            get
            {
                if (parent == null)
                {
                    return transformation.Forward;
                }
                else
                {
                    return transformation.Forward;
                }
            }
            set
            {
                transformation.Forward = value;
            }
        }

        public Vector3 Left
        {
            get
            {
                return transformation.Left;
            }
            set
            {
                transformation.Left = value;
            }
        }

        public Transform()
        {
            transformation = Matrix.Identity;
            Forward = transformation.Forward;
            Left = transformation.Left;
        }

        public Transform(Matrix m)
        {
            transformation = m;
            Forward = m.Forward;
            Left = m.Left;
        }

        public Transform(Vector3 translation, Vector3 scale, Vector3 rotation)
        {
            transformation = Matrix.CreateScale(scale) * Matrix.CreateFromYawPitchRoll(rotation.Y, rotation.X, rotation.Z) * Matrix.CreateTranslation(translation);
        }

        public Transform(Vector3 translation, Vector3 scale, Quaternion rotation)
        {
            transformation = Matrix.CreateScale(scale) * Matrix.CreateFromQuaternion(rotation) * Matrix.CreateTranslation(translation);
        }

        public Transform(Transform parent)
        {
            this.parent = parent;
        }

        public Vector3 WorldToLocal(Vector3 vector)
        {
            if (parent == null)
            {
                return transformation.Translation - vector;
            }
            else
            {
                return parent.WorldToLocal(transformation.Translation - vector);
            }
        }

        public Vector3 LocalToWorld(Vector3 vector)
        {
            if (parent == null)
            {
                return transformation.Translation + vector;
            }
            else
            {
                return parent.LocalToWorld(transformation.Translation + vector);
            }
        }

        public void Translate(Vector3 vector)
        {
            // Maybe?
            transformation.Translation += vector;
        }

        /// <summary>
        /// Rotates the transformation without altering the translation
        /// </summary>
        /// <param name="axis">the axis by which to rotate around</param>
        /// <param name="angle">the angle in radians to rotate by</param>
        public void Rotate(Vector3 axis, float angle)
        {
            // Ahk
            Vector3 translation = transformation.Translation;
            transformation *= Matrix.CreateFromAxisAngle(axis, angle);
            transformation.Translation = translation;
        }

        public void Scale(Vector3 scale)
        {
            transformation.Scale = scale;
        }

        /// <summary>
        /// Rotates the transformation purely
        /// </summary>
        /// <param name="axis">the axis by which to rotate around</param>
        /// <param name="angle">the angle in radians to rotate by</param>
        [Obsolete("Use this if you want to create a purely mathematical rotation")]
        public void PureRotate(Vector3 axis, float angle)
        {
            transformation *= Matrix.CreateFromAxisAngle(axis, angle);
        }
    }
}
