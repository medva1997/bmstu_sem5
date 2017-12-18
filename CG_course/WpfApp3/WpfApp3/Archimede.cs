
using System.Windows.Media.Media3D;

namespace WpfApp3
{
    class Archimede
    {
        private readonly Model _model;
        private readonly WaterGenerator _water;

        public Archimede(Model m, WaterGenerator w)
        {
            _model = m;
            _water = w;
        }

        public Transform3D LoadWaterMatrix()
        {
            int x =(int) (_model.GetTransformdBounds.X);
            int z = (int)(_model.GetTransformdBounds.Z);
            int y = (int)(_model.GetTransformdBounds.Y);
            int sx = (int)(_model.GetTransformdBounds.SizeX);
            int sz = (int)(_model.GetTransformdBounds.SizeZ);
            int sy = (int)(_model.GetTransformdBounds.SizeY);
            //Point3D[,] waterMatrix= new Point3D[(int)(_model.GetTransformdBounds.SizeX), (int)(_model.GetTransformdBounds.SizeZ)];

            double avgh=0.0;
            for (int i = 0; i < sx; i++)
            {
                for (int j = 0; j < sz; j++)
                {
                    //waterMatrix[i, j] = _water.GetWaterHeightPoint(x + i, z + j);
                    avgh += _water.GetWaterHeightPoint(x + i, z + j).Y;
                }
            }

            //avgh = _water.GetWaterHeightPoint(x, z).Y +
            //       _water.GetWaterHeightPoint(x+sx, z).Y +
            //       _water.GetWaterHeightPoint(x, z+sz).Y +
            //       _water.GetWaterHeightPoint(x+sx, z+sz).Y+0;
                   
            avgh =avgh/(sx * sz);
            //avgh = avgh / 4;
            double current = y + 0.25 * sy;
            TranslateTransform3D tr = new TranslateTransform3D(0, avgh-current, 0);
            return tr;
        }
    }

}
