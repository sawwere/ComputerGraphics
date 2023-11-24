using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndTask2
{
    public class Material
    {
        public float reflection;    // коэффициент отражения
        public float refraction;    // коэффициент преломления
        public float environment;   // коэффициент преломления среды
        public float ambient;       // коэффициент принятия фонового освещения
        public float diffuse;       // коэффициент принятия диффузного освещения
        public Vector3 color;        // цвет

        public Material(float refl, float refr, float amb, float dif, float env, Vector3 clr)
        {
            reflection = refl;
            refraction = refr;
            ambient = amb;
            diffuse = dif;
            environment = env;
            color = clr;
        }

        public Material(float refl, float refr, float amb, float dif, float env, Color clr)
        {
            reflection = refl;
            refraction = refr;
            ambient = amb;
            diffuse = dif;
            environment = env;
            color = new Vector3(clr.R / 255.0f, clr.G / 255.0f, clr.B / 255.0f);
        }

        public Material Clone()
        {
            return new Material(reflection,
                            refraction,
                            ambient,
                            diffuse,
                            environment,
                            color.Clone()
            );
        }

        public static Material Mirror()
        {
            return new Material(0.8f, 0f, 0.0f, 0.0f, 1f, new Vector3(1, 1, 1));
        }

        public static Material Wall(Color color)
        {
            return new Material(0.0f, 0f, 0.1f, 0.8f, 1f, new Vector3(color.R / 255.0f, color.G / 255.0f, color.B / 255.0f));
        }

        public static Material Wall(Vector3 color)
        {
            return new Material(0.0f, 0f, 0.1f, 0.8f, 1f, color);
        }

        public static Material Transparent()
        {
            return new Material(0.0f, 0.9f, 0.0f, 0.8f, 1.03f, new Vector3(1, 1, 1));
        }
    }
}
