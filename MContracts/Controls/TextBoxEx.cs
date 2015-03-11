using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace MContracts.Controls.PlaceHolder
{
	public class TextBoxEx : TextBox, IDisposable
	{
		#region Private fields
		private readonly TextAdorner m_textAdorner;
		
		private AdornerLayer m_alSingle;

		#endregion

		#region Initialization
		/// <summary>
		/// Initializes a new instance of the <see cref="TextBoxEx"/> class.
		/// </summary>
		public TextBoxEx()
		{
			m_textAdorner = new TextAdorner( this );
			Loaded += new RoutedEventHandler( OnLoaded );
		}
		#endregion

    
		#region Properties
		/// <summary>
		/// Gets or sets the place holder text.
		/// </summary>
		/// <value>The place holder text.</value>
		public string PlaceHolderText
		{
			get
			{
				return (string)GetValue( PlaceHolderTextProperty );
			}
			set
			{
				SetValue( PlaceHolderTextProperty, value );
			}
		}

	    #endregion 

		#region IDisposable Members
		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			Loaded -= new RoutedEventHandler( OnLoaded );
		}
		#endregion

		#region Implementation
		/// <summary>
		/// Called when [place holder text cahgned].
		/// </summary>
		/// <param name="d">The d.</param>
		/// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
		protected virtual void OnPlaceHolderTextCahgned( DependencyPropertyChangedEventArgs e )
		{
			m_textAdorner.UpdateFormattedText( PlaceHolderText );
		}

		/// <summary>
		/// Invoked when an unhandled <see cref="E:System.Windows.Input.Mouse.MouseEnter"/> attached event is raised on this element. Implement this method to add class handling for this event.
		/// </summary>
		/// <param name="e">The <see cref="T:System.Windows.Input.MouseEventArgs"/> that contains the event data.</param>
		protected override void OnMouseEnter( MouseEventArgs e )
		{
			base.OnMouseEnter(e);
			RemoveAddorner();
		}
		/// <summary>
		/// Invoked when an unhandled <see cref="E:System.Windows.Input.Mouse.MouseLeave"/> attached event is raised on this element. Implement this method to add class handling for this event.
		/// </summary>
		/// <param name="e">The <see cref="T:System.Windows.Input.MouseEventArgs"/> that contains the event data.</param>
		protected override void OnMouseLeave( MouseEventArgs e )
		{
			base.OnMouseLeave( e );
			AddAddorner();
		}
		/// <summary>
		/// Is called when content in this editing control changes.
		/// </summary>
		/// <param name="e">The arguments that are associated with the <see cref="E:System.Windows.Controls.Primitives.TextBoxBase.TextChanged"/> event.</param>
		protected override void OnTextChanged( TextChangedEventArgs e )
		{
			base.OnTextChanged( e );

			if ( !string.IsNullOrEmpty( Text ) )
			{
				RemoveAddorner();
			}
			else if ( !IsMouseOver )
			{
				AddAddorner();
			}
		}

		/// <summary>
		/// Called when [loaded]
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
		private void OnLoaded( object sender, RoutedEventArgs e )
		{
            m_alSingle = AdornerLayer.GetAdornerLayer(this);
            AddAddorner();
		}
   
		/// <summary>
		/// Adds the addorner.
		/// </summary>
		private void AddAddorner()
		{
            if ( string.IsNullOrEmpty( Text ) )
			{
                if (m_alSingle.GetAdorners(this) == null)
                    m_alSingle.Add(m_textAdorner);
			}
		}

		/// <summary>
		/// Removes the addorner.
		/// </summary>
		private void RemoveAddorner()
		{
            var adorners = m_alSingle.GetAdorners(this);

			if ( null != adorners && 0 < adorners.Length )
			{
                m_alSingle.Remove(adorners[0]);
			}
		}

		/// <summary>
		/// Called when [place holder text cahgned].
		/// </summary>
		/// <param name="d">The d.</param>
		/// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
		private static void OnPlaceHolderTextCahgned( DependencyObject d, DependencyPropertyChangedEventArgs e )
		{
			var instance = (TextBoxEx) d;
			instance.OnPlaceHolderTextCahgned( e );
		}
		#endregion

		#region Dependency properties
		public static readonly DependencyProperty PlaceHolderTextProperty =
			DependencyProperty.Register( "PlaceHolderText"
			, typeof( string )
			, typeof( TextBoxEx )
			, new PropertyMetadata( "type here.."
				, new PropertyChangedCallback( OnPlaceHolderTextCahgned ) ) );
		#endregion
	}
}