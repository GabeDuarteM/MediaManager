using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using MediaManager.Helpers;
using MediaManager.ViewModel;

namespace MediaManager.Commands
{
    public class RenomearCommand : ICommand
    {
        #region ICommand Members

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            if (parameter is RenomearViewModel && (parameter as RenomearViewModel).Episodes != null && (parameter as RenomearViewModel).Episodes.Count > 0)
                return true;
            else
                return false;
        }

        public void Execute(object parameter)
        {
            RenomearViewModel renomearVM = (parameter as RenomearViewModel);
            foreach (var item in renomearVM.Episodes)
            {
                if (item.IsSelected)
                {
                    Helper.LogMessage("O arquivo \"" + Path.Combine(item.FolderPath, item.Filename) + "\" será copiado para \"" + Path.Combine(item.Serie.FolderPath, item.FilenameRenamed) + "\"");
                    Helper.LogMessage("Método de processamento: " + ((Enums.MetodoDeProcessamento)Properties.Settings.Default.pref_MetodoDeProcessamento).ToString());

                    if (!Directory.Exists(item.Serie.FolderPath))
                    {
                        Directory.CreateDirectory(item.Serie.FolderPath);
                        Helper.LogMessage("Diretório \"" + item.Serie.FolderPath + "\" criado.");
                    }

                    if (File.Exists(Path.Combine(item.Serie.FolderPath, item.FilenameRenamed)))
                    {
                        if (!renomearVM.IsSilencioso)
                        {
                            if (MessageBox.Show("O episódio " + item.FilenameRenamed + " já existe. Você deseja sobrescrevê-lo pelo arquivo \"" + Path.Combine(item.FolderPath, item.Filename) + "\"?", Properties.Settings.Default.AppName, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                            {
                                File.Delete(Path.Combine(item.Serie.FolderPath, item.FilenameRenamed));
                                if (Helper.CreateHardLink(Path.Combine(item.Serie.FolderPath, item.FilenameRenamed),
                                        Path.Combine(item.FolderPath, item.Filename), IntPtr.Zero))
                                {
                                    item.FilePath = Path.Combine(item.Serie.FolderPath, item.FilenameRenamed);
                                    item.IsRenamed = true;
                                    DBHelper.UpdateEpisodioRenomeado(item);
                                }
                                else
                                {
                                    Helper.TratarException(new Exception("Código: " + Marshal.GetLastWin32Error() + "\r\nArquivo: " + Path.Combine(item.FolderPath, item.Filename)), "Ocorreu um erro ao criar o HardLink.\r\nDetalhes: ", true);
                                }
                            }
                        }
                        else
                        {
                            Helper.LogMessage("O arquivo " + Path.Combine(item.Serie.FolderPath, item.FilenameRenamed) + " já existe e não será sobrescrito.");
                        }
                    }
                    else
                    {
                        if (Helper.CreateHardLink(Path.Combine(item.Serie.FolderPath, item.FilenameRenamed),
                                Path.Combine(item.FolderPath, item.Filename), IntPtr.Zero))
                        {
                            item.FilePath = Path.Combine(item.Serie.FolderPath, item.FilenameRenamed);
                            item.IsRenamed = true;
                            DBHelper.UpdateEpisodioRenomeado(item);
                        }
                        else
                        {
                            Helper.TratarException(new Exception("Código: " + Marshal.GetLastWin32Error() + "\r\nArquivo: " + Path.Combine(item.FolderPath, item.Filename)), "Ocorreu um erro ao criar o HardLink.\r\nDetalhes: ", true);
                        }
                    }
                }
            }
            if (renomearVM.CloseAction != null)
                renomearVM.CloseAction();
        }
    }

    #endregion ICommand Members
}