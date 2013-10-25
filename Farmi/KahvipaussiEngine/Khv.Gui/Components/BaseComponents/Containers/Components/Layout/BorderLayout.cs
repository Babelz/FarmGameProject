using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Khv.Gui.Components.BaseComponents.Containers.Components.Layout
{
    /// <summary>
    /// Layout jolla on helppo tehdä rakenne
    /// Constraineja ovat Center, Up, Down, Left, Right
    /// Kun komponentin koko asetetaan uudelleen, layout pyrkii säilyttämään 
    /// komponentit oikeilla paikoillaan
    /// </summary>
    public class BorderLayout : LayoutManager
    {
        #region Vars
        private readonly static BorderLayoutConstraint center = new BorderLayoutConstraint("Center");
        private readonly static BorderLayoutConstraint left = new BorderLayoutConstraint("Left");
        private readonly static BorderLayoutConstraint up = new BorderLayoutConstraint("Up");
        private readonly static BorderLayoutConstraint down = new BorderLayoutConstraint("Down");
        private readonly static BorderLayoutConstraint right = new BorderLayoutConstraint("Right");
        private readonly Dictionary<string, PositioningDelegate> invokeList = new Dictionary<string, PositioningDelegate>();

        private Dictionary<string, Control> slots = new Dictionary<string, Control>();
        #endregion

        #region Properties
        /// <summary>
        /// Apuri, käskee kontrollin mennä keskelle
        /// Imee kaiken tilan mitä mahdollista
        /// </summary>
        public static BorderLayoutConstraint Center
        {
            get { return center; }
        }

        /// <summary>
        /// Käskee kontrollin mennä vasemmalle
        /// Korkeuden asettaminen ei vaikuta, koska tarvitaan vain leveys
        /// </summary>
        public static BorderLayoutConstraint Left
        {
            get { return left; }
        }

        /// <summary>
        /// Käskee kontrollin mennä ylös
        /// Leveyden asettamisella ei ole mitään vaikutusta, koska tarvitaan
        /// vain korkeus
        /// </summary>
        public static BorderLayoutConstraint Up
        {
            get { return up; }
        }

        /// <summary>
        /// Käskee kontrollin mennä alas
        /// Leveyden asettamisella ei ole mitään vaikutusta, koska tarvitaan
        /// vain korkeus
        /// </summary>
        public static BorderLayoutConstraint Down
        {
            get { return down; }
        }

        /// <summary>
        /// Käskee kontrollin mennä oikealle
        /// Korkeuden asettaminen ei vaikuta, koska tarvitaan vain leveys
        /// </summary>
        public static BorderLayoutConstraint Right
        {
            get { return right; }
        }
        #endregion

        #region Constraints
        /// <summary>
        /// Asettaa keskikomponentin keskelle
        /// </summary>
        /// <param name="c"></param>
        /// <param name="b"></param>
        private void CenterConstraint(Control c, BorderLayoutConstraint b)
        {
            Point position = new Point();

            if (slots.ContainsKey(left.Direction))
            {
                Control leftArea = slots[left.Direction];

                position.X = leftArea.Position.Relative.X + leftArea.Size.Width;

            }

            if (slots.ContainsKey(up.Direction))
            {
                Control upArea = slots[up.Direction];
                position.Y = upArea.Position.Relative.Y + upArea.Size.Height;
            }

            ControlSize size = new ControlSize(container.Size.Width - position.X, container.Size.Height - position.Y, SizeType.Fixed);

            if (slots.ContainsKey(right.Direction))
            {
                Control rightArea = slots[right.Direction];
                ControlSize temp = new ControlSize(size.Width - rightArea.Size.Width, size.Height, SizeType.Fixed);
                size = temp;
            }

            if (slots.ContainsKey(down.Direction))
            {
                Control downArea = slots[down.Direction];
                ControlSize temp = new ControlSize(size.Width, size.Height - downArea.Size.Height, SizeType.Fixed);
                size = temp;
            }
            c.Position.Relative = position;
            c.Size = size;

            slots[b.Direction] = c;
        }

        /// <summary>
        /// Asettelee vasemman komponentin vasemmalle
        /// </summary>
        /// <param name="control"></param>
        /// <param name="constraint"></param>
        private void LeftConstraint(Control control, BorderLayoutConstraint constraint)
        {
            Point position = new Point();

            if (slots.ContainsKey(up.Direction))
            {
                Control upArea = slots[up.Direction];
                position.Y = upArea.Size.Height;
            }
            ControlSize size = new ControlSize(control.Size.Width, container.Size.Height - position.Y);
            if (slots.ContainsKey(down.Direction))
            {
                Control downArea = slots[down.Direction];
                size = new ControlSize(size.Width, container.Size.Width - (position.Y - downArea.Size.Height));
            }
            control.Position.Relative = position;
            control.Size = size;

            slots[constraint.Direction] = control;
        }

        /// <summary>
        /// Asettaa yläkomponentin ylös
        /// </summary>
        /// <param name="control"></param>
        /// <param name="constraint"></param>
        private void UpConstraint(Control control, BorderLayoutConstraint constraint)
        {
            Point position = new Point();

            if (slots.ContainsKey(left.Direction))
            {
                Control leftArea = slots[left.Direction];
                position.X = leftArea.Size.Width;
            }

            ControlSize size = new ControlSize(container.Size.Width - position.X, control.Size.Height);

            if (slots.ContainsKey(right.Direction))
            {
                Control rightArea = slots[right.Direction];
                ControlSize temp = new ControlSize(container.Size.Width - (position.X - rightArea.Size.Width), size.Height);
                size = temp;
            }

            control.Size = size;
            control.Position.Relative = position;

            slots[constraint.Direction] = control;
        }

        /// <summary>
        /// Asettelee oikean komponentin oikealle
        /// </summary>
        /// <param name="control"></param>
        /// <param name="constraint"></param>
        private void RightConstraint(Control control, BorderLayoutConstraint constraint)
        {
            Point position = new Point();
            position.X = container.Size.Width - control.Size.Width;

            if (slots.ContainsKey(up.Direction))
            {
                Control upArea = slots[up.Direction];
                position.Y = upArea.Size.Height;
            }
            ControlSize size = new ControlSize(control.Size.Width, container.Size.Height - position.Y);
            if (slots.ContainsKey(down.Direction))
            {
                Control downArea = slots[down.Direction];
                ControlSize temp = new ControlSize(size.Width, container.Size.Height - position.Y - downArea.Size.Height);
                size = temp;
            }
            control.Size = size;
            control.Position.Relative = position;

            slots[constraint.Direction] = control;

        }

        /// <summary>
        /// Asettelee alakomponentin oikealle paikalle
        /// </summary>
        /// <param name="control"></param>
        /// <param name="constraint"></param>
        private void DownConstraint(Control control, BorderLayoutConstraint constraint)
        {
            Point position = new Point();
            position.Y = container.Size.Height - control.Size.Height;

            if (slots.ContainsKey(left.Direction))
            {
                Control leftArea = slots[left.Direction];
                position.X = leftArea.Size.Width;
            }

            ControlSize size = new ControlSize(container.Size.Width - position.X, control.Size.Height);

            if (slots.ContainsKey(right.Direction))
            {
                Control rightArea = slots[right.Direction];
                ControlSize temp = new ControlSize(size.Width - rightArea.Size.Width, size.Height);
                size = temp;
            }

            control.Size = size;
            control.Position.Relative = position;

            slots[constraint.Direction] = control;
        }

        public void RemoveComponent(BorderLayoutConstraint constraint)
        {
            slots.Remove(constraint.Direction);
        }

        /// <summary>
        /// Luo uuden BorderLayoutin
        /// </summary>
        public BorderLayout()
        {
            invokeList.Add(center.Direction, CenterConstraint);
            invokeList.Add(left.Direction, LeftConstraint);
            invokeList.Add(up.Direction, UpConstraint);
            invokeList.Add(down.Direction, DownConstraint);
            invokeList.Add(right.Direction, RightConstraint);
        }

        /// <summary>
        /// Validoi layoutin, korjaa kaikki päällekkäisyydet
        /// </summary>
        public override void Validate()
        {
            foreach (KeyValuePair<string, PositioningDelegate> pair in invokeList.Where(i => i.Key != center.Direction))
            {
                if (slots.ContainsKey(pair.Key))
                {
                    //Console.WriteLine(pair.Key);
                    pair.Value.Invoke(slots[pair.Key], new BorderLayoutConstraint(pair.Key));
                }
            }

            ReAlignCenter();
        }

        /// <summary>
        /// Asettelee keskellä olevan komponentin EHKÄ uudelleen
        /// </summary>
        private void ReAlignCenter()
        {
            if (slots.ContainsKey(center.Direction))
                CenterConstraint(slots[center.Direction], BorderLayout.Center);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Lisää kontrollin constrainien perusteella oikealle paikalle
        /// Asettelee komponentit uudelleen jos tulevat päällekkäin
        /// </summary>
        /// <param name="controlManager">Mihin controlmanageriin lisätään</param>
        /// <param name="controlToAdd">Mikä kontrolli lisätään</param>
        /// <param name="constraints">Mihin paikkaan layoutissa (Center, Left, Right, Up, Down)</param>
        public override void Add(ControlManager controlManager, Control controlToAdd, ILayoutConstraints constraints)
        {
            if (constraints != null && constraints is BorderLayoutConstraint)
            {
                BorderLayoutConstraint constraint = (constraints as BorderLayoutConstraint);
                invokeList[constraint.Direction].Invoke(controlToAdd, constraint);
                controlManager.AddControl(controlToAdd);

                ReAlignCenter();
            }
        }
        #endregion

        delegate void PositioningDelegate(Control control, BorderLayoutConstraint constraint);
    }
    public class BorderLayoutConstraint : ILayoutConstraints
    {
        #region Properties
        public string Direction
        {
            get;
            private set;
        }
        #endregion

        public BorderLayoutConstraint(string dir)
        {
            Direction = dir;
        }
    }
}
