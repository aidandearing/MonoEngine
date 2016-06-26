using System;
using Microsoft.Xna.Framework;

namespace Capstone
{
    class Transform
    {
        private Matrix transformation;

        public Matrix Transformation
        {
            get
            {
                return transformation;
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
                return transformation.Translation;
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
                return transformation.Rotation;
            }
        }
        
        public Vector3 Forward
        {
            get
            {
                return transformation.Forward;
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

        public Vector3 WorldToLocal(Vector3 vector)
        {
            return transformation.Translation - vector;
        }

        public Vector3 LocalToWorld(Vector3 vector)
        {
            return transformation.Translation + vector;
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
