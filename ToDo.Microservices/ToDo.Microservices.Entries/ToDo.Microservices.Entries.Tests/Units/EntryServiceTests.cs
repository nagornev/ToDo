using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Domain.Results;
using ToDo.Microservices.Entries.Database.Contexts;
using ToDo.Microservices.Entries.Domain.Collectings;
using ToDo.Microservices.Entries.Domain.Models;
using ToDo.Microservices.Entries.Infrastructure.Providers;
using ToDo.Microservices.Entries.Infrastructure.Repositories;
using ToDo.Microservices.Entries.Infrastructure.Services;
using ToDo.Microservices.Entries.Tests.Mock;
using ToDo.Microservices.Entries.UseCases.Caches;
using ToDo.Microservices.Entries.UseCases.Providers;
using ToDo.Microservices.Entries.UseCases.Repositories;
using ToDo.Microservices.Entries.UseCases.Services;

namespace ToDo.Microservices.Entries.Tests.Units
{
    public class EntryServiceTests
    {
        private EntryContext GetEntryContextMock(Func<IReadOnlyDictionary<User, IEnumerable<Entry>>> startContext = null)
        {
            return new EntryContextMock($"{Guid.NewGuid()}", startContext);
        }

        private ICategoryService GetAvailableCategoryServiceMock(Action<Mock<ICategoryService>> configure = null)
        {
            return new CategoryServiceMock(mock =>
            {
                mock.Setup(service => service.GetCategory(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .Returns<Guid, Guid>(async (userId, categoryId) => Result<Category>.Successful(new Category(categoryId, userId.ToString())));

                mock.Setup(service => service.GetCategories(It.IsAny<Guid>()))
                    .Returns<Guid>(async (userId) =>
                    {
                        Category userCategory = new Category(userId, "User category");
                        Category emptyCategory = new Category(Guid.Empty, "Empty category");

                        return Result<IEnumerable<Category>>.Successful(new Category[] { userCategory, emptyCategory });
                    });

                configure?.Invoke(mock);
            });
        }

        private ICategoryService GetUnavailableCategoryServiceMock(Action<Mock<ICategoryService>> configure = null)
        {
            return new CategoryServiceMock(mock =>
            {
                string errorMessage = "The category service is unavailable.";

                mock.Setup(service => service.GetCategories(It.IsAny<Guid>()))
                                             .Returns(async () => Result<IEnumerable<Category>>.Failure(error => error.InternalServer(errorMessage)));

                mock.Setup(service => service.GetCategory(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .Returns(async () => Result<Category>.Failure(error => error.InternalServer(errorMessage)));

                configure?.Invoke(mock);
            });
        }

        private ICategoryService GetEmptyCategoryServiceMock()
        {
            return new CategoryServiceMock((mock) => { });
        }

        #region GetEntries

        [Fact]
        public async void EntryService_GetEntries_UserExistAndEntryCollectionIsNotEmptyAndCategoryServiceIsAvailable_ShouldReturnSuccessTrueAndEntryComposeCollection()
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

            ICategoryService categoryService = GetAvailableCategoryServiceMock();

            IEntryСomposer entryComposer = new EntryComposer();

            IEntryService entryService = new EntryService(entryRepository, categoryService, entryComposer);

            //Act

            Result<IEnumerable<EntryCompose>> entriesResult = await entryService.GetEntries(user.Id);

            //Assert
            Assert.True(entriesResult.Success);
            Assert.NotEmpty(entriesResult.Content);
            Assert.All(entriesResult.Content, (entry) => Assert.Equal(user.Id, entry.Category.Id));
        }

        [Fact]
        public async void EntryService_GetEntries_UserExistAndEntryCollectionIsEmptyAndCategoryServiceIsAvailable_ShouldReturnSuccessTrueAndEmptyEntryComposeCollection()
        {
            //Arrage

            User user = User.Constructor(Guid.NewGuid()).Content;

            IEntryRepository entryRepository = new EntryRepository(GetEntryContextMock(() => new Dictionary<User, IEnumerable<Entry>>()
                                                                                            {
                                                                                                { user, Enumerable.Empty<Entry>() }
                                                                                            }));

            ICategoryService categoryService = GetAvailableCategoryServiceMock();

            IEntryСomposer entryComposer = new EntryComposer();

            IEntryService entryService = new EntryService(entryRepository, categoryService, entryComposer);

            //Act

            Result<IEnumerable<EntryCompose>> entriesResult = await entryService.GetEntries(user.Id);

            //Assert
            Assert.True(entriesResult.Success);
            Assert.Empty(entriesResult.Content);
        }

        [Fact]
        public async void EntryService_GetEntries_UserNotExistAndEntryCollectionIsEmptyAndCategoryServiceIsAvailable_ShouldReturnSuccessFalse()
        {
            //Arrage

            IEntryRepository entryRepository = new EntryRepository(GetEntryContextMock());

            ICategoryService categoryService = GetAvailableCategoryServiceMock();

            IEntryСomposer entryComposer = new EntryComposer();

            IEntryService entryService = new EntryService(entryRepository, categoryService, entryComposer);

            User user = User.Constructor(Guid.NewGuid()).Content;

            //Act

            Result<IEnumerable<EntryCompose>> entriesResult = await entryService.GetEntries(user.Id);

            //Assert
            Assert.False(entriesResult.Success);
        }

        [Fact]
        public async void EntryService_GetEntries_UserExistAndEntryCollectionIsNotEmptyAndCategoryServiceIsUnavailable_ShouldReturnSuccessFalse()
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

            ICategoryService categoryService = GetUnavailableCategoryServiceMock();

            IEntryСomposer entryComposer = new EntryComposer();

            IEntryService entryService = new EntryService(entryRepository, categoryService, entryComposer);

            //Act

            Result<IEnumerable<EntryCompose>> entriesResult = await entryService.GetEntries(user.Id);

            //Assert
            Assert.False(entriesResult.Success);
        }

        [Fact]
        public async void EntryService_GetEntries_UserExistAndEntryCollectionIsEmptyAndCategoryServiceIsUnavailable_ShouldReturnSuccessFalse()
        {
            //Arrage

            User user = User.Constructor(Guid.NewGuid()).Content;

            IEntryRepository entryRepository = new EntryRepository(GetEntryContextMock(() => new Dictionary<User, IEnumerable<Entry>>()
                                                                                            {
                                                                                                { user, Enumerable.Empty<Entry>() }
                                                                                            }));

            ICategoryService categoryService = GetUnavailableCategoryServiceMock();

            IEntryСomposer entryComposer = new EntryComposer();

            IEntryService entryService = new EntryService(entryRepository, categoryService, entryComposer);

            //Act

            Result<IEnumerable<EntryCompose>> entriesResult = await entryService.GetEntries(user.Id);

            //Assert
            Assert.False(entriesResult.Success);
        }

        [Fact]
        public async void EntryService_GetEntries_UserNotExistAndEntryCollectionIsEmptyAndCategoryServiceIsUnavailable_ShouldReturnSuccessFalse()
        {
            //Arrage

            IEntryRepository entryRepository = new EntryRepository(GetEntryContextMock());

            ICategoryService categoryService = GetUnavailableCategoryServiceMock();

            IEntryСomposer entryComposer = new EntryComposer();

            IEntryService entryService = new EntryService(entryRepository, categoryService, entryComposer);

            User user = User.Constructor(Guid.NewGuid()).Content;

            //Act

            Result<IEnumerable<EntryCompose>> entriesResult = await entryService.GetEntries(user.Id);

            //Assert
            Assert.False(entriesResult.Success);
        }

        #endregion

        #region GetEntry

        [Fact]
        public async void EntryService_GetEntry_UserExistAndEntryCollectionIsNotEmptyAndCategoryServiceIsAvailable_ShouldReturnSuccessTrueAndEntryCompose()
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

            ICategoryService categoryService = GetAvailableCategoryServiceMock();

            IEntryСomposer entryComposer = new EntryComposer();

            IEntryService entryService = new EntryService(entryRepository, categoryService, entryComposer);

            Entry entry = entries.First();

            //Act

            Result<EntryCompose> entryResult = await entryService.GetEntry(user.Id, entry.Id);

            //Assert
            Assert.True(entryResult.Success);
            Assert.Equal(entry.Id, entryResult.Content.Id);
        }

        [Fact]
        public async void EntryService_GetEntry_UserExistAndEntryCollectionIsEmptyAndCategoryServiceIsAvailable_ShouldReturnSuccessFalse()
        {
            //Arrage

            User user = User.Constructor(Guid.NewGuid()).Content;


            IEntryRepository entryRepository = new EntryRepository(GetEntryContextMock(() => new Dictionary<User, IEnumerable<Entry>>()
                                                                                            {
                                                                                                { user, Enumerable.Empty<Entry>() }
                                                                                            }));

            ICategoryService categoryService = GetAvailableCategoryServiceMock();

            IEntryСomposer entryComposer = new EntryComposer();

            IEntryService entryService = new EntryService(entryRepository, categoryService, entryComposer);

            Entry entry = Entry.New(user.Id, user.Id.ToString(), default).Content;

            //Act

            Result<EntryCompose> entryResult = await entryService.GetEntry(user.Id, entry.Id);

            //Assert
            Assert.False(entryResult.Success);
        }


        [Fact]
        public async void EntryService_GetEntry_UserNotExistAndEntryCollectionIsEmptyAndCategoryServiceIsAvailable_ShouldReturnSuccessFalse()
        {
            //Arrage

            IEntryRepository entryRepository = new EntryRepository(GetEntryContextMock());

            ICategoryService categoryService = GetAvailableCategoryServiceMock();

            IEntryСomposer entryComposer = new EntryComposer();

            IEntryService entryService = new EntryService(entryRepository, categoryService, entryComposer);

            User user = User.Constructor(Guid.NewGuid()).Content;

            Entry entry = Entry.New(user.Id, user.Id.ToString(), default).Content;

            //Act

            Result<EntryCompose> entryResult = await entryService.GetEntry(user.Id, entry.Id);

            //Assert
            Assert.False(entryResult.Success);
        }

        [Fact]
        public async void EntryService_GetEntry_UserExistAndEntryCollectionIsNotEmptyAndCategoryServiceIsUnavailable_ShouldReturnSuccessFalse()
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

            ICategoryService categoryService = GetUnavailableCategoryServiceMock();

            IEntryСomposer entryComposer = new EntryComposer();

            IEntryService entryService = new EntryService(entryRepository, categoryService, entryComposer);

            Entry entry = entries.First();

            //Act

            Result<EntryCompose> entryResult = await entryService.GetEntry(user.Id, entry.Id);

            //Assert
            Assert.False(entryResult.Success);
        }

        [Fact]
        public async void EntryService_GetEntry_UserExistAndEntryCollectionIsEmptyAndCategoryServiceIsUnavailable_ShouldReturnSuccessFalse()
        {
            //Arrage

            User user = User.Constructor(Guid.NewGuid()).Content;

            IEntryRepository entryRepository = new EntryRepository(GetEntryContextMock(() => new Dictionary<User, IEnumerable<Entry>>()
                                                                                            {
                                                                                                { user, Enumerable.Empty<Entry>() }
                                                                                            }));

            ICategoryService categoryService = GetUnavailableCategoryServiceMock();

            IEntryСomposer entryComposer = new EntryComposer();

            IEntryService entryService = new EntryService(entryRepository, categoryService, entryComposer);

            Entry entry = Entry.New(user.Id, user.Id.ToString(), default).Content;

            //Act

            Result<EntryCompose> entryResult = await entryService.GetEntry(user.Id, entry.Id);

            //Assert
            Assert.False(entryResult.Success);
        }


        [Fact]
        public async void EntryService_GetEntry_UserNotExistAndEntryCollectionIsEmptyAndCategoryServiceIsUnavailable_ShouldReturnSuccessFalse()
        {
            //Arrage


            IEntryRepository entryRepository = new EntryRepository(GetEntryContextMock());

            ICategoryService categoryService = GetUnavailableCategoryServiceMock();

            IEntryСomposer entryComposer = new EntryComposer();

            IEntryService entryService = new EntryService(entryRepository, categoryService, entryComposer);

            User user = User.Constructor(Guid.NewGuid()).Content;

            Entry entry = Entry.New(user.Id, user.Id.ToString(), default).Content;

            //Act

            Result<EntryCompose> entryResult = await entryService.GetEntry(user.Id, entry.Id);

            //Assert
            Assert.False(entryResult.Success);
        }

        #endregion

        #region CreateEntry

        [Fact]
        public async void EntryService_CreateEntry_UserExistAndEntryCollectionIsNotEmptyAndCategoryServiceIsAvailableAndCategoryExist_ShouldReturnSuccessTrue()
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

            ICategoryService categoryService = GetAvailableCategoryServiceMock();

            IEntryСomposer entryComposer = new EntryComposer();

            IEntryService entryService = new EntryService(entryRepository, categoryService, entryComposer);

            Guid categoryId = user.Id;

            //Act

            Result createResult = await entryService.CreateEntry(user.Id, categoryId, user.Id.ToString(), default);

            //Assert
            Assert.True(createResult.Success);
        }

        [Fact]
        public async void EntryService_CreateEntry_UserExistAndEntryCollectionIsNotEmptyAndCategoryServiceIsAvailableAndCategoryNotExist_ShouldReturnSuccessFalse()
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

            ICategoryService categoryService = GetAvailableCategoryServiceMock(mock =>
                                                                               {
                                                                                   mock.Setup(service => service.GetCategory(It.IsAny<Guid>(), It.IsAny<Guid>()))
                                                                                       .Returns<Guid, Guid>(async (userId, categoryId) => Result<Category>.Failure(error => error.InternalServer($"The category {categoryId} was not found.")));
                                                                               });

            IEntryСomposer entryComposer = new EntryComposer();

            IEntryService entryService = new EntryService(entryRepository, categoryService, entryComposer);

            Guid categoryId = Guid.NewGuid();

            //Act

            Result createResult = await entryService.CreateEntry(user.Id, categoryId, user.Id.ToString(), default);

            //Assert
            Assert.False(createResult.Success);
        }

        [Fact]
        public async void EntryService_CreateEntry_UserExistAndEntryCollectionIsEmptyAndCategoryServiceIsAvailableAndCategoryExist_ShouldReturnSuccessTrue()
        {
            //Arrage

            User user = User.Constructor(Guid.NewGuid()).Content;

            IEntryRepository entryRepository = new EntryRepository(GetEntryContextMock(() => new Dictionary<User, IEnumerable<Entry>>()
                                                                                            {
                                                                                                { user, Enumerable.Empty<Entry>() }
                                                                                            }));

            ICategoryService categoryService = GetAvailableCategoryServiceMock();

            IEntryСomposer entryComposer = new EntryComposer();

            IEntryService entryService = new EntryService(entryRepository, categoryService, entryComposer);

            Guid categoryId = user.Id;

            //Act

            Result createResult = await entryService.CreateEntry(user.Id, categoryId, user.Id.ToString(), default);

            //Assert
            Assert.True(createResult.Success);
        }

        [Fact]
        public async void EntryService_CreateEntry_UserExistAndEntryCollectionIsEmptyAndCategoryServiceIsAvailableAndCategoryNotExist_ShouldReturnSuccessFalse()
        {
            //Arrage

            User user = User.Constructor(Guid.NewGuid()).Content;

            IEntryRepository entryRepository = new EntryRepository(GetEntryContextMock(() => new Dictionary<User, IEnumerable<Entry>>()
                                                                                            {
                                                                                                { user, Enumerable.Empty<Entry>() }
                                                                                            }));

            ICategoryService categoryService = GetAvailableCategoryServiceMock(mock =>
                                                                              {
                                                                                  mock.Setup(service => service.GetCategory(It.IsAny<Guid>(), It.IsAny<Guid>()))
                                                                                      .Returns<Guid, Guid>(async (userId, categoryId) => Result<Category>.Failure(error => error.InternalServer($"The category {categoryId} was not found.")));
                                                                              });

            IEntryСomposer entryComposer = new EntryComposer();

            IEntryService entryService = new EntryService(entryRepository, categoryService, entryComposer);

            Guid categoryId = Guid.NewGuid();

            //Act

            Result createResult = await entryService.CreateEntry(user.Id, categoryId, user.Id.ToString(), default);

            //Assert
            Assert.False(createResult.Success);
        }

        [Fact]
        public async void EntryService_CreateEntry_UserNotExistAndEntryCollectionIsEmptyAndCategoryServiceIsAvailableAndCategoryExist_ShouldThrowDbUpdateException()
        {
            //Arrage

            IEntryRepository entryRepository = new EntryRepository(GetEntryContextMock());

            ICategoryService categoryService = GetAvailableCategoryServiceMock();

            IEntryСomposer entryComposer = new EntryComposer();

            IEntryService entryService = new EntryService(entryRepository, categoryService, entryComposer);

            User user = User.Constructor(Guid.NewGuid()).Content;

            Guid categoryId = user.Id;

            //Act & Assert
            await Assert.ThrowsAsync<DbUpdateException>(async () => await entryService.CreateEntry(user.Id, categoryId, user.Id.ToString(), default));
        }

        [Fact]
        public async void EntryService_CreateEntry_UserNotExistAndEntryCollectionIsEmptyAndCategoryServiceIsAvailableAndCategoryNotExist_ShouldReturnSuccessFalse()
        {
            //Arrage

            IEntryRepository entryRepository = new EntryRepository(GetEntryContextMock());

            ICategoryService categoryService = GetAvailableCategoryServiceMock(mock =>
                                                                              {
                                                                                  mock.Setup(service => service.GetCategory(It.IsAny<Guid>(), It.IsAny<Guid>()))
                                                                                      .Returns<Guid, Guid>(async (userId, categoryId) => Result<Category>.Failure(error => error.InternalServer($"The category {categoryId} was not found.")));
                                                                              });

            IEntryСomposer entryComposer = new EntryComposer();

            IEntryService entryService = new EntryService(entryRepository, categoryService, entryComposer);

            User user = User.Constructor(Guid.NewGuid()).Content;

            Guid categoryId = user.Id;

            //Act

            Result createResult = await entryService.CreateEntry(user.Id, categoryId, user.Id.ToString(), default);

            //Assert
            Assert.False(createResult.Success);
        }

        #endregion

        #region UpdateEntry

        [Fact]
        public async void EntryService_UpdateEntry_UserExistAndEntryCollectionIsNotEmptyAndCategoryServiceIsAvailableAndCategoryExist_ShouldReturnSuccessTrue()
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

            ICategoryService categoryService = GetAvailableCategoryServiceMock();

            IEntryСomposer entryComposer = new EntryComposer();

            IEntryService entryService = new EntryService(entryRepository, categoryService, entryComposer);

            Entry entry = Entry.Constructor(entries.First().Id, entries.First().Id, user.Id.ToString(), default, true).Content;

            //Act

            Result updateResult = await entryService.UpdateEntry(user.Id, entry.Id, entry.CategoryId, entry.Text, entry.Deadline, entry.Completed);

            //Assert
            Assert.True(updateResult.Success);
        }

        [Fact]
        public async void EntryService_UpdateEntry_UserExistAndEntryCollectionIsNotEmptyAndCategoryServiceIsAvailableAndCategoryNotExist_ShouldReturnSuccessFalse()
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


            ICategoryService categoryService = GetAvailableCategoryServiceMock(mock =>
                                                                               {
                                                                                   mock.Setup(service => service.GetCategory(It.IsAny<Guid>(), It.IsAny<Guid>()))
                                                                                       .Returns<Guid, Guid>(async (userId, categoryId) => Result<Category>.Failure(error => error.InternalServer($"The category {categoryId} was not found.")));
                                                                               });

            IEntryСomposer entryComposer = new EntryComposer();

            IEntryService entryService = new EntryService(entryRepository, categoryService, entryComposer);

            Entry entry = Entry.Constructor(entries.First().Id, entries.First().Id, user.Id.ToString(), default, true).Content;

            //Act

            Result updateResult = await entryService.UpdateEntry(user.Id, entry.Id, entry.CategoryId, entry.Text, entry.Deadline, entry.Completed);

            //Assert
            Assert.False(updateResult.Success);
        }

        [Fact]
        public async void EntryService_UpdateEntry_UserExistAndEntryCollectionIsEmptyAndCategoryServiceIsAvailableAndCategoryExist_ShouldReturnSuccessFalse()
        {
            //Arrage

            User user = User.Constructor(Guid.NewGuid()).Content;

            IEntryRepository entryRepository = new EntryRepository(GetEntryContextMock(() => new Dictionary<User, IEnumerable<Entry>>()
                                                                                            {
                                                                                                { user, Enumerable.Empty<Entry>() }
                                                                                            }));

            ICategoryService categoryService = GetAvailableCategoryServiceMock();

            IEntryСomposer entryComposer = new EntryComposer();

            IEntryService entryService = new EntryService(entryRepository, categoryService, entryComposer);

            Entry entry = Entry.Constructor(Guid.NewGuid(), user.Id, user.Id.ToString(), default, true).Content;

            //Act

            Result updateResult = await entryService.UpdateEntry(user.Id, entry.Id, entry.CategoryId, entry.Text, entry.Deadline, entry.Completed);

            //Assert
            Assert.False(updateResult.Success);
        }

        [Fact]
        public async void EntryService_UpdateEntry_UserExistAndEntryCollectionIsEmptyAndCategoryServiceIsAvailableAndCategoryNotExist_ShouldReturnSuccessFalse()
        {
            //Arrage

            User user = User.Constructor(Guid.NewGuid()).Content;

            IEntryRepository entryRepository = new EntryRepository(GetEntryContextMock(() => new Dictionary<User, IEnumerable<Entry>>()
                                                                                            {
                                                                                                { user, Enumerable.Empty<Entry>() }
                                                                                            }));

            ICategoryService categoryService = GetAvailableCategoryServiceMock(mock =>
                                                                               {
                                                                                   mock.Setup(service => service.GetCategory(It.IsAny<Guid>(), It.IsAny<Guid>()))
                                                                                       .Returns<Guid, Guid>(async (userId, categoryId) => Result<Category>.Failure( error => error.InternalServer($"The category {categoryId} was not found.")));
                                                                               });

            IEntryСomposer entryComposer = new EntryComposer();

            IEntryService entryService = new EntryService(entryRepository, categoryService, entryComposer);

            Entry entry = Entry.Constructor(Guid.NewGuid(), user.Id, user.Id.ToString(), default, true).Content;

            //Act

            Result updateResult = await entryService.UpdateEntry(user.Id, entry.Id, entry.CategoryId, entry.Text, entry.Deadline, entry.Completed);

            //Assert
            Assert.False(updateResult.Success);
        }


        [Fact]
        public async void EntryService_UpdateEntry_UserNotExistAndEntryCollectionIsEmptyAndCategoryServiceIsAvailableAndCategoryExist_ShouldReturnSuccessFalse()
        {
            //Arrage

            IEntryRepository entryRepository = new EntryRepository(GetEntryContextMock());

            ICategoryService categoryService = GetAvailableCategoryServiceMock();

            IEntryСomposer entryComposer = new EntryComposer();

            IEntryService entryService = new EntryService(entryRepository, categoryService, entryComposer);

            User user = User.Constructor(Guid.NewGuid()).Content;

            Entry entry = Entry.Constructor(Guid.NewGuid(), user.Id, user.Id.ToString(), default, true).Content;

            //Act

            Result updateResult = await entryService.UpdateEntry(user.Id, entry.Id, entry.CategoryId, entry.Text, entry.Deadline, entry.Completed);

            //Assert
            Assert.False(updateResult.Success);
        }

        [Fact]
        public async void EntryService_UpdateEntry_UserNotExistAndEntryCollectionIsEmptyAndCategoryServiceIsAvailableAndCategoryNotExist_ShouldReturnSuccessFalse()
        {
            //Arrage

            IEntryRepository entryRepository = new EntryRepository(GetEntryContextMock());

            ICategoryService categoryService = GetAvailableCategoryServiceMock(mock =>
                                                                               {
                                                                                   mock.Setup(service => service.GetCategory(It.IsAny<Guid>(), It.IsAny<Guid>()))
                                                                                       .Returns<Guid, Guid>(async (userId, categoryId) => Result<Category>.Failure  (error => error.InternalServer($"The category {categoryId} was not found.")));
                                                                               });

            IEntryСomposer entryComposer = new EntryComposer();

            IEntryService entryService = new EntryService(entryRepository, categoryService, entryComposer);

            User user = User.Constructor(Guid.NewGuid()).Content;

            Entry entry = Entry.Constructor(Guid.NewGuid(), user.Id, user.Id.ToString(), default, true).Content;

            //Act

            Result updateResult = await entryService.UpdateEntry(user.Id, entry.Id, entry.CategoryId, entry.Text, entry.Deadline, entry.Completed);

            //Assert
            Assert.False(updateResult.Success);
        }

        #endregion

        #region DeleteEntry


        [Fact]
        public async void EntryService_DeleteEntry_UserExistAndEntryCollectionIsNotEmpty_ShouldReturnSuccessTrue()
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

            ICategoryService categoryService = GetEmptyCategoryServiceMock();

            IEntryСomposer entryComposer = new EntryComposer();

            IEntryService entryService = new EntryService(entryRepository, categoryService, entryComposer);

            Entry entry = entries.First();

            //Act

            Result deleteResult = await entryService.DeleteEntry(user.Id, entry.Id);

            //Assert
            Assert.True(deleteResult.Success);
        }

        [Fact]
        public async void EntryService_DeleteEntry_UserExistAndEntryCollectionIsEmpty_ShouldReturnSuccessFalse()
        {
            //Arrage

            User user = User.Constructor(Guid.NewGuid()).Content;


            IEntryRepository entryRepository = new EntryRepository(GetEntryContextMock(() => new Dictionary<User, IEnumerable<Entry>>()
                                                                                            {
                                                                                                { user, Enumerable.Empty<Entry>() }
                                                                                            }));

            ICategoryService categoryService = GetEmptyCategoryServiceMock();

            IEntryСomposer entryComposer = new EntryComposer();

            IEntryService entryService = new EntryService(entryRepository, categoryService, entryComposer);

            Entry entry = Entry.New(user.Id, user.Id.ToString(), default).Content;

            //Act

            Result deleteResult = await entryService.DeleteEntry(user.Id, entry.Id);

            //Assert
            Assert.False(deleteResult.Success);
        }

        [Fact]
        public async void EntryService_DeleteEntry_UserNotExistAndEntryCollectionIsEmpty_ShouldReturnSuccessFalse()
        {
            //Arrage

            IEntryRepository entryRepository = new EntryRepository(GetEntryContextMock());

            ICategoryService categoryService = GetEmptyCategoryServiceMock();

            IEntryСomposer entryComposer = new EntryComposer();

            IEntryService entryService = new EntryService(entryRepository, categoryService, entryComposer);

            User user = User.Constructor(Guid.NewGuid()).Content;

            Entry entry = Entry.New(user.Id, user.Id.ToString(), default).Content;

            //Act

            Result deleteResult = await entryService.DeleteEntry(user.Id, entry.Id);

            //Assert
            Assert.False(deleteResult.Success);
        }

        #endregion
    }
}
