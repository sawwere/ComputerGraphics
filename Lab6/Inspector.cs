using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tools;
using Tools.Primitives;

namespace Lab6
{
    public partial class Inspector : UserControl
    {
        private SceneObject sceneObject;
        Point3D worldPosition;
        Point3D worldRotation;

        public void GetUpdate(SceneObject obj, Tools.Scene.Camera camera)
        {
            if (obj == null)
            {
                return;
            }
            sceneObject = obj;
            buttonColor.BackColor = sceneObject.Color;

            worldPosition = sceneObject.Transform.position + camera.position;
            textBoxName.Text = sceneObject.Name;
            textBoxPosX.Text = worldPosition.X.ToString("0.000");
            textBoxPosY.Text = worldPosition.Y.ToString("0.000");
            textBoxPosZ.Text = worldPosition.Z.ToString("0.000");

            worldRotation = sceneObject.Transform.rotation + camera.rotation;
            textBoxRotationX.Text = worldRotation.X.ToString("0.000");
            textBoxRotationY.Text = worldRotation.Y.ToString("0.000");
            textBoxRotationZ.Text = worldRotation.Z.ToString("0.000");

            textBoxScaleX.Text = sceneObject.Transform.scale.X.ToString("0.000");
            textBoxScaleY.Text = sceneObject.Transform.scale.Y.ToString("0.000");
            textBoxScaleZ.Text = sceneObject.Transform.scale.Z.ToString("0.000");
        }

        public Inspector()
        {
            InitializeComponent();
        }

        private void textBoxPosX_TextChanged(object sender, EventArgs e)
        {
            Console.WriteLine("textBoxPosX_TextChanged");
        }

        private void buttonToDefault_Click(object sender, EventArgs e)
        {
            if (sceneObject == null) 
            { 
                return; 
            }
            sceneObject.Transform.Translate(-1 * worldPosition);
            sceneObject.Transform.Rotate(-1 * worldRotation);
            sceneObject.Transform.Scale(new Point3D(1 /sceneObject.Transform.scale.X, 1 / sceneObject.Transform.scale.Y, 1 / sceneObject.Transform.scale.Z));

            (Parent as MainForm).Render(); //TODO
        }

        private void textBoxName_TextChanged(object sender, EventArgs e)
        {
            if (sceneObject == null)
                return;
            sceneObject.Name = textBoxName.Text;
        }

        private void textBoxName_KeyUp(object sender, KeyEventArgs e)
        {
            (Parent as MainForm).UpdateHierarchy();
        }

        private void buttonColor_Click(object sender, EventArgs e)
        {
            if (sceneObject == null)
            {
                return;
            }
            if (colorDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            (sender as Button).BackColor = colorDialog1.Color;
            sceneObject.Color = colorDialog1.Color;
        }
    }
}
