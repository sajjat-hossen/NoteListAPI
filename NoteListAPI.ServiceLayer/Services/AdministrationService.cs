﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NoteListAPI.DomainLayer.Models;
using NoteListAPI.ServiceLayer.IServices;
using NoteListAPI.ServiceLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NoteListAPI.ServiceLayer.Services
{
    public class AdministrationService : IAdministrationService
    {
        #region Fileds

        private readonly UserManager<IdentityUser<int>> _userManager;
        private readonly SignInManager<IdentityUser<int>> _signInManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        #endregion

        #region Constructor

        public AdministrationService(UserManager<IdentityUser<int>> userManager, IHttpContextAccessor httpContextAccessor, SignInManager<IdentityUser<int>> signInManager, RoleManager<IdentityRole<int>> roleManager)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        #endregion

        #region GetAllUser

        public IEnumerable<UserViewModel> GetAllUser()
        {
            var users = _userManager.Users;

            var models = users.Select(u =>
            new UserViewModel
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email
            });

            return models;
        }

        #endregion

        #region FindUserByIdAsync

        public async Task<IdentityUser<int>> FindUserByIdAsync(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            return user;
        }

        #endregion

        #region GetUserRolesAsync

        public async Task<IEnumerable<string>> GetUserRolesAsync(IdentityUser<int> user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            return roles;
        }

        #endregion

        #region GetUserRolesModel

        public async Task<UserRoleViewModel> GetUserRolesModel(IdentityUser<int> user)
        {
            var model = new UserRoleViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Roles = new List<UserRole>()
            };

            var existingUserRoles = await GetUserRolesAsync(user);

            var roles = await _roleManager.Roles.ToListAsync();

            var userRoles = roles.Select(role => new UserRole()
            {
                RoleName = role.Name,
                IsSelected = existingUserRoles.Any(c => c == role.Name) ? true : false
            });

            model.Roles.AddRange(userRoles);

            return model;
        }

        #endregion

        #region UpdateUserRolesAsync

        public async Task<bool> UpdateUserRolesAsync(UserRoleViewModel model)
        {
            var user = await FindUserByIdAsync(model.Id);
            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);

            if (!result.Succeeded)
            {
                return false;
            }

            var allSelectedRoles = model.Roles.Where(c => c.IsSelected)
                .Select(c => c.RoleName)
                .ToList();

            if (allSelectedRoles.Any())
            {
                result = await _userManager.AddToRolesAsync(user, allSelectedRoles);

                if (!result.Succeeded)
                {
                    return false;
                }

                var logedUserId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
                var logedUser = await FindUserByIdAsync(Convert.ToInt32(logedUserId));
                await _signInManager.RefreshSignInAsync(logedUser);
            }

            return true;

        }

        #endregion

        //#region GetRoleClaimsAsync

        //public async Task<List<RoleClaimViewModel>> GetRoleClaimsAsync()
        //{
        //    var models = new List<RoleClaimViewModel>();
        //    var roles = await _roleManager.Roles.ToListAsync();

        //    foreach (var identityRole in roles)
        //    {
        //        var existingRoleClaims = await _roleManager.GetClaimsAsync(identityRole);

        //        var roleClaims = ClaimsStore.GetAllClaims().Select(claim => new RoleClaim
        //        {
        //            ClaimType = claim.Type,
        //            IsSelected = existingRoleClaims.Any(c => c.Type == claim.Type)
        //        }).ToList();

        //        var roleClaimModel = new RoleClaimViewModel
        //        {
        //            RoleName = identityRole.Name,
        //            RoleClaims = roleClaims
        //        };

        //        models.Add(roleClaimModel);
        //    }

        //    return models;
        //}



        //#endregion

        //#region UpdateRoleClaimsAsync

        //public async Task<bool> UpdateRoleClaimsAsync(List<RoleClaimViewModel> models)
        //{
        //    var roles = await _roleManager.Roles.ToListAsync();
        //    foreach (IdentityRole<int> identityRole in roles)
        //    {
        //        var existingRoleClaims = await _roleManager.GetClaimsAsync(identityRole);

        //        foreach (Claim claim in existingRoleClaims)
        //        {
        //            await _roleManager.RemoveClaimAsync(identityRole, claim);
        //        }
        //    }

        //    foreach (var model in models)
        //    {
        //        var identityRole = await _roleManager.FindByNameAsync(model.RoleName);

        //        var allSelectedClaims = model.RoleClaims.Where(c => c.IsSelected)
        //        .Select(c => new Claim(c.ClaimType, c.ClaimType))
        //        .ToList();

        //        foreach (var claim in allSelectedClaims)
        //        {
        //            await _roleManager.AddClaimAsync(identityRole, claim);
        //        }
        //    }

        //    return true;
        //}

        //#endregion
    }
}
