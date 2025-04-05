using Framework.UI;
using UnityEngine;

namespace Framework.Managers
{
    public class ScreenUIEntity : Entity, IScreenUIEntity
    {
        public bool IsVisible => this.gameObject.activeInHierarchy;

        protected virtual void Show()
        {
            this.gameObject.SetActive(true);
        }

        protected virtual void Hide()
        {
            this.gameObject.SetActive(false);
        }

        public void UpdateVisibility(bool visibility)
        {
            if (this.IsVisible)
            {
                if (!visibility)
                {
                    this.Hide();
                }
                else
                {
                    Debug.LogWarning($"{this.GetType().FullName}|{this.gameObject.name}: cannot show it's already visible");
                }
            }
            else
            {
                if (visibility)
                {
                    this.Show();
                }
                else
                {
                    Debug.LogWarning($"{this.GetType().FullName} | {this.gameObject.name}: cannot hide it's already visible");
                }
            }
        }
    }
}