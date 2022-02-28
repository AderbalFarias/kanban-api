using Kanban.Api.Controllers;
using Kanban.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Kanban.UnitTest.Controllers
{
    public class CardControllerTest
    {
        #region Fields 

        private readonly Mock<ICardService> _mockCardService;
        private readonly Mock<ILogger<CardController>> _mockLogger;
        private readonly CardController _cardController;
        private IQueryable<Domain.Entities.Card> CardResults;

        #endregion End Fields 

        #region Constructor

        public CardControllerTest()
        {
            _mockCardService = new Mock<ICardService>();
            _mockLogger = new Mock<ILogger<CardController>>();

            CardResults = MockCard.AsQueryable();
            ServiceSetup();

            _cardController = new CardController(_mockCardService.Object, _mockLogger.Object);
        }

        #endregion End Constructor

        #region Setups 

        private Task ServiceSetup()
        {
            // Arrange

            _mockCardService.Setup(s => s.AddAsync(It.IsAny<Domain.Entities.Card>()))
                .Returns(async () => await Task.FromResult(CardResults.First()));

            _mockCardService.Setup(s => s.DeleteAsync(It.IsAny<Domain.Entities.Card>()))
                .Returns(async () => await Task.FromResult(1));

            _mockCardService.Setup(s => s.UpdateAsync(It.IsAny<Domain.Entities.Card>()))
                .Returns(async () => await Task.FromResult(1));

            _mockCardService.Setup(s => s.GetByIdAsync(It.IsAny<Guid>()))
                .Returns<Guid>(predicate => Task.FromResult(CardResults.First(f => f.Id == predicate)));

            _mockCardService.Setup(s => s.GetAllAsync())
                .Returns(async () => await Task.FromResult(CardResults));

            return Task.CompletedTask;
        }

        #endregion End Setups 

        #region Mocks

        private IEnumerable<Domain.Entities.Card> MockCard =>
            new List<Domain.Entities.Card>
            {
                new Domain.Entities.Card(Guid.NewGuid(), "Titulo test 1", "Conteudo test 1", "Lista test 1"),
                new Domain.Entities.Card(Guid.NewGuid(), "Titulo test 2", "Conteudo test 2", "Lista test 2")
            };

        private Api.Models.Card MockNewCardModel =>
            new Api.Models.Card
            {
                Titulo = "New titulo test 1",
                Conteudo = "New content test 1",
                Lista = "Lista x"
            };

        #endregion End Mocks

        #region Tests

        [Fact]
        public async Task GetAll_Should_Return_Ok_With_2_Cards()
        {
            // Act
            var result = await _cardController.GetAllAsync();

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var cards = Assert.IsAssignableFrom<IEnumerable<Domain.Entities.Card>>(okObjectResult.Value);
            Assert.Equal((int)HttpStatusCode.OK, okObjectResult.StatusCode);
            Assert.Equal(2, cards.Count());
        }

        [Fact]
        public async Task GetById_Should_Return_Ok_With_Card_Data()
        {
            var result = await _cardController.GetById(CardResults.First().Id);

            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var card = Assert.IsAssignableFrom<Domain.Entities.Card>(okObjectResult.Value);
            Assert.Equal((int)HttpStatusCode.OK, okObjectResult.StatusCode);
            Assert.Equal(CardResults.First().Id, card.Id);
        }

        [Fact]
        public async Task Add_Should_Return_Ok_And_Card_Data()
        {
            var result = await _cardController.Add(MockNewCardModel);

            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var card = Assert.IsAssignableFrom<Domain.Entities.Card>(okObjectResult.Value);
            Assert.Equal((int)HttpStatusCode.OK, okObjectResult.StatusCode);
            Assert.NotEqual(Guid.Empty, card.Id);
        }

        [Fact]
        public async Task Add_Should_Return_BadRequest()
        {
            var card = MockNewCardModel;
            card.Id = Guid.NewGuid();
            var result = await _cardController.Add(card);

            var badRequestObjectResult = Assert.IsType<BadRequestResult>(result);
            Assert.Equal((int)HttpStatusCode.BadRequest, badRequestObjectResult.StatusCode);
        }

        [Fact]
        public async Task Update_Should_Return_Ok_And_Card_Data()
        {
            var card = MockNewCardModel;
            card.Id = CardResults.First().Id;
            var result = await _cardController.Update(card.Id, card);

            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var resultData = Assert.IsAssignableFrom<Domain.Entities.Card>(okObjectResult.Value);
            Assert.Equal((int)HttpStatusCode.OK, okObjectResult.StatusCode);
            Assert.Equal(card.Id, resultData.Id);
        }

        [Fact]
        public async Task Update_Should_Return_NotFound()
        {
            var card = MockNewCardModel;
            card.Id = CardResults.First().Id;
            var result = await _cardController.Update(Guid.NewGuid(), new Api.Models.Card());

            var notFoundResultObjectResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal((int)HttpStatusCode.NotFound, notFoundResultObjectResult.StatusCode);
        }

        [Fact]
        public async Task Update_Should_Return_BadRequest()
        {
            var card = MockNewCardModel;
            card.Id = CardResults.First().Id;
            var result = await _cardController.Update(Guid.NewGuid(), card);

            var badRequestObjectResult = Assert.IsType<BadRequestResult>(result);
            Assert.Equal((int)HttpStatusCode.BadRequest, badRequestObjectResult.StatusCode);
        }

        [Fact]
        public async Task Delete_Should_Return_Ok_And_Cards_Remaining()
        {
            var result = await _cardController.Delete(CardResults.First().Id);

            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, okObjectResult.StatusCode);
        }

        [Fact]
        public async Task Delete_Should_Return_NotFound_When_Id_Is_Empty()
        {
            var result = await _cardController.Delete(Guid.Empty);

            var notFoundResultObjectResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal((int)HttpStatusCode.NotFound, notFoundResultObjectResult.StatusCode);
        }

        [Theory]
        [InlineData("Titulo 1", "Test 1", "")]
        [InlineData("Titulo 2", "Test 2", null)]
        [InlineData("", "Test", "Test")]
        [InlineData(null, "Test", "Test d")]
        [InlineData("xx", "", "Test xx")]
        [InlineData("Test", null, "xxx")]
        public async Task Add_Should_Return_BadRequest_InvalidModel(string titulo, string conteudo, string lista)
        {
            var card = new Api.Models.Card
            {
                Titulo = titulo,
                Conteudo = conteudo,
                Lista = lista
            };

            var validationContext = new ValidationContext(card, null, null);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(card, validationContext, validationResults, true);
            foreach (var validationResult in validationResults)
                _cardController.ModelState.AddModelError(validationResult.MemberNames.First(), validationResult.ErrorMessage);

            var result = await _cardController.Add(card);

            var badRequestObjectResult = Assert.IsType<BadRequestResult>(result);
            Assert.Equal((int)HttpStatusCode.BadRequest, badRequestObjectResult.StatusCode);
        }

        [Theory]
        [InlineData("Titulo 1", "Test 1", "")]
        [InlineData("Titulo 2", "Test 2", null)]
        [InlineData("", "Test", "Test")]
        [InlineData(null, "Test", "Test d")]
        [InlineData("xx", "", "Test xx")]
        [InlineData("Test", null, "xxx")]
        public async Task Update_Should_Return_BadRequest_InvalidModel(string titulo, string conteudo, string lista)
        {
            var card = new Api.Models.Card
            {
                Titulo = titulo,
                Conteudo = conteudo,
                Lista = lista,
                Id = CardResults.First().Id
            };

            var validationContext = new ValidationContext(card, null, null);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(card, validationContext, validationResults, true);
            foreach (var validationResult in validationResults)
                _cardController.ModelState.AddModelError(validationResult.MemberNames.First(), validationResult.ErrorMessage);

            var result = await _cardController.Update(card.Id, card);

            var badRequestObjectResult = Assert.IsType<BadRequestResult>(result);
            Assert.Equal((int)HttpStatusCode.BadRequest, badRequestObjectResult.StatusCode);
        }

        #endregion End Tests
    }
}
