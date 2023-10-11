using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TrelloClone.Data;
using TrelloClone.Data.Repositories;
using TrelloClone.Models;
using TrelloClone.Models.Enum;
using TrelloClone.ViewModels;

namespace TrelloClone.Services
{
    public class BoardService
    {
        private readonly TrelloCloneDbContext _dbContext;
        private readonly RepositoryManager _repository;

        public BoardService(TrelloCloneDbContext dbContext, RepositoryManager repository)
        {
            _dbContext = dbContext;
            _repository = repository;
        }

        public async Task<BoardList> ListBoard(int userId)
        {
            var model = new BoardList();

            var allBoards = await _repository.BoardRepository.GetAll(false);

            foreach (var board in allBoards.Where(x => x.UserId == userId))
            {
                model.Boards.Add(new BoardList.Board
                {
                    Id = board.Id,
                    EmployeeName = board.EmployeeName,
                    Title = board.Title
                });
            }

            return model;
        }

        public async Task<BoardList> ListBoardSupervisor(string supervisorName)
        {
            var model = new BoardList();

            var allUsers = await _repository.UserRepository.GetAllUsers(false);
            var allBoards = await _repository.BoardRepository.GetAll(false);

            foreach (var user in allUsers.Where(x => x.SupervisorName == supervisorName))
            {
                foreach (var board in allBoards.Where(x => x.UserId == user.Id))
                {
                    model.Boards.Add(new BoardList.Board
                    {
                        Id = board.Id,
                        EmployeeName=board.EmployeeName,
                        Title = board.Title
                    });
                }
            }

            return model;
        }

        public BoardView GetBoard(int id)
        {
            var model = new BoardView();

            var board = _dbContext.Boards
                .Include(b => b.Columns)
                .ThenInclude(c => c.Cards)
                .SingleOrDefault(x => x.Id == id);

            if (board == null)
                return model;
            model.Id = board.Id;
            model.EmployeeName = board.EmployeeName;
            model.Title = board.Title;

            foreach (var column in board.Columns)
            {
                var modelColumn = new BoardView.Column
                {
                    Title = column.Title,
                    Id = column.Id
                };

                foreach (var card in column.Cards)
                {
                    var modelCard = new BoardView.Card
                    {
                        Id = card.Id,
                        Name = card.Name,
                        Requirement = card.Requirement,
                        Term = card.Term,
                        EmployeeAssessment = card.EmployeeAssessment,
                        EmployeeComment = card.EmployeeComment,
                        SupervisorAssessment = card.SupervisorAssessment,
                        SupervisorComment = card.SupervisorComment,
                        Points = card.Points
                    };

                    modelColumn.Cards.Add(modelCard);
                }

                model.Columns.Add(modelColumn);
            }

            return model;
        }

        public void AddCard(AddCard viewModel)
        {
            var board = _dbContext.Boards
                .Include(b => b.Columns)
                .SingleOrDefault(x => x.Id == viewModel.Id);

            if (board != null)
            {
                var firstColumn = board.Columns.FirstOrDefault();
                var secondColumn = board.Columns.FirstOrDefault();
                var thirdColumn = board.Columns.FirstOrDefault();

                if (firstColumn == null || secondColumn == null || thirdColumn == null)
                {
                    firstColumn = new Models.Column { Title = "Создано" };
                    secondColumn = new Models.Column { Title = "Согласование" };
                    thirdColumn = new Models.Column { Title = "Согласовано" };
                    board.Columns.Add(firstColumn);
                    board.Columns.Add(secondColumn);
                    board.Columns.Add(thirdColumn);
                }

                firstColumn.Cards.Add(new Models.Card
                {
                    Name = viewModel.Name,
                    Requirement = viewModel.Requirement,
                    Term = viewModel.Term
                });
            }

            _dbContext.SaveChanges();
        }

        public void AddBoard(NewBoard viewModel)
        {
            _dbContext.Boards.Add(new Models.Board
            {
                UserId = viewModel.UserId,
                EmployeeName = viewModel.EmployeeName,
                Title = viewModel.Title
            });

            _dbContext.SaveChanges();
        }

        public void DeleteBoard(int id)
        {
            var board = _dbContext.Boards
                .Include(i => i.Columns)
                .ThenInclude(x => x.Cards)
                .FirstOrDefault(i => i.Id == id);

            _dbContext.Boards.Remove(board);
            _dbContext.SaveChanges();
        }

        public void Move(MoveCardCommand command)
        {
            var card = _dbContext.Cards.SingleOrDefault(x => x.Id == command.CardId);
            card.ColumnId = command.ColumnId;
            _dbContext.SaveChanges();
        }
    }
}
