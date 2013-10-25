using Khv.Engine.Structs;
using Khv.Gui.Components.BaseComponents.Containers.Components;
using Microsoft.Xna.Framework.Graphics;
using Khv.Gui.Components.BaseComponents.Containers.Components.Layout;

namespace Khv.Gui.Components.BaseComponents.Containers.Collections
{
    public class LayoutPanel : Control, ILayoutable
    {
        #region Vars
        protected ControlManager controlManager;
        private LayoutManager layout;
        #endregion

        #region Properties
        /// <summary>
        /// Asettaa layoutin. 
        /// Jos olemassa oleva layout on olemassa, niin käy huonosti.
        /// </summary>
        public LayoutManager Layout
        {
            // TODO: ehkä voisi swappia kontrollit uuteen?
            get
            {
                return layout;
            }
            set
            {
                layout = value;
                if (value != null)
                {
                    layout.Container = this;
                }
            }
        }
        /// <summary>
        /// Asettaa komponentin koon. 
        /// Jos on layout käytössä, validoi sen constrainien mukaan.
        /// ja korjaa koot oikeiksi. Kertoo myös parentille että sen tulisi
        /// valitoida itsensä.
        /// </summary>
        public override ControlSize Size
        {
            get
            {
                return base.Size;
            }
            set
            {
                // Jos ei ole koko layouttia niin ei tarvi valitoida mitään.
                if (layout == null)
                {
                    base.Size = value;
                }

                else if (NeedsToBeValidated(value, base.Size))
                {
                    // Tämä koko tuskin on oikea niin valitoidaan se constrainejen mukaan.
                    base.Size = value;
                    layout.Validate();

                    // Koska tämän containerin koko muuttui pitää myös parentin asetella se uudelleen constrainien mukaan.
                    if (Parent != null && Parent is ILayoutable)
                    {
                        (Parent as ILayoutable).Layout.Validate();
                    }

                }
            }
        }
        #endregion

        /// <summary>
        /// Luo uuden LayoutPanelin absolute (null) layoutilla.
        /// </summary>
        public LayoutPanel() : this(null)
        {
            
        }
        /// <summary>
        /// Luo uuden LayoutPanelin valitulla layoutilla.
        /// </summary>
        /// <param name="layout">Millainen layoutti laitetaan.</param>
        public LayoutPanel(LayoutManager layout)
        {
            Layout = layout;
            controlManager = new ControlManager(this);
            Visible = true;
            Enabled = true;
        }

        #region Methods
        /// <summary>
        /// Lisää uuden kontrollin
        /// Kutsuu AddControl(c, null) (olettaa absolute layouttia).
        /// </summary>
        /// <param name="c"></param>
        public void AddControl(Control c)
        {
            AddControl(c, null);
        }
        /// <summary>
        /// Lisää uuden kontrollin. 
        /// </summary>
        /// <param name="control">kontrolli joka lisätään.</param>
        /// <param name="constraints">kontrollin paikka minne lisätään (jos layout käytössä).</param>
        public void AddControl(Control control, ILayoutConstraints constraints)
        {
            // Ei ole absolute.
            if (Layout != null)
            {
                Layout.Add(controlManager, control, constraints);
            }
            else
            {
                controlManager.AddControl(control);
            }
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
            controlManager.Update(gameTime);

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            controlManager.Draw(spriteBatch);
        }
      

        /// <summary>
        /// Katsoo pitääkö valitoida layout koon perusteella.
        /// </summary>
        /// <param name="newSize"></param>
        /// <param name="oldSize"></param>
        /// <returns></returns>
        private bool NeedsToBeValidated(ControlSize newSize, ControlSize oldSize)
        {
            return (newSize.Width != oldSize.Width || newSize.Height != oldSize.Height);
        }
        #endregion
    }
}
