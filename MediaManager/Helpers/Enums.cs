using System;
using System.ComponentModel;
using System.Reflection;

namespace MediaManager.Helpers
{
    /// <summary>
    /// Classe contendo todos os enums utilizados.
    /// </summary>
    public static class Enums
    {
        /// <summary>
        /// NOVO = Recém criado (new Video()), Simples = Video com as informações basicas vindas da API, Completo = Video com as informações completas vindas da API.
        /// </summary>
        public enum Estado
        {
            Novo,
            Simples,
            Completo,
            CompletoSemForeignKeys
        }

        /// <summary>
        /// Define o tipo de conteúdo a ser usado.
        /// </summary>
        public enum TipoConteudo
        {
            [Description("Selecione")]
            Selecione = 0,

            [Description("Filme")]
            Filme = 1,

            [Description("Série")]
            Série = 2,

            [Description("Anime")]
            Anime = 3,

            [Description("Episódio")]
            Episódio = 4,

            [Description("Anime, filme e série")]
            AnimeFilmeSérie = 7
        }

        public enum TipoImagem
        {
            [Description("Todos")]
            Todos = 0,

            [Description("Fanart")]
            Fanart = 1,

            [Description("Poster")]
            Poster = 2
        }

        [Serializable]
        public enum MetodoDeProcessamento
        {
            [Description("Hardlink")]
            HardLink = 0,

            [Description("Copiar")]
            Copiar = 1
        }

        public enum EstadoEpisodio
        {
            [Description("Selecione")]
            Selecione = 0,

            [Description("Arquivado")]
            Arquivado = 1,

            [Description("Baixado")]
            Baixado = 2,

            [Description("Baixando")]
            Baixando = 3,

            [Description("Desejado")]
            Desejado = 4,

            [Description("Ignorado")]
            Ignorado = 5,

            [Description("Não estreado")]
            Novo = 6
        }

        public enum eTipoMensagem
        {
            Padrao = 0,
            Alerta = 1,
            AlertaSimNao = 2,
            AlertaSimNaoCancela = 3,
            Informativa = 4,
            QuestionamentoSimNao = 5,
            QuestionamentoSimNaoCancela = 6,
            Erro = 7
        }

        public enum eQualidadeDownload
        {
            [Description("Padrão")]
            Padrao = 0,

            [Description("WEB-DL FullHD")]
            WebDL_FullHD = 1,

            [Description("FullHD")]
            FullHD = 2,

            [Description("WEB-DL HD")]
            WebDL_HD = 3,

            [Description("HD")]
            HD = 4,

            [Description("SD")]
            SD = 5
        }

        public static string GetDescricao(this Enum tipoEnum)
        {
            Type type = tipoEnum.GetType();
            string name = Enum.GetName(type, tipoEnum);
            if (name != null)
            {
                FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    DescriptionAttribute attr =
                           Attribute.GetCustomAttribute(field,
                             typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (attr != null)
                    {
                        return attr.Description;
                    }
                }
            }
            return tipoEnum.ToString();
        }
    }
}