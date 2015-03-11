using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Windows;

namespace MContracts
{
    public sealed class AppMessageBox
    {

        public static string ApplicationName { get; set; }

        // Сводка:
        //     Отображает окно сообщения с сообщением, которое возвращает результат.
        //
        // Параметры:
        //   messageBoxText:
        //     Строка System.String, задающая отображаемый текст.
        //
        // Возвращает:
        //     Значение System.Windows.MessageBoxResult, идентифицирующее кнопку, нажатую
        //     пользователем в окне сообщения.
        [SecurityCritical]
        public static MessageBoxResult Show(string messageBoxText)
        {
            return MessageBox.Show(messageBoxText, ApplicationName);
        }

       
        //
        // Сводка:
        //     Отображает окно сообщения с сообщением, заголовком и кнопкой, которое возвращает
        //     результат.
        //
        // Параметры:
        //   messageBoxText:
        //     Строка System.String, задающая отображаемый текст.
        //
        //   caption:
        //     Строка System.String, задающая отображаемый заголовок окна.
        //
        //   button:
        //     Значение System.Windows.MessageBoxButton, определяющее, какие кнопки (кнопка)
        //     подлежат отображению.
        //
        // Возвращает:
        //     Значение System.Windows.MessageBoxResult, идентифицирующее кнопку, нажатую
        //     пользователем в окне сообщения.
        [SecurityCritical]
        public static MessageBoxResult Show(string messageBoxText, MessageBoxButton button)
        {
            return MessageBox.Show(messageBoxText, ApplicationName, button);
        }

       
        //
        // Сводка:
        //     Отображает окно сообщения с сообщением, заголовком, кнопкой и значком, которое
        //     возвращает результат.
        //
        // Параметры:
        //   messageBoxText:
        //     Строка System.String, задающая отображаемый текст.
        //
        //   caption:
        //     Строка System.String, задающая отображаемый заголовок окна.
        //
        //   button:
        //     Значение System.Windows.MessageBoxButton, определяющее, какие кнопки (кнопка)
        //     подлежат отображению.
        //
        //   icon:
        //     Значение System.Windows.MessageBoxImage, задающее отображаемый значок.
        //
        // Возвращает:
        //     Значение System.Windows.MessageBoxResult, идентифицирующее кнопку, нажатую
        //     пользователем в окне сообщения.
        [SecurityCritical]
        public static MessageBoxResult Show(string messageBoxText, MessageBoxButton button,
                                            MessageBoxImage icon)
        {
            return MessageBox.Show(messageBoxText, ApplicationName, button, icon);
        }

        //
        // Сводка:
        //     Отображает окно сообщения с сообщением, заголовком, кнопкой и значком, которое
        //     принимает результат окна сообщения по умолчанию и возвращает результат.
        //
        // Параметры:
        //   messageBoxText:
        //     Строка System.String, задающая отображаемый текст.
        //
        //   caption:
        //     Строка System.String, задающая отображаемый заголовок окна.
        //
        //   button:
        //     Значение System.Windows.MessageBoxButton, определяющее, какие кнопки (кнопка)
        //     подлежат отображению.
        //
        //   icon:
        //     Значение System.Windows.MessageBoxImage, задающее отображаемый значок.
        //
        //   defaultResult:
        //     Значение System.Windows.MessageBoxResult, задающее результат окна сообщений
        //     по умолчанию.
        //
        // Возвращает:
        //     Значение System.Windows.MessageBoxResult, идентифицирующее кнопку, нажатую
        //     пользователем в окне сообщения.
        [SecurityCritical]
        public static MessageBoxResult Show(string messageBoxText, MessageBoxButton button,
                                            MessageBoxImage icon, MessageBoxResult defaultResult)
        {
            return MessageBox.Show(messageBoxText, ApplicationName, button, icon, defaultResult);
        }

      
        //
        // Сводка:
        //     Отображает окно сообщения с сообщением, заголовком, кнопкой и значком, которое
        //     принимает результат окна сообщения по умолчанию, совместимо с указанными
        //     параметрами и возвращает результат.
        //
        // Параметры:
        //   messageBoxText:
        //     Строка System.String, задающая отображаемый текст.
        //
        //   caption:
        //     Строка System.String, задающая отображаемый заголовок окна.
        //
        //   button:
        //     Значение System.Windows.MessageBoxButton, определяющее, какие кнопки (кнопка)
        //     подлежат отображению.
        //
        //   icon:
        //     Значение System.Windows.MessageBoxImage, задающее отображаемый значок.
        //
        //   defaultResult:
        //     Значение System.Windows.MessageBoxResult, задающее результат окна сообщений
        //     по умолчанию.
        //
        //   options:
        //     Объект значений System.Windows.MessageBoxOptions, задающий параметры.
        //
        // Возвращает:
        //     Значение System.Windows.MessageBoxResult, идентифицирующее кнопку, нажатую
        //     пользователем в окне сообщения.
        [SecurityCritical]
        public static MessageBoxResult Show(string messageBoxText, MessageBoxButton button,
                                            MessageBoxImage icon, MessageBoxResult defaultResult,
                                            MessageBoxOptions options)
        {
            return MessageBox.Show(messageBoxText, ApplicationName, button, icon, defaultResult, options);
        }


    }
}
