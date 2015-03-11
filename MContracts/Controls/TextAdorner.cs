using System.Globalization;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using CommonBase;

namespace MContracts.Controls.PlaceHolder
{
	public class TextAdorner : Adorner
	{
		#region Private fileds
		private readonly Point m_anchor = new Point( 7, 1 );

		private FormattedText m_formattedText;
		#endregion

		#region Initialization
		/// <summary>
		/// Initializes a new instance of the <see cref="TextAdorner"/> class.
		/// </summary>
		/// <param name="adornedElement">The element to bind the adorner to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		/// Raised when adornedElement is null.
		/// </exception>
		public TextAdorner( TextBoxEx adornedElement )
			: base( adornedElement )
		{
			UpdateFormattedText( adornedElement.PlaceHolderText );
		}
		#endregion

		#region Public methods
		/// <summary>
		/// Updates the formatted text.
		/// </summary>
		/// <param name="placeHolderText">The place holder text.</param>
		public void UpdateFormattedText( string placeHolderText )
		{
            
			m_formattedText = new FormattedText(
				placeHolderText.Return(x=>x, "Введите текст")
				, CultureInfo.CurrentUICulture
				, FlowDirection
				, new Typeface( "Verdana" ), 11
				, new SolidColorBrush( new Color
				{
					A = 150,
					R = 128,
					G = 128,
					B = 128,
				} ) );
		}
		#endregion

		#region Implementation
		/// <summary>
		/// When overridden in a derived class, participates in rendering operations that are directed by the layout system. The rendering instructions for this element are not used directly when this method is invoked, and are instead preserved for later asynchronous use by layout and drawing.
		/// </summary>
		/// <param name="drawingContext">The drawing instructions for a specific element. This context is provided to the layout system.</param>
		protected override void OnRender( DrawingContext drawingContext )
		{
            if (this.ActualHeight > 0)
                drawingContext.DrawText( m_formattedText, m_anchor );
		}
		#endregion
	}
}