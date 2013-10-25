using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Khv.Gui.Effects
{
    public class EffectContainer
    {
        #region Vars
        private List<Effect> allEffects;
        private List<Effect> notDrawableEffects;
        private List<DrawableEffect> drawableEffects;
        #endregion

        #region Properties
        public bool ContainsOverridingEffects
        {
            get;
            private set;
        }
        public List<Effect> AllEffects
        {
            get
            {
                return allEffects;
            }
        }
        public List<DrawableEffect> DrawableEffects
        {
            get
            {
                return drawableEffects;
            }
        }
        public List<Effect> NotDrawableEffects
        {
            get
            {
                return notDrawableEffects;
            }
        }
        #endregion

        public EffectContainer()
        {
            allEffects = new List<Effect>();
            notDrawableEffects = new List<Effect>();
            drawableEffects = new List<DrawableEffect>();

            ContainsOverridingEffects = false;
        }

        // Poistee efektin kaikista listoista ja 
        // päivittää ContainsOverridingEffects propertyn valueta.
        private void RemoveFromLists(Effect effect)
        {
            allEffects.Remove(effect);

            if (effect is DrawableEffect)
            {
                DrawableEffect drawable = (DrawableEffect)effect;

                drawableEffects.Remove(drawable);

                foreach (DrawableEffect drawableEffect in drawableEffects)
                {
                    if (drawableEffect.IsOverridingDraw)
                    {
                        ContainsOverridingEffects = true;
                        break;
                    }

                    ContainsOverridingEffects = false;
                }
            }
            else
            {
                notDrawableEffects.Remove(effect);
            }
        }
        /// <summary>
        /// Lisää uuden efektin cointaineriin.
        /// </summary>
        public void AddEffects(Effect effect)
        {
            // Kästätään suoraan drawableksi, palauttaa nullin jos tyyppi ei ole oikea.
            DrawableEffect drawableEffect = effect as DrawableEffect;
            
            allEffects.Add(effect);

            // Lisätään efekti oikeaan listaan.
            if (drawableEffect != null)
            {
                drawableEffects.Add(drawableEffect);
            }
            else
            {
                notDrawableEffects.Add(effect);
            }

            if (drawableEffect != null && drawableEffect.IsOverridingDraw)
            {
                ContainsOverridingEffects = true;
            }
        }
        /// <summary>
        /// Remove metodi jolla poistetaan drawable efektejä.
        /// </summary>
        public void RemoveEffect(Predicate<DrawableEffect> predicate)
        {
            DrawableEffect effect = drawableEffects.Find(e => predicate(e));

            if (effect == null)
            {
                throw new NullReferenceException("Drawable effect with given predicate was not found.");
            }
            else
            {
                RemoveFromLists(effect);
            }
        }
        /// <summary>
        /// Remove metodi jolla poistetaan normaaleja efektejä.
        /// </summary>
        public void RemoveEffect(Predicate<Effect> predicate)
        {
            Effect effect = notDrawableEffects.Find(e => predicate(e));

            if (effect == null)
            {
                throw new NullReferenceException("Effect with given predicate was not found.");
            }
            else
            {
                RemoveFromLists(effect);
            }
        }
    }
}
