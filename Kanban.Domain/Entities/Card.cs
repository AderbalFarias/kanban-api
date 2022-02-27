using System;

namespace Kanban.Domain.Entities
{
    public class Card
    {
        public Card(Guid id, string titulo, string conteudo, string lista)
        {
            this.Id = id;
            this.Titulo = titulo;
            this.Conteudo = conteudo;
            this.Lista = lista;
        }

        public Card(string titulo, string conteudo, string lista)
        {
            this.Titulo = titulo;   
            this.Conteudo = conteudo;
            this.Lista = lista;
        }

        public Guid Id { get; private set; }

        public string Titulo { get; private set; }

        public string Conteudo { get; private set; }

        public string Lista { get; private set; }
    }
}
