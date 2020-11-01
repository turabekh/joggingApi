using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Models.IdentityModels;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tests.MockServices
{
    public static class IdentityServiceMock
    {

        public static Mock<RoleManager<Role>> GetRoleMockManager(List<Role> roles)
        {
            var mockRoleManager = new Mock<RoleManager<Role>>(
                                new Mock<IRoleStore<Role>>().Object,
                                new IRoleValidator<Role>[0],
                                new Mock<ILookupNormalizer>().Object,
                                new Mock<IdentityErrorDescriber>().Object,
                                new Mock<ILogger<RoleManager<Role>>>().Object);
            mockRoleManager.Setup(x => x.Roles).Returns(roles.AsQueryable());
            mockRoleManager.Setup(x => x.DeleteAsync(It.IsAny<Role>())).ReturnsAsync(IdentityResult.Success);
            mockRoleManager.Setup(x => x.CreateAsync(It.IsAny<Role>())).ReturnsAsync(IdentityResult.Success);
            mockRoleManager.Setup(x => x.UpdateAsync(It.IsAny<Role>())).ReturnsAsync(IdentityResult.Success);
            mockRoleManager.Setup(x => x.RoleExistsAsync(It.IsAny<string>())).ReturnsAsync(true);
            return mockRoleManager;
        }

        public static Mock<UserManager<TUser>> MockUserManager<TUser>(List<TUser> ls) where TUser : class
        {
            var store = new Mock<IUserStore<TUser>>();
            var mgr = new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
            mgr.Object.UserValidators.Add(new UserValidator<TUser>());
            mgr.Object.PasswordValidators.Add(new PasswordValidator<TUser>());
            mgr.Setup(x => x.DeleteAsync(It.IsAny<TUser>())).ReturnsAsync(IdentityResult.Success);
            mgr.Setup(x => x.CreateAsync(It.IsAny<TUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success).Callback<TUser, string>((x, y) => ls.Add(x));
            mgr.Setup(x => x.UpdateAsync(It.IsAny<TUser>())).ReturnsAsync(IdentityResult.Success);
            mgr.Setup(x => x.GetRolesAsync(It.IsAny<TUser>())).ReturnsAsync(new List<string>() { "Admin" });
            mgr.Setup(x => x.Users).Returns(ls.AsQueryable());
            mgr.Setup(x => x.AddToRolesAsync(It.IsAny<TUser>(), It.IsAny<List<string>>())).ReturnsAsync(IdentityResult.Success);
            mgr.Setup(x => x.RemoveFromRolesAsync(It.IsAny<TUser>(), It.IsAny<List<string>>())).ReturnsAsync(IdentityResult.Success);
            mgr.Setup(x => x.GetRolesAsync(It.IsAny<TUser>())).ReturnsAsync(new List<string>());
            return mgr;
        }
    }
}
