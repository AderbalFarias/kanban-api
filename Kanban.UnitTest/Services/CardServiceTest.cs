using Kanban.Domain.Entities;
using Kanban.Domain.Interfaces.Repositories;
using Kanban.Domain.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace Kanban.UnitTest.Services
{
    public class CardServiceTest
    {
        #region Fields 

        private readonly Mock<IBaseRepository> _mockBaseRepository;
        private readonly Mock<ILogger<CardService>> _mockLogger;
        private readonly CardService _cardServcice;
        private IQueryable<Card> CardResults;

        #endregion End Fields 

        #region Constructor

        public CardServiceTest()
        {
            _mockBaseRepository = new Mock<IBaseRepository>();
            _mockLogger = new Mock<ILogger<CardService>>();

            CardResults = MockCard.AsQueryable();
            RepositorySetup();

            _cardServcice = new CardService(_mockBaseRepository.Object, _mockLogger.Object);
        }

        #endregion End Constructor

        #region Setups 

        private Task RepositorySetup()
        {
            _mockBaseRepository.Setup(s => s.Add(It.IsAny<Card>()))
                .Returns(Task.FromResult(1));

            _mockBaseRepository.Setup(s => s.Update(It.IsAny<Card>()))
                .Returns(Task.FromResult(1));

            _mockBaseRepository.Setup(s => s.Delete(It.IsAny<Card>()))
                .Returns(Task.FromResult(1));

            _mockBaseRepository.Setup(s => s.GetAsync<Card>())
                .Returns(async () => await Task.FromResult(CardResults));

            _mockBaseRepository.Setup(s => s.Get(It.IsAny<Expression<Func<Card, bool>>>(), It.IsAny<string>()))
                .Returns<Expression<Func<Card, bool>>, string>((predicate, include) => CardResults.Where(predicate));

            _mockBaseRepository.Setup(s => s.GetAsync(It.IsAny<Expression<Func<Card, bool>>>()))
                .Returns<Expression<Func<Card, bool>>>(async (predicate) => await Task.FromResult(CardResults.Where(predicate)));

            _mockBaseRepository.Setup(s => s.GetAsync(It.IsAny<Expression<Func<Card, bool>>>(), It.IsAny<string>()))
                .Returns<Expression<Func<Card, bool>>, string>(async (predicate, include) => await Task.FromResult(CardResults.Where(predicate)));

            _mockBaseRepository.Setup(s => s.GetObjectAsync(It.IsAny<Expression<Func<Card, bool>>>()))
                .Returns<Expression<Func<Card, bool>>>(predicate => Task.FromResult(CardResults.FirstOrDefault(predicate)));

            _mockBaseRepository.Setup(s => s.GetAnyAsync(It.IsAny<Expression<Func<Card, bool>>>()))
                .Returns<Expression<Func<Card, bool>>>(predicate => Task.FromResult(CardResults.Any(predicate)));


            return Task.CompletedTask;
        }

        #endregion End Setups 

        #region Mocks

        private IEnumerable<Card> MockCard => 
            new List<Card>
            {
                new Card
                {
                    Id = Guid.NewGuid(),
                    Titulo = "Titulo test 1",
                    Conteudo = "Conteudo test 1",
                    Lista = "Lista test 1"
                },
                new Card
                {
                    Id = Guid.NewGuid(),
                    Titulo = "Titulo test 2",
                    Conteudo = "Conteudo test 2",
                    Lista = "Lista test 2"
                },
            };

        #endregion End Mocks

        #region Tests

        [Fact]
        public async Task GetAll_Should_Return_2_Cards()
        {
            var cards = await _cardServcice.GetAll();

            Assert.NotNull(cards);
            Assert.Equal(2, cards.Count());
        }

        [Fact]
        public async Task GetById_Should_Return_Data()
        {
            var entity = await _cardServcice.GetById(CardResults.First().Id);

            Assert.NotNull(entity);
            Assert.Equal(CardResults.First().Id, entity.Id);
            Assert.NotEqual(CardResults.Last()?.Id, entity.Id);
        }

        [Fact]
        public async Task GetById_Should_Not_Return_Data()
        {
            var entity = await _cardServcice.GetById(Guid.NewGuid());

            Assert.Null(entity);
        }

        [Fact]
        public async Task Add_Should_Be_Called_Once()
        {
            var entity = MockCard.First();

            await _cardServcice.Add(entity);

            _mockBaseRepository.Verify(x => x.Add(entity), Times.Once);
        }

        [Fact]
        public async Task Update_Should_Throw_Exception()
        {
            var entity = MockCard.First();

            await Assert.ThrowsAsync<KeyNotFoundException>(async () => await _cardServcice.Update(entity));
        }

        [Fact]
        public async Task Update_Should_Return_Exception_Message()
        {
            var entity = MockCard.First();

            var logException = await Record.ExceptionAsync(async () => await _cardServcice.Update(entity));

            Assert.Contains($"Card not found", logException.Message);
        }

        [Fact]
        public async Task Update_Should_Be_Called_Once()
        {
            await _cardServcice.Update(CardResults.First());

            _mockBaseRepository.Verify(x => x.Update(CardResults.First()), Times.Once);
        }

        [Fact]
        public async Task Delete_Should_Return_Exception_Message()
        {
            var entity = MockCard.First();

            var logException = await Record.ExceptionAsync(async () => await _cardServcice.Delete(entity.Id));

            Assert.Contains($"Card not found", logException.Message);
        }

        [Fact]
        public async Task Delete_Should_Throw_Exception()
        {
            var entity = MockCard.First();

            await Assert.ThrowsAsync<KeyNotFoundException>(async () => await _cardServcice.Delete(entity.Id));
        }

        [Fact]
        public async Task Delete_Should_Be_Called_Once()
        {
            await _cardServcice.Delete(CardResults.First().Id);

            _mockBaseRepository.Verify(x => x.Delete(CardResults.First()), Times.Once);
        }

        #endregion End Tests
    }
}
