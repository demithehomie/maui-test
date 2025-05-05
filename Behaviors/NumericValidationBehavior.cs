using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace BTGClientManager.Behaviors
{
    /// <summary>
    /// Permite digitar apenas números em um Entry.
    /// Se o texto não for numérico, pinta em vermelho.
    /// </summary>
    public class NumericValidationBehavior : Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.TextChanged += OnTextChanged;
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.TextChanged -= OnTextChanged;
        }

        void OnTextChanged(object? sender, TextChangedEventArgs e)
        {
            bool isValid = double.TryParse(e.NewTextValue, out _);
            if (sender is Entry entry)
            {
                entry.TextColor = isValid
                    ? Colors.Black
                    : Colors.Red;
            }
        }
    }
}
