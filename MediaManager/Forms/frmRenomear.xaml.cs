using MediaManager.Helpers;
using MediaManager.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace MediaManager.Forms
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class frmRenomear : Window
    {
        public frmRenomear(Helpers.Helper.TipoConteudo conteudo)
        {
            InitializeComponent();
            PopularGrid(conteudo);
        }

        public IEnumerable<FileInfo> PopularGrid(Helpers.Helper.TipoConteudo conteudo)
        {
            switch (conteudo)
            {
                case Helpers.Helper.TipoConteudo.show:
                    using (Context db = new Context())
                    {
                        var series = from serie in db.Series
                                     where serie.IsAnime == false
                                     select serie;
                        foreach (var serie in series)
                        {
                            try
                            {
                                DirectoryInfo dir = new DirectoryInfo(serie.FolderPath);
                                var files = dir.EnumerateFiles("*", SearchOption.AllDirectories);

                                foreach (var file in files)
                                {
                                    Arquivos arquivo = new Arquivos { Arquivo = file.Name, ArquivoRenomeado = "", Nome = serie.Title };
                                    grdArquivos.Items.Add(arquivo);
                                }
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }
                    break;

                case MediaManager.Helpers.Helper.TipoConteudo.movie:
                    break;

                case MediaManager.Helpers.Helper.TipoConteudo.anime:
                    break;

                case MediaManager.Helpers.Helper.TipoConteudo.movieShowAnime:
                    break;
            }
            return null;
        }

        private void cbxAll_Checked(object sender, RoutedEventArgs e)
        {
        }

        private void cbxAll_Unchecked(object sender, RoutedEventArgs e)
        {
        }
    }

    public class Arquivos
    {
        public string Arquivo { get; set; }

        public string Nome { get; set; }

        public string ArquivoRenomeado { get; set; }
    }
}