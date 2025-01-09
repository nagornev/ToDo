using Microsoft.EntityFrameworkCore;
using ToDo.Domain.Results;
using ToDo.Microservices.Entries.Database.Contexts;
using ToDo.Microservices.Entries.Domain.Models;
using ToDo.Microservices.Entries.Infrastructure.Repositories;
using ToDo.Microservices.Entries.Tests.Mock;
using ToDo.Microservices.Entries.UseCases.Repositories;

namespace ToDo.Microservices.Entries.Tests.Units
{
    public class EntryRepositoryTests
    {
        private EntryContext GetEntryContextMock(Func<IReadOnlyDictionary<User, IEnumerable<Entry>>> startContext = null)
        {
            return new EntryContextMock($"{Guid.NewGuid()}", startContext);
        }

        #region Get

        [Fact]
        public async void EntryRepository_Get_UserExistAndEntryCollectionIsNotEmpty_ShouldReturnSuccessTrueAndEntryCollection()
        {
            //Arrage

            User user = User.Constructor(Guid.NewGuid()).Content;

            Entry[] entries =
            {
                Entry.New(user.Id, user.Id.ToString(), default).Content,
                Entry.New(user.Id, user.Id.ToString(), default).Content,
                Entry.New(user.Id, user.Id.ToString(), default).Content,
            };

            IEntryRepository entryRepository = new EntryRepository(GetEntryContextMock(() => new Dictionary<User, IEnumerable<Entry>>()
                                                                                            {
                                                                                                { user, entries }
                                                                                            })
                                                                   );

            //Act

            Result<IEnumerable<Entry>> entriesResult = await entryRepository.Get(user.Id);

            //Arrage

            Assert.True(entriesResult.Success);
            Assert.NotEmpty(entriesResult.Content);
            Assert.All(entriesResult.Content, (entry) => Assert.Equal(user.Id, entry.CategoryId));
        }

        [Fact]
        public async void EntryRepository_Get_UserExistAndEntryCollectionIsEmpty_ShouldReturnSuccessTrueAndEmptyEntryCollection()
        {
            //Arrage

            User user = User.Constructor(Guid.NewGuid()).Content;

            IEntryRepository entryRepository = new EntryRepository(GetEntryContextMock(() => new Dictionary<User, IEnumerable<Entry>>()
                                                                                            {
                                                                                                { user, Enumerable.Empty<Entry>() }
                                                                                            })
                                                                   );

            //Act

            Result<IEnumerable<Entry>> entriesResult = await entryRepository.Get(user.Id);

            //Arrage

            Assert.True(entriesResult.Success);
            Assert.Empty(entriesResult.Content);
        }

        [Fact]
        public async void EntryRepository_Get_UserNotExistAndEntryCollectionIsEmpty_ShouldReturnSuccessFalse()
        {
            //Arrage

            IEntryRepository entryRepository = new EntryRepository(GetEntryContextMock());

            User user = User.Constructor(Guid.NewGuid()).Content;

            //Act

            Result<IEnumerable<Entry>> entriesResult = await entryRepository.Get(user.Id);

            //Arrage

            Assert.False(entriesResult.Success);
        }

        [Fact]
        public async void EntryRepository_Get_UserExistAndEntryCollectionIsNotEmpty_ShouldReturnSuccessTrueAndEntry()
        {
            //Arrage

            User user = User.Constructor(Guid.NewGuid()).Content;

            Entry[] entries =
            {
                Entry.New(user.Id, user.Id.ToString(), default).Content,
                Entry.New(user.Id, user.Id.ToString(), default).Content,
                Entry.New(user.Id, user.Id.ToString(), default).Content,
            };

            IEntryRepository entryRepository = new EntryRepository(GetEntryContextMock(() => new Dictionary<User, IEnumerable<Entry>>()
                                                                                            {
                                                                                                { user, entries }
                                                                                            })
                                                                   );

            Entry entry = entries.First();

            //Act

            Result<Entry> entryResult = await entryRepository.Get(user.Id, entry.Id);

            //Arrage

            Assert.True(entryResult.Success);
            Assert.Equal(user.Id, entry.CategoryId);
        }

        [Fact]
        public async void EntryRepository_Get_UserExistAndEntryCollectionIsEmpty_ShouldReturnSuccessFalse()
        {
            //Arrage

            User user = User.Constructor(Guid.NewGuid()).Content;

            IEntryRepository entryRepository = new EntryRepository(GetEntryContextMock(() => new Dictionary<User, IEnumerable<Entry>>()
                                                                                            {
                                                                                                { user, Enumerable.Empty<Entry>() }
                                                                                            })
                                                                   );

            Entry entry = Entry.New(user.Id, user.Id.ToString(), default).Content;

            //Act

            Result<Entry> entryResult = await entryRepository.Get(user.Id, entry.Id);

            //Arrage

            Assert.False(entryResult.Success);
        }

        [Fact]
        public async void EntryRepository_Get_UserExistAndEntryCollectionIsNotEmpty_ShouldReturnSuccessFalse()
        {
            //Arrage

            User firstUser = User.Constructor(Guid.NewGuid()).Content;

            Entry[] firstEntries =
            {
                Entry.New(firstUser.Id, firstUser.Id.ToString(), default).Content,
                Entry.New(firstUser.Id, firstUser.Id.ToString(), default).Content,
                Entry.New(firstUser.Id, firstUser.Id.ToString(), default).Content,
            };


            User secondUser = User.Constructor(Guid.NewGuid()).Content;

            Entry[] secondEntries =
            {
                Entry.New(secondUser.Id, secondUser.Id.ToString(), default).Content,
                Entry.New(secondUser.Id, secondUser.Id.ToString(), default).Content,
                Entry.New(secondUser.Id, secondUser.Id.ToString(), default).Content,
            };

            IEntryRepository entryRepository = new EntryRepository(GetEntryContextMock(() => new Dictionary<User, IEnumerable<Entry>>()
                                                                                            {
                                                                                                { firstUser, firstEntries },
                                                                                                { secondUser, secondEntries },
                                                                                            })
                                                                   );

            Entry secondEntry = secondEntries.First();

            //Act

            Result<Entry> entryResult = await entryRepository.Get(firstUser.Id, secondEntry.Id);

            //Arrage

            Assert.False(entryResult.Success);
        }

        #endregion

        #region Create

        [Fact]
        public async void EntryRepository_Create_UserExistAndEntryCollectionIsNotEmpty_ShouldReturnSuccessTrue()
        {
            //Arrage

            User user = User.Constructor(Guid.NewGuid()).Content;

            Entry[] entries =
            {
                Entry.New(user.Id, user.Id.ToString(), default).Content,
                Entry.New(user.Id, user.Id.ToString(), default).Content,
                Entry.New(user.Id, user.Id.ToString(), default).Content,
            };

            IEntryRepository entryRepository = new EntryRepository(GetEntryContextMock(() => new Dictionary<User, IEnumerable<Entry>>()
                                                                                            {
                                                                                                { user, entries }
                                                                                            })
                                                                   );

            Entry entry = Entry.New(user.Id, user.Id.ToString(), default).Content;

            //Act

            Result createResult = await entryRepository.Create(user.Id, entry);

            //Arrage

            Assert.True(createResult.Success);
        }

        [Fact]
        public async void EntryRepository_Create_UserExistAndEntryCollectionIsEmpty_ShouldReturnSuccessTrue()
        {
            //Arrage

            User user = User.Constructor(Guid.NewGuid()).Content;

            IEntryRepository entryRepository = new EntryRepository(GetEntryContextMock(() => new Dictionary<User, IEnumerable<Entry>>()
                                                                                            {
                                                                                                { user, Enumerable.Empty<Entry>() }
                                                                                            })
                                                                   );

            Entry entry = Entry.New(user.Id, user.Id.ToString(), default).Content;

            //Act

            Result createResult = await entryRepository.Create(user.Id, entry);

            //Arrage

            Assert.True(createResult.Success);
        }

        [Fact]
        public async void EntryRepository_Create_UserExistAndEntryCollectionIsEmpty_ShouldThrowDbUpdateException()
        {
            //Arrage

            IEntryRepository entryRepository = new EntryRepository(GetEntryContextMock());

            User user = User.Constructor(Guid.NewGuid()).Content;

            Entry entry = Entry.New(user.Id, user.Id.ToString(), default).Content;

            //Act & Arrage

            await Assert.ThrowsAsync<DbUpdateException>(async () => await entryRepository.Create(user.Id, entry));
        }

        #endregion

        #region Update

        [Fact]
        public async void EntryRepository_Update_UserExistAndEntryCollectionIsNotEmpty_ShouldReturnSuccessTrue()
        {
            //Arrage

            User user = User.Constructor(Guid.NewGuid()).Content;

            Entry[] entries =
            {
                Entry.New(user.Id, user.Id.ToString(), default).Content,
                Entry.New(user.Id, user.Id.ToString(), default).Content,
                Entry.New(user.Id, user.Id.ToString(), default).Content,
            };

            IEntryRepository entryRepository = new EntryRepository(GetEntryContextMock(() => new Dictionary<User, IEnumerable<Entry>>()
                                                                                            {
                                                                                                { user, entries }
                                                                                            })
                                                                   );

            Entry entry = Entry.Constructor(entries.First().Id, user.Id, user.Id.ToString(), default, true).Content;

            //Act

            Result updateResult = await entryRepository.Update(user.Id, entry);

            //Arrage

            Assert.True(updateResult.Success);
        }

        [Fact]
        public async void EntryRepository_Update_UserExistAndEntryCollectionIsEmpty_ShouldReturnSuccessFalse()
        {
            //Arrage

            User user = User.Constructor(Guid.NewGuid()).Content;

            IEntryRepository entryRepository = new EntryRepository(GetEntryContextMock(() => new Dictionary<User, IEnumerable<Entry>>()
                                                                                            {
                                                                                                { user, Enumerable.Empty<Entry>() }
                                                                                            })
                                                                   );

            Entry entry = Entry.New(user.Id, user.Id.ToString(), default).Content;

            //Act

            Result updateResult = await entryRepository.Update(user.Id, entry);

            //Arrage

            Assert.False(updateResult.Success);
        }

        [Fact]
        public async void EntryRepository_Update_UserNotExistAndEntryCollectionIsEmpty_ShouldReturnSuccessFalse()
        {
            //Arrage

            IEntryRepository entryRepository = new EntryRepository(GetEntryContextMock());

            User user = User.Constructor(Guid.NewGuid()).Content;

            Entry entry = Entry.New(user.Id, user.Id.ToString(), default).Content;

            //Act

            Result updateResult = await entryRepository.Update(user.Id, entry);

            //Arrage

            Assert.False(updateResult.Success);
        }

        #endregion

        #region Delete

        [Fact]
        public async void EntryRepository_Delete_UserExistAndEntryCollectionIsNotEmpty_ShouldReturnSuccessTrue()
        {
            //Arrage

            User user = User.Constructor(Guid.NewGuid()).Content;

            Entry[] entries =
            {
                Entry.New(user.Id, user.Id.ToString(), default).Content,
                Entry.New(user.Id, user.Id.ToString(), default).Content,
                Entry.New(user.Id, user.Id.ToString(), default).Content,
            };

            IEntryRepository entryRepository = new EntryRepository(GetEntryContextMock(() => new Dictionary<User, IEnumerable<Entry>>()
                                                                                            {
                                                                                                { user, entries }
                                                                                            })
                                                                   );

            Entry entry = entries.First();

            //Act

            Result deleteResult = await entryRepository.Delete(user.Id, entry.Id);

            //Arrage

            Assert.True(deleteResult.Success);
        }

        [Fact]
        public async void EntryRepository_Delete_UserExistAndEntryCollectionIsEmpty_ShouldReturnSuccessFalse()
        {
            //Arrage

            User user = User.Constructor(Guid.NewGuid()).Content;

            IEntryRepository entryRepository = new EntryRepository(GetEntryContextMock(() => new Dictionary<User, IEnumerable<Entry>>()
                                                                                            {
                                                                                                { user, Enumerable.Empty<Entry>() }
                                                                                            })
                                                                   );

            Entry entry = Entry.New(user.Id, user.Id.ToString(), default).Content;

            //Act

            Result deleteResult = await entryRepository.Delete(user.Id, entry.Id);

            //Arrage

            Assert.False(deleteResult.Success);
        }

        [Fact]
        public async void EntryRepository_Delete_UserNotExistAndEntryCollectionIsEmpty_ShouldReturnSuccessFalse()
        {
            //Arrage


            IEntryRepository entryRepository = new EntryRepository(GetEntryContextMock());

            User user = User.Constructor(Guid.NewGuid()).Content;

            Entry entry = Entry.New(user.Id, user.Id.ToString(), default).Content;

            //Act

            Result deleteResult = await entryRepository.Delete(user.Id, entry.Id);

            //Arrage

            Assert.False(deleteResult.Success);
        }

        #endregion

        #region DeleteByCategory

        [Fact]
        public async void EntryRepository_DeleteByCategory_UserExistAndEntryCollectionIsNotEmpty_ShouldReturnSuccessTrue()
        {
            //Arrage

            User user = User.Constructor(Guid.NewGuid()).Content;

            Entry[] entries =
            {
                Entry.New(user.Id, user.Id.ToString(), default).Content,
                Entry.New(user.Id, user.Id.ToString(), default).Content,
                Entry.New(user.Id, user.Id.ToString(), default).Content,
            };

            IEntryRepository entryRepository = new EntryRepository(GetEntryContextMock(() => new Dictionary<User, IEnumerable<Entry>>()
                                                                                            {
                                                                                                { user, entries }
                                                                                            }));

            Entry entry = entries.First();

            //Act

            Result deleteResult = await entryRepository.DeleteByCategory(user.Id, user.Id);

            //Arrage

            Assert.True(deleteResult.Success);
        }

        [Fact]
        public async void EntryRepository_DeleteByCategory_UserExistAndEntryCollectionIsEmpty_ShouldReturnSuccessFalse()
        {
            //Arrage

            User user = User.Constructor(Guid.NewGuid()).Content;

            IEntryRepository entryRepository = new EntryRepository(GetEntryContextMock(() => new Dictionary<User, IEnumerable<Entry>>()
                                                                                            {
                                                                                                { user, Enumerable.Empty<Entry>() }
                                                                                            }));

            Guid categoryId = Guid.NewGuid();

            //Act

            Result deleteResult = await entryRepository.DeleteByCategory(user.Id, categoryId);

            //Arrage

            Assert.False(deleteResult.Success);
        }

        [Fact]
        public async void EntryRepository_DeleteByCategory_UserNotExistAndEntryCollectionIsEmpty_ShouldReturnSuccessFalse()
        {
            //Arrage

            IEntryRepository entryRepository = new EntryRepository(GetEntryContextMock());

            User user = User.Constructor(Guid.NewGuid()).Content;

            Guid categoryId = Guid.NewGuid();

            //Act

            Result deleteResult = await entryRepository.DeleteByCategory(user.Id, categoryId);

            //Arrage

            Assert.False(deleteResult.Success);
        }

        #endregion
    }
}
