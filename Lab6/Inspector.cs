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

        public void GetUpdate(object obj)
        {
            sceneObject = obj as SceneObject;
            if (sceneObject == null )
            {
                return;
            }
            textBoxName.Text = sceneObject.Name;
            textBoxPosX.Text = sceneObject.position.X.ToString("0.000");
            textBoxPosY.Text = sceneObject.position.Y.ToString("0.000");
            textBoxPosZ.Text = sceneObject.position.Z.ToString("0.000");

            textBoxRotationX.Text = sceneObject.rotation.X.ToString("0.000");
            textBoxRotationY.Text = sceneObject.rotation.Y.ToString("0.000");
            textBoxRotationZ.Text = sceneObject.rotation.Z.ToString("0.000");

            textBoxScaleX.Text = sceneObject.scale.X.ToString("0.000");
            textBoxScaleY.Text = sceneObject.scale.Y.ToString("0.000");
            textBoxScaleZ.Text = sceneObject.scale.Z.ToString("0.000");
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
            sceneObject.Translate(new Point3D(-sceneObject.position.X, -sceneObject.position.Y, -sceneObject.position.Z));
            sceneObject.Rotate(new Point3D(-sceneObject.rotation.X, -sceneObject.rotation.Y, -sceneObject.rotation.Z));
            sceneObject.Scale(new Point3D(1 /sceneObject.scale.X, 1 / sceneObject.scale.Y, 1 / sceneObject.scale.Z));

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
    }
}
