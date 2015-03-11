using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using UIShared.Commands;

namespace UIShared.Controls
{
    /// <summary>
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class CommandsControl : UserControl
    {
         private void InitCommandsSource()
        {            
            ClearCommandsPanel();
            if (DataContext == null) return;
            BuildCommandsPanel();
        }
        
        class CommandPropertyInfo
        {
            public ApplicationCommandAttribute Attribute { get; set; }
            public string Name { get; set; }
        }

        private void BuildCommandsPanel()
        {
            IEnumerable<CommandPropertyInfo> commands = MakeCommandsList();
            CreateButtons(commands);
        }

        public Orientation Orientation
        {
            get { return Tray.Orientation; }
            set { Tray.Orientation = value; }
        }

        private void CreateButtons(IEnumerable<CommandPropertyInfo> commands)
        {
            foreach (var commandPropertyInfo in commands)
            {
                var btn = new Button
                    {
                        Margin = new Thickness(0, 0, 2, 0),
                        ToolTip = commandPropertyInfo.Attribute.ToolTip,
                        Command = CreateProxyCommand(commandPropertyInfo)
                    };
                var img = new Image
                    {Source = new BitmapImage(new Uri(commandPropertyInfo.Attribute.ImageResource, UriKind.Relative))};
                btn.Height = 26;
                btn.Content = img;
                var btnIndex = commandPanel.Items.Add(btn);
                if (commandPropertyInfo.Attribute.Separator != SeparatorType.None)
                {
                    var separator = new Separator() { Height = btn.Height / 2, Width = btn.Height,  };
                    switch (commandPropertyInfo.Attribute.Separator)
                    {
                        case SeparatorType.None:
                            break;
                        case SeparatorType.Before:
                            commandPanel.Items.Insert(btnIndex, separator);
                            break;
                        case SeparatorType.After:
                            commandPanel.Items.Add(separator);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                
            }
        }



        private ICommand CreateProxyCommand(CommandPropertyInfo commandPropertyInfo)
        {
            switch (commandPropertyInfo.Attribute.CmdType)
            {
                case AppCommandType.Silent:
                    return new CommandProxyCommand(commandPropertyInfo.Name, DataContext);
                case AppCommandType.Confirm:
                    return new ConfirmableCommandProxyCommand(commandPropertyInfo.Name, DataContext, commandPropertyInfo.Attribute.ConfirmMessage);
                default:
                    return new CommandProxyCommand(commandPropertyInfo.Name, DataContext);
            }
           
        }
        
        private IEnumerable<CommandPropertyInfo> MakeCommandsList()
        {
            var properties = DataContext.GetType().GetProperties();
            return from propertyInfo in properties let attributes = propertyInfo.GetCustomAttributes(true) 
                   from t in attributes 
                   let applicationCommandAttribute = t as ApplicationCommandAttribute
                   where applicationCommandAttribute != null 
                   select new CommandPropertyInfo()
                   {Attribute = applicationCommandAttribute, Name = propertyInfo.Name};
        }

        private void ClearCommandsPanel()
        {
            commandPanel.Items.Clear();
        }

        public CommandsControl()
        {
            InitializeComponent();
            DataContextChanged += CommandsControl_DataContextChanged;            
        }

        void CommandsControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            InitCommandsSource();
        }

        private void commandPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
    }
}
