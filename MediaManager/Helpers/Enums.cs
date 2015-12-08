using System;

namespace MediaManager.Helpers
{
    /// <summary>
    /// Classe contendo todos os enums utilizados.
    /// </summary>
    public class Enums
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
            Selecione = 0,
            Filme = 1,
            Série = 2,
            Anime = 3,
            Episódio = 4,
            AnimeFilmeSérie = 7
        }

        public enum TipoImagem
        {
            Todos = 0,
            Fanart = 1,
            Poster = 2
        }

        [Serializable]
        public enum MetodoDeProcessamento
        {
            HardLink = 0,
            Copiar = 1
        }

        public enum EstadoEpisodio
        {
            Selecione = 0,
            Arquivado = 1,
            Baixado = 2,
            Baixando = 3,
            Desejado = 4,
            Ignorado = 5
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
    }
}