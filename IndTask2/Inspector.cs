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

namespace IndTask2
{
    public partial class Inspector : UserControl
    {
        private SceneObject sceneObject;

        public void GetUpdate(object obj)
        {
            sceneObject = obj as SceneObject;
            if (sceneObject == null)
            {
                return;
            }
            textBoxName.Text = sceneObject.Name;
            textBoxPosX.Text = sceneObject.Transform.position.X.ToString("0.000");
            textBoxPosY.Text = sceneObject.Transform.position.Y.ToString("0.000");
            textBoxPosZ.Text = sceneObject.Transform.position.Z.ToString("0.000");

            textBoxRotationX.Text = sceneObject.Transform.rotation.X.ToString("0.000");
            textBoxRotationY.Text = sceneObject.Transform.rotation.Y.ToString("0.000");
            textBoxRotationZ.Text = sceneObject.Transform.rotation.Z.ToString("0.000");

            textBoxScaleX.Text = sceneObject.Transform.scale.X.ToString("0.000");
            textBoxScaleY.Text = sceneObject.Transform.scale.Y.ToString("0.000");
            textBoxScaleZ.Text = sceneObject.Transform.scale.Z.ToString("0.000");
        }

        public Inspector()
        {
            InitializeComponent();
        }

        private void buttonToDefault_Click(object sender, EventArgs e)
        {
            if (sceneObject == null)
            {
                return;
            }
            sceneObject.Transform.Translate(new Point3D(-sceneObject.Transform.position.X, -sceneObject.Transform.position.Y, -sceneObject.Transform.position.Z));
            sceneObject.Transform.Rotate(new Point3D(-sceneObject.Transform.rotation.X, -sceneObject.Transform.rotation.Y, -sceneObject.Transform.rotation.Z));
            sceneObject.Transform.Scale(new Point3D(1 / sceneObject.Transform.scale.X, 1 / sceneObject.Transform.scale.Y, 1 / sceneObject.Transform.scale.Z));

            //(Parent as MainForm).Render(); //TODO
        }

        private void textBoxName_TextChanged(object sender, EventArgs e)
        {
            if (sceneObject == null)
                return;
            sceneObject.Name = textBoxName.Text;
        }

        private void textBoxName_KeyUp(object sender, KeyEventArgs e)
        {
            //(Parent as MainForm).UpdateHierarchy();
        }
    }
}
