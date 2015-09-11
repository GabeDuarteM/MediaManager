using System;
using System.Windows;
using System.Windows.Input;
using MediaManager.Forms;
using MediaManager.ViewModel;

namespace MediaManager.Commands
{
    public class CommandsPreferencias
    {
        public class CommandLimparBancoDeDados : ICommand
        {
            #region ICommand Members

            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                if (MessageBox.Show("Ao realizar esta ação, todas as informações armazenadas sobre o conteúdo que você possui serão apagadas (entretanto todos os arquivos de vídeo que você possui permanecerá intacto). Você realmente deseja realizar esta ação?", Properties.Settings.Default.AppName, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    using (Model.Context db = new Model.Context())
                    {
                        db.Database.ExecuteSqlCommand(@"/*Disable Constraints & Triggers*/
exec sp_MSforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL'
exec sp_MSforeachtable 'ALTER TABLE ? DISABLE TRIGGER ALL'

/*Perform delete operation on all table for cleanup*/
exec sp_MSforeachtable 'DELETE ?'

/*Enable Constraints & Triggers again*/
exec sp_MSforeachtable 'ALTER TABLE ? CHECK CONSTRAINT ALL'
exec sp_MSforeachtable 'ALTER TABLE ? ENABLE TRIGGER ALL'

/*Reset Identity on tables with identity column*/
exec sp_MSforeachtable 'IF OBJECTPROPERTY(OBJECT_ID(''?''), ''TableHasIdentity'') = 1 BEGIN DBCC CHECKIDENT (''?'',RESEED,0) END'");
                    }
                    frmMain.MainVM.AtualizarConteudo(Helpers.Helper.Enums.ContentType.movieShowAnime);
                }
            }

            #endregion ICommand Members
        }

        #region Commands EscolherPasta

        public class CommandEscolherPastaAnimes : ICommand
        {
            public event EventHandler CanExecuteChanged { add { CommandManager.RequerySuggested += value; } remove { CommandManager.RequerySuggested -= value; } }

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                if (parameter is PreferenciasViewModel)
                {
                    PreferenciasViewModel preferenciasVM = parameter as PreferenciasViewModel;
                    Ookii.Dialogs.VistaFolderBrowserDialog folderDialog = new Ookii.Dialogs.VistaFolderBrowserDialog();
                    folderDialog.SelectedPath = preferenciasVM.PastaAnimes;
                    if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        preferenciasVM.PastaAnimes = folderDialog.SelectedPath;
                    }
                }
            }
        }

        public class CommandEscolherPastaFilmes : ICommand
        {
            public event EventHandler CanExecuteChanged { add { CommandManager.RequerySuggested += value; } remove { CommandManager.RequerySuggested -= value; } }

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                if (parameter is PreferenciasViewModel)
                {
                    PreferenciasViewModel preferenciasVM = parameter as PreferenciasViewModel;
                    Ookii.Dialogs.VistaFolderBrowserDialog folderDialog = new Ookii.Dialogs.VistaFolderBrowserDialog();
                    folderDialog.SelectedPath = preferenciasVM.PastaFilmes;
                    if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        preferenciasVM.PastaFilmes = folderDialog.SelectedPath;
                    }
                }
            }
        }

        public class CommandEscolherPastaSeries : ICommand
        {
            public event EventHandler CanExecuteChanged { add { CommandManager.RequerySuggested += value; } remove { CommandManager.RequerySuggested -= value; } }

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                if (parameter is PreferenciasViewModel)
                {
                    PreferenciasViewModel preferenciasVM = parameter as PreferenciasViewModel;
                    Ookii.Dialogs.VistaFolderBrowserDialog folderDialog = new Ookii.Dialogs.VistaFolderBrowserDialog();
                    folderDialog.SelectedPath = preferenciasVM.PastaSeries;
                    if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        preferenciasVM.PastaSeries = folderDialog.SelectedPath;
                    }
                }
            }
        }

        public class CommandEscolherPastaDownloads : ICommand
        {
            public event EventHandler CanExecuteChanged { add { CommandManager.RequerySuggested += value; } remove { CommandManager.RequerySuggested -= value; } }

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                if (parameter is PreferenciasViewModel)
                {
                    PreferenciasViewModel preferenciasVM = parameter as PreferenciasViewModel;
                    Ookii.Dialogs.VistaFolderBrowserDialog folderDialog = new Ookii.Dialogs.VistaFolderBrowserDialog();
                    folderDialog.SelectedPath = preferenciasVM.PastaDownloads;
                    if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        preferenciasVM.PastaDownloads = folderDialog.SelectedPath;
                    }
                }
            }
        }

        #endregion Commands EscolherPasta

        public class CommandSalvar : ICommand
        {
            public event EventHandler CanExecuteChanged { add { CommandManager.RequerySuggested += value; } remove { CommandManager.RequerySuggested -= value; } }

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                if (parameter is PreferenciasViewModel)
                {
                    PreferenciasViewModel preferenciasVM = parameter as PreferenciasViewModel;
                    Properties.Settings.Default.pref_FormatoAnimes = preferenciasVM.FormatoParaAnimes;
                    Properties.Settings.Default.pref_FormatoFilmes = preferenciasVM.FormatoParaFilmes;
                    Properties.Settings.Default.pref_FormatoSeries = preferenciasVM.FormatoParaSeries;
                    Properties.Settings.Default.pref_IdiomaPesquisa = preferenciasVM.IdiomaSelecionado;
                    Properties.Settings.Default.pref_IntervaloDeProcuraConteudoNovo = preferenciasVM.IntervaloDeProcuraConteudoNovo;
                    Properties.Settings.Default.pref_PastaAnimes = preferenciasVM.PastaAnimes;
                    Properties.Settings.Default.pref_PastaDownloads = preferenciasVM.PastaDownloads;
                    Properties.Settings.Default.pref_PastaFilmes = preferenciasVM.PastaFilmes;
                    Properties.Settings.Default.pref_PastaSeries = preferenciasVM.PastaSeries;
                    Properties.Settings.Default.Save();

                    if (preferenciasVM.CloseAction != null)
                        preferenciasVM.CloseAction();
                }
            }
        }
    }
}