using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using MediaManager.Properties;

namespace ConfigurableInputMessageBox
{
    public enum inputType
    {
        Default,
        AdicionarConteudo,
        SemResultados
    }

    public static class DialogCloser
    {
        public static readonly DependencyProperty DialogResultProperty =
            DependencyProperty.RegisterAttached(
                "DialogResult",
                typeof(bool?),
                typeof(DialogCloser),
                new PropertyMetadata(DialogResultChanged));

        public static void SetDialogResult(Window target, bool? value)
        {
            target.SetValue(DialogResultProperty, value);
        }

        private static void DialogResultChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var window = d as Window;
            if (window != null)
                window.DialogResult = e.NewValue as bool?;
        }
    }

    /// <summary>
    ///
    /// Um frame MessageBox que possui um input e que é totalmente configurável.
    ///
    /// No construtor é informado um enum inputType (caso não informado é utilizado o inputType.Default) contendo as configurações padrões da janela.
    /// Este enum deve ser ajustado para atender as necessidades.
    ///
    /// Propriedades da janela:
    /// Title = Título da janela.
    /// ButtonWidth = Tamanho dos botões.
    /// CancelButtonText = Texto do botão Cancelar/fechar.
    /// OkButtonText = Texto do botão OK.
    /// Height = Altura da janela.
    /// Width = Largura da janela.
    /// InputText = Texto informado no input da janela.
    /// Message = Mensagem informativa da janela (Exemplo: "Informe um valor..."). Esta mensagem é exibida como Placeholder da caixa de texto.
    /// ValidationMessage = Mensagem a ser exibida quando o input não está preenchido.
    ///
    /// </summary>
    public partial class InputMessageBox : Window
    {
        public InputMessageBoxViewModel InputViewModel { get; set; }

        public InputMessageBox(inputType inputType = inputType.Default)
        {
            InitializeComponent();

            InputViewModel = new InputMessageBoxViewModel(inputType);

            DataContext = InputViewModel;
        }

        private void CenterWindowOnScreen()
        {
            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;
            double windowWidth = InputViewModel.Properties.Width;
            double windowHeight = InputViewModel.Properties.Height;
            Left = (screenWidth / 2) - (windowWidth / 2);
            Top = (screenHeight / 2) - (windowHeight / 2);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CenterWindowOnScreen();
        }
    }

    public class InputMessageBoxProperties : INotifyPropertyChanged, IDataErrorInfo
    {
        private int _buttonWidth;
        private string _cancelButtonText;
        private int _height;
        private string _inputText;
        private string _message;
        private string _okButtonText;
        private string _title;
        private string _validationMessage;
        private int _width;

        public int ButtonWidth { get { return _buttonWidth; } set { _buttonWidth = value; OnPropertyChanged(); } }

        public string CancelButtonText { get { return _cancelButtonText; } set { _cancelButtonText = value; OnPropertyChanged(); } }

        public int Height { get { return _height; } set { _height = value; OnPropertyChanged(); } }

        public string InputText { get { return _inputText; } set { _inputText = value; OnPropertyChanged(); } }

        public Thickness MarginOkButton { get { return new Thickness(10, 10, ButtonWidth + 15, 10); } }

        public string Message { get { return _message; } set { _message = value; OnPropertyChanged(); } }

        public string OkButtonText { get { return _okButtonText; } set { _okButtonText = value; OnPropertyChanged(); } }

        public string Title { get { return _title; } set { _title = value; OnPropertyChanged(); } }

        public string ValidationMessage { get { return _validationMessage; } set { _validationMessage = value; OnPropertyChanged(); } }

        public int Width { get { return _width; } set { _width = value; OnPropertyChanged(); } }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion INotifyPropertyChanged Members

        #region IDataErrorInfo Members

        public string Error { get; private set; }

        public string this[string columnName]
        {
            get
            {
                if (columnName == "InputText")
                {
                    if (string.IsNullOrWhiteSpace(InputText))
                    {
                        Error = ValidationMessage;
                    }
                    else
                    {
                        Error = null;
                    }
                }
                return Error;
            }
        }

        #endregion IDataErrorInfo Members
    }

    public class InputMessageBoxViewModel : INotifyPropertyChanged
    {
        private bool? _dialogResult;
        private InputMessageBoxProperties _properties;

        public bool? DialogResult { get { return _dialogResult; } set { _dialogResult = value; OnPropertyChanged(); } }

        public ICommand OkButtonClickCommand { get; private set; }

        public InputMessageBoxProperties Properties { get { return _properties; } }

        public InputMessageBoxViewModel(inputType inputType = inputType.Default)
        {
            _properties = new InputMessageBoxProperties();

            OkButtonClickCommand = new OkButtonClickCommand(this);

            switch (inputType)
            {
                case inputType.Default:
                    {
                        Properties.ButtonWidth = 75;
                        Properties.CancelButtonText = "Cancelar";
                        Properties.Height = 108;
                        Properties.Width = 430;
                        Properties.Message = "Informe um valor...";
                        Properties.OkButtonText = "Ok";
                        Properties.Title = string.Format("InputMessageBox");
                        break;
                    }
                case inputType.AdicionarConteudo:
                    {
                        Properties.ButtonWidth = 75;
                        Properties.CancelButtonText = "Cancelar";
                        Properties.Height = 108;
                        Properties.Message = "Digite o nome do conteudo a ser pesquisado...";
                        Properties.ValidationMessage = "Digite o nome do conteudo a ser pesquisado.";
                        Properties.OkButtonText = "Pesquisar";
                        Properties.Title = string.Format("Pesquisar - {0}", Settings.Default.AppName);
                        Properties.Width = 430;
                        break;
                    }
                case inputType.SemResultados:
                    {
                        Properties.ButtonWidth = 75;
                        Properties.CancelButtonText = "Cancelar";
                        Properties.Height = 108;
                        Properties.Message = "Não foram encontrados resultados para este nome, informe um novo nome.";
                        Properties.ValidationMessage = "Digite o nome do conteudo a ser pesquisado.";
                        Properties.OkButtonText = "Pesquisar";
                        Properties.Title = string.Format("Pesquisar - {0}", Settings.Default.AppName);
                        Properties.Width = 430;
                        break;
                    }
                default:
                    break;
            }
        }

        public void Save()
        {
            DialogResult = true;
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion INotifyPropertyChanged Members
    }

    public class OkButtonClickCommand : ICommand
    {
        private InputMessageBoxViewModel _inputVM;

        public OkButtonClickCommand(InputMessageBoxViewModel inputVM)
        {
            _inputVM = inputVM;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return string.IsNullOrWhiteSpace(_inputVM.Properties.Error);
        }

        public void Execute(object parameter)
        {
            _inputVM.Save();
        }
    }
}