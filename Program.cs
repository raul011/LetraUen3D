using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Holamundo
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var gameLetra = new Game())
            {
                gameLetra.Run();
            }


        }


    }

    public class Game : GameWindow
    {
        public Escena escena;
        public float angleX = 0.0f;
        public float angleY = 0.0f;

        // Tamaño de la ventana
        public const int WindowWidth = 1000;
        public const int WindowHeight = 1000;

        public Game() : base(WindowWidth, WindowHeight, GraphicsMode.Default, "Tarea Hola mundo en openGL")
        {
            escena = new Escena();
            Inicializar_Escena();
        }

        private void Inicializar_Escena()
        {
            var objetoU = new Objeto3d();
            var parteU = CrearParteU();
            objetoU.Add(1, parteU);

            escena.Add(1, objetoU);
        }

        private Parte CrearParteU()
        {
            var parteU = new Parte();
            var poligonos = Vertice.CrearPoligonos();

            // PILAR IZQUIERDO
            parteU.Add(1, poligonos["caraFrontalIzq"]);
            parteU.Add(2, poligonos["caraTraseraIzq"]);
            parteU.Add(3, poligonos["caraSuperiorIzq"]);
            parteU.Add(4, poligonos["tapainfladoizq"]);
            parteU.Add(5, poligonos["tapaladoizq_pilar_izq"]);
            parteU.Add(6, poligonos["tapaladoder_pilar_izq"]);


            //PILAR DERECHO
            parteU.Add(7, poligonos["caraFrontalDer"]);
            parteU.Add(8, poligonos["caraTraseraDer"]);
            parteU.Add(9, poligonos["caraSuperiorDer"]);
            parteU.Add(10, poligonos["tapainfladoder"]);
            parteU.Add(11, poligonos["tapaladoizq_pilar_der"]);
            parteU.Add(12, poligonos["tapaladoder_pilar_der"]);
            //PILAR DE ABAJO
            parteU.Add(13, poligonos["caraBaseFrontal"]);
            parteU.Add(14, poligonos["caraBaseTrasera"]);
            parteU.Add(15, poligonos["caraBaseIzq"]);
            parteU.Add(16, poligonos["caraBaseDer"]);
            parteU.Add(17, poligonos["carasuelo"]);
            parteU.Add(18, poligonos["caraarriba"]);

            return parteU;
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            GL.ClearColor(0.1f, 0.1f, 0.1f, 1.0f);
            GL.Enable(EnableCap.DepthTest);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(-2, 2, -2, 2, -5, 5);  // para configurar la camara de cámara
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            // para la posicion de la letra
            GL.Translate(0.0f, -0.2f, -3.0f);
            GL.Rotate(angleX, 1.0, 0.0, 0.0);
            GL.Rotate(angleY, 0.0, 1.0, 0.0);

            escena.Draw();

            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            float rotationSpeed = 24.0f;
            angleX += rotationSpeed * (float)e.Time;
            angleY += rotationSpeed * (float)e.Time;
        }


    }

    public class Escena
    {
        private List<Objeto3d> objetos = new List<Objeto3d>();
        private List<int> ids = new List<int>();

        public void Add(int id, Objeto3d objeto)
        {
            int index = ids.IndexOf(id);
            if (index >= 0)
            {
                objetos[index] = objeto;
            }
            else
            {
                ids.Add(id);
                objetos.Add(objeto);
            }
        }
        
        public void Draw()
        {
            foreach (var objeto in objetos)
            {
                objeto.Dibujar();
            }
        }


    }

    public class Objeto3d
    {
        public List<Parte> partes = new List<Parte>();
        public List<int> ids = new List<int>();

        public void Add(int id, Parte parte)
        {
            int index = ids.IndexOf(id);
            if (index >= 0)
            {
                partes[index] = parte;
            }
            else
            {
                ids.Add(id);
                partes.Add(parte);
            }
        }

        public void Dibujar()
        {
            foreach (var parte in partes)
            {
                parte.Draw();
            }
        }


    }


    public class Parte
    {
        public List<Poligono> poligonos = new List<Poligono>();
        public List<int> ids = new List<int>();

        public void Add(int id, Poligono poligono)
        {
            int index = ids.IndexOf(id);
            if (index >= 0)
            {
                poligonos[index] = poligono;
            }
            else
            {
                ids.Add(id);
                poligonos.Add(poligono);
            }
        }

        public void Draw()
        {
            foreach (var poligono in poligonos)
            {
                poligono.Dibujar();
            }
        }
    }



    public class Poligono
    {
        private Dictionary<int, Vertice> vertices = new Dictionary<int, Vertice>();
        public Color4 color { get; set; }
        System.Drawing.Color miColor = System.Drawing.Color.Red;


        public void Add(int id, Vertice vertice)
        {
            vertices[id] = vertice;
        }

        public Vertice Get(int id)
        {
            return vertices.ContainsKey(id) ? vertices[id] : null;
        }

        public void Dibujar()
        {
            GL.Begin(PrimitiveType.Polygon);
            GL.Color4(color);
            foreach (var vertice in vertices.Values)
            {
                GL.Vertex3(vertice.X, vertice.Y, vertice.Z);
            }
            GL.End();
        }

    }

    public class Vertice
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public Vertice(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        private static Poligono CrearPoligonoConColor(Color4 color, params Vertice[] vertices)
        {
            var poligono = new Poligono();
            for (int i = 0; i < vertices.Length; i++)
            {
                poligono.Add(i, vertices[i]);
            }
            poligono.color = color;
            return poligono;
        }

        public static Dictionary<string, Poligono> CrearPoligonos()
        {
            var poligonos = new Dictionary<string, Poligono>();

            // Colores
            var color1 = new Color4(1.0f, 1.0f, 0.0f, 1.0f); // Amarillo
            var color2 = new Color4(1.0f, 0.0f, 0.0f, 0.0f); // Rojo
            var color3 = new Color4(0.0f, 1.0f, 0.0f, 0.0f); // verde
            var color_negro = new Color4(0.0f, 0.0f, 0.0f, 0.7f); // negro
            var color_azul = new Color4(0.0f, 0.0f, 1.0f, 1.0f); // azul
            var color_turquesa = new Color4(0.01f, 0.68f, 0.98f, 1.0f); // Turquesa
            var color_azul_vibrante = new Color4(0.1f, 0.3f, 1.0f, 1.0f); // Azul vibrante (#5363FB)
            var colorAzulTransparente = new Color4(0.0f, 0.0f, 1.0f, 0.3f); // Azul con 30% de transparencia

            // Pilares de la Letra
            // Pilar izquierdo

            //IA1,IB1,IC1,ID1,IA2,IB2  etc....  SON MIS PUNTOS X,Y,Z CREADOS EN GEOGEBRA PARA GUIARMA AL ARMAR LA LETRA
            poligonos["caraFrontalIzq"] = CrearPoligonoConColor(
                color_azul_vibrante,
                new Vertice(-0.7f, 0.8f, 0.0f), //VA1
                new Vertice(-0.7f, -0.8f, 0.0f), //VB1
                new Vertice(-0.4f, -0.8f, 0.0f), //VC1
                new Vertice(-0.4f, 0.8f, 0.0f)); //VD1

            poligonos["caraTraseraIzq"] = CrearPoligonoConColor(
                color_azul_vibrante,
                new Vertice(-0.7f, 0.8f, 0.3f),//VA2
                new Vertice(-0.7f, -0.8f, 0.3f),//VB2
                new Vertice(-0.4f, -0.8f, 0.3f),//VC2
                new Vertice(-0.4f, 0.8f, 0.3f));//VD2

            poligonos["caraSuperiorIzq"] = CrearPoligonoConColor(
                color_turquesa,
                new Vertice(-0.7f, 0.8f, 0.0f), //VA1
                new Vertice(-0.7f, 0.8f, 0.3f), //VA2
                new Vertice(-0.4f, 0.8f, 0.3f), //VD2
                new Vertice(-0.4f, 0.8f, 0.0f)); //VD1
            poligonos["tapainfladoizq"] = CrearPoligonoConColor(
                color2,
                new Vertice(-0.7f, -0.8f, 0.0f), //IA1
                new Vertice(-0.4f, -0.8f, 0.0f), //VC1
                new Vertice(-0.4f, -0.8f, 0.3f), //VC2
                new Vertice(-0.7f, -0.8f, 0.3f)); //VB2

            //tapaladoizq_pilar_izq
            poligonos["tapaladoizq_pilar_izq"] = CrearPoligonoConColor(
                color_azul,
                new Vertice(-0.7f, 0.8f, 0.0f), //VA1
                new Vertice(-0.7f, 0.8f, 0.3f), //VA2
                new Vertice(-0.7f, -0.8f, 0.3f), //VB2
                new Vertice(-0.7f, -0.8f, 0.0f)); //IA1

            //tapaladoder_pilar_izq
            poligonos["tapaladoder_pilar_izq"] = CrearPoligonoConColor(
                color_negro,
                new Vertice(-0.4f, 0.8f, 0.0f), //VD1
                new Vertice(-0.4f, 0.8f, 0.3f), //VD2
                new Vertice(-0.4f, -0.8f, 0.3f), //VC2
                new Vertice(-0.4f, -0.8f, 0.0f)); //VC1

            // Pilar derechoPILAR DERECHO  ---------PILAR DERECHO --PILAR DERECHO
            poligonos["caraFrontalDer"] = CrearPoligonoConColor(
                color1,
                new Vertice(0.4f, 0.8f, 0.0f), //VD3
                new Vertice(0.7f, 0.8f, 0.0f), //VA3
                new Vertice(0.7f, -0.8f, 0.0f), //VB3
                new Vertice(0.4f, -0.8f, 0.0f)); //VC3

            poligonos["caraTraseraDer"] = CrearPoligonoConColor(
                color_azul_vibrante,
                new Vertice(0.4f, 0.8f, 0.3f), //VD4
                new Vertice(0.7f, 0.8f, 0.3f), //VA4
                new Vertice(0.7f, -0.8f, 0.3f), //VB4
                new Vertice(0.4f, -0.8f, 0.3f)); //VC4

            poligonos["caraSuperiorDer"] = CrearPoligonoConColor(
                color_turquesa,
                new Vertice(0.7f, 0.8f, 0.0f), //VA3
                new Vertice(0.4f, 0.8f, 0.0f), //VD3
                new Vertice(0.4f, 0.8f, 0.3f), //VD4
                new Vertice(0.7f, 0.8f, 0.3f)); //VA4

            poligonos["tapainfladoder"] = CrearPoligonoConColor(
                color2,
                new Vertice(0.4f, -0.8f, 0.3f), //VC4
                new Vertice(0.4f, -0.8f, 0.0f), //VC3
                new Vertice(0.7f, -0.8f, 0.0f), //VB3
                new Vertice(0.7f, -0.8f, 0.3f)); //VB4

            //tapaladoizq_pilar_der
            poligonos["tapaladoizq_pilar_der"] = CrearPoligonoConColor(
                color_negro,
                new Vertice(0.4f, 0.8f, 0.0f), //VD3
                new Vertice(0.4f, 0.8f, 0.3f), //VD4
                new Vertice(0.4f, -0.8f, 0.3f), //VC4
                new Vertice(0.4f, -0.8f, 0.0f));//VC3

            //tapaladoder_pilar_der
            poligonos["tapaladoder_pilar_der"] = CrearPoligonoConColor(
                color_azul,
                new Vertice(0.7f, 0.8f, 0.3f), //VA4
                new Vertice(0.7f, 0.8f, 0.0f), //VA3
                new Vertice(0.7f, -0.8f, 0.0f), //VB3
                new Vertice(0.7f, -0.8f, 0.3f)); //VB4

            // Base de la "U" PILAR DE ABAJO
            poligonos["caraBaseFrontal"] = CrearPoligonoConColor(
                color_azul_vibrante,
                new Vertice(-0.7f, -1.1f, 0.3f),//ID2  
                new Vertice(0.7f, -1.1f, 0.3f), //IC2
                new Vertice(0.7f, -0.8f, 0.3f), //IB2
                new Vertice(-0.7f, -0.8f, 0.3f) //IA2
            );

            poligonos["caraBaseTrasera"] = CrearPoligonoConColor(
                color_azul_vibrante,
                new Vertice(-0.7f, -0.8f, 0.0f), //IA1
                new Vertice(0.7f, -0.8f, 0.0f),  //IB1
                new Vertice(0.7f, -1.1f, 0.0f),  //IC1
                new Vertice(-0.7f, -1.1f, 0.0f)  //ID1
            );
            poligonos["caraarriba"] = CrearPoligonoConColor(
               color_turquesa,
               new Vertice(-0.7f, -0.8f, 0.0f), //VB1
               new Vertice(-0.7f, -0.8f, 0.3f), //IA2
               new Vertice(0.7f, -0.8f, 0.3f), //IB2
               new Vertice(0.7f, -0.8f, 0.0f)); //IB1

            //tapa abajo base (suelo)
            poligonos["carasuelo"] = CrearPoligonoConColor(
                color_negro,
                new Vertice(-0.7f, -1.1f, 0.3f),//ID2 
                new Vertice(0.7f, -1.1f, 0.3f), //IC2
                new Vertice(0.7f, -1.1f, 0.0f),  //IC1
                new Vertice(-0.7f, -1.1f, 0.0f));  //ID1

            // Caras laterales de la base (parte interna)
            poligonos["caraBaseIzq"] = CrearPoligonoConColor(
                colorAzulTransparente,
                new Vertice(-0.7f, -0.8f, 0.3f), //IA2
                new Vertice(-0.7f, -1.1f, 0.3f),//ID2 
                new Vertice(-0.7f, -1.1f, 0.0f),  //ID1
                new Vertice(-0.7f, -0.8f, 0.0f)); //IA1

            poligonos["caraBaseDer"] = CrearPoligonoConColor(
                colorAzulTransparente,
                new Vertice(0.7f, -1.1f, 0.3f), //IC2
                new Vertice(0.7f, -0.8f, 0.3f), //IB2
                new Vertice(0.7f, -0.8f, 0.0f),  //IB1
                new Vertice(0.7f, -1.1f, 0.0f));  //IC1

            return poligonos;
        }
    }


}
