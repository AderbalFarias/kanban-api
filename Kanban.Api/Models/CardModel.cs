using Kanban.Domain.Entities;
using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace Kanban.Api.Models
{
    public class CardModel
    {
        public Guid Id { get; set; }


        [Required(ErrorMessage = "O titulo é obrigatório")]
        public string Titulo { get; set; }


        [Required(ErrorMessage = "O conteúdo é obrigatório")]
        public string Conteudo { get; set; }


        [Required(ErrorMessage = "A list é obrigatória")]
        public string Lista { get; set; }

        internal Card MapToEntityAdd() => new Card(this.Titulo, this.Conteudo, this.Lista);

        internal Card MapToEntity() => new Card(this.Id, this.Titulo, this.Conteudo, this.Lista);

        internal CardModel MapToCard(Card entity) => 
            new CardModel
            {
                Id = entity.Id,
                Titulo = entity.Titulo,
                Conteudo = entity.Conteudo,
                Lista = entity.Lista
            };
    }
}
